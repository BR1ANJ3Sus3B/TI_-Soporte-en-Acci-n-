import assert from 'node:assert/strict';
import { once } from 'node:events';
import { createServer } from 'node:http';
import { after, before, describe, it } from 'node:test';
import { exportJWK, generateKeyPair, SignJWT } from 'jose';
import { verifyAccessToken } from '../src/middleware/authenticate.js';

describe('Keycloak JWT validation (local JWKS fixture)', () => {
  let server;
  let issuer;
  let config;
  let privateKey;

  before(async () => {
    const pair = await generateKeyPair('RS256');
    privateKey = pair.privateKey;
    const publicJwk = await exportJWK(pair.publicKey);
    Object.assign(publicJwk, { kid: 'local-test-key', alg: 'RS256', use: 'sig' });
    server = createServer((request, response) => {
      if (request.url !== '/certs') return response.writeHead(404).end();
      response.writeHead(200, { 'Content-Type': 'application/json' });
      response.end(JSON.stringify({ keys: [publicJwk] }));
    });
    server.listen(0, '127.0.0.1');
    await once(server, 'listening');
    issuer = 'https://issuer.example.test/realms/test';
    config = {
      keycloakIssuer: issuer,
      keycloakAudience: ['ti-soporte-api'],
      keycloakJwksUri: `http://127.0.0.1:${server.address().port}/certs`,
      keycloakClientId: 'ti-soporte-flutter',
    };
  });

  after(async () => {
    if (!server) return;
    server.close();
    await once(server, 'close');
  });

  it('valida firma, issuer, audience y subject del token', async () => {
    const token = await new SignJWT({ realm_access: { roles: ['estudiante'] } })
      .setProtectedHeader({ alg: 'RS256', kid: 'local-test-key' })
      .setIssuer(issuer)
      .setAudience('ti-soporte-api')
      .setSubject('user-123')
      .setIssuedAt()
      .setExpirationTime('2m')
      .sign(privateKey);
    const claims = await verifyAccessToken(token, config);
    assert.equal(claims.sub, 'user-123');
    assert.deepEqual(claims.realm_access.roles, ['estudiante']);
  });

  it('rechaza un token expirado aunque firma, issuer y audience sean correctos', async () => {
    const token = await new SignJWT({})
      .setProtectedHeader({ alg: 'RS256', kid: 'local-test-key' })
      .setIssuer(issuer).setAudience('ti-soporte-api').setSubject('user-123')
      .setIssuedAt().setExpirationTime(Math.floor(Date.now() / 1000) - 60)
      .sign(privateKey);
    await assert.rejects(() => verifyAccessToken(token, config), { code: 'ERR_JWT_EXPIRED' });
  });

  it('rechaza issuer y audience distintos', async () => {
    const buildToken = (tokenIssuer, audience) => new SignJWT({})
      .setProtectedHeader({ alg: 'RS256', kid: 'local-test-key' })
      .setIssuer(tokenIssuer)
      .setAudience(audience)
      .setSubject('user-123')
      .setIssuedAt()
      .setExpirationTime('2m')
      .sign(privateKey);
    await assert.rejects(() => buildToken('https://wrong.example/realms/test', 'ti-soporte-api')
      .then((token) => verifyAccessToken(token, config)));
    await assert.rejects(() => buildToken(issuer, 'another-api')
      .then((token) => verifyAccessToken(token, config)));
  });
});

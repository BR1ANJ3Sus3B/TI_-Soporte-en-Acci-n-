import assert from 'node:assert/strict';
import { once } from 'node:events';
import { createServer } from 'node:http';
import { test } from 'node:test';
import { createApp } from '../src/app.js';
import { createRegistrationService, validateRegistration } from '../src/services/registration.js';

const input = { name: 'Test Player', email: 'player@example.test', password: 'Test-only-Password-123!' };
const config = { baseUrl: 'https://identity.example.test', realm: 'ti-soporte', clientId: 'registration', clientSecret: 'fixture-secret' };

test('validación normaliza correo y rechaza escalada de rol y contraseña débil', () => {
  assert.equal(validateRegistration({ ...input, email: ' Player@Example.Test ' }).email, input.email);
  for (const body of [{ ...input, role: 'administrador' }, { ...input, password: 'weak' }, { ...input, email: 'invalid' }]) {
    assert.throws(() => validateRegistration(body), { status: 400 });
  }
});

function fixture({ smtp = true, createStatus = 201, mailStatus = 204, composite = false, users = [] } = {}) {
  const calls = [];
  const fetcher = async (url, options) => {
    const path = new URL(url).pathname;
    const body = options.body instanceof URLSearchParams ? null : options.body ? JSON.parse(options.body) : null;
    calls.push({ path, method: options.method, body });
    if (path.endsWith('/token')) return Response.json({ access_token: 'fixture-token' });
    if (path.endsWith('/admin/realms/ti-soporte')) return Response.json({ smtpServer: smtp ? { host: 'smtp.example.test', from: 'sender@example.test' } : {} });
    if (path.endsWith('/roles/jugador')) return Response.json({ id: 'role-player', name: 'jugador', composite });
    if (path.endsWith('/users') && options.method === 'GET') return Response.json(users);
    if (path.endsWith('/users') && options.method === 'POST') return new Response(null, {
      status: createStatus, headers: { location: 'https://identity.example.test/admin/realms/ti-soporte/users/new-user' },
    });
    if (path.endsWith('/role-mappings')) return Response.json({ realmMappings: [{ id: 'default', name: 'default-roles' }] });
    if (path.endsWith('/groups')) return Response.json([]);
    if (path.endsWith('/send-verify-email')) return new Response(null, { status: mailStatus });
    return new Response(null, { status: 204 });
  };
  return { calls, service: createRegistrationService(config, fetcher) };
}

test('crea deshabilitado, elimina roles por defecto, asigna solo jugador y exige verificar correo', async () => {
  const { calls, service } = fixture();
  await service.register(input);
  const create = calls.find((c) => c.path.endsWith('/users'));
  assert.equal(create.body.enabled, false);
  assert.equal(create.body.emailVerified, false);
  assert.deepEqual(create.body.requiredActions, ['VERIFY_EMAIL']);
  assert.equal(create.body.credentials[0].value, input.password);
  const assignment = calls.find((c) => c.path.endsWith('/role-mappings/realm') && c.method === 'POST');
  assert.deepEqual(assignment.body.map((r) => r.name), ['jugador']);
  assert.ok(calls.some((c) => c.method === 'DELETE'));
  assert.ok(calls.at(-1).path.endsWith('/send-verify-email'));
});

test('sin SMTP no crea usuario; fallo de correo no devuelve éxito; duplicado no modifica cuenta', async () => {
  const noMail = fixture({ smtp: false });
  await assert.rejects(() => noMail.service.register(input), { status: 503 });
  assert.equal(noMail.calls.some((c) => c.path.endsWith('/users')), false);
  const failed = fixture({ mailStatus: 500 });
  await assert.rejects(() => failed.service.register(input), { status: 503 });
  const duplicate = fixture({ createStatus: 409 });
  await duplicate.service.register(input);
  assert.equal(duplicate.calls.some((c) => c.path.includes('new-user')), false);
});

test('endpoint público valida, limita frecuencia y no acepta rol administrativo', async () => {
  let creations = 0;
  const app = createApp({ config: { corsOrigins: [], registration: { allowHttp: true } },
    registrationService: { register: async () => { creations++; }, resend: async () => {} } });
  const server = createServer(app).listen(0, '127.0.0.1');
  await once(server, 'listening');
  const url = `http://127.0.0.1:${server.address().port}/api/v1/auth/register`;
  const send = (body) => fetch(url, { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(body) });
  try {
    assert.equal((await send({ ...input, role: 'administrador' })).status, 400);
    assert.equal((await send(input)).status, 202);
    assert.equal((await send(input)).status, 429);
    assert.equal(creations, 1);
  } finally { await new Promise((resolve) => server.close(resolve)); }
});

test('falta de credencial de servicio bloquea registro sin contactar Keycloak', async () => {
  const service = createRegistrationService({}, () => assert.fail('no network'));
  await assert.rejects(() => service.register(input), { status: 503 });
});

test('rol jugador compuesto bloquea creación; reenvío no altera cuentas ni crea tokens', async () => {
  const invalidRole = fixture({ composite: true });
  await assert.rejects(() => invalidRole.service.register(input), { status: 503 });
  assert.equal(invalidRole.calls.some((c) => c.path.endsWith('/users')), false);
  const eligible = fixture({ users: [{ id: 'existing', email: input.email, enabled: true, emailVerified: false }] });
  await eligible.service.resend({ email: input.email });
  assert.ok(eligible.calls.at(-1).path.endsWith('/existing/send-verify-email'));
  const verified = fixture({ users: [{ id: 'existing', email: input.email, enabled: true, emailVerified: true }] });
  await verified.service.resend({ email: input.email });
  assert.equal(verified.calls.some((c) => c.path.endsWith('/send-verify-email')), false);
  const missing = fixture();
  await missing.service.resend({ email: input.email });
  assert.equal(missing.calls.some((c) => c.path.endsWith('/send-verify-email')), false);
});

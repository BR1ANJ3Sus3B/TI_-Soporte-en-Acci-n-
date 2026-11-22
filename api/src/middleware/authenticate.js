import { createRemoteJWKSet, jwtVerify } from 'jose';
import { env, isAuthConfigured } from '../config/env.js';
import { ApiError } from '../errors.js';

const jwksCache = new Map();

function getJwks(config) {
  const url = config.keycloakJwksUri ||
    `${config.keycloakIssuer}/protocol/openid-connect/certs`;
  let jwks = jwksCache.get(url);
  if (!jwks) {
    jwks = createRemoteJWKSet(new URL(url));
    jwksCache.set(url, jwks);
  }
  return jwks;
}

export async function verifyAccessToken(token, config = env) {
  if (!isAuthConfigured(config)) {
    throw new ApiError(503, 'keycloak_not_configured', 'La validación OIDC no está configurada en la API.');
  }
  const { payload } = await jwtVerify(token, getJwks(config), {
    issuer: config.keycloakIssuer,
    audience: config.keycloakAudience,
    algorithms: ['RS256'],
  });
  if (typeof payload.sub !== 'string' || !payload.sub) {
    throw new ApiError(401, 'invalid_token', 'El access token no tiene un subject válido.');
  }
  return payload;
}

export function rolesFromClaims(claims, clientId = env.keycloakClientId) {
  const roles = new Set();
  const realmRoles = claims?.realm_access?.roles;
  if (Array.isArray(realmRoles)) realmRoles.filter((role) => typeof role === 'string').forEach((role) => roles.add(role));
  const clientRoles = claims?.resource_access?.[clientId]?.roles;
  if (Array.isArray(clientRoles)) clientRoles.filter((role) => typeof role === 'string').forEach((role) => roles.add(role));
  return [...roles];
}

export function createAuthenticate({ verifier = verifyAccessToken, config = env } = {}) {
  return async function authenticate(request, _response, next) {
    const match = /^Bearer\s+([^\s]+)$/i.exec(request.get('authorization') ?? '');
    if (!match) return next(new ApiError(401, 'missing_bearer_token', 'Envía el access token como Bearer token.'));
    try {
      const claims = await verifier(match[1], config);
      request.auth = {
        subject: claims.sub,
        claims,
        roles: rolesFromClaims(claims, config.keycloakClientId),
      };
      return next();
    } catch (error) {
      if (error instanceof ApiError) return next(error);
      if (error instanceof TypeError || error?.name === 'AbortError' || error?.code === 'ERR_JWKS_TIMEOUT') {
        return next(new ApiError(503, 'keycloak_unavailable', 'Keycloak no está disponible para validar el token.'));
      }
      return next(new ApiError(401, 'invalid_token', 'El access token es inválido, expiró o no corresponde a este issuer/audience.'));
    }
  };
}

export function requireRoles(...allowedRoles) {
  return function authorize(request, _response, next) {
    if (!request.auth?.roles?.some((role) => allowedRoles.includes(role))) {
      return next(new ApiError(403, 'forbidden', 'El rol de esta cuenta no permite realizar esta operación.'));
    }
    return next();
  };
}

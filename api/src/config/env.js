const csv = (value) => (value ?? '').split(',').map((item) => item.trim()).filter(Boolean);

export const env = Object.freeze({
  port: Number.parseInt(process.env.PORT ?? '3000', 10),
  mongoUri: process.env.MONGODB_URI ?? '',
  keycloakIssuer: (process.env.KEYCLOAK_ISSUER ?? '').replace(/\/$/, ''),
  keycloakAudience: csv(process.env.KEYCLOAK_AUDIENCE),
  keycloakJwksUri: process.env.KEYCLOAK_JWKS_URI ?? '',
  keycloakClientId: process.env.KEYCLOAK_CLIENT_ID ?? '',
  corsOrigins: csv(process.env.CORS_ORIGINS ?? 'http://localhost:5173,http://localhost:4173'),
  logLevel: process.env.LOG_LEVEL ?? 'info',
  metricsBearerToken: process.env.METRICS_BEARER_TOKEN ?? '',
  registration: {
    baseUrl: process.env.KEYCLOAK_ADMIN_BASE_URL ?? '',
    realm: process.env.KEYCLOAK_REGISTRATION_REALM ?? 'ti-soporte',
    clientId: process.env.KEYCLOAK_REGISTRATION_CLIENT_ID ?? '',
    clientSecret: process.env.KEYCLOAK_REGISTRATION_CLIENT_SECRET ?? '',
    allowHttp: process.env.REGISTRATION_ALLOW_HTTP === 'true',
  },
  // Set only to the trusted TLS reverse proxy IP/subnet, never arbitrary clients.
  trustedProxy: process.env.TRUSTED_PROXY ?? '',
});

export function isAuthConfigured(config = env) {
  return Boolean(config.keycloakIssuer && config.keycloakAudience.length);
}

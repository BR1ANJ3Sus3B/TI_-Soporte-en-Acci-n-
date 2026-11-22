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
});

export function isAuthConfigured(config = env) {
  return Boolean(config.keycloakIssuer && config.keycloakAudience.length);
}

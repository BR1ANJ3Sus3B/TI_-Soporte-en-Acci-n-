import client from '@prometheus-io/client';

export const registry = new client.Registry();
client.collectDefaultMetrics({ register: registry, prefix: 'ti_soporte_api_' });

export const httpRequests = new client.Counter({
  name: 'ti_soporte_api_http_requests_total',
  help: 'HTTP requests handled by the TI Soporte API.',
  labelNames: ['method', 'route', 'status'],
  registers: [registry],
});

export const httpDuration = new client.Histogram({
  name: 'ti_soporte_api_http_request_duration_seconds',
  help: 'HTTP request duration in seconds.',
  labelNames: ['method', 'route', 'status'],
  buckets: [0.01, 0.025, 0.05, 0.1, 0.25, 0.5, 1, 2.5, 5],
  registers: [registry],
});

export const missionAttemptReports = new client.Counter({
  name: 'ti_soporte_api_mission_attempt_reports_total',
  help: 'Mission attempt reports, including idempotent replays.',
  labelNames: ['outcome', 'status'],
  registers: [registry],
});

export const missionAttemptReviews = new client.Counter({
  name: 'ti_soporte_api_mission_attempt_reviews_total',
  help: 'Administrator reviews of mission attempts.',
  labelNames: ['decision'],
  registers: [registry],
});

export function observeHttpRequests(request, response, next) {
  const start = process.hrtime.bigint();
  response.on('finish', () => {
    const route = request.route?.path;
    const normalizedRoute = route
      ? `${request.baseUrl ?? ''}${route}`
      : request.path === '/metrics' ? '/metrics' : '/unmatched';
    const labels = {
      method: request.method,
      route: String(normalizedRoute),
      status: String(response.statusCode),
    };
    const duration = Number(process.hrtime.bigint() - start) / 1e9;
    httpRequests.inc(labels);
    httpDuration.observe(labels, duration);
  });
  next();
}

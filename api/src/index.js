const express = require('express');
const client = require('prom-client');

const app = express();
const port = Number(process.env.PORT || 4000);

client.collectDefaultMetrics({ prefix: 'ti_api_' });

const telemetryEvents = new client.Counter({
  name: 'client_telemetry_events_total',
  help: 'Eventos de telemetria recibidos desde los clientes.',
  labelNames: ['platform', 'name'],
});

const telemetryDuration = new client.Histogram({
  name: 'client_telemetry_event_duration_seconds',
  help: 'Duracion reportada por los clientes para sus eventos.',
  labelNames: ['platform', 'name'],
  buckets: [0.05, 0.1, 0.25, 0.5, 1, 2, 5, 10],
});

app.use(express.json({ limit: '64kb' }));

app.get('/health', (_request, response) => {
  response.json({ status: 'ok' });
});

app.post('/api/telemetry/events', (request, response) => {
  const { name, platform = 'flutter', durationMs } = request.body || {};

  if (typeof name !== 'string' || !/^[a-z0-9_.-]{1,80}$/i.test(name)) {
    return response.status(400).json({ error: 'name debe tener entre 1 y 80 caracteres alfanumericos, punto, guion o guion bajo.' });
  }

  if (typeof platform !== 'string' || !/^[a-z0-9_.-]{1,30}$/i.test(platform)) {
    return response.status(400).json({ error: 'platform no es valido.' });
  }

  const labels = { platform, name };
  telemetryEvents.inc(labels);

  if (Number.isFinite(durationMs) && durationMs >= 0) {
    telemetryDuration.observe(labels, durationMs / 1000);
  }

  return response.status(202).json({ accepted: true });
});

app.get('/metrics', async (_request, response, next) => {
  try {
    response.set('Content-Type', client.register.contentType);
    response.end(await client.register.metrics());
  } catch (error) {
    next(error);
  }
});

app.use((error, _request, response, _next) => {
  console.error(error);
  response.status(500).json({ error: 'Error interno.' });
});

app.listen(port, () => {
  console.log(`API de telemetria disponible en el puerto ${port}`);
});

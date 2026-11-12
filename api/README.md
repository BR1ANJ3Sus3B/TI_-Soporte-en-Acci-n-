# API y telemetria

La API recibe telemetria de Flutter en `POST /api/telemetry/events` y expone metricas Prometheus en `GET /metrics`.

Ejemplo de evento:

```json
{"name":"app.started","platform":"flutter","durationMs":120}
```

Al iniciar el entorno de monitoreo con Docker Compose, Prometheus consulta automaticamente `api:4000/metrics`.

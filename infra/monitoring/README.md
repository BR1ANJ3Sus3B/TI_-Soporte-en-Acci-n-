# Monitoreo local

Este entorno inicia Prometheus y Grafana para validar el monitoreo localmente.

```powershell
docker compose -f infra/monitoring/docker-compose.yml up -d
```

- Grafana: http://localhost:3000 (`admin` / `admin` por defecto).
- Prometheus: http://localhost:9090.
- Dashboard aprovisionado: **Monitoreo - Vista general**.

Para detener los servicios:

```powershell
docker compose -f infra/monitoring/docker-compose.yml down
```

Se pueden cambiar las credenciales antes de iniciar definiendo `GRAFANA_ADMIN_USER` y `GRAFANA_ADMIN_PASSWORD` en el entorno.

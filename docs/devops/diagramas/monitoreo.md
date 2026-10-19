# Diagrama — Monitoreo con Prometheus y Grafana

Este documento presenta la arquitectura de monitoreo propuesta para **TI: Soporte en Acción**.

> **Estado.** Prometheus y Grafana **no están implementados**. El siguiente diagrama es una **propuesta**. No existen datos ni umbrales calibrados.

## 1. Arquitectura de monitoreo

```mermaid
flowchart LR
    subgraph Fuentes["Fuentes de metricas"]
        API["API REST<br/>instrumentacion /metrics"]
        NODE["Host / contenedores<br/>node_exporter / cAdvisor"]
        DB["MongoDB<br/>exportador (propuesto)"]
        GH["GitHub Actions<br/>estado de workflows"]
        CLI["Clientes Unity / Flutter<br/>metricas propias hacia la API"]
    end
    API --> PROM["Prometheus<br/>scrape + almacenamiento"]
    NODE --> PROM
    DB --> PROM
    PROM --> GRAF["Grafana<br/>dashboards y alertas"]
    GH --> GRAF
    CLI --> API
    GRAF --> ALERT["Alertas<br/>al responsable del servicio"]
```

## 2. Flujo de recolección y alerta

```mermaid
sequenceDiagram
    autonumber
    participant SVC as Servicio
    participant EXP as Exportador / instrumentacion
    participant PROM as Prometheus
    participant GRAF as Grafana
    participant RES as Responsable del servicio

    EXP->>PROM: Expone metricas en /metrics
    loop Cada scrape_interval
        PROM->>EXP: Solicita metricas
        EXP-->>PROM: Devuelve series temporales
    end
    GRAF->>PROM: Consulta (PromQL)
    PROM-->>GRAF: Datos para los paneles
    GRAF->>RES: Alerta si se supera un umbral propuesto
    RES->>SVC: Revisa y aplica accion correctiva
```

## 3. Dashboards propuestos

| Dashboard | Panel | Métrica base | Estado |
| --- | --- | --- | --- |
| API — Rendimiento | Latencia p95 | `http_request_duration_seconds` | Propuesta |
| API — Salud | Disponibilidad y errores 5xx | `up`, `http_requests_total` | Propuesta |
| Infraestructura | CPU, RAM, disco | `node_*` | Propuesta |
| Base de datos | Conexiones | Métricas del driver | Propuesta |
| CI/CD | Estado de workflows | API de GitHub Actions | Propuesta |

## 4. Alertas propuestas

| Alerta | Condición propuesta | Prioridad |
| --- | --- | --- |
| API no disponible | `up == 0` durante 1 min | P1 |
| Latencia alta | p95 > 2000 ms durante 5 min | P2 |
| Errores 5xx altos | tasa > 1 % durante 5 min | P2 |
| Disco casi lleno | uso > 85 % | P3 |

## Notas

- Prometheus no recoge métricas de aplicaciones de escritorio o móviles de forma nativa; Unity y Flutter requieren **instrumentación propia** que las envíe a la API.
- El detalle de métricas y umbrales está en el [Documento 4](../04_Metricas_Monitoreo.md).
- La arquitectura general está en [`arquitectura.md`](arquitectura.md).

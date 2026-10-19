# Documento 4. Métricas para el monitoreo de la aplicación

## Introducción

Este documento define las métricas de monitoreo de **TI: Soporte en Acción** para sus cinco ámbitos: el videojuego (Unity), la aplicación móvil (Flutter), el backend (API), la infraestructura y la cadena CI/CD. Para cada métrica se indica descripción, unidad, método de medición, herramienta, umbral propuesto y acción correctiva.

> **Advertencia de alcance.** **Prometheus y Grafana no están implementados** en el repositorio; su uso es una **propuesta**. No existen datos, umbrales calibrados ni resultados de pruebas. Todos los valores son **umbrales propuestos**. Las métricas que requieren instrumentación (por ejemplo, exportadores de la API) solo podrán medirse cuando el componente exista.

## Convenciones

- **Umbral propuesto:** valor de referencia para generar una alerta; sujeto a validación.
- **Estado:** indica si la métrica puede medirse hoy o si depende de componentes pendientes.

---

## Métricas de Unity (videojuego)

| Métrica | Descripción | Unidad | Método de medición | Herramienta | Umbral propuesto | Acción correctiva |
| --- | --- | --- | --- | --- | --- | --- |
| FPS | Fotogramas por segundo durante el primer nivel | fps | Muestreo en tiempo de ejecución | Unity Profiler / Frame Debugger | ≥ 30 (RNF-05) | Optimizar sprites, animaciones y culling |
| Memoria | Uso de memoria del proceso del juego | MB | Muestreo de memoria gestionada y nativa | Unity Profiler | ≤ 1024 MB (propuesto) | Revisar fugas y recursos no liberados |
| Tiempo de carga | Tiempo desde el arranque hasta la escena jugable | s | Cronometraje de carga de escena | Unity Profiler | ≤ 15 s (propuesto) | Optimizar assets y carga asíncrona |
| Errores | Excepciones y errores en ejecución | conteo | Registro de errores | Consola de Unity / logs | 0 por sesión (propuesto) | Corregir la causa y añadir try/catch |
| Misiones completadas | Misiones finalizadas por sesión | conteo | Evento de juego al cerrar ticket | Instrumentación propia | Registrar (sin umbral) | Verificar lógica de cierre (RF-11) |
| Progreso | XP y avance del jugador registrados | puntos / % | Envío de resultado a la API | API + instrumentación | Registrar (sin umbral) | Revisar integración con la API (RF-13) |

---

## Métricas de Flutter (aplicación móvil)

> Componente **pendiente** (sin `pubspec.yaml` ni código). Las métricas podrán medirse cuando exista la aplicación.

| Métrica | Descripción | Unidad | Método de medición | Herramienta | Umbral propuesto | Acción correctiva |
| --- | --- | --- | --- | --- | --- | --- |
| Rendimiento de UI | Fotogramas descartados por renderizado lento | % | Perfil de rendimiento | Flutter DevTools | < 5 % (propuesto) | Reducir reconstrucciones y widgets pesados |
| Tiempo de inicio | Arranque en frío hasta la primera pantalla útil | s | Medición de *startup* | Flutter DevTools | ≤ 3 s (propuesto) | Diferir inicializaciones costosas |
| Errores | Excepciones capturadas | conteo | Registro de errores | Manejo de errores / logs | 0 por sesión (propuesto) | Corregir y reportar al servidor |
| Autenticación | Éxitos y fallos de inicio de sesión | % | Evento al autenticar | Instrumentación | Tasa de éxito ≥ 95 % (propuesto) | Revisar mensajes y servicio de acceso |
| Notificaciones | Notificaciones entregadas/procesadas | conteo | Registro de recepción | Instrumentación | Registrar (sin umbral) | Revisar permisos y configuración |
| Sincronización | Actualizaciones de progreso correctas | % | Comparación con la API | Instrumentación | ≥ 99 % (propuesto) | Reintentar y manejar pérdida de conexión (RNF-10) |

---

## Métricas de backend (API REST)

> Componente **pendiente** (sin `package.json` ni código). La instrumentación con Prometheus depende de que la API exista.

| Métrica | Descripción | Unidad | Método de medición | Herramienta | Umbral propuesto | Acción correctiva |
| --- | --- | --- | --- | --- | --- | --- |
| Latencia | Tiempo de respuesta por endpoint (p95) | ms | Histograma de duración HTTP | Prometheus (`http_request_duration_seconds`) | ≤ 2000 ms (RNF-04) | Optimizar consultas e índices |
| Disponibilidad | Proporción de tiempo con servicio activo | % | Sondeo de endpoint de salud | Prometheus (`up`) | ≥ 99.0 % (propuesto) | Revisar el proceso y reiniciar |
| Solicitudes | Número de peticiones recibidas | req/s | Contador de peticiones HTTP | Prometheus (`http_requests_total`) | Registrar (sin umbral) | Analizar picos y capacidad |
| Errores HTTP | Proporción de respuestas 5xx/4xx | % | Contador por código de estado | Prometheus | 5xx < 1 % (propuesto) | Investigar la causa del error |
| Conexiones | Conexiones activas a la base de datos | conteo | Métrica del driver de MongoDB | Driver / exportador | < 80 % del máximo (propuesto) | Revisar *pool* y consultas lentas |

---

## Métricas de infraestructura

> El entorno de producción **no está desplegado**. Estas métricas requieren `node_exporter` u otro exportador (propuesto).

| Métrica | Descripción | Unidad | Método de medición | Herramienta | Umbral propuesto | Acción correctiva |
| --- | --- | --- | --- | --- | --- | --- |
| CPU | Uso de CPU del host | % | Métrica del sistema | Prometheus (`node_cpu_seconds_total`) | < 80 % (propuesto) | Escalar o revisar procesos |
| RAM | Uso de memoria | % | Métrica del sistema | Prometheus (`node_memory_*`) | < 85 % (propuesto) | Revisar fugas y capacidad |
| Disco | Uso y espacio libre | % | Métrica del sistema | Prometheus (`node_filesystem_*`) | < 85 % (propuesto) | Limpiar o ampliar almacenamiento |
| Red | Tráfico entrante/saliente | bytes/s | Métrica del sistema | Prometheus (`node_network_*`) | Registrar (sin umbral) | Analizar picos y posibles ataques |
| Contenedores | Estado y reinicios de contenedores | conteo | Métrica de contenedores | cAdvisor (propuesto) | Reinicios = 0 (propuesto) | Revisar logs del contenedor |

---

## Métricas de CI/CD

> La cadena CI/CD es una **propuesta** (no existe `.github/workflows/`). Los datos provienen de GitHub Actions.

| Métrica | Descripción | Unidad | Método de medición | Herramienta | Umbral propuesto | Acción correctiva |
| --- | --- | --- | --- | --- | --- | --- |
| Compilaciones exitosas | Proporción de ejecuciones correctas | % | Historial de workflows | GitHub Actions | ≥ 95 % (propuesto) | Corregir causas de fallo |
| Frecuencia de despliegue | Liberaciones por periodo | despliegues/semana | Conteo de liberaciones | GitHub Releases/tags | Registrar (sin umbral) | Ajustar cadencia |
| Fallos | Ejecuciones fallidas | conteo | Historial de workflows | GitHub Actions | Tendencia a la baja | Revisar pruebas y *lint* |
| Recuperación | Tiempo de restauración tras fallo | min | Comparación de ejecuciones | GitHub Actions | ≤ 30 min (propuesto) | Automatizar reversión |

---

## Propuesta de dashboards con Prometheus y Grafana

> **Propuesta pendiente de implementación.** No existen archivos de configuración de Prometheus ni de Grafana en el repositorio. El diagrama de esta arquitectura de monitoreo está en [diagramas/monitoreo.md](diagramas/monitoreo.md).

### Arquitectura propuesta

```
Componentes → exportadores / instrumentación → Prometheus (scrape) → Grafana (visualización) → Alertas
```

### Paneles propuestos

| Dashboard | Panel | Métrica base |
| --- | --- | --- |
| API — Rendimiento | Latencia p95 por endpoint | `http_request_duration_seconds` |
| API — Salud | Disponibilidad y errores 5xx | `up`, `http_requests_total` |
| Infraestructura | CPU, RAM y disco | `node_cpu_seconds_total`, `node_memory_*`, `node_filesystem_*` |
| Base de datos | Conexiones y operaciones | métricas del driver de MongoDB |
| CI/CD | Estado de workflows | API de GitHub Actions |
| Clientes (futuro) | FPS, memoria, errores | Instrumentación de Unity/Flutter hacia la API |

### Alertas propuestas (ejemplos)

| Alerta | Condición propuesta | Prioridad |
| --- | --- | --- |
| API no disponible | `up == 0` durante 1 min | P1 |
| Latencia alta | p95 > 2000 ms durante 5 min | P2 |
| Disco casi lleno | uso > 85 % | P3 |
| Errores 5xx altos | tasa > 1 % durante 5 min | P2 |

## Consideraciones

- Las métricas de clientes (Unity/Flutter) requieren instrumentación **propia** hacia la API; Prometheus no las recoge de forma nativa desde aplicaciones de escritorio o móviles.
- No se debe presentar ningún valor como medido hasta que exista la instrumentación y datos reales.
- Los umbrales propuestos deberán recalibrarse tras las primeras mediciones.

## Referencias internas

- [Justificación](01_Justificacion_Liberacion_Despliegue.md)
- [Entorno y requerimientos](02_Entornos_Requerimientos.md)
- [Niveles de servicio (SLA)](03_Niveles_Servicio_SLA.md)
- [Parámetros de configuración](05_Parametros_Configuracion.md)
- [Diagrama de monitoreo](diagramas/monitoreo.md)

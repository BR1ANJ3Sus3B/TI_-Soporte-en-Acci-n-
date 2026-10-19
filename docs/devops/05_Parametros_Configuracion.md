# Documento 5. Parámetros de configuración de las herramientas utilizadas

## Introducción

Este documento reúne los parámetros de configuración de las herramientas del proyecto **TI: Soporte en Acción**. Para cada herramienta se describe: **propósito**, **versión detectada**, **archivo de configuración**, **parámetros importantes**, **variables de entorno**, **puertos**, **seguridad** e **integración con otros componentes**.

> **Criterio de veracidad.** Cuando una versión o configuración **no se encuentra en el repositorio**, se indica **Pendiente de confirmar**. No se inventan valores. Las secciones marcadas como *propuesta* corresponden a herramientas **no implementadas** (Docker, Prometheus, Grafana, Express, Flutter, Node.js).

---

## Unity

| Aspecto | Detalle |
| --- | --- |
| **Propósito** | Motor del videojuego 2D pixel art (personaje Alex, misiones y escenas). |
| **Versión detectada** | **6000.6.3f1** (Unity 6), verificada en `videojuego/ProjectSettings/ProjectVersion.txt`. |
| **Archivo de configuración** | `videojuego/ProjectSettings/*.asset`, `videojuego/Packages/manifest.json`. |
| **Parámetros importantes** | URP 2D (`com.unity.render-pipelines.universal` 17.6.0); *input* con `com.unity.inputsystem` 1.20.0; resolución y calidad en ProjectSettings. |
| **Variables de entorno** | No aplica (configuración por assets del proyecto). |
| **Puertos** | No aplica. |
| **Seguridad** | Artefactos generados excluidos (`Library/`, `Temp/`, `Logs/`); sin credenciales en el proyecto Unity. |
| **Integración** | Enviará resultados a la API REST (RF-13, RF-14) — **pendiente**. |

Script presente: `videojuego/Assets/Scripts/PlayerMovement.cs` (movimiento con WASD).

---

## Flutter

| Aspecto | Detalle |
| --- | --- |
| **Propósito** | Aplicación móvil de acompañamiento (misiones, guías, progreso, perfil). |
| **Versión detectada** | **Pendiente de confirmar** (no existe `pubspec.yaml` versionado). |
| **Archivo de configuración** | `movil/pubspec.yaml` — **Pendiente de confirmar** (no existe). |
| **Parámetros importantes** | Dependencias, *assets* (`assets/images`, `assets/icons`, `assets/fonts`) y configuración de build. |
| **Variables de entorno** | Propuesta: `API_BASE_URL` para apuntar a la API. |
| **Puertos** | No aplica (cliente). |
| **Seguridad** | Almacenamiento seguro de *token* de sesión (**propuesta**); sin credenciales de base de datos (RNF-14). |
| **Integración** | Consumo de la API REST mediante HTTPS. |

Estructura prevista presente con `.gitkeep` en `movil/lib/core/**` y `movil/lib/features/**` (solo carpetas).

---

## Dart

| Aspecto | Detalle |
| --- | --- |
| **Propósito** | Lenguaje de la aplicación móvil Flutter. |
| **Versión detectada** | **Pendiente de confirmar**. |
| **Archivo de configuración** | Definida por el SDK (nuevamente requiere `pubspec.yaml`). |
| **Parámetros importantes** | Restricción de SDK (`environment: sdk`) — pendiente. |
| **Variables de entorno** | No aplica. |
| **Puertos** | No aplica. |
| **Seguridad** | Análisis estático (`dart analyze`) propuesto en CI. |
| **Integración** | Con Flutter. |

---

## Node.js

| Aspecto | Detalle |
| --- | --- |
| **Propósito** | Entorno de ejecución del backend (API REST). |
| **Versión detectada** | **Pendiente de confirmar** (no existe `package.json` ni `.nvmrc`). |
| **Archivo de configuración** | `api/package.json` — **Pendiente de confirmar**. |
| **Parámetros importantes** | Scripts (`start`, `dev`, `test`), dependencias y versión de Node propuestas. |
| **Variables de entorno** | `PORT`, `NODE_ENV`, `MONGODB_URI`, `JWT_SECRET` (parcialmente en `api/.env.example`). |
| **Puertos** | `4000` (**verificado** en `api/.env.example`). |
| **Seguridad** | No exponer secretos; validar entradas; rate limiting (**propuesto**). |
| **Integración** | Base de la API con Express y Mongoose; consumido por Unity, Flutter y React. |

---

## Express

| Aspecto | Detalle |
| --- | --- |
| **Propósito** | Framework HTTP para la API REST. |
| **Versión detectada** | **Pendiente de confirmar** (no implementado). |
| **Archivo de configuración** | Código en `api/src/` — **Pendiente de confirmar**. |
| **Parámetros importantes** | Middlewares (CORS, JSON, autenticación, errores); rutas en `api/src/routes/`. |
| **Variables de entorno** | `PORT`, `CORS_ORIGIN` (propuesta), `JWT_SECRET`. |
| **Puertos** | `4000` (verificado como propuesta en `.env.example`). |
| **Seguridad** | Autorización en el servidor (RNF-03); HTTPS en producción (RNF-02). |
| **Integración** | MongoDB vía Mongoose; consumido por los tres clientes. |

Estructura prevista presente con `.gitkeep`: `api/src/{config,controllers,middlewares,models,routes,services,validators}` y `api/tests/`.

---

## Git

| Aspecto | Detalle |
| --- | --- |
| **Propósito** | Control de versiones del monorepositorio. |
| **Versión detectada** | **Pendiente de confirmar** (herramienta en uso; versión local no registrada). |
| **Archivo de configuración** | `.gitignore` (raíz), `videojuego/.gitignore`, `.gitattributes` (Unity). |
| **Parámetros importantes** | Ramas `main`, `develop`, `feature/*`; convención de mensajes (`feat:`, `fix:`, `docs:`, `chore:`). |
| **Variables de entorno** | No aplica. |
| **Puertos** | No aplica. |
| **Seguridad** | Exclusión de `.env` y artefactos generados. |
| **Integración** | Base del flujo CI/CD en GitHub. |

---

## GitHub

| Aspecto | Detalle |
| --- | --- |
| **Propósito** | Repositorio remoto, colaboración y automatización. |
| **Versión detectada** | No aplica (servicio). |
| **Archivo de configuración** | Remoto `origin` = `https://github.com/BR1ANJ3Sus3B/TI_-Soporte-en-Acci-n-.git`. |
| **Parámetros importantes** | Rama por defecto `main`; ramas `develop` y `feature/*`; *pull requests*. |
| **Variables de entorno** | Secretos en *GitHub Secrets* (**propuesto**). |
| **Puertos** | No aplica (HTTPS 443). |
| **Seguridad** | No versionar secretos; protección de `main` (**propuesta**). |
| **Integración** | Origen de los workflows de GitHub Actions. |

---

## GitHub Actions

| Aspecto | Detalle |
| --- | --- |
| **Propósito** | Integración y entrega continuas. |
| **Versión detectada** | **Pendiente de confirmar** (no existe `.github/workflows/`). |
| **Archivo de configuración** | `.github/workflows/*.yml` — **Pendiente de confirmar**. |
| **Parámetros importantes** | Disparadores (`push`, `pull_request`), filtros `paths`, *matrix* por componente, artefactos. |
| **Variables de entorno** | `secrets.*` para datos sensibles. |
| **Puertos** | No aplica. |
| **Seguridad** | Uso de secretos cifrados; permisos mínimos del token. |
| **Integración** | Compila Unity, Flutter, React y la API; publica artefactos. |

---

## Docker

| Aspecto | Detalle |
| --- | --- |
| **Propósito** | Empaquetar y ejecutar servicios en contenedores (API, PWA). |
| **Versión detectada** | **Pendiente de confirmar** (no existe `Dockerfile`). |
| **Archivo de configuración** | `Dockerfile` por servicio — **Pendiente de confirmar**. |
| **Parámetros importantes** | Imagen base, `WORKDIR`, `EXPOSE`, `ENV`, *multi-stage*. |
| **Variables de entorno** | Inyectadas en ejecución (no horneadas en la imagen). |
| **Puertos** | API `4000`; MongoDB `27017`. |
| **Seguridad** | Ejecutar como usuario no root; no incluir secretos en la imagen. |
| **Integración** | Base de Docker Compose. |

---

## Docker Compose

| Aspecto | Detalle |
| --- | --- |
| **Propósito** | Orquestar varios servicios (API, MongoDB y, opcionalmente, monitoreo). |
| **Versión detectada** | **Pendiente de confirmar** (no existe `docker-compose.yml`). |
| **Archivo de configuración** | `docker-compose.yml` — **Pendiente de confirmar**. |
| **Parámetros importantes** | `services`, `networks`, `volumes`, `depends_on`, `environment`. |
| **Variables de entorno** | Desde archivo `.env` no versionado. |
| **Puertos** | Publicar solo los necesarios (API `4000`; base de datos en red interna). |
| **Seguridad** | Base de datos no expuesta al exterior; red interna dedicada. |
| **Integración** | Reproduce el entorno de pruebas/prueba local. |

---

## Base de datos (MongoDB)

| Aspecto | Detalle |
| --- | --- |
| **Propósito** | Persistencia de usuarios, misiones, intentos, progreso y guías. |
| **Versión detectada** | **Pendiente de confirmar** (no se especifica versión). |
| **Archivo de configuración** | `database/README.md` (modelo); conexión en `api/src/config/database.js` — **Pendiente**. |
| **Parámetros importantes** | Nombre de base `ti_soporte_en_accion`; índices únicos en `nombreUsuario` y `correo`. |
| **Variables de entorno** | `MONGODB_URI=mongodb://127.0.0.1:27017/ti_soporte_en_accion` (verificado en `.env.example`). |
| **Puertos** | `27017` (por defecto). |
| **Seguridad** | Acceso solo desde la API (RNF-14); contraseñas con hash (RNF-01); respaldo diario (RNF-13). |
| **Integración** | Únicamente a través de la API; nunca desde los clientes. |

---

## Prometheus

| Aspecto | Detalle |
| --- | --- |
| **Propósito** | Recolectar y almacenar métricas de series temporales. |
| **Versión detectada** | **Pendiente de confirmar** (no implementado). |
| **Archivo de configuración** | `prometheus.yml` — **Pendiente de confirmar**. |
| **Parámetros importantes** | `scrape_configs`, `scrape_interval`, `targets`. |
| **Variables de entorno** | No aplica. |
| **Puertos** | `9090` (por defecto). |
| **Seguridad** | Interfaz no expuesta públicamente (**propuesta**). |
| **Integración** | Alimenta los dashboards de Grafana. |

---

## Grafana

| Aspecto | Detalle |
| --- | --- |
| **Propósito** | Visualizar métricas y gestionar alertas. |
| **Versión detectada** | **Pendiente de confirmar** (no implementado). |
| **Archivo de configuración** | *Datasources* y dashboards — **Pendiente de confirmar**. |
| **Parámetros importantes** | Fuente de datos (Prometheus), paneles y alertas. |
| **Variables de entorno** | Credenciales de administrador (no versionar). |
| **Puertos** | `3000` (por defecto). |
| **Seguridad** | Cambiar credenciales por defecto; acceso restringido. |
| **Integración** | Consulta a Prometheus; alertas hacia el responsable del servicio. |

---

## Autenticación

| Aspecto | Detalle |
| --- | --- |
| **Propósito** | Identificar y autorizar a jugadores y administradores. |
| **Versión/Tecnología detectada** | Basada en tokens (`JWT_SECRET` en `api/.env.example`); **implementación pendiente**. |
| **Archivo de configuración** | `api/src/middlewares/` (previsto) — **Pendiente**. |
| **Parámetros importantes** | Firma y expiración del token; roles `jugador` y `administrador`. |
| **Variables de entorno** | `JWT_SECRET` (**placeholder** `REEMPLAZAR_POR_UN_SECRETO_SEGURO`). |
| **Puertos** | No aplica. |
| **Seguridad** | Contraseñas con hash y *salt* (RNF-01); autorización en el servidor (RNF-03). |
| **Integración** | Usada por Unity, Flutter y React a través de la API. |

---

## API REST

| Aspecto | Detalle |
| --- | --- |
| **Propósito** | Único punto de acceso a los datos compartidos. |
| **Versión detectada** | **Pendiente de confirmar** (sin implementación). |
| **Archivo de configuración** | `api/src/routes/`, `api/src/controllers/` (previstos) — **Pendiente**. |
| **Parámetros importantes** | Operaciones mínimas: registro/acceso, perfil, catálogo, intentos, progreso, administración (documentadas en `docs/proyecto/README.md`). |
| **Variables de entorno** | `PORT` (`4000`), `MONGODB_URI`, `JWT_SECRET`. |
| **Puertos** | `4000` (verificado en `.env.example`). |
| **Seguridad** | HTTPS (RNF-02); autorización por rol y propiedad (RNF-03); validación de entradas. |
| **Integración** | Unity, Flutter y React la consumen; conversa con MongoDB. |

---

## Resumen de puertos

| Servicio | Puerto | Estado |
| --- | --- | --- |
| API REST | `4000` | Verificado (`.env.example`) |
| MongoDB | `27017` | Por defecto (propuesto) |
| Prometheus | `9090` | Propuesto |
| Grafana | `3000` | Propuesto |

## Referencias internas

- [Justificación](01_Justificacion_Liberacion_Despliegue.md)
- [Entorno y requerimientos](02_Entornos_Requerimientos.md)
- [Niveles de servicio (SLA)](03_Niveles_Servicio_SLA.md)
- [Métricas de monitoreo](04_Metricas_Monitoreo.md)

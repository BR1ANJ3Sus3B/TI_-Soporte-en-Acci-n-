# Documento 2. Entorno y requerimientos para la liberación y despliegue continuo

## Introducción

Este documento describe los entornos de **desarrollo**, **pruebas** y **producción** del proyecto **TI: Soporte en Acción**, junto con los requerimientos de hardware, software, dependencias, red y seguridad necesarios para la liberación y el despliegue continuo. Se distingue entre lo verificado en el repositorio y lo **propuesto**.

> **Advertencia.** El entorno de producción **no está desplegado**. Las versiones no verificadas se señalan como **Pendiente de confirmar**.

## Entorno de desarrollo

Entorno local del responsable único del proyecto.

| Elemento | Detalle | Estado |
| --- | --- | --- |
| Sistema operativo | Windows (plataforma de Unity según RNF-07) | Verificado |
| Editor de videojuego | Unity 6000.6.3f1 (Unity 6) con URP 2D 17.6.0 | Verificado en `videojuego/ProjectSettings/ProjectVersion.txt` y `Packages/manifest.json` |
| SDK móvil | Flutter + Dart | Pendiente de confirmar (sin `pubspec.yaml`) |
| Entorno web | Node.js + npm/Vite | Pendiente de confirmar (sin `package.json`) |
| Editor de código | Visual Studio / Rider / VS Code (según `manifest.json`) | Verificado como dependencia de Unity |
| Control de versiones | Git + GitHub | Verificado |

Características:
- Cada componente se edita en su carpeta (`videojuego/`, `movil/`, `web/`, `api/`).
- Los archivos generados (`Library/`, `build/`, `.dart_tool/`, `node_modules/`) no se versionan.
- El archivo `.env` real nunca se sube al repositorio; se parte de `api/.env.example`.

## Entorno de pruebas

Entorno donde se verifican los cambios antes de integrarlos en `main`.

| Elemento | Propuesta |
| --- | --- |
| Ejecución | Pipeline de GitHub Actions sobre runners alojados en la nube |
| Base de datos de prueba | Instancia MongoDB local o contenedor efímero |
| Datos | Datos de ejemplo para desarrollo (`database/seeds/`, **pendiente**) |
| Pruebas | Unitarias, de integración y de endpoints (propuestas) |
| Hardware de referencia | Equipo de referencia por documentar (RNF-05) |

> El entorno de pruebas **no existe todavía**: requiere que primero se configuren el proyecto Flutter, el proyecto React, la API y los workflows.

## Entorno de producción

Entorno previsto para usuarios finales. **No desplegado.**

| Componente | Distribución propuesta |
| --- | --- |
| API REST | Servidor con Node.js en ejecución gestionada (contenedor propuesto) |
| Base de datos | MongoDB accesible **solo** desde el servidor (nunca desde los clientes) |
| PWA | Servida por HTTPS (hosting estático propuesto) |
| Unity | Ejecutable de Windows distribuido a jugadores |
| Flutter | APK de Android distribuido a jugadores |

Requisitos de operación propuestos:
- HTTPS obligatorio (RNF-02).
- Secretos fuera del repositorio.
- Respaldo diario durante las pruebas con datos persistentes (RNF-13).

## Requerimientos de hardware

| Entorno | CPU | RAM | Disco | Notas |
| --- | --- | --- | --- | --- |
| Desarrollo (Unity) | 4 núcleos o más (propuesto) | 8 GB mínimo (propuesto) | 20 GB libres | Requisitos recomendados de Unity; confirmar con el editor |
| Pruebas (CI) | Runner estándar de GitHub Actions | 7 GB (runner estándar) | 14 GB SSD | Valor del runner alojado |
| Producción (API) | 1–2 vCPU (propuesto) | 1–2 GB (propuesto) | 10 GB (propuesto) | Según hosting elegido |
| Equipo de referencia (RNF-05) | Pendiente de confirmar | Pendiente de confirmar | Pendiente de confirmar | RNF-05 exige documentar el equipo de la prueba de FPS |

## Requerimientos de software

| Software | Versión | Estado |
| --- | --- | --- |
| Unity Editor | 6000.6.3f1 | Verificado |
| Universal Render Pipeline (URP) | 17.6.0 | Verificado |
| Unity Test Framework | 1.8.0 | Verificado |
| Flutter / Dart | Pendiente de confirmar | Sin proyecto Flutter |
| Node.js / npm | Pendiente de confirmar | Sin `package.json` |
| Express | Pendiente de confirmar | Propuesta del servidor |
| MongoDB / Mongoose | Pendiente de confirmar | Modelo documentado |
| Git | Pendiente de confirmar | Herramienta en uso |
| Docker / Docker Compose | Pendiente de confirmar | No instalado/definido en el repo |

## Dependencias

**Verificadas (Unity, en `videojuego/Packages/manifest.json`):** `com.unity.2d.animation` 16.0.1, `com.unity.2d.tilemap` 1.0.0, `com.unity.render-pipelines.universal` 17.6.0, `com.unity.inputsystem` 1.20.0, `com.unity.test-framework` 1.8.0, entre otros módulos.

**Propuestas (pendientes):**
- API: `express`, `mongoose`, `dotenv`, `jsonwebtoken`, `bcrypt` (o equivalente), y herramientas de prueba.
- Flutter: dependencias de `pubspec.yaml` (**pendiente**).
- PWA: React, Vite y dependencias de `package.json` (**pendiente**).

## Requerimientos de red

| Requisito | Detalle |
| --- | --- |
| HTTPS | Obligatorio para la API y la PWA en producción (RNF-02) |
| Acceso a la base de datos | Restringido al servidor de la API (RNF-14) |
| CORS | Configurado para permitir solo los orígenes de la PWA (propuesto) |
| Puertos (locales/previstos) | API `4000` (verificado en `api/.env.example`); MongoDB `27017` |
| Resolución DNS / conectividad | Necesaria en el pipeline para descargar dependencias |

## Requerimientos de seguridad

| Requisito | Origen | Estado |
| --- | --- | --- |
| Contraseñas con hash y salt | RNF-01 | Pendiente de implementar |
| Comunicaciones por HTTPS | RNF-02 | Pendiente |
| Autorización en el servidor | RNF-03 | Pendiente |
| Secretos fuera del repositorio (`.env`, secretos de GitHub) | RNF-02 / RNF-14 | Parcial (existe `.env.example`) |
| Índices únicos en `nombreUsuario` y `correo` | `database/README.md` | Pendiente |
| `JWT_SECRET` seguro (placeholder `REEMPLAZAR_POR_UN_SECRETO_SEGURO`) | `api/.env.example` | Pendiente de reemplazar |

## GitHub Actions

**Estado:** *propuesta pendiente de implementación.* No existe `.github/workflows/`.

Configuración propuesta:
- Carpeta `.github/workflows/`.
- Un flujo por componente con detección de cambios por ruta (matrix o filtros `paths`).
- Almacenamiento de secretos con *GitHub Secrets* (nunca en el YAML).
- Ejecución en `push` y `pull_request` hacia `develop` y `main`.
- Artefactos publicados y versionados por liberación.

## Docker

**Estado:** *propuesta pendiente de implementación.* No existen `Dockerfile` ni `docker-compose.yml`.

Propuesta:
- `Dockerfile` para la API (Node.js).
- `Dockerfile` para la PWA (build estático + servidor).
- `docker-compose.yml` con los servicios `api`, `mongodb` y, opcionalmente, `prometheus` y `grafana`.
- Red interna para aislar la base de datos.
- Variables de entorno mediante archivo `.env` no versionado.

## Configuración de variables de entorno

Partiendo de `api/.env.example` (**verificado**):

| Variable | Ejemplo | Descripción | Sensible |
| --- | --- | --- | --- |
| `PORT` | `4000` | Puerto de la API | No |
| `MONGODB_URI` | `mongodb://127.0.0.1:27017/ti_soporte_en_accion` | Cadena de conexión a MongoDB | **Sí** |
| `JWT_SECRET` | `REEMPLAZAR_POR_UN_SECRETO_SEGURO` | Clave de firma de tokens | **Sí** |

Propuestas adicionales (pendientes): `NODE_ENV`, `CORS_ORIGIN`, `API_BASE_URL` (clientes), `LOG_LEVEL`.

## Proceso de liberación

1. Crear una rama `feature/*` o `fix/*` desde `develop`.
2. Desarrollar y confirmar cambios con mensajes convencionales.
3. Abrir una *pull request* hacia `develop`; se ejecuta el pipeline de CI.
4. Fusionar en `develop` si el pipeline está en verde.
5. Promover `develop` a `main` mediante *pull request* cuando la versión sea estable.
6. Generar artefactos versionados y aplicar la etiqueta `vX.Y.Z`.
7. Publicar/desplegar según el componente.

Véase el flujo en [diagramas/pipeline_cicd.md](diagramas/pipeline_cicd.md).

## Validaciones

| Momento | Validación propuesta |
| --- | --- |
| Compilación | El componente compila sin errores |
| Análisis estático | Sin errores de *lint* |
| Pruebas | Pruebas automatizadas en verde |
| Empaquetado | Se genera el artefacto esperado |
| Enlace de servicios | La API responde en el entorno de pruebas |
| Seguridad | Sin secretos en el repositorio |

## Criterios de aceptación

- El pipeline se ejecuta automáticamente en cada *pull request* hacia `develop`.
- Ningún cambio llega a `main` sin el pipeline en verde.
- Cada liberación produce un artefacto identificable por versión.
- Ningún secreto aparece en el repositorio ni en los registros.
- Un respaldo permite restaurar usuarios, misiones y progreso (RNF-13).
- La reversión a la versión anterior es posible por componente.

## Referencias internas

- [Justificación](01_Justificacion_Liberacion_Despliegue.md)
- [Niveles de servicio (SLA)](03_Niveles_Servicio_SLA.md)
- [Métricas de monitoreo](04_Metricas_Monitoreo.md)
- [Parámetros de configuración](05_Parametros_Configuracion.md)

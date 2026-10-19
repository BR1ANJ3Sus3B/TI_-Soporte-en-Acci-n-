# Documentación DevOps — TI: Soporte en Acción

## Presentación

Este apartado reúne la documentación de **DevOps** del proyecto integrador **TI: Soporte en Acción**: un videojuego educativo 2D en Unity, una aplicación móvil en Flutter y una PWA en React que comparten servicios a través de una API REST con base de datos MongoDB.

La documentación describe cómo se **planifica** la liberación y el despliegue continuo, los entornos y sus requerimientos, los niveles de servicio, las métricas de monitoreo y los parámetros de configuración de las herramientas del proyecto.

> **Advertencia de alcance.** El repositorio se encuentra en una etapa de **diseño y alcance**. Salvo Unity (proyecto real con un script) y Git/GitHub, los componentes de aplicación y toda la infraestructura DevOps (CI/CD, Docker, Prometheus, Grafana) están **pendientes de implementación**. Este documento presenta **propuestas** y **objetivos propuestos**, no resultados verificados. No se registran métricas reales ni resultados de pruebas porque aún no se han ejecutado.

## Objetivo

Definir el marco de trabajo DevOps del proyecto para:

- Establecer la justificación técnica y académica de la integración y la entrega continuas.
- Describir los entornos de desarrollo, pruebas y producción y sus requerimientos.
- Acordar niveles de servicio (SLA/SLO/SLI) para los componentes previstos.
- Identificar las métricas de monitoreo y una propuesta de dashboards con Prometheus y Grafana.
- Documentar los parámetros de configuración de las herramientas utilizadas.

## Tecnologías utilizadas

| Componente | Tecnología | Estado en el repositorio |
| --- | --- | --- |
| Videojuego | Unity + C# | Implementado parcialmente (1 script: `PlayerMovement.cs`) |
| Aplicación móvil | Flutter + Dart | Pendiente (solo estructura de carpetas) |
| PWA | React + Vite | Pendiente (solo estructura de carpetas) |
| Backend | Node.js + Express + Mongoose | Pendiente (solo `.env.example`) |
| Base de datos | MongoDB | Pendiente (modelo documentado) |
| Control de versiones | Git + GitHub | Implementado (ramas `main`, `develop`, `feature/*`) |
| CI/CD | GitHub Actions | Pendiente (propuesta) |
| Contenedores | Docker + Docker Compose | Pendiente (propuesta) |
| Monitoreo | Prometheus + Grafana | Pendiente (propuesta) |

## Índice de documentos

1. [Justificación del trabajo para la liberación y despliegue continuo](01_Justificacion_Liberacion_Despliegue.md)
2. [Entorno y requerimientos para la liberación y despliegue continuo](02_Entornos_Requerimientos.md)
3. [Niveles de servicio acordados (SLA)](03_Niveles_Servicio_SLA.md)
4. [Métricas para el monitoreo de la aplicación](04_Metricas_Monitoreo.md)
5. [Parámetros de configuración de las herramientas utilizadas](05_Parametros_Configuracion.md)

## Diagramas

![Arquitectura propuesta de TI: Soporte en Acción con Unity, React, Flutter, MongoDB y monitoreo con Grafana](<../imagenes/Arquitectura TI educativa con Grafana.png>)

Vista conceptual de la plataforma y sus servicios de identidad, datos y monitoreo. Consulta el [detalle de arquitectura y alcance de la ilustración](diagramas/arquitectura.md).

- [Arquitectura Unity + Flutter + API REST](diagramas/arquitectura.md)
- [Pipeline CI/CD y ambientes](diagramas/pipeline_cicd.md)
- [Monitoreo con Prometheus y Grafana](diagramas/monitoreo.md)

## Estado de implementación

| Elemento | Estado | Observación |
| --- | --- | --- |
| Repositorio monorepo | Implementado | Carpetas `videojuego/`, `movil/`, `web/`, `api/`, `database/`, `docs/` |
| Estrategia de ramas | Implementado | `main`, `develop`, `feature/*`, convención `fix/*` |
| Unity (videojuego) | Implementado parcial | Proyecto Unity 6 (6000.6.3f1), URP 2D, `PlayerMovement.cs` |
| App móvil Flutter | Pendiente | Sin `pubspec.yaml` ni código en `lib/` |
| PWA React | Pendiente | Sin `package.json` ni código en `src/` |
| API REST | Pendiente | Sin `package.json` ni código en `api/src/` |
| MongoDB | Pendiente | Solo modelo previsto en `database/README.md` |
| Workflows de GitHub Actions | Pendiente | No existe `.github/workflows/` |
| Docker / Docker Compose | Pendiente | No existen `Dockerfile` ni `docker-compose.yml` |
| Prometheus / Grafana | Pendiente | No existen archivos de configuración |

## Recomendaciones técnicas

1. **Implementar primero los servicios compartidos** (API + MongoDB), ya que bloquean la integración de Unity, Flutter y React.
2. **Crear la carpeta `.github/workflows/`** con pipelines de lint, build y pruebas por componente antes de automatizar la entrega.
3. **Añadir un `package.json` verificado** en `api/` y `web/` y un `pubspec.yaml` en `movil/`; los pipelines no pueden ejecutarse sin ellos.
4. **Contenerizar la API y la base de datos** con Docker Compose para reproducir el entorno de pruebas.
5. **Instrumentar métricas** con Prometheus en la API y en la infraestructura; Grafana solo debe emplearse con datos reales, no simulados.
6. **No registrar secretos** en el repositorio: usar variables de entorno y secretos de GitHub (ver `api/.env.example`).
7. **Actualizar este apartado** marcando cada propuesta como *aprobada* o *implementada* cuando exista evidencia en el repositorio.

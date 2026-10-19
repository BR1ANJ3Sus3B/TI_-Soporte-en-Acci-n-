# Documento 1. Justificación del trabajo para la liberación y despliegue continuo

## Introducción

**TI: Soporte en Acción** es un proyecto integrador compuesto por tres productos relacionados —un videojuego en Unity y C#, una aplicación móvil en Flutter y Dart y una PWA en React— que comparten servicios a través de una API REST con base de datos MongoDB. La convivencia de varios componentes con ciclos de vida distintos (un ejecutable de escritorio, una aplicación móvil y una aplicación web) exige un método de trabajo que permita integrar los cambios de forma ordenada, verificable y repetible.

El presente documento justifica la adopción de prácticas DevOps —integración continua (CI), entrega continua (CD), control de versiones, automatización de pruebas y procedimientos de reversión— y describe cómo se aplicarán al proyecto. Se distingue en todo momento entre lo que el repositorio **ya implementa** (el monorepositorio y la estrategia de ramas con Git) y lo que constituye una **propuesta pendiente de implementación** (los pipelines de GitHub Actions, los contenedores y el monitoreo).

## Problemática

El proyecto presenta las siguientes dificultades que motivan el trabajo DevOps:

1. **Múltiples componentes y lenguajes.** Unity (C#), Flutter (Dart) y React/Node (JavaScript) requieren entornos y compiladores distintos; sin automatización, cada liberación depende de procedimientos manuales propensos a error.
2. **Dependencia de servicios compartidos.** Los tres clientes dependen de la misma API y base de datos. Un cambio incompatible en la API puede romper los tres productos a la vez.
3. **Generación de builds no trivial.** Unity y Flutter producen artefactos pesados que no conviene versionar en el repositorio, pero sí distribuir de forma identificable.
4. **Responsable único.** El proyecto es desarrollado por una sola persona; la automatización reduce la carga operativa y la posibilidad de omitir pasos.
5. **Riesgo de inconsistencias.** Sin control de calidad automatizado, la verificación de requisitos (RF-01 a RF-21, RNF-01 a RNF-14) queda sujeta a revisión manual.
6. **Ausencia actual de infraestructura DevOps.** En el repositorio no existen workflows, `Dockerfile`, `docker-compose.yml` ni configuración de monitoreo; esta documentación define lo que debe construirse.

## Justificación técnica

La liberación y el despliegue continuos aportan ventajas concretas al proyecto:

- **Detección temprana de errores.** La integración frecuente de ramas `feature/*` en `develop` revela incompatibilidades antes de llegar a `main`.
- **Reproducibilidad.** Un pipeline describe, de forma explícita y versionada, cómo se compila y prueba cada componente.
- **Trazabilidad.** Cada artefacto queda asociado a un commit, una rama y una versión identificable.
- **Reducción del error humano.** Los pasos repetitivos (compilar, empaquetar, publicar) se automatizan.
- **Consistencia entre componentes.** Al construir y probar los tres clientes y la API de la misma manera, se reduce la divergencia de comportamiento.

## Justificación académica

El proyecto integra tres asignaturas (Creación de Videojuegos, Desarrollo Móvil Integral y Aplicaciones Web Progresivas). Documentar y justificar un proceso DevOps permite:

- Demostrar el dominio de **control de versiones, integración y entrega continuas**, competencias propias de la ingeniería de software.
- Sustentar con criterios técnicos la **organización del trabajo individual** del responsable del proyecto.
- Producir **evidencia evaluable** (pipelines, builds, registros) alineada con los criterios de aceptación del alcance inicial.
- Relacionar los requerimientos no funcionales (RNF-01 a RNF-14) con mecanismos verificables de la cadena de entrega.

## Objetivo general

Establecer un proceso de liberación y despliegue continuo que permita integrar, verificar y publicar de forma automatizada los componentes del proyecto (Unity, Flutter, React y la API), reduciendo el error manual y garantizando trazabilidad y reproducibilidad.

## Objetivos específicos

1. Definir una estrategia de ramas coherente con el flujo `rama de trabajo → develop → main` ya documentado.
2. Diseñar pipelines de integración continua que compilen y prueben cada componente.
3. Automatizar la generación de artefactos de Unity y Flutter con una versión identificable.
4. Definir un esquema de versionamiento aplicable a los tres productos y a la API.
5. Establecer procedimientos de reversión (rollback) para cada componente.
6. Documentar los entornos, niveles de servicio y métricas asociados.

## Beneficios de DevOps

| Beneficio | Aplicación en el proyecto |
| --- | --- |
| Integración frecuente | Fusionar ramas `feature/*` en `develop` con verificación automática. |
| Retroalimentación rápida | Resultado del pipeline por cada cambio propuesto. |
| Menor esfuerzo de liberación | Artefactos generados de forma automática. |
| Trazabilidad | Vinculación entre commit, build y versión. |
| Calidad sostenida | Pruebas automatizadas y revisión antes de `main`. |
| Reproducibilidad | Entornos definidos por configuración (Docker, propuesto). |

## Integración continua (CI)

**Definición propuesta.** Cada vez que se realiza un *push* o una *pull request* hacia `develop` o `main`, se ejecutará un flujo que:

1. Obtiene el código del repositorio.
2. Prepara el entorno correspondiente al componente modificado.
3. Ejecuta análisis estático (*lint*) y pruebas automatizadas.
4. Compila el componente para comprobar que la integración no rompe la construcción.

**Estado.** *Propuesta pendiente de implementación.* No existe `.github/workflows/` en el repositorio. La CI se ilustra en [diagramas/pipeline_cicd.md](diagramas/pipeline_cicd.md).

## Entrega y despliegue continuo (CD)

**Entrega continua (propuesta).** Al integrar cambios en `main`, el pipeline generará artefactos versionados (build de Unity, APK de Flutter, bundle de la PWA) listos para publicar, quedando disponibles como artefactos del flujo para su revisión manual.

**Despliegue continuo (propuesta).** El despliegue a producción de la API y la PWA podrá automatizarse una vez que existan los servicios y el entorno de producción. El videojuego y la aplicación móvil se distribuirán como artefactos; su publicación en tiendas queda fuera del alcance del prototipo académico.

> *Nota.* No se asume que el despliegue continuo esté activo: **el entorno de producción no está desplegado** en el repositorio actual.

## Estrategia de ramas

Se mantiene la estrategia ya documentada en `README.md`:

| Rama | Propósito | Integración |
| --- | --- | --- |
| `main` | Versión estable y publicable | Recibe solo lo estable desde `develop` |
| `develop` | Integración del trabajo en curso | Recibe ramas `feature/*` y `fix/*` |
| `feature/*` | Desarrollo de una funcionalidad | Se crea desde `develop` |
| `fix/*` | Corrección (`fix/descripcion-del-error`) | Se crea desde `develop` |
| `docs/*` | Documentación | Ejemplo: `docs/documentacion` |

Flujo: `feature/* → develop → main`.

Reglas propuestas:
- No integrar directamente en `main`.
- Toda integración en `develop` pasa por el pipeline de CI.
- Toda promoción a `main` requiere el pipeline en verde y una etiqueta de versión.

## Control de versiones

- **Herramienta:** Git, con repositorio remoto en GitHub (`BR1ANJ3Sus3B/TI_-Soporte-en-Acci-n-`).
- **Modelo:** monorepositorio (un repositorio, múltiples componentes).
- **Convención de mensajes (propuesta):** prefijos `feat:`, `fix:`, `docs:`, `chore:`, `test:`.
- **Artefactos excluidos:** `Library/`, `Temp/`, `Logs/`, `build/`, `.dart_tool/`, `node_modules/`, archivos `.env` (véase `.gitignore`).
- **Etiquetas (propuesta):** `vX.Y.Z` aplicadas a las liberaciones sobre `main`.

## Automatización de pruebas

| Componente | Prueba propuesta | Herramienta propuesta | Estado |
| --- | --- | --- | --- |
| Unity (C#) | Pruebas de edición/Play Mode | Unity Test Framework (`com.unity.test-framework` 1.8.0, **presente** en `manifest.json`) | Pendiente de implementar pruebas |
| Flutter (Dart) | Pruebas unitarias y de widgets | `flutter test` | Pendiente (sin proyecto Flutter) |
| API (Node/Express) | Pruebas de endpoints | Jest / Supertest (propuesto, sin `package.json`) | Pendiente |
| React (PWA) | Pruebas de componentes | Vitest / Testing Library (propuesto) | Pendiente |

> No se documentan resultados de pruebas: **aún no se han ejecutado**.

## Generación de builds de Unity y Flutter

**Unity (propuesta).** Compilación desde línea de comandos en modo *batch* sobre un `Unity.exe` con la versión `6000.6.3f1` (versión verificada en `ProjectSettings/ProjectVersion.txt`). El pipeline deberá ejecutarse en un entorno con el editor instalado o mediante una imagen con Unity. Artefacto propuesto: ejecutable para Windows.

**Flutter (propuesta).** Compilación con `flutter build apk` (Android) una vez que exista el proyecto Flutter. La versión inicial prioriza Android (dato de alcance en `docs/proyecto/README.md`).

**PWA (propuesta).** Compilación con `npm run build` (Vite) una vez que exista `package.json`.

## Versionamiento

Se propone **Versionado Semántico** (`MAYOR.MENOR.PARCHE`) aplicado de forma independiente por producto y etiquetado en Git:

- **MAYOR:** cambios incompatibles (por ejemplo, ruptura de contrato de la API).
- **MENOR:** nuevas funciones compatibles.
- **PARCHE:** correcciones compatibles.

La versión se reflejará en: el artefacto generado, la etiqueta de Git y las variables de entorno documentadas en el [Documento 5](05_Parametros_Configuracion.md).

## Procedimientos de rollback

| Componente | Estrategia de reversión propuesta |
| --- | --- |
| API | Redesplegar la versión anterior (imagen o artefacto previo) y restaurar la configuración. |
| PWA | Publicar de nuevo el bundle de la versión estable anterior. |
| Unity | Distribuir de nuevo el ejecutable de la versión anterior. |
| Flutter | Reinstalar el APK de la versión anterior. |
| Base de datos | Restaurar el respaldo previo (respaldo diario previsto en RNF-13). |

Reglas propuestas:
- Cada liberación conserva el artefacto previo identificado por versión.
- La reversión se decide ante una regresión confirmada en producción.
- Tras revertir, se registra el incidente y se documenta la causa raíz.

## Conclusión

La liberación y el despliegue continuos aportan orden, trazabilidad y reproducibilidad a un proyecto de tres componentes con servicios compartidos y responsable único. La estrategia de ramas y el control de versiones **ya están definidos** en el repositorio; los pipelines, los contenedores y el monitoreo son **propuestas** cuya implementación se detalla en los documentos siguientes. El enfoque propuesto protege la estabilidad de `main`, facilita la verificación de los requisitos y prepara la transición hacia un despliegue automatizado una vez que los servicios existan.

## Referencias internas

- [Entorno y requerimientos](02_Entornos_Requerimientos.md)
- [Niveles de servicio (SLA)](03_Niveles_Servicio_SLA.md)
- [Métricas de monitoreo](04_Metricas_Monitoreo.md)
- [Parámetros de configuración](05_Parametros_Configuracion.md)
- [Pipeline CI/CD](diagramas/pipeline_cicd.md)

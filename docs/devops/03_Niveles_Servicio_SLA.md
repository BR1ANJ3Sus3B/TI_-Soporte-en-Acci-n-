# Documento 3. Niveles de servicio acordados (SLA)

## Definición y objetivo

Los **Acuerdos de Nivel de Servicio (SLA)** definen compromisos medibles sobre la disponibilidad, el rendimiento y la capacidad de respuesta de los servicios de **TI: Soporte en Acción**. Su objetivo es establecer expectativas claras entre el proveedor del servicio (el responsable del proyecto) y sus usuarios (jugadores, administradores y evaluadores).

> **Advertencia de alcance.** Ninguno de los servicios descritos está desplegado. Los valores incluidos se presentan como **objetivos propuestos** y **pendientes de aprobación**; no existen mediciones reales. Algunos objetivos derivan de requisitos no funcionales ya redactados (RNF-04, RNF-05, RNF-13); otros son propuestas nuevas de este documento. Se diferencian explícitamente en las tablas.

## Alcance de los servicios

| Servicio | Descripción | Estado |
| --- | --- | --- |
| API REST | Autenticación, catálogo, progreso y administración | Pendiente de implementar |
| Base de datos MongoDB | Persistencia de cuentas, misiones, intentos y progreso | Pendiente de implementar |
| PWA (React) | Panel del jugador y administración | Pendiente de implementar |
| Aplicación móvil (Flutter) | Consulta de misiones, guías y progreso | Pendiente de implementar |
| Videojuego (Unity) | Experiencia de juego local y envío de resultados | Implementado parcialmente |

## SLA, SLO y SLI

| Término | Significado | Ejemplo en el proyecto |
| --- | --- | --- |
| **SLA** | Acuerdo de nivel de servicio (compromiso) | «La API estará disponible el 99.0 % del tiempo mensual» (*propuesto*) |
| **SLO** | Objetivo de nivel de servicio (meta interna) | «El 95 % de las consultas responde en ≤ 2 s» (*RNF-04, meta a validar*) |
| **SLI** | Indicador de nivel de servicio (medición) | Latencia del percentil 95 de `GET /api/misiones` (*propuesto*) |

## Disponibilidad

| Servicio | Objetivo propuesto | Origen | Estado |
| --- | --- | --- | --- |
| API REST | 99.0 % mensual | Propuesta de este documento | Pendiente de aprobar |
| Base de datos | 99.0 % mensual | Propuesta de este documento | Pendiente de aprobar |
| PWA | 99.0 % mensual | Propuesta de este documento | Pendiente de aprobar |
| App móvil | No aplica (cliente local) | — | — |
| Videojuego | No aplica (ejecución local) | — | — |

> La disponibilidad se calcula como `(tiempo total − tiempo indisponible) / tiempo total`. Aún **no hay datos** para calcularla.

## Rendimiento

| Servicio | Objetivo propuesto | Origen | Estado |
| --- | --- | --- | --- |
| API (consultas de perfil, misiones y progreso) | 95 % ≤ 2 s con 20 usuarios concurrentes | **RNF-04** (meta a validar) | Aprobado como requisito, no medido |
| Videojuego (primer nivel) | ≥ 30 FPS durante 10 minutos en equipo de referencia | **RNF-05** (meta a validar) | Aprobado como requisito, no medido |
| PWA | Interacción utilizable en 360, 768 y 1366 px | **RNF-08** | Aprobado como requisito, no medido |

## Tiempos de respuesta

| Operación | Objetivo propuesto | Estado |
| --- | --- | --- |
| Inicio de sesión | ≤ 2 s (p95) | Propuesto |
| Consulta de catálogo de misiones | ≤ 2 s (p95) | Propuesto (alineado con RNF-04) |
| Consulta de progreso | ≤ 2 s (p95) | Propuesto (alineado con RNF-04) |
| Registro de intento | ≤ 3 s (p95) | Propuesto |
| Carga inicial del videojuego | ≤ 15 s (propuesto) | Pendiente de confirmar |

## Tiempos de recuperación

| Indicador | Descripción | Objetivo propuesto | Estado |
| --- | --- | --- | --- |
| MTTR | Tiempo medio de reparación | ≤ 4 h (P1) | Propuesto |
| MTBF | Tiempo medio entre fallos | No definido | Pendiente |

## RTO y RPO

| Indicador | Significado | Objetivo propuesto | Origen |
| --- | --- | --- | --- |
| **RTO** | Tiempo máximo de recuperación tras un fallo | 4 h (propuesto) | Este documento |
| **RPO** | Pérdida máxima de datos aceptable | 24 h (respaldo diario) | **RNF-13** (respaldo diario) |

> RNF-13 establece un respaldo **diario** durante las pruebas con datos persistentes, lo que es coherente con un RPO de 24 h.

## Clasificación de incidentes

| Nivel | Descripción | Ejemplo |
| --- | --- | --- |
| Crítico | Servicio no disponible para todos los usuarios | API caída; base de datos inaccesible |
| Mayor | Función principal degradada | El registro de intentos falla |
| Menor | Función secundaria afectada | Una guía no carga |
| Cosmético | Defecto sin impacto funcional | Texto mal alineado |

## Prioridades P1, P2, P3 y P4

| Prioridad | Impacto | Tiempo de respuesta propuesto | Tiempo de resolución propuesto |
| --- | --- | --- | --- |
| **P1** | Crítico: servicio totalmente caído | ≤ 30 min | ≤ 4 h |
| **P2** | Mayor: función principal degradada | ≤ 2 h | ≤ 1 día |
| **P3** | Menor: función secundaria | ≤ 1 día | ≤ 3 días |
| **P4** | Cosmético | ≤ 3 días | Próxima liberación |

> Todos los tiempos son **propuestos** y no han sido validados con mediciones.

## Responsables

| Rol | Responsabilidad | Persona |
| --- | --- | --- |
| Responsable del servicio | Mantener el servicio y atender incidentes | Brian Jesús Mendoza Márquez (responsable único) |
| Administrador | Gestionar catálogo, guías y cuentas | Rol `administrador` (aplicación) |
| Jugador | Reportar incidentes funcionales | Rol `jugador` (aplicación) |

## Escalamiento

En un proyecto con responsable único no existe escalamiento a múltiples niveles. Se propone:

1. **Detección** (monitoreo o reporte del usuario).
2. **Registro** del incidente con su prioridad.
3. **Atención** por el responsable del servicio.
4. **Escalamiento a terceros** (proveedor de hosting) solo si el incidente depende de la infraestructura externa.

## Seguimiento

- Registro de incidentes con fecha, prioridad, causa y solución.
- Revisión de los SLI tras cada incidente.
- Revisión periódica de cumplimiento de los objetivos propuestos.

## Indicadores de cumplimiento

| Indicador | Fórmula | Meta propuesta |
| --- | --- | --- |
| Cumplimiento de disponibilidad | Tiempo disponible / tiempo total | ≥ 99.0 % |
| Cumplimiento de latencia | Consultas ≤ 2 s / consultas totales | ≥ 95 % |
| Cumplimiento de recuperación | Incidentes resueltos en plazo / total | ≥ 90 % |
| Frecuencia de respaldo | Respaldos realizados / esperados | 100 % (RNF-13) |

> **Compromisos aprobados vs. objetivos propuestos.** Solo son compromisos formales los requisitos no funcionales ya documentados (RNF-04, RNF-05, RNF-08, RNF-13) y sujeta su cifra a validación en el entorno elegido. Los valores de disponibilidad, MTTR, RTO y prioridades de este documento son **propuestas pendientes de aprobación**.

## Referencias internas

- [Justificación](01_Justificacion_Liberacion_Despliegue.md)
- [Entorno y requerimientos](02_Entornos_Requerimientos.md)
- [Métricas de monitoreo](04_Metricas_Monitoreo.md)
- [Parámetros de configuración](05_Parametros_Configuracion.md)

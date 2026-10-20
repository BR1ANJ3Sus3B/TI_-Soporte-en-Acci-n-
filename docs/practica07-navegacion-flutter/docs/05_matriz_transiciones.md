# 05 · Matriz de transiciones y reglas de acceso

La matriz contiene **51 transiciones**. Una flecha indica un cambio de pantalla; el comportamiento real requiere validación de credenciales, estado de sesión y permisos. Las rutas con error pueden permanecer en la misma pantalla.

| Origen | Destino | Condición / evento | Rol |
|---|---|---|---|
| P01 | P02 | Inicio de la aplicación | Todos |
| P02 | P03 | Sin sesión, token inválido o cierre previo | Todos |
| P02 | P09 | Sesión válida: cargar inicio según rol | Todos |
| P03 | P09 | Credenciales válidas, cuenta activa y permisos | Todos |
| P03 | P03 | Credenciales incorrectas bajo el umbral | Todos |
| P03 | P05 | Exceso de intentos fallidos | Todos |
| P03 | P04 | Selecciona recuperación | Todos |
| P03 | P10 | Selecciona crear cuenta | Todos |
| P03 | P07 | Selecciona información del software | Todos |
| P04 | P06 | Solicitud aceptada: verificar código | Todos |
| P04 | P03 | Cancelar y regresar | Todos |
| P05 | P06 | Solicitar desbloqueo | Todos |
| P05 | P03 | Regresar; el bloqueo se mantiene | Todos |
| P06 | P03 | Código correcto y desbloqueo o recuperación confirmados | Todos |
| P06 | P06 | Código incorrecto o caducado | Todos |
| P06 | P04 | Reiniciar recuperación | Todos |
| P10 | P03 | Registro validado y sin duplicados | Todos |
| P10 | P10 | Campos inválidos o cuenta duplicada | Todos |
| P07 | P08 | Ver equipo y empresa | Todos |
| P08 | P07 | Regresar | Todos |
| P07 | P03 | Volver sin autenticación | Todos |
| P07 | P09 | Volver con sesión válida | Todos |
| P09 | P07 | Ver información del software | Todos |
| P09 | P11 | Estudiante: consultar misiones | Estudiante/Jugador |
| P09 | P13 | Estudiante: abrir guías | Estudiante/Jugador |
| P09 | P15 | Estudiante: consultar progreso | Estudiante/Jugador |
| P09 | P16 | Abrir perfil | Todos |
| P11 | P12 | Misión disponible o consultar misión bloqueada | Estudiante/Jugador |
| P11 | P09 | Regresar al inicio | Estudiante/Jugador |
| P11 | P15 | Ir a progreso | Estudiante/Jugador |
| P12 | P13 | Consultar guía de apoyo | Estudiante/Jugador |
| P12 | P11 | Regresar a misiones | Estudiante/Jugador |
| P13 | P14 | Seleccionar guía | Estudiante/Jugador |
| P13 | P12 | Volver al detalle de la misión de origen | Estudiante/Jugador |
| P13 | P09 | Regresar al inicio | Estudiante/Jugador |
| P14 | P13 | Volver al catálogo de guías | Estudiante/Jugador |
| P14 | P12 | Volver al detalle si se conserva el contexto | Estudiante/Jugador |
| P15 | P11 | Ir a misiones | Estudiante/Jugador |
| P15 | P09 | Regresar al inicio | Estudiante/Jugador |
| P15 | P16 | Abrir perfil | Estudiante/Jugador |
| P16 | P15 | Solo estudiante: ver mi progreso | Estudiante/Jugador |
| P16 | P09 | Volver al inicio | Todos |
| P16 | P03 | Cerrar sesión, invalidar sesión local y remota | Todos |
| P09 | P17 | Docente: seguimiento autorizado | Docente/Evaluador |
| P17 | P09 | Regresar a inicio | Docente/Evaluador |
| P09 | P18 | Administrador: módulo usuarios | Administrador |
| P18 | P09 | Regresar a inicio | Administrador |
| P09 | P19 | Editor: edición de misiones | Editor de contenido |
| P19 | P09 | Regresar a inicio | Editor de contenido |
| P09 | P20 | Institución: indicadores agregados | Institución educativa |
| P20 | P09 | Regresar a inicio | Institución educativa |

Las 51 transiciones también están disponibles en formato CSV en [`matriz_transiciones.csv`](matriz_transiciones.csv).

## Decisiones de seguridad de navegación

- La decisión de rol proviene de la sesión y se verifica en el servicio, no de un botón del prototipo.
- Ante un fallo de validación se muestra un error neutro, sin revelar si el correo existe.
- Los enlaces de un rol distinto no aparecen como opción operativa.
- El código de recuperación o desbloqueo debe verificarse con el servidor y caducar.

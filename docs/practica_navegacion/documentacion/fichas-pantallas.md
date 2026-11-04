# Fichas de las 20 pantallas

Las rutas exactas de sketch y wireframe están en `../js/image-manifest.js`; cada par usa originales de `docs/imagenes/` sin copias. La condición común de las pantallas privadas es sesión y rol autorizado.

## P01 — Splash Screen
**Descripción:** presentación inicial. **Plataforma:** Flutter/PWA. **Rol:** Público. **Acción:** inicialización automática. **Siguiente:** P02. **Requerimiento:** RF-01. **Observación:** no admite interacción.

## P02 — Loading Screen
**Descripción:** carga recursos y revisa sesión. **Plataforma:** Flutter/PWA. **Rol:** Público. **Acción:** cargar. **Siguiente:** P03. **Requerimiento:** RF-01. **Observación:** transición automática.

## P03 — Login Screen
**Descripción:** inicia sesión. **Plataforma:** Flutter/PWA. **Rol:** Público. **Acciones:** iniciar, recuperar, registrar y opciones sociales propuestas. **Siguientes:** P04/P05/P06/P10. **Requerimientos:** RF-02, RF-05. **Observación:** Facebook, Google e Instagram no están configurados.

## P04 — Registro
**Descripción:** crea cuenta de jugador. **Plataforma:** Flutter/PWA. **Rol:** Público. **Acción:** registrarme. **Siguiente:** P03. **Requerimientos:** RF-03, RF-20. **Observación:** no otorga administración.

## P05 — Recovery Account
**Descripción:** solicita enlace de recuperación. **Plataforma:** Flutter/PWA. **Rol:** Público. **Acción:** enviar enlace. **Siguiente:** P03. **Requerimiento:** RF-04. **Observación:** datos de prueba.

## P06 — Lock Account
**Descripción:** comunica bloqueo por intentos fallidos. **Plataforma:** Flutter/PWA. **Rol:** Público. **Acción:** solicitar desbloqueo. **Siguientes:** P07/P03. **Requerimientos:** RF-04, RF-05. **Observación:** requiere condición de bloqueo.

## P07 — Unlock Account Code
**Descripción:** valida código de desbloqueo. **Plataforma:** Flutter/PWA. **Rol:** Público. **Acción:** desbloquear. **Siguiente:** P03. **Requerimiento:** RF-04. **Observación:** sólo avanza con código válido.

## P08 — About Software
**Descripción:** información del software. **Plataforma:** Flutter/PWA. **Rol:** Público. **Acción:** volver. **Siguientes:** P03/P10. **Requerimiento:** pendiente. **Observación:** acceso informativo.

## P09 — About Team / Enterprise
**Descripción:** presenta equipo y NovaTech Solutions. **Plataforma:** Flutter/PWA. **Rol:** Público. **Acción:** volver. **Siguientes:** P03/P10. **Requerimiento:** pendiente. **Observación:** acceso informativo.

## P10 — Home
**Descripción:** entrada a módulos. **Plataforma:** Flutter/PWA. **Rol:** Jugador/Estudiante. **Acciones:** abrir módulos, cerrar sesión. **Siguientes:** P11/P12/P16/P17/P20/P08/P09/P03. **Requerimiento:** RF-05. **Observación:** requiere sesión.

## P11 — Dashboard
**Descripción:** resumen personal. **Plataforma:** Flutter/PWA. **Rol:** Jugador/Estudiante. **Acción:** consultar resumen. **Siguientes:** P10/P12/P16/P17. **Requerimiento:** RF-12. **Observación:** no gestiona cuentas.

## P12 — Mapa de NovaTech
**Descripción:** zonas y misiones. **Plataforma:** Flutter/PWA. **Rol:** Jugador/Estudiante. **Acción:** seleccionar misión. **Siguiente:** P13. **Requerimiento:** RF-06. **Observación:** requiere sesión.

## P13 — Detalle de misión
**Descripción:** objetivos y recompensa. **Plataforma:** Flutter/PWA. **Rol:** Jugador/Estudiante. **Acción:** comenzar. **Siguiente:** P14. **Requerimientos:** RF-06, RF-07. **Observación:** valida selección.

## P14 — Juego / Misión
**Descripción:** misión jugable. **Plataforma:** Unity. **Rol:** Jugador/Estudiante. **Acciones:** jugar, pausar. **Siguientes:** P13/P15/P17. **Requerimiento:** RF-07. **Observación:** interacción de videojuego.

## P15 — Diálogo / Historia
**Descripción:** diálogo narrativo. **Plataforma:** Unity. **Rol:** Jugador/Estudiante. **Acción:** continuar. **Siguientes:** P14/P17. **Requerimiento:** RF-08. **Observación:** instrucciones también textuales (RNF-11).

## P16 — Inventario / Herramientas
**Descripción:** consulta herramientas. **Plataforma:** Unity/Flutter/PWA. **Rol:** Jugador/Estudiante. **Acción:** consultar. **Siguientes:** P10/P11. **Requerimiento:** RF-13. **Observación:** información propia.

## P17 — Progreso del jugador
**Descripción:** experiencia, nivel y estadísticas. **Plataforma:** Unity/Flutter/PWA. **Rol:** Jugador/Estudiante; consulta autorizada para Docente/Evaluador. **Acción:** consultar. **Siguiente:** P18. **Requerimiento:** RF-14. **Observación:** pantalla docente propia pendiente.

## P18 — Logros / Insignias
**Descripción:** reconocimientos. **Plataforma:** Unity/Flutter/PWA. **Rol:** Jugador/Estudiante. **Acción:** consultar. **Siguientes:** P17/P10. **Requerimiento:** RF-15. **Observación:** resultados propios.

## P19 — Configuración
**Descripción:** preferencias. **Plataforma:** Flutter/PWA. **Rol:** Jugador/Estudiante. **Acción:** guardar. **Siguientes:** P20/P10. **Requerimiento:** RF-11. **Observación:** no cambia roles.

## P20 — Perfil de usuario
**Descripción:** datos básicos. **Plataforma:** Flutter/PWA. **Rol:** Jugador/Estudiante. **Acciones:** editar perfil, abrir configuración. **Siguientes:** P10/P19. **Requerimiento:** RF-10. **Observación:** no administra otros usuarios.

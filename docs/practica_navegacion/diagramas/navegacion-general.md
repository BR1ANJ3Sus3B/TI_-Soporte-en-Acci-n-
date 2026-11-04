# Diagrama general de navegación

El archivo `navegacion-general.mmd` es la fuente Mermaid del flujo. P01–P02 son rutas automáticas; P03 presenta alternativas de autenticación. Las rutas posteriores a P10 requieren sesión y rol Jugador/Estudiante, salvo las consultas autorizadas de progreso para Docente/Evaluador. P14 y P15 son interacciones Unity; el resto se documenta para Flutter/PWA.

Los retornos a P10 representan navegación de módulo, y el retorno P07 → P03 sólo ocurre cuando el código es válido. Un cierre de sesión devuelve P10 → P03.

# Flujos de navegacion

P01 inicia automaticamente P02. P03 centraliza registro, recuperacion, bloqueo y rutas de acceso segun el rol. P06 conduce a P07 tras bloqueo y P07 retorna a P03 con codigo valido. Cada panel de rol retorna a su panel principal y el cierre de sesion vuelve a P03.

Los flujos implementados se encuentran en `../diagramas/navegacion-general.mmd` y `../diagramas/roles/`. El prototipo filtra nodos por rol y plataforma; P14/P15 se marcan como Unity, mientras que los paneles por rol son PWA.

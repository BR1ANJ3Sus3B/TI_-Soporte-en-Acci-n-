# Arquitectura de acceso

- **Flutter → Keycloak:** cliente móvil público con Authorization Code + PKCE.
- **PWA:** recursos públicos y demostración. Sin conexión directa a Keycloak.
- **API educativa:** pendiente; deberá validar la identidad y permisos del cliente.
- **MongoDB:** accesible a través de la API, nunca directamente desde los clientes.

`AuthProvider` informa `unavailable` y no concede roles. `RouteGuard` continúa
bloqueando las vistas privadas. El cliente web no procesa callbacks OIDC ni
almacena tokens. Una sesión móvil no sustituye la futura autenticación web.

La configuración de infraestructura de Keycloak se conserva como servicio compartido
del repositorio, pero su cliente de aplicación es `ti-soporte-flutter`.

El contenido de `demoData.ts` es público y ficticio. El service worker almacena
recursos estáticos; no debe usarse como prueba de autenticación ni autorización.

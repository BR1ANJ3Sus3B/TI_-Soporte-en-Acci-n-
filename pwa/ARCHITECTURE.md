# Arquitectura

La PWA se ejecuta como CSR con renderizado SSR sólo para rutas públicas. `AuthProvider` no toca APIs de navegador durante SSR; la inicialización OIDC ocurre tras la hidratación.

## Autenticación

`services/authService.ts` implementa OpenID Connect Authorization Code Flow con PKCE S256 contra Keycloak. El nombre del realm se codifica con `encodeURIComponent`. Al iniciar sesión se conserva solamente `state`, el verificador PKCE y la ruta de retorno de forma temporal en `sessionStorage`; se validan y eliminan en el callback. Ningún access token, refresh token o ID token se persiste en almacenamiento web.

Los tokens permanecen en memoria, se renuevan antes de expirar y se eliminan antes de redirigir al endpoint de logout. Al recargar, se solicita una autorización silenciosa (`prompt=none`) para recuperar la sesión SSO de Keycloak sin persistir tokens en el cliente.

`AuthContext` expone estado, usuario, inicio/cierre de sesión, reintento y comprobación de roles. Los roles de realm `estudiante`, `docente` y `administrador` se mapean a la navegación student, teacher y admin. `RouteGuard` protege rutas para UX; esto no sustituye la validación de JWT y autorización del backend.

## API y datos

`apiClient` puede adjuntar el Bearer token en memoria al API configurado por `VITE_API_BASE_URL`. Un backend futuro debe verificar firma, emisor, audiencia, caducidad y roles antes de permitir operaciones. Los datos educativos demostrativos permanecen separados de la identidad de Keycloak.

`localStorageService` sólo se usa para preferencias no sensibles. No se deben guardar allí perfiles privados, contraseñas ni tokens.

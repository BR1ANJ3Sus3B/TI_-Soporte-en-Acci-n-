# PWA · TI: Soporte en Acción

## Desarrollo

```powershell
npm install
Copy-Item .env.example .env
npm run dev
```

`npm run dev` inicia la PWA en `http://localhost:5173`. Para validar producción use `npm run build` y `npm run start:ssr` (puerto 4173).

## Keycloak

La PWA usa OpenID Connect Authorization Code Flow con PKCE S256. Los tokens de acceso, actualización e identidad sólo existen en memoria; no se escriben en `localStorage` ni `sessionStorage`. El único valor temporal en `sessionStorage` es el `state` y verificador PKCE, que se elimina al regresar de Keycloak.

Configure estas variables públicas en `.env` (no se incluyen secretos):

```dotenv
VITE_KEYCLOAK_URL=http://localhost:8080
VITE_KEYCLOAK_REALM=TI Soporte en Acción
VITE_KEYCLOAK_CLIENT_ID=ti-soporte-pwa
```

En el cliente `ti-soporte-pwa` del realm `TI Soporte en Acción`, verifique:

- Standard Flow habilitado y PKCE `S256`.
- Valid redirect URIs: `http://localhost:5173/*` y, si se usa SSR, `http://localhost:4173/*`.
- Valid post logout redirect URIs: las mismas rutas.
- Web origins: `http://localhost:5173` y `http://localhost:4173` según corresponda.
- El usuario `alex.estudiante` tiene el rol de realm `estudiante`.

Desde la pantalla de inicio de sesión, pulse **Iniciar sesión** y autentíquese como `alex.estudiante`. El perfil y la navegación usarán los claims reales de Keycloak. Los roles `estudiante`, `docente` y `administrador` se convierten respectivamente a los permisos visuales student, teacher y admin. El servidor/API debe validar el JWT de forma independiente: la protección del cliente sólo controla la navegación y experiencia.

## API

`apiClient` adjunta un Bearer token sólo a `VITE_API_BASE_URL` cuando existe una sesión. No hay API REST en este repositorio que valide JWT todavía; esa validación de audiencia, firma, emisor y roles sigue siendo obligatoria en el backend antes de exponer datos protegidos.

Unity no forma parte de esta PWA y no se modifica.

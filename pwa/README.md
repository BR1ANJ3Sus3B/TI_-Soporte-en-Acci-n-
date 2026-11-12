# PWA · TI: Soporte en Acción

## Desarrollo

```powershell
npm ci
npm run dev
```

La PWA se inicia en http://localhost:5173. La demostración pública está en `/demo`.
Para producción: `npm run build` y `npm run start:ssr` (puerto 4173).

## Autenticación

Keycloak corresponde a la **aplicación móvil Flutter**, en la rama
`feature/mobile-flutter`. La integración directa OIDC de esta PWA se retiró:
no realiza redirecciones, intercambio de códigos ni almacenamiento de tokens.
Las antiguas variables `VITE_KEYCLOAK_*` ya no se utilizan y se pueden retirar
del entorno local. No se necesita un cliente `ti-soporte-pwa` para esta versión.

Las rutas protegidas siguen cerradas; quitar Keycloak no concede permisos ni
convierte los paneles administrativos en públicos. `/demo` y los recursos públicos
siguen disponibles. El mecanismo de sesión web queda pendiente de definir con la API.
Iniciar sesión en Flutter no inicia automáticamente una sesión en la PWA.

## Datos y validación

Las misiones y el progreso todavía son datos demostrativos. `apiClient` conserva
la URL `VITE_API_BASE_URL`, pero no adjunta tokens de Flutter ni inventa sesiones.
Un futuro backend debe comprobar identidad y permisos antes de exponer datos privados.

```powershell
npm run lint
npm run build
```

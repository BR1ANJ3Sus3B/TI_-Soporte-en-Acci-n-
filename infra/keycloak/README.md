# Keycloak para la aplicación Flutter

La infraestructura de identidad se conserva, pero su consumidor es la aplicación
móvil Flutter (`feature/mobile-flutter`), no la PWA. No borrar el volumen ni los
usuarios existentes al cambiar de cliente.

Con Docker Desktop activo y `KC_ADMIN_PASSWORD` definido en `.env`:

```powershell
docker compose up -d
docker compose ps
```

Consola: http://localhost:8080. Conservar el realm existente o crear `ti-soporte`
para una instalación nueva. Crear el cliente público `ti-soporte-flutter`:

- Client authentication: Off; Standard flow: On.
- Direct access grants e Implicit flow: Off.
- PKCE: S256.
- Redirect URI y post logout redirect URI: `com.tisoporte.accion:/oauthredirect`.

Flutter recibe el issuer mediante `KEYCLOAK_ISSUER` y el client ID mediante
`KEYCLOAK_CLIENT_ID`, usando `--dart-define`. En el emulador Android se accede al PC
con `10.0.2.2`; en dispositivo físico usar una dirección accesible. El discovery
debe publicar URLs alcanzables desde el dispositivo.

Las instrucciones completas están en `movil/KEYCLOAK.md` de la rama móvil.
La PWA ya no lee `VITE_KEYCLOAK_*`. Deshabilitar el cliente `ti-soporte-pwa` en el
servidor solo después de comprobar que no tenga otros consumidores.
`start-dev` es exclusivamente para desarrollo local.

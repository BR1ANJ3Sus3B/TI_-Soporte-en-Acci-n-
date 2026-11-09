# Keycloak local · TI: Soporte en Acción

Esta infraestructura es solo para desarrollo local. `start-dev` no es una configuración de producción.

## Inicio

1. Abra Docker Desktop y espere a que el motor Linux esté en ejecución.
2. Revise el secreto local en `.env`; no lo suba a Git. Si debe regenerarlo, use una contraseña larga y aleatoria.
3. Desde esta carpeta ejecute:

   ```powershell
   docker compose config
   docker compose up -d
   docker compose ps
   ```

4. Abra [http://localhost:8080](http://localhost:8080) y entre a la consola de administración con el usuario `admin` y la contraseña definida en `.env`.

## Realm y roles

1. Cree el realm `ti-soporte`.
2. En **Realm roles**, cree `estudiante`, `docente` y `administrador`.
3. Cree usuarios de prueba y asigne el rol institucional correspondiente. No permita que el autorregistro asigne roles docente o administrador.

## Cliente de desarrollo CSR

En el realm cree un cliente OpenID Connect llamado `ti-soporte-pwa`:

- Client authentication: Off.
- Standard flow: On.
- Implicit flow: Off.
- Direct access grants: Off.
- PKCE challenge method: `S256`.
- Valid redirect URIs: `http://localhost:5173/*`.
- Web origins: `http://localhost:5173`.

La PWA incluye una arquitectura preparada para BFF; si se activa el BFF, cree además un cliente confidencial para él, use una callback de servidor específica y conserve su secreto fuera de Vite. No exponga secretos ni tokens en `VITE_*`.

## Variables públicas de la PWA

En `pwa/.env.local` use únicamente valores públicos:

```dotenv
VITE_KEYCLOAK_URL=http://localhost:8080
VITE_KEYCLOAK_REALM=ti-soporte
VITE_KEYCLOAK_CLIENT_ID=ti-soporte-pwa
```

La PWA no almacena contraseñas, access tokens ni refresh tokens en localStorage.

## Operación y diagnóstico

```powershell
docker compose logs --tail=100 keycloak
docker compose restart keycloak
docker compose stop
docker compose up -d
```

No use `docker compose down -v`: eliminaría el volumen `keycloak_data` y con él el realm y usuarios locales. Si el puerto 8080 está ocupado, libérelo o cambie el mapeo de puertos y actualice las URL de la PWA. Si Docker indica que no puede conectar con el motor, inicie Docker Desktop antes de ejecutar Compose.

## Proveedores sociales

Google y Facebook deben configurarse en **Identity Providers** con credenciales emitidas por cada proveedor. Habilite los flags de la PWA únicamente después de probar sus aliases en Keycloak y el BFF. Instagram permanece deshabilitado hasta contar con una integración OIDC compatible y autorizada.

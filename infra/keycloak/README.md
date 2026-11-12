# Keycloak para Flutter

Servicio local de identidad para la aplicación móvil. Copiar `.env.example` a
`.env`, asignar `KC_ADMIN_PASSWORD` y ejecutar desde esta carpeta:

```powershell
docker compose up -d
docker compose ps
```

Consola: http://localhost:8080, usuario `admin`. Para una instalación nueva,
crear el realm `ti-soporte` y el cliente público `ti-soporte-flutter` siguiendo
[movil/KEYCLOAK.md](../../movil/KEYCLOAK.md). Si ya existe un realm, conservarlo y
configurar su URL real en Flutter. Los usuarios y roles permanecen en el volumen.

Esta configuración usa `start-dev`. No ejecutar dos copias de este servicio al
mismo tiempo ni borrar el volumen existente para cambiar de cliente. La migración
de la PWA a Flutter no exige borrar cuentas. La PWA ya no usa Keycloak directamente.

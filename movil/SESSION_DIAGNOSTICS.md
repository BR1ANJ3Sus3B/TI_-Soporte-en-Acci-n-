# Diagnóstico de sesión (2026-11-22)

## Cambios comprobados

- Un HTTP 401 de una consulta pública ya no llama a `expireSession`, ni borra
  el indicador persistido de invitado. Antes podía convertir al invitado en una
  sesión expirada y provocar consultas de perfil privado al reintentar Home.
- Una respuesta tardía de un token anterior no invalida una cuenta nueva ni un
  invitado. Las consultas privadas siguen exigiendo token.
- Catálogos de misiones, detalle, áreas y logros usan peticiones públicas.
- Home invitado toma perfil/progreso local. Los tests verifican dos cargas
  consecutivas y restauración del estado invitado incluso con HTTP 401/500 público.
- Las sesiones irrecuperables ofrecen iniciar sesión o continuar como invitado.
- Flutter pide `openid ti-soporte-api`; el servidor acepta esa autorización.
  No se ha inspeccionado un token de usuario real para confirmar su audiencia.

## Evidencia de servicios reales

- `GET /api/v1/missions` y `/levels` sin token: 200, listas vacías.
- `GET /api/v1/profile` sin token: 401 `missing_bearer_token`.
- Perfil con token malformado: 401 `invalid_token`.
- API lista, MongoDB conectado, issuer `http://localhost:8080/realms/ti-soporte`,
  audiencia `ti-soporte-api`, JWKS interno del mismo realm.
- El dispositivo `44994d2c7d76` estuvo conectado con reverse 8080/3000; se
  desconectó antes de instalar la nueva compilación.

No se capturó la solicitud original del teléfono. No se afirma que el error del
usuario haya sido causado exclusivamente por un mapper de audiencia, ni que el
login autenticado real haya pasado. Los logs nuevos `[api]` en debug muestran
método, ruta agrupada, status, estado, presencia de token y código de error de
una lista permitida; no muestran JWT, claims, cuerpos ni identificadores privados.

## Ejecutar

```powershell
& 'C:\Users\jesus\AppData\Local\Temp\opencode\ti-flutter-keycloak\scripts\run-flutter.ps1' -DeviceId '44994d2c7d76'
```

Si una sesión antigua sigue siendo rechazada, usar Iniciar sesión para obtener
un token nuevo con el scope de API; o Continuar como invitado para seguir con
contenido público y progreso local. No borrar almacenamiento ni datos del teléfono.

Verificaciones: Flutter analyze sin problemas, 33 tests aprobados y un test de
defines condicionado omitido en la suite sin defines; API 12 tests aprobados,
incluida expiración criptográficamente válida y rechazo de audience/issuer incorrectos.
APK debug compilado con `keycloak.local.json`, build ID `session-fix-20261122`.
Pendiente: instalación y prueba de Home/login/callback/perfil privado/logout en
Android con cuenta autorizada. Las pruebas unitarias usan dobles explícitos y no
son evidencia de una autenticación real.

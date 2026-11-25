# Keycloak en Flutter Android

La app usa `flutter_appauth` con Authorization Code + PKCE en el navegador del
sistema; es un cliente público y no lleva Client Secret. La pantalla Stitch y el
modo invitado siguen disponibles aunque no se compile con Keycloak.

## Configuración local verificada

Inspección local del 2026-11-22:

- Keycloak 26.4 está activo en el puerto `8080`.
- El realm usado por Flutter y la API es `ti-soporte`.
- Discovery devuelve exactamente `http://localhost:8080/realms/ti-soporte` como
  issuer; el sufijo `/.well-known/openid-configuration` se añade internamente.
- `ti-soporte-flutter` acepta una solicitud de autorización con callback
  `com.tisoporte.accion:/oauthredirect` y scope `openid`.
- El realm devuelve `invalid_scope` al pedir `profile` o `email`. Flutter solicita
  `openid ti-soporte-api`: Discovery anuncia este scope de API y la solicitud
  de autorización con ambos llega al formulario de Keycloak. La audiencia del
  token emitido debe validarse con una autenticación real; no basta Discovery.
- La API responde `/health/ready` con MongoDB conectado y configuración Keycloak.
- También existe el realm `TI Soporte en Acción`, pero no es el issuer seleccionado
  ni el issuer de la API activa.

Discovery responde URLs con `localhost`. En Android físico se conserva ese issuer
y se crea ADB reverse para que el teléfono llegue al PC. No reemplaces el issuer por
`10.0.2.2` (solo emulador) ni por una IP LAN que Discovery no publique.

## Causa del mensaje «Falta KEYCLOAK_ISSUER»

`AuthService` lee `KEYCLOAK_ISSUER` con `String.fromEnvironment`, que es un valor
de compilación. Flutter no carga `.env` ni `keycloak.local.json` automáticamente.
El APK instalado en el teléfono tenía hora de instalación 12:49:50 y el APK debug
en `movil/build/app/outputs/flutter-apk/app-debug.apk` hora 12:49:37 del
2026-11-22. Esa compilación anterior se hizo con `flutter build apk --debug` sin
`--dart-define-from-file`; por eso el valor compilado quedó vacío aunque el JSON
local existiera. Además, el repositorio habitual está en otra rama/worktree sin
`movil/` ni `scripts/run-flutter.ps1`; compilar desde allí tampoco usa estos cambios.

## Ejecutar desde el worktree correcto

El archivo local `movil/keycloak.local.json` ya existe, tiene JSON válido y contiene
el issuer/Client ID comprobados. Está ignorado por Git; no lo copies a otra rama.
Desde PowerShell ejecuta el script por su ruta completa:

```powershell
& 'C:\Users\jesus\AppData\Local\Temp\opencode\ti-flutter-keycloak\scripts\run-flutter.ps1'
```

Con un solo Android conectado lo detecta automáticamente. Si hay varios:

```powershell
flutter devices
& 'C:\Users\jesus\AppData\Local\Temp\opencode\ti-flutter-keycloak\scripts\run-flutter.ps1' -DeviceId '<id real de flutter devices>'
```

El script comprueba JSON, issuer/Client ID no vacíos, Discovery, readiness de API,
ejecuta un test de Dart con esos defines, configura/verifica `adb reverse` para
8080 y 3000, y luego invoca `flutter run` desde `movil/` con el JSON como argumento
de compilación. No crea el JSON ni permite autenticación con valores vacíos.

Para usar solamente el modo invitado, sin configurar Keycloak:

```powershell
& 'C:\Users\jesus\AppData\Local\Temp\opencode\ti-flutter-keycloak\scripts\run-flutter.ps1' -GuestOnly
```

También se puede compilar explícitamente desde la raíz del worktree correcto:

```powershell
Set-Location 'C:\Users\jesus\AppData\Local\Temp\opencode\ti-flutter-keycloak\movil'
flutter pub get
flutter analyze
flutter test
flutter build apk --debug --dart-define-from-file=keycloak.local.json
```

El script `run-flutter.ps1` además ejecuta `test/build_configuration_test.dart` con
`VERIFY_KEYCLOAK_DART_DEFINES=true` y compara los valores compilados con los del JSON;
no pases defines al suite completo porque sus tests de valores por defecto se
ejecutan deliberadamente sin configuración. En debug se registra únicamente si
issuer o Client ID están presentes, el ambiente y un identificador de build; nunca
valores, contraseñas o tokens.

## Red y callback Android

El teléfono detectado para esta verificación es Android 15, modelo `23053RN02L`,
ADB `44994d2c7d76`. El script ejecuta y verifica:

```powershell
adb -s <id-detectado> reverse tcp:8080 tcp:8080
adb -s <id-detectado> reverse tcp:3000 tcp:3000
adb -s <id-detectado> reverse --list
```

El manifiesto registra el callback `com.tisoporte.accion:/oauthredirect`, que
coincide con `AuthService.redirectUri`. Android permite cleartext solo en debug;
AppAuth permite HTTP solo mediante `KEYCLOAK_ALLOW_HTTP=true` en debug. Release
mantiene HTTP deshabilitado. No se desactiva validación TLS.

## Estado de autenticación y pruebas

Discovery y el formulario de autorización fueron comprobados sin enviar credenciales.
No se ha completado un login con usuario real, ni comprobado un token real contra
`/api/v1/profile`; no se inventa esa evidencia. Para ello se necesita una cuenta de
prueba autorizada. El test de dart-defines demuestra configuración de compilación,
no una autenticación.

La salida debug `[build-config]` muestra solo `issuerConfigured`,
`clientIdConfigured`, `environment` y `buildId`. Las contraseñas se escriben solo en
Keycloak; tokens se guardan en Keystore/Keychain. Ajustes → Cerrar sesión elimina la
sesión local y solicita logout a Keycloak.

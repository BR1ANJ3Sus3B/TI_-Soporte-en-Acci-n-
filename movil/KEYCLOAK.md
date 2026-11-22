# Keycloak en la aplicación Flutter

La integración directa pertenece a Flutter, no a la PWA. Se usa
`flutter_appauth` con Authorization Code + PKCE, navegador del sistema y cliente
público sin secreto. Implementación de acceso para **Android e iOS**; Windows y
Flutter web muestran un aviso de plataforma no compatible con este acceso móvil.

## Resultado de la inspección del servidor

La inspección confirmó que Keycloak 26.4 está accesible localmente y que el realm
existente es `TI Soporte en Acción`. Su issuer de discovery es:

```text
http://localhost:8080/realms/TI%20Soporte%20en%20Acci%C3%B3n
```

El realm `ti-soporte` no existe en esta instancia. La petición OIDC para el Client
ID `ti-soporte-flutter` devuelve **Client not found**: ese ID solo figuraba como
propuesta en la documentación, pero el cliente no está creado en el realm real.
El Compose local inicia Keycloak con un volumen; no importa realms ni clientes.

`infra/keycloak/.env.example` declara `KC_ADMIN_PASSWORD`. Se añadió una contraseña
al `.env` local ignorado por Git, pero Keycloak la rechazó al probarla como
administrador: `KC_BOOTSTRAP_ADMIN_PASSWORD` solo inicializa el admin de un volumen
nuevo; no cambia el admin guardado en un volumen ya inicializado. No se borró ni
recreó ese volumen. Usa las credenciales del admin ya existente para crear el
cliente en el realm actual; no hace falta recrear el realm ni los usuarios.

```text
https://<host-real>/realms/<realm-real>/.well-known/openid-configuration
```

La variable `KEYCLOAK_ISSUER` usa exactamente el `issuer` que devuelve ese JSON,
sin añadir `/.well-known/openid-configuration`. Flutter ya añade ese sufijo.

## Cliente del servidor

En el realm existente (no elimines ni recrees usuarios), comprueba que exista el
cliente público indicado abajo. Si falta, créalo; el repositorio no aprovisiona
ese cliente automáticamente:

| Opción | Valor |
| --- | --- |
| Client ID | `ti-soporte-flutter` |
| Client authentication | Off |
| Standard flow | On |
| Implicit flow | Off |
| Direct access grants | Off |
| PKCE challenge method | S256 |
| Valid redirect URIs | `com.tisoporte.accion:/oauthredirect` |
| Valid post logout redirect URIs | `com.tisoporte.accion:/oauthredirect` |

No se necesita un origen web para el cliente nativo. Configurar registro y
recuperación de contraseña en el realm si se desean esas opciones en su formulario.
El cliente anterior de la PWA puede deshabilitarse manualmente cuando se confirme
que no tiene otros consumidores. No se modifica el servidor automáticamente.

## Configurar Flutter sin guardar valores locales en Git

`String.fromEnvironment` se resuelve **al compilar**: Flutter no lee `.env` por
sí solo. Copia la plantilla desde `movil/`. El issuer local comprobado viene
prellenado; el Client ID queda vacío porque no está creado en Keycloak. Completa
ese campo en el archivo local ignorado por Git:

```powershell
Copy-Item keycloak.local.json.example keycloak.local.json
# Define KEYCLOAK_CLIENT_ID con el ID que registres en el realm existente.
flutter pub get
flutter run --dart-define-from-file=keycloak.local.json
```

El Client ID propuesto por la documentación es `ti-soporte-flutter`; úsalo solo
si lo creas con ese ID. La plantilla local permite HTTP en debug. No uses ese valor
para release: usa HTTPS y `KEYCLOAK_ALLOW_HTTP=false`.

El mismo archivo se pasa a una compilación así:

```powershell
flutter build apk --debug --dart-define-from-file=keycloak.local.json
```

## Acceder desde Android físico

`10.0.2.2` es la dirección especial del emulador Android y **no sirve para un
teléfono físico**. Para desarrollo USB, si el Keycloak local realmente está
escuchando en el PC en el puerto 8080, usa el puente de ADB:

```powershell
adb devices
adb reverse tcp:8080 tcp:8080
```

El discovery comprobado anuncia `http://localhost:8080/realms/TI%20Soporte%20en%20Acci%C3%B3n`.
Para el móvil por USB, usa ese mismo issuer junto con `adb reverse` para que el
teléfono alcance el servidor del PC. El Client ID seguirá pendiente hasta crearlo
en Keycloak. Después:

```powershell
flutter run --dart-define-from-file=keycloak.local.json
```

Alternativamente, usa un host HTTPS alcanzable desde el teléfono. El PC reportó
`192.168.1.87` como su IPv4 Wi-Fi durante esta inspección, pero esa dirección puede
cambiar y **no** es el issuer publicado por el servidor comprobado. Para usar LAN,
el teléfono y el PC deben estar en la misma red, el firewall permitir el puerto y
Keycloak debe anunciar en discovery esa autoridad pública. No cambies el issuer
por una IP sin configurar y volver a comprobar discovery.

## Ejecutar en emulador Android

Desde `movil/`, con un emulador Android y Keycloak local en el puerto 8080:

```powershell
flutter pub get
flutter run --dart-define-from-file=keycloak.local.json
```

Usa el realm que confirme el servidor, no un nombre supuesto. Para un emulador,
`10.0.2.2` reemplaza el host del PC si el discovery y Keycloak permiten esa URL;
el `issuer` que anuncie el servidor debe coincidir con el configurado en Flutter.

HTTP se permite solo como opción explícita de desarrollo y el manifiesto Android
lo habilita solo en debug. Para release e iOS usar un issuer HTTPS con certificado
válido. iOS requiere macOS/Xcode para compilar y verificar el retorno del navegador.

## Sesión

- Las contraseñas se introducen en Keycloak, nunca en un formulario local de Flutter.
- Login cancelado vuelve a la pantalla de acceso.
- Access/refresh/ID tokens se conservan cifrados mediante Android Keystore/iOS
  Keychain. Al reiniciar, se intenta renovar la sesión con el refresh token antes
  de mostrar contenido privado. Si Keycloak lo revocó o venció, se eliminan y
  se vuelve a solicitar el acceso.
- Se renueva el token antes de expirar y al volver del segundo plano. Si falla,
  se elimina la sesión local y las rutas privadas muestran el acceso.
- `AuthService.instance.accessToken()` es el punto de acceso para futuras llamadas
  a la API; no devuelve tokens vencidos. La API deberá validar firma, issuer,
  audiencia, expiración y permisos.
- Ajustes → Cerrar sesión elimina primero los tokens y solicita logout a Keycloak.
  Si el navegador no completa el logout, se informa que solo se cerró la sesión local.
- El perfil muestra la identidad de la sesión; XP, logros y misiones siguen siendo
  datos de demostración. No existe todavía sincronización con la API educativa.

## Comprobar en dispositivo

1. Iniciar sesión y confirmar retorno a `/home` e identidad en Perfil/Ajustes.
2. Cancelar un login: no debe abrir contenido privado.
3. Cerrar sesión y pulsar Atrás: no debe recuperar las rutas privadas.
4. Dejar vencer la sesión o revocarla en Keycloak y comprobar la renovación/salida.
5. Reiniciar la aplicación con un refresh token vigente y confirmar que restaura
   la sesión; revocar el token en Keycloak y confirmar que regresa al login.

Pruebas locales: `flutter analyze` y `flutter test`. Los tests con proveedor simulado
no sustituyen la validación del navegador y el servidor real. Las reglas definitivas
de roles y acceso a datos deben validarse también en la API; Flutter solo refleja los
roles del token para presentación.

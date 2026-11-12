# Keycloak en la aplicación Flutter

La integración directa pertenece a Flutter, no a la PWA. Se usa
`flutter_appauth` con Authorization Code + PKCE, navegador del sistema y cliente
público sin secreto. Implementación de acceso para **Android e iOS**; Windows y
Flutter web muestran un aviso de plataforma no compatible con este acceso móvil.

## Cliente del servidor

En el realm existente (no es necesario eliminar ni recrear usuarios), crear:

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

## Ejecutar en Android

Desde `movil/`, con un emulador Android y Keycloak local en el puerto 8080:

```powershell
flutter pub get
flutter run --dart-define="KEYCLOAK_ISSUER=http://10.0.2.2:8080/realms/ti-soporte" --dart-define="KEYCLOAK_CLIENT_ID=ti-soporte-flutter" --dart-define="KEYCLOAK_ALLOW_HTTP=true"
```

Usar el nombre **real** del realm: si se llama `TI Soporte en Acción`, el segmento
de URL será `TI%20Soporte%20en%20Acci%C3%B3n`. La URL issuer no incluye
`/.well-known/openid-configuration`; la app añade ese sufijo.

`10.0.2.2` apunta al PC desde el emulador Android. En teléfono físico, utilizar
la dirección accesible del PC o un dominio HTTPS. Revisar que el discovery publique
endpoints alcanzables desde el dispositivo y el mismo issuer configurado.
HTTP se permite solo como opción explícita de desarrollo y el manifiesto Android
lo habilita solo en debug. Para release e iOS usar un issuer HTTPS con certificado
válido. iOS requiere macOS/Xcode para compilar y verificar el retorno del navegador.

## Sesión

- Las contraseñas se introducen en Keycloak, nunca en un formulario local de Flutter.
- Login cancelado vuelve a la pantalla de acceso.
- Access/refresh/ID tokens se conservan solo en memoria. Al cerrar el proceso,
  se debe iniciar nuevamente el flujo; puede reutilizarse la sesión SSO del navegador.
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
5. Reiniciar la aplicación: no debe haber tokens persistidos.

Pruebas locales: `flutter analyze` y `flutter test`. Los tests con proveedor simulado
no sustituyen la validación del navegador y el servidor real.

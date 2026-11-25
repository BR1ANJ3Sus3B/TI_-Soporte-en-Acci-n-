# Flutter — estado de integración

## Implementado

- Bienvenida con invitado, login PKCE y registro nativo `/register`.
- Pantalla nativa de registro conectada a `/auth/register` y reenvío de correo.
- Lecturas públicas sin Bearer para catálogos; perfil y progreso cloud protegidos.
- Invitado con perfil y progreso local, sin consultas automáticas a perfil privado.
- El progreso local se aplica también al catálogo público recibido del servidor.
- Sesión segura con renovación, restauración, logout y rechazo de respuestas
  tardías de sesiones anteriores. Estado restaurado autenticado y logout signedOut.
- Carga, error, reintento, opciones de volver a login o continuar como invitado.
- Script PowerShell verifica los dart-defines y selecciona Android real.
- Tema Stitch, logo y fuentes locales conservados.

## Servicios y configuración comprobados durante estas tareas

- Realm usado: `ti-soporte`; issuer `http://localhost:8080/realms/ti-soporte`.
- Cliente móvil: `ti-soporte-flutter`; autorización aceptada con
  `openid ti-soporte-api`. API exige audiencia `ti-soporte-api`.
- API healthy, MongoDB conectado. Catálogos públicos devuelven 200; perfil sin
  token devuelve 401. Los catálogos Mongo comprobados están vacíos.
- JWT continúa validando firma RS256, issuer, audiencia, expiración y subject.
- Registro implementado pero bloqueado en la instancia local: falta credencial
  de servicio de Keycloak, SMTP sin verificar y HTTP de registro no habilitado.

## Pendientes para cerrar la entrega funcional

1. Configurar el cliente de servicio y SMTP siguiendo `NATIVE_REGISTRATION.md`.
   Es configuración del backend, no debe incluirse en el APK.
2. Completar registro, correo, login, callback, perfil y logout con una cuenta
   de prueba autorizada en Android conectado. Las pruebas con dobles no prueban
   ese recorrido real. No se han capturado pantallas reales del dispositivo.
3. Cargar contenido educativo aprobado: misiones, niveles, guías y logros. El
   mapa actual muestra áreas; aún no hay pantalla dedicada de niveles/guías.
4. Resultados Unity y recompensas oficiales: el servidor requiere revisión de
   intentos; no existe atestación del juego que permita conceder XP automáticamente.
5. iOS: validar permisos, callback, build y ejecución en macOS/Xcode.
6. Distribución Android: configurar firma release y endpoints HTTPS reales.

No se considera Flutter terminado de extremo a extremo mientras falten los pasos
1–3 y su verificación. No se sustituyen catálogos vacíos por datos oficiales falsos
ni se conceden recompensas al iniciar una misión.

## Ejecutar desde el worktree móvil

```powershell
& 'C:\Users\jesus\AppData\Local\Temp\opencode\ti-flutter-keycloak\scripts\run-flutter.ps1'
```

Para invitado sin servicios: añadir `-GuestOnly`. Para prevalidación: `-ValidateOnly`.
Para varios Android: `-DeviceId` con un ID real de `flutter devices`.

Ver `SESSION_DIAGNOSTICS.md` para el rechazo de sesión y `KEYCLOAK.md` para OIDC.

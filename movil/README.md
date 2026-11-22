# TI: Soporte en Acción · aplicación móvil

Aplicación Flutter del proyecto educativo ambientado en NovaTech Solutions. Por defecto solicita datos a la API; las pantallas de demostración local se activan explícitamente con `DEMO_MODE=true`.

## Tecnologías

- Flutter y Dart con Material 3.
- Sistema visual Stitch/Cybersecurity Neon con fuentes Space Grotesk, Space Mono e Inter.
- Cliente HTTP Bearer hacia la API; MongoDB permanece únicamente detrás del backend.

## Pantallas implementadas

- Splash e inicio de sesión móvil con Keycloak (Android/iOS).
- Dashboard, listado y detalle de misiones.
- Mapa de áreas de NovaTech.
- Perfil, progreso, detalle de logros y configuración.

La navegación usa rutas nombradas desde `lib/main.dart`. Las rutas privadas están
protegidas por `SessionGate`. El acceso usa AppAuth y Keycloak; el contenido educativo
sigue siendo demostrativo. Ver [configuración de Keycloak](KEYCLOAK.md).

## Estructura relevante

```text
lib/
├── data/mock_data.dart          # ejemplos usados solo en DEMO_MODE y tests
├── models/                      # modelos de dominio
├── screens/                     # pantallas del jugador
├── services/                    # sesión Keycloak, HTTP y repositorio de datos
├── theme/                       # AppColors y AppTheme
└── widgets/                     # navegación, tarjetas, XP, paneles y popup
test/
├── achievement_notification_test.dart
├── api_client_test.dart
├── api_model_test.dart
├── auth_service_test.dart
├── mission_ui_test.dart
├── stitch_design_test.dart
└── widget_test.dart
```

## Sistema visual

Los tokens oficiales están en `lib/theme/app_colors.dart`: fondo `#06132A`, superficie `#020D25`, contenedores `#0F1B33`, cian `#4AE4EC`, violeta para elementos RPG y colores semánticos para éxito, advertencia y error. La tipografía se organiza en `lib/theme/app_typography.dart`. Las fuentes OFL están empaquetadas en `assets/fonts/` para que funcionen sin red.

Los widgets reutilizables incluyen `NovaPanel`, `NovaPrimaryButton`, `DemoDataNotice`, `XpBar`, `MissionCard`, `StatCard`, chips de estado/dificultad, navegación inferior y la ventana de logro desbloqueado.

## Ejecución

Desde `movil/`:

```powershell
flutter pub get
Copy-Item keycloak.local.json.example keycloak.local.json
# Edita keycloak.local.json: completa el Client ID solo cuando exista en Keycloak.
flutter run --dart-define-from-file=keycloak.local.json
```

Flutter no carga variables desde `.env`; el archivo local se usa mediante
`--dart-define-from-file` y está ignorado por Git. Consulta
[KEYCLOAK.md](KEYCLOAK.md) para emulador, teléfono físico y ajustes del cliente.
Para iniciar API/MongoDB local (usa un volumen nuevo y privado):

```powershell
# Desde la raíz del repositorio; inicia primero Keycloak y el stack de monitoreo.
Copy-Item infra/api/.env.example infra/api/.env
docker compose -f infra/api/compose.yaml up -d --build --wait
```

En teléfono físico conectado por USB reenvía ambos puertos: `adb reverse tcp:8080 tcp:8080`
y `adb reverse tcp:3000 tcp:3000`. El cliente Flutter requiere que se cree en
Keycloak y que la API tenga configurada la audiencia del access token antes de que
las rutas protegidas devuelvan datos.

## Validación

```powershell
dart format lib test
flutter analyze
flutter test
flutter build apk --debug
```

Sin variables de Keycloak la app muestra la configuración pendiente. Para iniciar
sesión real, usar el comando de [KEYCLOAK.md](KEYCLOAK.md) en Android/iOS.

Las pruebas cubren el cliente HTTP, serialización/modelos, rutas, filtros,
autenticación con proveedor simulado y diseño. El inicio/cierre real aún requiere
el cliente de Keycloak y un dispositivo.

## Pendiente de integración verificado

- Crear el cliente público `ti-soporte-flutter` en el realm local `TI Soporte en Acción` y registrar el callback; el servidor aún responde `Client not found` para ese cliente.
- Añadir a Keycloak el Audience mapper que se acuerde y definir `KEYCLOAK_AUDIENCE` en `infra/api/.env`; hasta entonces las rutas protegidas de la API responden `503` por configuración OIDC incompleta.
- El backend Node/Express/Mongoose y el Compose API/Mongo están implementados y arrancan. Poblar el catálogo Mongo con misiones, niveles, guías y logros revisados; la API parte vacía y no carga ejemplos como datos reales.
- Integrar Unity como fuente verificable de intentos; el endpoint actual deja los reportes pendientes y un administrador debe aprobarlos antes de acreditar XP.
- Acordar el login web de la PWA y su consumo del mismo progreso; la sesión móvil no se comparte directamente con el navegador.
- Definir umbrales de nivel/XP y políticas de caché/sincronización offline antes de calcular progreso o sincronizar conflictos.
- Hacer una prueba real de login/logout en Android físico; no se detectó dispositivo durante esta validación. iOS requiere macOS/Xcode.

El detalle de bloqueos y contratos pendientes está en [INTEGRATION_STATUS.md](INTEGRATION_STATUS.md).

Los datos simulados solo se activan con `--dart-define=DEMO_MODE=true`; el modo normal consulta la API y muestra carga, error, vacío y reintento. La autorización de recursos sigue siendo responsabilidad del backend.

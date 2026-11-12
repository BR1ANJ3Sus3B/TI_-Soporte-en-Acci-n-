# TI: Soporte en Acción · aplicación móvil

Aplicación Flutter del proyecto educativo ambientado en NovaTech Solutions. Presenta el recorrido del jugador con datos simulados: inicio, misiones, mapa de áreas, progreso, perfil, logros y ajustes.

## Tecnologías

- Flutter y Dart con Material 3.
- Tema oscuro NovaTech y componentes reutilizables.
- Datos mock locales; no hay dependencia directa de MongoDB.

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
├── data/mock_data.dart          # jugador, misiones, zonas, personajes y logros
├── models/                      # modelos de dominio
├── screens/                     # pantallas del jugador
├── services/                    # sesión Keycloak y cola de notificaciones de logros
├── theme/                       # AppColors y AppTheme
└── widgets/                     # navegación, tarjetas, XP, paneles y popup
test/
├── achievement_notification_test.dart
└── widget_test.dart
```

## Sistema visual

La paleta está centralizada en `lib/theme/app_colors.dart`: fondo `#0B1120`, superficies `#111827`, tarjetas `#1F2937`, azul `#2563EB`, cian `#06B6D4`, verde de éxito, ámbar de advertencia y rojo de error. Se conservan los degradados fondo oscuro y azul-cian para acciones y experiencia.

Los widgets reutilizables incluyen `NovaPanel`, `NovaPrimaryButton`, `XpBar`, `MissionCard`, `StatCard`, chips de estado/dificultad, navegación inferior y la ventana de logro desbloqueado.

## Ejecución

Desde `movil/`:

```bash
flutter pub get
flutter run
```

## Validación

```bash
dart format .
flutter analyze
flutter test
```

Sin variables de Keycloak la app muestra la configuración pendiente. Para iniciar
sesión real, usar el comando de [KEYCLOAK.md](KEYCLOAK.md) en Android/iOS.

Las pruebas cubren el arranque, las notificaciones y el ciclo de sesión con un
proveedor simulado. El inicio/cierre real debe comprobarse en un dispositivo.

## Pendiente de integración

- Configurar el cliente móvil en el servidor Keycloak y validar el recorrido real.
- API REST, MongoDB y persistencia remota.
- Roles y permisos reales (Administrador, Docente, Evaluador, Editor y Soporte).
- Sincronización de progreso y contenido remoto.

Las pantallas actuales mantienen el acceso del jugador y usan datos mock; los roles futuros no se simulan como un sistema de seguridad real.

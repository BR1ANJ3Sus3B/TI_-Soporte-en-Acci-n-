# TI: Soporte en Acción · aplicación móvil

Aplicación Flutter del proyecto educativo ambientado en NovaTech Solutions. Presenta el recorrido del jugador con datos simulados: inicio, misiones, mapa de áreas, progreso, perfil, logros y ajustes.

## Tecnologías

- Flutter y Dart con Material 3.
- Tema oscuro NovaTech y componentes reutilizables.
- Datos mock locales; no hay dependencia directa de MongoDB.

## Pantallas implementadas

- Splash y acceso visual (inicio de sesión o invitado).
- Dashboard, listado y detalle de misiones.
- Mapa de áreas de NovaTech.
- Perfil, progreso, detalle de logros y configuración.

La navegación usa rutas nombradas desde `lib/main.dart`. El acceso es demostrativo y no representa autenticación real.

## Estructura relevante

```text
lib/
├── data/mock_data.dart          # jugador, misiones, zonas, personajes y logros
├── models/                      # modelos de dominio
├── screens/                     # pantallas del jugador
├── services/                    # cola de notificaciones de logros
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

Las pruebas actuales cubren el arranque de la app y la cola de notificaciones: no duplica logros durante la sesión y entrega la cola en orden.

## Pendiente de integración

- Keycloak y autenticación real.
- API REST, MongoDB y persistencia remota.
- Roles y permisos reales (Administrador, Docente, Evaluador, Editor y Soporte).
- Sincronización de progreso y contenido remoto.

Las pantallas actuales mantienen el acceso del jugador y usan datos mock; los roles futuros no se simulan como un sistema de seguridad real.

# TI: Soporte en Acción · aplicación móvil

Aplicación Flutter del proyecto educativo ambientado en NovaTech Solutions. Presenta el recorrido del jugador con datos simulados: inicio, misiones, mapa de áreas, progreso, perfil, logros y ajustes.

## Tecnologías

- Flutter y Dart con Material 3.
- Sistema visual Stitch/Cybersecurity Neon con fuentes Space Grotesk, Space Mono e Inter.
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

- Configurar el cliente móvil en el servidor Keycloak y validar el recorrido real en dispositivo.
- Implementar API REST; actualmente `api/` solo contiene README y `.gitkeep`, sin servidor ni endpoints.
- No conectar Flutter directamente a MongoDB; la base solo estará detrás de la API.
- Sustituir misiones, mapas, logros, XP, habilidades y actividad simulados por respuestas documentadas de API.
- Acordar contratos de perfil, catálogo, niveles, progreso y logros antes de implementar repositorios.
- Aplicar permisos de forma autoritativa en la API; la app solo muestra roles del token.
- Sincronizar resultados verificados de Unity y contenido de la PWA mediante la API común.
- Definir caché offline/sync y resolución de conflicto cuando el backend establezca su fuente de verdad.

El detalle de bloqueos y contratos pendientes está en [INTEGRATION_STATUS.md](INTEGRATION_STATUS.md).

Las pantallas actuales mantienen el acceso del jugador y usan datos mock; los roles futuros no se simulan como un sistema de seguridad real.

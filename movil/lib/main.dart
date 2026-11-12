import 'package:flutter/material.dart';
import 'package:ti_soporte_accion/screens/achievement_detail_screen.dart';
import 'package:ti_soporte_accion/screens/home_screen.dart';
import 'package:ti_soporte_accion/screens/login_screen.dart';
import 'package:ti_soporte_accion/screens/map_screen.dart';
import 'package:ti_soporte_accion/screens/mission_detail_screen.dart';
import 'package:ti_soporte_accion/screens/missions_screen.dart';
import 'package:ti_soporte_accion/screens/profile_screen.dart';
import 'package:ti_soporte_accion/screens/progress_screen.dart';
import 'package:ti_soporte_accion/screens/settings_screen.dart';
import 'package:ti_soporte_accion/screens/splash_screen.dart';
import 'package:ti_soporte_accion/theme/app_theme.dart';
import 'package:ti_soporte_accion/widgets/achievement_unlock_popup.dart';
import 'package:ti_soporte_accion/widgets/session_gate.dart';

/// Clave global del navegador raíz.
///
/// Permite abrir pantallas (por ejemplo, el detalle de un logro) desde la capa
/// de notificaciones, que se dibuja por encima de todas las rutas.
final GlobalKey<NavigatorState> appNavigatorKey = GlobalKey<NavigatorState>();

void main() {
  runApp(const TiSoporteApp());
}

/// Aplicación principal "TI: Soporte en Acción".
///
/// Acceso con Keycloak; el contenido educativo aún usa datos demostrativos.
class TiSoporteApp extends StatelessWidget {
  const TiSoporteApp({super.key});

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'TI: Soporte en Acción',
      debugShowCheckedModeBanner: false,
      navigatorKey: appNavigatorKey,
      theme: AppTheme.dark,
      initialRoute: SplashScreen.routeName,
      builder: (BuildContext context, Widget? child) {
        return AchievementPopupHost(
          navigatorKey: appNavigatorKey,
          child: child ?? const SizedBox.shrink(),
        );
      },
      routes: <String, WidgetBuilder>{
        SplashScreen.routeName: (_) => const SplashScreen(),
        LoginScreen.routeName: (_) => const LoginScreen(),
        HomeScreen.routeName: (_) => const SessionGate(child: HomeScreen()),
        MissionsScreen.routeName: (_) =>
            const SessionGate(child: MissionsScreen()),
        MissionDetailScreen.routeName: (_) =>
            const SessionGate(child: MissionDetailScreen()),
        MapScreen.routeName: (_) => const SessionGate(child: MapScreen()),
        ProfileScreen.routeName: (_) =>
            const SessionGate(child: ProfileScreen()),
        ProgressScreen.routeName: (_) =>
            const SessionGate(child: ProgressScreen()),
        SettingsScreen.routeName: (_) =>
            const SessionGate(child: SettingsScreen()),
        AchievementDetailScreen.routeName: (_) =>
            const SessionGate(child: AchievementDetailScreen()),
      },
    );
  }
}

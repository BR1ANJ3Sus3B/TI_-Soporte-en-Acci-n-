import 'package:flutter/material.dart';
import 'package:ti_soporte_accion/screens/login_screen.dart';
import 'package:ti_soporte_accion/services/auth_service.dart';

/// Cada ruta de la aplicación comprueba la sesión, incluso por navegación directa.
class SessionGate extends StatefulWidget {
  const SessionGate({required this.child, super.key});
  final Widget child;

  @override
  State<SessionGate> createState() => _SessionGateState();
}

class _SessionGateState extends State<SessionGate> with WidgetsBindingObserver {
  @override
  void initState() {
    super.initState();
    WidgetsBinding.instance.addObserver(this);
  }

  @override
  void didChangeAppLifecycleState(AppLifecycleState state) {
    if (state == AppLifecycleState.resumed) {
      AuthService.instance.accessToken().then((_) {
        if (mounted) setState(() {});
      });
    }
  }

  @override
  void dispose() {
    WidgetsBinding.instance.removeObserver(this);
    super.dispose();
  }

  @override
  Widget build(BuildContext context) => ListenableBuilder(
    listenable: AuthService.instance,
    builder: (_, _) => AuthService.instance.isAuthenticated
        ? widget.child
        : const LoginScreen(),
  );
}

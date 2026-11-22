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
        : const _AccountRequired(),
  );
}

class _AccountRequired extends StatelessWidget {
  const _AccountRequired();

  @override
  Widget build(BuildContext context) => Scaffold(
    body: Center(
      child: Padding(
        padding: const EdgeInsets.all(24),
        child: Column(
          mainAxisSize: MainAxisSize.min,
          children: <Widget>[
            const Icon(Icons.cloud_sync_outlined, size: 48),
            const SizedBox(height: 16),
            const Text(
              'Crea una cuenta para sincronizar tu progreso y administrar tu perfil.',
              textAlign: TextAlign.center,
            ),
            const SizedBox(height: 16),
            FilledButton(
              onPressed: () => Navigator.of(context).pushNamedAndRemoveUntil(
                LoginScreen.routeName,
                (_) => false,
              ),
              child: const Text('Ver opciones de cuenta'),
            ),
            TextButton(
              onPressed: () => Navigator.of(context).pushNamedAndRemoveUntil(
                '/home',
                (_) => false,
              ),
              child: const Text('Volver al contenido público'),
            ),
          ],
        ),
      ),
    ),
  );
}

import 'package:flutter/material.dart';
import 'package:ti_soporte_accion/services/auth_service.dart';
import 'package:ti_soporte_accion/theme/app_colors.dart';
import 'package:ti_soporte_accion/widgets/nova_panel.dart';

class LoginScreen extends StatefulWidget {
  const LoginScreen({super.key});
  static const String routeName = '/login';

  @override
  State<LoginScreen> createState() => _LoginScreenState();
}

class _LoginScreenState extends State<LoginScreen> {
  bool _busy = false;
  String? _error;

  Future<void> _login() async {
    if (_busy) return;
    setState(() {
      _busy = true;
      _error = null;
    });
    try {
      final success = await AuthService.instance.login();
      if (success && mounted) {
        Navigator.of(context).pushNamedAndRemoveUntil('/home', (_) => false);
      }
    } catch (_) {
      if (mounted) {
        setState(
          () => _error = 'No se pudo iniciar sesión. Revisa la conexión y la configuración de Keycloak.',
        );
      }
    } finally {
      if (mounted) setState(() => _busy = false);
    }
  }

  @override
  Widget build(BuildContext context) {
    final auth = AuthService.instance;
    return Scaffold(
      body: Container(
        decoration: const BoxDecoration(gradient: AppColors.backgroundGradient),
        child: SafeArea(
          child: Center(
            child: SingleChildScrollView(
              padding: const EdgeInsets.all(24),
              child: ConstrainedBox(
                constraints: const BoxConstraints(maxWidth: 430),
                child: NovaPanel(
                  child: Column(
                    mainAxisSize: MainAxisSize.min,
                    crossAxisAlignment: CrossAxisAlignment.stretch,
                    children: <Widget>[
                      const Icon(
                        Icons.support_agent_rounded,
                        size: 58,
                        color: AppColors.cyan,
                      ),
                      const SizedBox(height: 20),
                      const Text(
                        'TI: Soporte en Acción',
                        textAlign: TextAlign.center,
                        style: TextStyle(
                          fontSize: 26,
                          fontWeight: FontWeight.bold,
                        ),
                      ),
                      const SizedBox(height: 16),
                      const Text(
                        'Inicia sesión con tu cuenta de NovaTech. Las credenciales '
                        'se solicitan en Keycloak mediante el navegador del dispositivo.',
                      ),
                      const SizedBox(height: 24),
                      ElevatedButton.icon(
                        onPressed:
                            !_busy && auth.isConfigured && auth.isSupported
                            ? _login
                            : null,
                        icon: const Icon(Icons.login),
                        label: Text(
                          _busy
                              ? 'Iniciando sesión…'
                              : 'Iniciar sesión con Keycloak',
                        ),
                      ),
                      if (_busy) const LinearProgressIndicator(),
                      if (!auth.isSupported)
                        const Text(
                          'El inicio de sesión móvil está disponible en Android e iOS.',
                        ),
                      if (!auth.isConfigured)
                        const Text(
                          'Configuración pendiente: define KEYCLOAK_ISSUER y '
                          'KEYCLOAK_CLIENT_ID al ejecutar Flutter.',
                        ),
                      if (_error != null)
                        Text(
                          _error!,
                          style: const TextStyle(color: Colors.redAccent),
                        ),
                      const SizedBox(height: 16),
                      const Text(
                        'Misiones y progreso siguen siendo datos de demostración '
                        'hasta conectar la API educativa.',
                        textAlign: TextAlign.center,
                      ),
                    ],
                  ),
                ),
              ),
            ),
          ),
        ),
      ),
    );
  }
}

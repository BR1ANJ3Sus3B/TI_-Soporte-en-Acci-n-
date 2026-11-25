import 'package:flutter/material.dart';
import 'package:ti_soporte_accion/services/auth_service.dart';
import 'package:ti_soporte_accion/theme/app_colors.dart';
import 'package:ti_soporte_accion/theme/app_typography.dart';
import 'package:ti_soporte_accion/widgets/nova_panel.dart';

/// Entrada pública: la cuenta es optativa y el invitado no toca Keycloak.
class LoginScreen extends StatefulWidget {
  const LoginScreen({super.key});
  static const String routeName = '/login';

  @override
  State<LoginScreen> createState() => _LoginScreenState();
}

class _LoginScreenState extends State<LoginScreen> {
  bool _busy = false;
  String? _message;

  Future<void> _continueAsGuest() async {
    if (_busy) return;
    setState(() => _busy = true);
    await AuthService.instance.continueAsGuest();
    if (mounted) {
      Navigator.of(context).pushNamedAndRemoveUntil('/home', (_) => false);
    }
  }

  Future<void> _login() async {
    final auth = AuthService.instance;
    if (_busy) return;
    if (!auth.isSupported || !auth.isConfigured) {
      setState(
        () => _message = 'El inicio de sesión no está disponible en esta compilación. Puedes continuar como invitado.',
      );
      return;
    }
    setState(() {
      _busy = true;
      _message = null;
    });
    try {
      final success = await auth.login();
      if (success && mounted) {
        Navigator.of(context).pushNamedAndRemoveUntil('/home', (_) => false);
      }
    } catch (_) {
      if (mounted) {
        setState(
          () => _message = 'No fue posible iniciar sesión. Inténtalo más tarde o continúa como invitado.',
        );
      }
    } finally {
      if (mounted) setState(() => _busy = false);
    }
  }

  @override
  Widget build(BuildContext context) => Scaffold(
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
                  crossAxisAlignment: CrossAxisAlignment.stretch,
                  children: <Widget>[
                    const Icon(
                      Icons.support_agent_rounded,
                      size: 62,
                      color: AppColors.cyan,
                    ),
                    const SizedBox(height: 16),
                    Text(
                      'TI: Soporte en Acción',
                      textAlign: TextAlign.center,
                      style: AppTypography.heading(27),
                    ),
                    const SizedBox(height: 6),
                    Text(
                      'NOVATECH SOLUTIONS',
                      textAlign: TextAlign.center,
                      style: AppTypography.label(12, color: AppColors.cyan),
                    ),
                    const SizedBox(height: 18),
                    Text(
                      'Aprende a resolver retos de soporte TI, explora misiones y avanza a tu ritmo.',
                      textAlign: TextAlign.center,
                      style: AppTypography.body(
                        15,
                        color: AppColors.textSecondary,
                      ),
                    ),
                    const SizedBox(height: 26),
                    FilledButton.icon(
                      onPressed: _busy ? null : _continueAsGuest,
                      icon: const Icon(Icons.play_arrow_rounded),
                      label: const Text('Comenzar como invitado'),
                    ),
                    const SizedBox(height: 10),
                    OutlinedButton.icon(
                      onPressed:
                          _busy ||
                              !AuthService.instance.isSupported ||
                              !AuthService.instance.isConfigured
                          ? null
                          : _login,
                      icon: const Icon(Icons.login_rounded),
                      label: const Text('Iniciar sesión'),
                    ),
                    TextButton(
                      onPressed: _busy
                          ? null
                          : () => Navigator.of(context).pushNamed('/register'),
                      child: const Text('Crear una cuenta gratis'),
                    ),
                    const SizedBox(height: 12),
                    const Text(
                      'Con una cuenta puedes sincronizar tu progreso entre dispositivos, personalizar tu perfil y obtener logros verificados. Como invitado, tu progreso permanece solo en este dispositivo.',
                      textAlign: TextAlign.center,
                      style: TextStyle(
                        color: AppColors.textSecondary,
                        fontSize: 12,
                        height: 1.4,
                      ),
                    ),
                    if (_busy)
                      const Padding(
                        padding: EdgeInsets.only(top: 14),
                        child: LinearProgressIndicator(),
                      ),
                    if (_message != null)
                      Padding(
                        padding: const EdgeInsets.only(top: 14),
                        child: Text(
                          _message!,
                          textAlign: TextAlign.center,
                          style: AppTypography.body(
                            13,
                            color: AppColors.warning,
                          ),
                        ),
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

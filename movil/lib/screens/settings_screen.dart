import 'package:flutter/material.dart';
import 'package:ti_soporte_accion/services/auth_service.dart';
import 'package:ti_soporte_accion/services/app_data_repository.dart';
import 'package:ti_soporte_accion/theme/app_colors.dart';
import 'package:ti_soporte_accion/theme/app_typography.dart';
import 'package:ti_soporte_accion/widgets/custom_bottom_nav.dart';
import 'package:ti_soporte_accion/widgets/demo_data_notice.dart';
import 'package:ti_soporte_accion/widgets/nova_panel.dart';

/// Ajustes reales disponibles en esta fase: cuenta, sesión y versión.
class SettingsScreen extends StatelessWidget {
  const SettingsScreen({super.key});

  static const String routeName = '/settings';

  Future<void> _logout(BuildContext context) async {
    final navigator = Navigator.of(context);
    final messenger = ScaffoldMessenger.of(context);
    try {
      await AuthService.instance.logout();
    } catch (_) {
      messenger.showSnackBar(
        const SnackBar(
          content: Text(
            'No se completó el cierre de sesión de forma segura. Revisa la conexión e inténtalo de nuevo.',
          ),
        ),
      );
    }
    if (!navigator.mounted) return;
    navigator.pushNamedAndRemoveUntil('/login', (_) => false);
  }

  @override
  Widget build(BuildContext context) {
    final auth = AuthService.instance;
    final repository = AppDataRepository.instance;
    return Scaffold(
      appBar: AppBar(title: const Text('Ajustes')),
      body: Container(
        decoration: const BoxDecoration(gradient: AppColors.backgroundGradient),
        child: SafeArea(
          child: ListView(
            padding: const EdgeInsets.all(20),
            children: <Widget>[
              Text('Cuenta', style: Theme.of(context).textTheme.headlineSmall),
              const SizedBox(height: 12),
              NovaPanel(
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    Row(
                      children: [
                        const CircleAvatar(
                          backgroundColor: AppColors.surfaceDark,
                          child: Icon(
                            Icons.person,
                            color: AppColors.primaryCyan,
                          ),
                        ),
                        const SizedBox(width: 12),
                        Expanded(
                          child: Column(
                            crossAxisAlignment: CrossAxisAlignment.start,
                            children: [
                              Text(
                                auth.displayName,
                                style: Theme.of(context).textTheme.titleMedium,
                              ),
                              const SizedBox(height: 4),
                              Text(
                                auth.email.isEmpty
                                    ? 'Correo no disponible'
                                    : auth.email,
                                style: Theme.of(context).textTheme.bodySmall
                                    ?.copyWith(color: AppColors.textSecondary),
                              ),
                            ],
                          ),
                        ),
                      ],
                    ),
                    const Divider(height: 24),
                    _AccountValue(
                      label: 'Rol indicado por Keycloak',
                      value: auth.roleLabel,
                    ),
                    const SizedBox(height: 10),
                    _AccountValue(
                      label: 'Sesión',
                      value: auth.isAuthenticated
                          ? 'Autenticada'
                          : 'Sin sesión',
                    ),
                    const SizedBox(height: 16),
                    SizedBox(
                      width: double.infinity,
                      child: OutlinedButton.icon(
                        onPressed: () => _logout(context),
                        icon: const Icon(Icons.logout_rounded),
                        label: const Text('Cerrar sesión'),
                      ),
                    ),
                  ],
                ),
              ),
              const SizedBox(height: 20),
              Text(
                'Datos y sincronización',
                style: Theme.of(context).textTheme.headlineSmall,
              ),
              const SizedBox(height: 12),
              if (repository.demoMode)
                const DemoDataNotice(
                  message:
                      'Modo de demostración activo. '
                      'Los datos educativos no se sincronizan con Unity ni la PWA.',
                )
              else
                Text(
                  repository.apiConfigurationIssue ??
                      'API_BASE_URL configurada; el acceso real depende de que API/MongoDB '
                          'estén activos y Keycloak emita la audiencia requerida.',
                  style: AppTypography.label(
                    13,
                    color: repository.apiConfigured
                        ? AppColors.warning
                        : AppColors.error,
                  ),
                ),
              const SizedBox(height: 20),
              Text(
                'Privacidad y seguridad',
                style: Theme.of(context).textTheme.headlineSmall,
              ),
              const SizedBox(height: 12),
              const NovaPanel(
                child: Text(
                  'La contraseña se solicita directamente a Keycloak. Los tokens '
                  'se guardan en Android Keystore / iOS Keychain. La API debe validar '
                  'firma, issuer, audiencia, expiración y permisos en cada solicitud.',
                  style: TextStyle(color: AppColors.textSecondary, height: 1.5),
                ),
              ),
              const SizedBox(height: 20),
              const _AccountValue(label: 'Versión', value: '1.0.0+1'),
              const SizedBox(height: 100),
            ],
          ),
        ),
      ),
      bottomNavigationBar: CustomBottomNav(
        currentIndex: 4,
        onTap: (int index) {
          const routes = <String>[
            '/home',
            '/missions',
            '/map',
            '/progress',
            '/settings',
          ];
          if (index == 4) return;
          Navigator.of(context).pushReplacementNamed(routes[index]);
        },
      ),
    );
  }
}

class _AccountValue extends StatelessWidget {
  const _AccountValue({required this.label, required this.value});
  final String label;
  final String value;

  @override
  Widget build(BuildContext context) => Row(
    mainAxisAlignment: MainAxisAlignment.spaceBetween,
    children: [
      Expanded(
        child: Text(
          label,
          style: const TextStyle(color: AppColors.textSecondary),
        ),
      ),
      const SizedBox(width: 12),
      Flexible(
        child: Text(
          value,
          textAlign: TextAlign.end,
          style: const TextStyle(
            color: AppColors.textPrimary,
            fontWeight: FontWeight.w600,
          ),
        ),
      ),
    ],
  );
}

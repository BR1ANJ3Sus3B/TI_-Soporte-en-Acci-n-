import 'package:flutter/material.dart';
import 'package:ti_soporte_accion/models/player_profile.dart';
import 'package:ti_soporte_accion/services/app_data_repository.dart';
import 'package:ti_soporte_accion/services/auth_service.dart';
import 'package:ti_soporte_accion/theme/app_colors.dart';
import 'package:ti_soporte_accion/theme/app_typography.dart';
import 'package:ti_soporte_accion/widgets/async_content.dart';
import 'package:ti_soporte_accion/widgets/custom_bottom_nav.dart';
import 'package:ti_soporte_accion/widgets/demo_data_notice.dart';
import 'package:ti_soporte_accion/widgets/nova_panel.dart';
import 'package:ti_soporte_accion/widgets/stat_card.dart';

class ProfileScreen extends StatelessWidget {
  const ProfileScreen({super.key});
  static const String routeName = '/profile';

  @override
  Widget build(BuildContext context) {
    final repository = AppDataRepository.instance;
    return Scaffold(
      body: Container(
        decoration: const BoxDecoration(gradient: AppColors.backgroundGradient),
        child: SafeArea(
          child: AsyncContent<PlayerProfile>(
            load: repository.getProfile,
            builder: (context, profile) => SingleChildScrollView(
              padding: const EdgeInsets.all(20),
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  Row(
                    children: [
                      Expanded(
                        child: Text('Perfil', style: AppTypography.heading(24)),
                      ),
                      IconButton(
                        tooltip: 'Abrir ajustes',
                        onPressed: () =>
                            Navigator.of(context).pushNamed('/settings'),
                        icon: const Icon(Icons.settings_outlined),
                      ),
                      IconButton(
                        tooltip: repository.demoMode
                            ? 'La edición requiere el perfil del servidor'
                            : 'Editar nombre visible',
                        onPressed: repository.demoMode
                            ? null
                            : () => _editDisplayName(
                                context,
                                repository,
                                profile.name,
                              ),
                        icon: const Icon(Icons.edit_outlined),
                      ),
                    ],
                  ),
                  if (repository.demoMode) ...[
                    const DemoDataNotice(
                      message:
                          'Estadísticas de ejemplo; la identidad '
                          'de la cuenta sí procede de la sesión Keycloak.',
                    ),
                    const SizedBox(height: 16),
                  ],
                  NovaPanel(
                    child: Row(
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
                                profile.name,
                                style: AppTypography.heading(18),
                              ),
                              const SizedBox(height: 4),
                              Text(
                                profile.rank,
                                style: AppTypography.label(13),
                              ),
                              const SizedBox(height: 4),
                              Text(
                                AuthService.instance.email.isEmpty
                                    ? 'Correo no disponible'
                                    : AuthService.instance.email,
                                style: AppTypography.label(12),
                              ),
                            ],
                          ),
                        ),
                      ],
                    ),
                  ),
                  const SizedBox(height: 20),
                  Text(
                    'Estadísticas educativas',
                    style: AppTypography.heading(19),
                  ),
                  const SizedBox(height: 8),
                  GridView.count(
                    crossAxisCount: 2,
                    shrinkWrap: true,
                    physics: const NeverScrollableScrollPhysics(),
                    crossAxisSpacing: 12,
                    mainAxisSpacing: 12,
                    childAspectRatio: 1.45,
                    children: [
                      StatCard(
                        title: 'Misiones completadas',
                        value: _displayCount(profile.missionsCompleted),
                        icon: Icons.task_alt_rounded,
                        color: AppColors.successLight,
                      ),
                      StatCard(
                        title: 'Incidentes aprobados',
                        value: _displayCount(profile.incidentsResolved),
                        icon: Icons.verified_rounded,
                        color: AppColors.primaryCyan,
                      ),
                    ],
                  ),
                  const SizedBox(height: 12),
                  NovaPanel(
                    child: Column(
                      crossAxisAlignment: CrossAxisAlignment.start,
                      children: [
                        Text(
                          profile.level == null
                              ? 'Nivel no disponible'
                              : 'Nivel ${profile.level}',
                          style: AppTypography.heading(17),
                        ),
                        const SizedBox(height: 8),
                        Text(
                          profile.currentXp == null
                              ? 'XP no disponible'
                              : 'XP: ${profile.currentXp}',
                          style: AppTypography.metric(13),
                        ),
                        if (profile.xpForNextLevel == null)
                          Padding(
                            padding: const EdgeInsets.only(top: 6),
                            child: Text(
                              'El backend no define el umbral del siguiente nivel.',
                              style: AppTypography.label(12),
                            ),
                          ),
                      ],
                    ),
                  ),
                  const SizedBox(height: 100),
                ],
              ),
            ),
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
            '/profile',
          ];
          if (index != 4) {
            Navigator.of(context).pushReplacementNamed(routes[index]);
          }
        },
      ),
    );
  }

  static String _displayCount(int? count) => count?.toString() ?? '—';

  static Future<void> _editDisplayName(
    BuildContext context,
    AppDataRepository repository,
    String currentName,
  ) async {
    final controller = TextEditingController(text: currentName);
    String? validationMessage;
    final newName = await showDialog<String>(
      context: context,
      builder: (dialogContext) => StatefulBuilder(
        builder: (context, setDialogState) => AlertDialog(
          title: const Text('Nombre visible'),
          content: Column(
            mainAxisSize: MainAxisSize.min,
            children: [
              TextField(
                controller: controller,
                autofocus: true,
                maxLength: 80,
                textCapitalization: TextCapitalization.words,
                decoration: const InputDecoration(labelText: 'Nombre'),
              ),
              if (validationMessage != null)
                Text(
                  validationMessage!,
                  style: const TextStyle(color: AppColors.error),
                ),
            ],
          ),
          actions: [
            TextButton(
              onPressed: () => Navigator.pop(dialogContext),
              child: const Text('Cancelar'),
            ),
            FilledButton(
              onPressed: () {
                final value = controller.text.trim();
                if (value.isEmpty) {
                  setDialogState(
                    () => validationMessage = 'El nombre no puede estar vacío.',
                  );
                  return;
                }
                Navigator.pop(dialogContext, value);
              },
              child: const Text('Guardar'),
            ),
          ],
        ),
      ),
    );
    controller.dispose();
    if (newName == null || !context.mounted) {
      return;
    }
    try {
      await repository.updateDisplayName(newName);
      if (!context.mounted) return;
      ScaffoldMessenger.of(context).showSnackBar(
        const SnackBar(content: Text('Nombre actualizado en el servidor.')),
      );
      Navigator.of(context).pushReplacementNamed(ProfileScreen.routeName);
    } catch (error) {
      if (!context.mounted) return;
      ScaffoldMessenger.of(context)
          .showSnackBar(SnackBar(content: Text(error.toString())));
    }
  }
}

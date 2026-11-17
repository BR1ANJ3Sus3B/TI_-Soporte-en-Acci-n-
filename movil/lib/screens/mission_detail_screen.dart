import 'package:flutter/material.dart';
import 'package:ti_soporte_accion/data/mock_data.dart';
import 'package:ti_soporte_accion/models/mission.dart';
import 'package:ti_soporte_accion/theme/app_colors.dart';
import 'package:ti_soporte_accion/widgets/chips.dart';
import 'package:ti_soporte_accion/widgets/demo_data_notice.dart';
import 'package:ti_soporte_accion/theme/app_typography.dart';

/// Pantalla que muestra el detalle completo de una misión.
class MissionDetailScreen extends StatelessWidget {
  const MissionDetailScreen({super.key});

  static const String routeName = '/mission-detail';

  @override
  Widget build(BuildContext context) {
    final id = ModalRoute.of(context)?.settings.arguments as String?;
    final matches = MockData.missions.where((Mission m) => m.id == id);
    if (matches.isEmpty) {
      return Scaffold(
        appBar: AppBar(title: const Text('Detalle de misión')),
        body: const Center(
          child: Text('No se encontró esa misión de demostración.'),
        ),
      );
    }
    final mission = matches.first;

    return Scaffold(
      appBar: AppBar(
        title: const Text('Detalle de misión'),
        backgroundColor: Colors.transparent,
      ),
      body: Container(
        decoration: const BoxDecoration(gradient: AppColors.backgroundGradient),
        child: SafeArea(
          child: SingleChildScrollView(
            padding: const EdgeInsets.all(20),
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: <Widget>[
                const DemoDataNotice(
                  message:
                      'Este detalle procede del catálogo '
                      'local de ejemplo; el servidor todavía no ha confirmado la misión ni su recompensa.',
                ),
                const SizedBox(height: 16),
                Text(mission.title, style: AppTypography.heading(23)),
                const SizedBox(height: 12),
                Text(
                  mission.description,
                  style: const TextStyle(
                    color: AppColors.textSecondary,
                    fontSize: 14,
                    height: 1.6,
                  ),
                ),
                const SizedBox(height: 16),
                Wrap(
                  spacing: 10,
                  runSpacing: 10,
                  children: <Widget>[
                    StatusChip(
                      status: mission.status,
                      text: mission.isCompleted
                          ? 'Completada'
                          : (mission.status == MissionStatus.inProgress
                                ? 'En curso'
                                : 'Disponible'),
                    ),
                    DifficultyChip(
                      label: mission.difficultyLabel,
                      color: mission.difficultyColor,
                    ),
                    Chip(
                      avatar: const Icon(
                        Icons.schedule_rounded,
                        size: 16,
                        color: AppColors.cyan,
                      ),
                      label: Text(mission.estimatedTime),
                      backgroundColor: AppColors.cyan.withValues(alpha: 0.2),
                      side: const BorderSide(color: AppColors.cyan),
                      labelStyle: const TextStyle(
                        color: AppColors.cyan,
                        fontWeight: FontWeight.w600,
                        fontSize: 12,
                      ),
                    ),
                    Chip(
                      avatar: const Icon(
                        Icons.rocket_launch_rounded,
                        size: 16,
                        color: AppColors.cyan,
                      ),
                      label: Text('${mission.xpReward} XP disponible'),
                      backgroundColor: AppColors.cyan.withValues(alpha: 0.2),
                      side: const BorderSide(color: AppColors.cyan),
                      labelStyle: const TextStyle(
                        color: AppColors.cyan,
                        fontWeight: FontWeight.w600,
                        fontSize: 12,
                      ),
                    ),
                  ],
                ),
                const SizedBox(height: 20),
                _InfoRow(
                  icon: Icons.apartment_rounded,
                  title: 'Departamento',
                  value: mission.department,
                ),
                const Divider(color: AppColors.border, height: 32),
                _InfoRow(
                  icon: Icons.people_rounded,
                  title: 'Usuario afectado',
                  value: mission.affectedUser,
                ),
                const Divider(color: AppColors.border, height: 32),
                _InfoRow(
                  icon: Icons.map_rounded,
                  title: 'Área',
                  value: mission.area,
                ),
                const Divider(color: AppColors.border, height: 32),
                _InfoRow(
                  icon: Icons.trending_up_rounded,
                  title: 'Dificultad',
                  value: mission.difficultyLabel,
                ),
                const SizedBox(height: 24),
                Text('Objetivos', style: AppTypography.heading(19)),
                const SizedBox(height: 12),
                Container(
                  padding: const EdgeInsets.all(16),
                  decoration: BoxDecoration(
                    gradient: const LinearGradient(
                      begin: Alignment.topLeft,
                      end: Alignment.bottomRight,
                      colors: <Color>[AppColors.card, AppColors.surfaceDark],
                    ),
                    borderRadius: BorderRadius.circular(16),
                    border: Border.all(color: AppColors.border),
                  ),
                  child: Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: List<Widget>.generate(mission.objectives.length, (
                      int index,
                    ) {
                      return Padding(
                        padding: const EdgeInsets.symmetric(vertical: 8),
                        child: Row(
                          crossAxisAlignment: CrossAxisAlignment.start,
                          children: <Widget>[
                            CircleAvatar(
                              radius: 12,
                              backgroundColor: AppColors.cyan.withValues(
                                alpha: 0.25,
                              ),
                              child: Text(
                                '${index + 1}',
                                style: const TextStyle(
                                  color: AppColors.cyan,
                                  fontSize: 12,
                                  fontWeight: FontWeight.w700,
                                ),
                              ),
                            ),
                            const SizedBox(width: 12),
                            Expanded(
                              child: Text(
                                mission.objectives[index],
                                style: const TextStyle(
                                  color: AppColors.textPrimary,
                                  height: 1.5,
                                ),
                              ),
                            ),
                          ],
                        ),
                      );
                    }),
                  ),
                ),
                const SizedBox(height: 24),
                SizedBox(
                  width: double.infinity,
                  child: FilledButton(
                    onPressed: mission.isCompleted
                        ? null
                        : () {
                            showDialog<void>(
                              context: context,
                              builder: (context) => AlertDialog(
                                title: const Text(
                                  'Iniciar desde el videojuego',
                                ),
                                content: const Text(
                                  'La aplicación móvil no inicia misiones '
                                  'ni otorga recompensas. La integración con Unity y el servidor '
                                  'educativo aún está pendiente.',
                                ),
                                actions: [
                                  TextButton(
                                    onPressed: () => Navigator.pop(context),
                                    child: const Text('Entendido'),
                                  ),
                                ],
                              ),
                            );
                          },
                    style: FilledButton.styleFrom(
                      padding: const EdgeInsets.symmetric(vertical: 14),
                      backgroundColor: AppColors.primaryCyan,
                      shape: RoundedRectangleBorder(
                        borderRadius: BorderRadius.circular(999),
                      ),
                    ),
                    child: Text(
                      mission.isCompleted
                          ? 'Misión completada · demo'
                          : mission.status == MissionStatus.inProgress
                          ? 'Continuar en videojuego'
                          : 'Iniciar en videojuego',
                      style: TextStyle(
                        color: AppColors.backgroundDark,
                        fontSize: 16,
                        fontWeight: FontWeight.w700,
                      ),
                    ),
                  ),
                ),
              ],
            ),
          ),
        ),
      ),
    );
  }
}

class _InfoRow extends StatelessWidget {
  const _InfoRow({
    required this.icon,
    required this.title,
    required this.value,
  });

  final IconData icon;
  final String title;
  final String value;

  @override
  Widget build(BuildContext context) {
    return Row(
      children: <Widget>[
        Icon(icon, color: AppColors.cyan),
        const SizedBox(width: 12),
        Expanded(
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: <Widget>[
              Text(
                title,
                style: const TextStyle(
                  color: AppColors.textSecondary,
                  fontSize: 12,
                ),
              ),
              const SizedBox(height: 4),
              Text(
                value,
                style: const TextStyle(
                  color: AppColors.textPrimary,
                  fontSize: 14,
                  fontWeight: FontWeight.w600,
                ),
              ),
            ],
          ),
        ),
      ],
    );
  }
}

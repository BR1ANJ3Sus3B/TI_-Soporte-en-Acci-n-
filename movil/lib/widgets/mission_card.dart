import 'package:flutter/material.dart';
import 'package:ti_soporte_accion/models/mission.dart';
import 'package:ti_soporte_accion/theme/app_colors.dart';
import 'package:ti_soporte_accion/widgets/chips.dart';
import 'package:ti_soporte_accion/widgets/nova_panel.dart';
import 'package:ti_soporte_accion/theme/app_typography.dart';

/// Tarjeta reutilizable para mostrar una misión en la lista de misiones.
class MissionCard extends StatelessWidget {
  const MissionCard({super.key, required this.mission, this.onTap});

  /// Misión a mostrar.
  final Mission mission;

  /// Callback cuando se toca la tarjeta.
  final VoidCallback? onTap;

  @override
  Widget build(BuildContext context) {
    return Padding(
      padding: const EdgeInsets.only(bottom: 16),
      child: NovaPanel(
        onTap: onTap,
        accent: mission.status == MissionStatus.completed
            ? AppColors.success
            : mission.status == MissionStatus.inProgress
            ? AppColors.warning
            : AppColors.cyan,
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: <Widget>[
            Row(
              mainAxisAlignment: MainAxisAlignment.spaceBetween,
              children: <Widget>[
                Expanded(
                  child: Text(
                    mission.title,
                    style: AppTypography.heading(17, weight: FontWeight.w600),
                  ),
                ),
                StatusChip(
                  status: mission.status,
                  text: mission.isCompleted
                      ? 'Completada'
                      : (mission.status == MissionStatus.inProgress
                            ? 'En curso'
                            : 'Disponible'),
                ),
              ],
            ),
            const SizedBox(height: 8),
            Text(
              'Categoría: ${mission.area} • Departamento: ${mission.department}',
              style: const TextStyle(
                color: AppColors.textSecondary,
                fontSize: 12,
              ),
            ),
            const SizedBox(height: 12),
            Wrap(
              spacing: 10,
              runSpacing: 10,
              children: <Widget>[
                DifficultyChip(
                  label: mission.difficultyLabel,
                  color: mission.difficultyColor,
                ),
                Chip(
                  avatar: const Icon(
                    Icons.rocket_launch_rounded,
                    size: 16,
                    color: AppColors.cyan,
                  ),
                  label: Text('${mission.xpReward} XP'),
                  backgroundColor: AppColors.cyan.withValues(alpha: 0.2),
                  side: const BorderSide(color: AppColors.cyan),
                  labelStyle: AppTypography.metric(12),
                ),
              ],
            ),
            const SizedBox(height: 12),
            Row(
              mainAxisAlignment: MainAxisAlignment.end,
              children: <Widget>[
                TextButton.icon(
                  onPressed: onTap,
                  icon: const Icon(
                    Icons.visibility_rounded,
                    color: AppColors.primaryCyan,
                  ),
                  label: Text(
                    'Ver detalle',
                    style: const TextStyle(
                      color: AppColors.primaryCyan,
                      fontWeight: FontWeight.w600,
                    ),
                  ),
                ),
              ],
            ),
          ],
        ),
      ),
    );
  }
}

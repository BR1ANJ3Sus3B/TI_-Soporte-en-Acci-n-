import 'package:flutter/material.dart';
import 'package:ti_soporte_accion/models/mission.dart';
import 'package:ti_soporte_accion/theme/app_colors.dart';
import 'package:ti_soporte_accion/widgets/chips.dart';

/// Tarjeta reutilizable para mostrar una misión en la lista de misiones.
class MissionCard extends StatelessWidget {
  const MissionCard({
    super.key,
    required this.mission,
    this.onTap,
    this.showStartButton = false,
  });

  /// Misión a mostrar.
  final Mission mission;

  /// Callback cuando se toca la tarjeta.
  final VoidCallback? onTap;

  /// Indica si debe mostrar el botón "Ver detalles" o "Iniciar misión".
  final bool showStartButton;

  @override
  Widget build(BuildContext context) {
    return Material(
      color: Colors.transparent,
      child: InkWell(
        onTap: onTap,
        borderRadius: BorderRadius.circular(16),
        child: Container(
          margin: const EdgeInsets.only(bottom: 16),
          padding: const EdgeInsets.all(16),
          decoration: BoxDecoration(
            gradient: const LinearGradient(
              begin: Alignment.topLeft,
              end: Alignment.bottomRight,
              colors: <Color>[AppColors.card, Color(0xFF182235)],
            ),
            borderRadius: BorderRadius.circular(16),
            border: Border.all(color: AppColors.border),
            boxShadow: <BoxShadow>[
              BoxShadow(
                color: Colors.black.withValues(alpha: 0.3),
                blurRadius: 12,
                offset: const Offset(0, 8),
              ),
            ],
          ),
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: <Widget>[
              Row(
                mainAxisAlignment: MainAxisAlignment.spaceBetween,
                children: <Widget>[
                  Expanded(
                    child: Text(
                      mission.title,
                      style: const TextStyle(
                        color: AppColors.textPrimary,
                        fontSize: 16,
                        fontWeight: FontWeight.w700,
                      ),
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
                    labelStyle: const TextStyle(
                      color: AppColors.cyan,
                      fontSize: 12,
                      fontWeight: FontWeight.w600,
                    ),
                  ),
                ],
              ),
              const SizedBox(height: 12),
              Row(
                mainAxisAlignment: MainAxisAlignment.end,
                children: <Widget>[
                  TextButton.icon(
                    onPressed: onTap,
                    icon: Icon(
                      showStartButton
                          ? Icons.play_arrow_rounded
                          : Icons.visibility_rounded,
                      color: AppColors.cyan,
                    ),
                    label: Text(
                      showStartButton ? 'Iniciar misión' : 'Ver detalles',
                      style: const TextStyle(
                        color: AppColors.cyan,
                        fontWeight: FontWeight.w600,
                      ),
                    ),
                  ),
                ],
              ),
            ],
          ),
        ),
      ),
    );
  }
}

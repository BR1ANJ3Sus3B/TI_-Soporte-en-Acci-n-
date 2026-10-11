import 'package:flutter/material.dart';
import 'package:ti_soporte_accion/models/mission.dart';
import 'package:ti_soporte_accion/theme/app_colors.dart';

/// Chip reutilizable para mostrar el estado de una misión.
class StatusChip extends StatelessWidget {
  const StatusChip({super.key, required this.status, required this.text});

  /// Estado de la misión.
  final MissionStatus status;

  /// Texto a mostrar.
  final String text;

  Color get _color {
    switch (status) {
      case MissionStatus.completed:
        return AppColors.success;
      case MissionStatus.inProgress:
        return AppColors.warning;
      case MissionStatus.available:
        return AppColors.cyan;
    }
  }

  IconData get _icon {
    switch (status) {
      case MissionStatus.completed:
        return Icons.check_circle_rounded;
      case MissionStatus.inProgress:
        return Icons.schedule_rounded;
      case MissionStatus.available:
        return Icons.play_circle_outline_rounded;
    }
  }

  @override
  Widget build(BuildContext context) {
    return Container(
      padding: const EdgeInsets.symmetric(horizontal: 12, vertical: 6),
      decoration: BoxDecoration(
        color: _color.withValues(alpha: 0.2),
        borderRadius: BorderRadius.circular(999),
        border: Border.all(color: _color),
      ),
      child: Row(
        mainAxisSize: MainAxisSize.min,
        children: <Widget>[
          Icon(_icon, size: 16, color: _color),
          const SizedBox(width: 6),
          Text(
            text,
            style: TextStyle(
              color: _color,
              fontSize: 12,
              fontWeight: FontWeight.w600,
            ),
          ),
        ],
      ),
    );
  }
}

/// Chip reutilizable para mostrar la dificultad de una misión.
class DifficultyChip extends StatelessWidget {
  const DifficultyChip({super.key, required this.label, required this.color});

  /// Etiqueta de dificultad (Fácil, Media, Difícil).
  final String label;

  /// Color asociado a la dificultad.
  final Color color;

  @override
  Widget build(BuildContext context) {
    return Container(
      padding: const EdgeInsets.symmetric(horizontal: 10, vertical: 6),
      decoration: BoxDecoration(
        color: color.withValues(alpha: 0.2),
        borderRadius: BorderRadius.circular(999),
        border: Border.all(color: color),
      ),
      child: Text(
        label,
        style: TextStyle(
          color: color,
          fontSize: 12,
          fontWeight: FontWeight.w600,
        ),
      ),
    );
  }
}

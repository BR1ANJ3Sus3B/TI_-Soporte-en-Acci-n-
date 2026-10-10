import 'package:flutter/material.dart';
import 'package:ti_soporte_accion/theme/app_colors.dart';

/// Barra de experiencia reutilizable para mostrar el avance dentro del nivel.
class XpBar extends StatelessWidget {
  const XpBar({
    super.key,
    required this.current,
    required this.max,
    this.showLabel = true,
  });

  /// Experiencia actual.
  final int current;

  /// Experiencia máxima para el siguiente nivel.
  final int max;

  /// Muestra etiqueta con valores numéricos.
  final bool showLabel;

  @override
  Widget build(BuildContext context) {
    final progress = max <= 0 ? 0.0 : (current / max).clamp(0.0, 1.0);
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: <Widget>[
        if (showLabel)
          Row(
            mainAxisAlignment: MainAxisAlignment.spaceBetween,
            children: <Widget>[
              const Text(
                'Experiencia',
                style: TextStyle(
                  color: AppColors.textSecondary,
                  fontSize: 12,
                  fontWeight: FontWeight.w600,
                ),
              ),
              Text(
                '$current / $max XP',
                style: const TextStyle(
                  color: AppColors.cyan,
                  fontSize: 12,
                  fontWeight: FontWeight.w700,
                ),
              ),
            ],
          ),
        const SizedBox(height: 8),
        ClipRRect(
          borderRadius: BorderRadius.circular(999),
          child: Container(
            height: 10,
            color: AppColors.card,
            child: FractionallySizedBox(
              alignment: Alignment.centerLeft,
              widthFactor: progress,
              child: Container(
                decoration: const BoxDecoration(
                  gradient: AppColors.primaryGradient,
                ),
              ),
            ),
          ),
        ),
      ],
    );
  }
}

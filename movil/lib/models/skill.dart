import 'package:flutter/material.dart';
import 'package:ti_soporte_accion/theme/app_colors.dart';

/// Habilidad del jugador mostrada en la pantalla de progreso.
///
/// Se representa con bloques (por ejemplo ████████░░) para reforzar el estilo
/// RPG del proyecto.
class Skill {
  const Skill({
    required this.name,
    this.level,
    required this.icon,
    required this.color,
    this.progressValue,
    this.maxLevel = 10,
  });

  /// Nombre de la habilidad (Soporte Técnico, Redes, etc.).
  final String name;

  /// Nivel actual de la habilidad.
  final int? level;

  /// Nivel máximo posible.
  final int maxLevel;

  /// Icono asociado a la habilidad.
  final IconData icon;

  /// Color de acento de la habilidad.
  final Color color;

  /// Valor normalizado recibido del backend (0.0–1.0), si está disponible.
  final double? progressValue;

  /// Porcentaje de avance de la habilidad (0.0 a 1.0).
  double get progress {
    if (progressValue != null) return progressValue!.clamp(0.0, 1.0);
    if (maxLevel <= 0 || level == null) return 0;
    return (level! / maxLevel).clamp(0.0, 1.0);
  }

  factory Skill.fromJson(Map<String, dynamic> json) {
    final iconName = json['icon'] as String? ?? 'code';
    final name = json['name'] as String;
    final icon = switch (iconName) {
      'headset' => Icons.headset_mic_rounded,
      'network' => Icons.hub_rounded,
      'shield' => Icons.shield_rounded,
      'database' => Icons.storage_rounded,
      _ => Icons.code_rounded,
    };
    final color = switch (iconName) {
      'headset' => AppColors.successLight,
      'network' => AppColors.primaryCyan,
      'shield' => AppColors.purple,
      'database' => AppColors.purpleSecondary,
      _ => AppColors.primaryCyan,
    };
    final rawProgress = json['progress'];
    final normalizedProgress = rawProgress is num
        ? rawProgress.toDouble()
        : null;
    return Skill(
      name: name,
      level: (json['level'] as num?)?.toInt(),
      maxLevel: (json['maxLevel'] as num?)?.toInt() ?? 10,
      icon: icon,
      color: color,
      progressValue: normalizedProgress,
    );
  }
}

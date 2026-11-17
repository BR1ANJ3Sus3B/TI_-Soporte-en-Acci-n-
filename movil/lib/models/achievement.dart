import 'package:flutter/material.dart';
import 'package:ti_soporte_accion/theme/app_colors.dart';

/// Rareza de un logro. Determina el tono visual de la notificación.
enum AchievementRarity { common, uncommon, rare, epic, legendary }

/// Estado del logro.
enum AchievementStatus { locked, inProgress, unlocked }

/// Logro desbloqueable por el jugador.
class Achievement {
  const Achievement({
    required this.id,
    required this.title,
    required this.description,
    required this.icon,
    required this.unlocked,
    this.status = AchievementStatus.locked,
    this.xp = 0,
    this.category = 'General',
    this.rarity = AchievementRarity.common,
  });

  final String id;
  final String title;
  final String description;
  final IconData icon;
  final bool unlocked;
  final AchievementStatus status;
  final int xp;
  final String category;
  final AchievementRarity rarity;
}

/// Utilidades visuales de la rareza de un logro.
extension AchievementRarityX on AchievementRarity {
  /// Nombre legible de la rareza.
  String get label => switch (this) {
    AchievementRarity.common => 'Común',
    AchievementRarity.uncommon => 'Poco común',
    AchievementRarity.rare => 'Raro',
    AchievementRarity.epic => 'Épico',
    AchievementRarity.legendary => 'Legendario',
  };

  /// Color asociado a la rareza (borde e insignia).
  Color get color => switch (this) {
    AchievementRarity.common => AppColors.textSecondary,
    AchievementRarity.uncommon => AppColors.successLight,
    AchievementRarity.rare => AppColors.primaryCyan,
    AchievementRarity.epic => AppColors.purpleSecondary,
    AchievementRarity.legendary => AppColors.purple,
  };
}

/// Utilidades visuales del estado de un logro.
extension AchievementStatusX on AchievementStatus {
  /// Nombre legible del estado.
  String get label => switch (this) {
    AchievementStatus.locked => 'Bloqueado',
    AchievementStatus.inProgress => 'En progreso',
    AchievementStatus.unlocked => 'Desbloqueado',
  };
}

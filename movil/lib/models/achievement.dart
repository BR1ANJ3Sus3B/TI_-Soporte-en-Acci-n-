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
    this.unlockedAt,
    this.criteriaDescription = '',
  });

  final String id;
  final String title;
  final String description;
  final IconData icon;
  final bool unlocked;
  final AchievementStatus status;
  final int xp;
  final String category;
  final AchievementRarity? rarity;
  final DateTime? unlockedAt;
  final String criteriaDescription;

  factory Achievement.fromJson(Map<String, dynamic> json) {
    final icon = switch (json['icon']) {
      'shield' => Icons.shield_rounded,
      'rocket' => Icons.rocket_launch_rounded,
      _ => Icons.emoji_events_rounded,
    };
    final rarity = switch (json['rarity']) {
      'uncommon' => AchievementRarity.uncommon,
      'rare' => AchievementRarity.rare,
      'epic' => AchievementRarity.epic,
      'legendary' => AchievementRarity.legendary,
      'common' => AchievementRarity.common,
      _ => null,
    };
    final unlocked = json['unlocked'] == true;
    return Achievement(
      id: json['id'] as String,
      title: json['title'] as String,
      description: json['description'] as String,
      icon: icon,
      unlocked: unlocked,
      status: unlocked ? AchievementStatus.unlocked : AchievementStatus.locked,
      xp: (json['xp'] as num?)?.toInt() ?? 0,
      category: json['category'] as String? ?? 'General',
      rarity: rarity,
      unlockedAt: DateTime.tryParse(json['date'] as String? ?? ''),
      criteriaDescription: json['criteriaDescription'] as String? ?? '',
    );
  }
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

import 'package:flutter/material.dart';
import 'package:ti_soporte_accion/models/achievement.dart';
import 'package:ti_soporte_accion/theme/app_colors.dart';

/// Pantalla de detalle de un logro.
///
/// Se abre al tocar una notificación de logro desbloqueado o una tarjeta de la
/// pantalla de progreso. Recibe el [Achievement] por `arguments` de la ruta.
class AchievementDetailScreen extends StatelessWidget {
  const AchievementDetailScreen({super.key});

  static const String routeName = '/achievement-detail';

  @override
  Widget build(BuildContext context) {
    final achievement =
        ModalRoute.of(context)?.settings.arguments as Achievement?;
    if (achievement == null) {
      return const Scaffold(
        backgroundColor: AppColors.background,
        body: Center(
          child: Text(
            'Logro no disponible.',
            style: TextStyle(color: AppColors.textSecondary),
          ),
        ),
      );
    }

    final rarityColor = achievement.rarity.color;
    final bool unlocked =
        achievement.unlocked ||
        achievement.status == AchievementStatus.unlocked;

    return Scaffold(
      appBar: AppBar(
        title: const Text('Detalle de logro'),
        backgroundColor: Colors.transparent,
      ),
      body: Container(
        decoration: const BoxDecoration(gradient: AppColors.backgroundGradient),
        child: SafeArea(
          child: SingleChildScrollView(
            padding: const EdgeInsets.all(20),
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.stretch,
              children: <Widget>[
                Container(
                  padding: const EdgeInsets.symmetric(vertical: 28),
                  decoration: BoxDecoration(
                    gradient: LinearGradient(
                      begin: Alignment.topLeft,
                      end: Alignment.bottomRight,
                      colors: <Color>[
                        AppColors.card,
                        Color.lerp(AppColors.card, rarityColor, 0.18)!,
                      ],
                    ),
                    borderRadius: BorderRadius.circular(20),
                    border: Border.all(color: rarityColor, width: 1.5),
                    boxShadow: <BoxShadow>[
                      BoxShadow(
                        color: rarityColor.withValues(alpha: 0.3),
                        blurRadius: 22,
                        spreadRadius: 1,
                      ),
                    ],
                  ),
                  child: Column(
                    children: <Widget>[
                      Container(
                        width: 96,
                        height: 96,
                        decoration: BoxDecoration(
                          shape: BoxShape.circle,
                          color: rarityColor.withValues(alpha: 0.16),
                          border: Border.all(color: rarityColor, width: 3),
                          boxShadow: <BoxShadow>[
                            BoxShadow(
                              color: rarityColor.withValues(alpha: 0.5),
                              blurRadius: 20,
                            ),
                          ],
                        ),
                        child: Icon(
                          unlocked ? achievement.icon : Icons.lock_rounded,
                          color: unlocked
                              ? rarityColor
                              : AppColors.textSecondary,
                          size: 44,
                        ),
                      ),
                      const SizedBox(height: 16),
                      Padding(
                        padding: const EdgeInsets.symmetric(horizontal: 20),
                        child: Text(
                          achievement.title,
                          textAlign: TextAlign.center,
                          style: const TextStyle(
                            color: AppColors.textPrimary,
                            fontSize: 20,
                            fontWeight: FontWeight.w800,
                          ),
                        ),
                      ),
                      const SizedBox(height: 10),
                      Container(
                        padding: const EdgeInsets.symmetric(
                          horizontal: 12,
                          vertical: 4,
                        ),
                        decoration: BoxDecoration(
                          color: rarityColor.withValues(alpha: 0.18),
                          borderRadius: BorderRadius.circular(999),
                          border: Border.all(
                            color: rarityColor.withValues(alpha: 0.75),
                          ),
                        ),
                        child: Text(
                          achievement.rarity.label,
                          style: TextStyle(
                            color: rarityColor,
                            fontSize: 12,
                            fontWeight: FontWeight.w800,
                            letterSpacing: 0.5,
                          ),
                        ),
                      ),
                    ],
                  ),
                ),
                const SizedBox(height: 20),
                Container(
                  padding: const EdgeInsets.all(16),
                  decoration: BoxDecoration(
                    gradient: const LinearGradient(
                      begin: Alignment.topLeft,
                      end: Alignment.bottomRight,
                      colors: <Color>[AppColors.card, Color(0xFF182235)],
                    ),
                    borderRadius: BorderRadius.circular(16),
                    border: Border.all(color: AppColors.border),
                  ),
                  child: Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: <Widget>[
                      Text(
                        achievement.description,
                        style: const TextStyle(
                          color: AppColors.textPrimary,
                          fontSize: 14,
                          height: 1.6,
                        ),
                      ),
                      const Divider(color: AppColors.border, height: 32),
                      _InfoRow(
                        icon: Icons.category_rounded,
                        title: 'Categoría',
                        value: achievement.category,
                      ),
                      const SizedBox(height: 14),
                      _InfoRow(
                        icon: Icons.emoji_events_rounded,
                        title: 'Rareza',
                        value: achievement.rarity.label,
                        valueColor: rarityColor,
                      ),
                      const SizedBox(height: 14),
                      _InfoRow(
                        icon: Icons.rocket_launch_rounded,
                        title: 'Recompensa',
                        value: '+${achievement.xp} XP',
                        valueColor: AppColors.success,
                      ),
                      const SizedBox(height: 14),
                      _InfoRow(
                        icon: unlocked
                            ? Icons.verified_rounded
                            : Icons.lock_clock_rounded,
                        title: 'Estado',
                        value: achievement.status.label,
                        valueColor: unlocked
                            ? AppColors.success
                            : AppColors.warning,
                      ),
                    ],
                  ),
                ),
                const SizedBox(height: 24),
                SizedBox(
                  width: double.infinity,
                  child: FilledButton(
                    onPressed: () => Navigator.of(context).maybePop(),
                    style: FilledButton.styleFrom(
                      padding: const EdgeInsets.symmetric(vertical: 14),
                      backgroundColor: AppColors.primary,
                      shape: RoundedRectangleBorder(
                        borderRadius: BorderRadius.circular(999),
                      ),
                    ),
                    child: const Text(
                      'Volver',
                      style: TextStyle(
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
    this.valueColor = AppColors.textPrimary,
  });

  final IconData icon;
  final String title;
  final String value;
  final Color valueColor;

  @override
  Widget build(BuildContext context) {
    return Row(
      children: <Widget>[
        Icon(icon, color: AppColors.cyan, size: 20),
        const SizedBox(width: 12),
        Expanded(
          child: Text(
            title,
            style: const TextStyle(
              color: AppColors.textSecondary,
              fontSize: 13,
            ),
          ),
        ),
        Text(
          value,
          style: TextStyle(
            color: valueColor,
            fontSize: 14,
            fontWeight: FontWeight.w700,
          ),
        ),
      ],
    );
  }
}

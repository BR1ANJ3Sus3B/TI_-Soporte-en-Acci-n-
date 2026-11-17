import 'package:flutter/material.dart';
import 'package:ti_soporte_accion/data/mock_data.dart';
import 'package:ti_soporte_accion/models/achievement.dart';
import 'package:ti_soporte_accion/theme/app_colors.dart';
import 'package:ti_soporte_accion/widgets/custom_bottom_nav.dart';
import 'package:ti_soporte_accion/widgets/xp_bar.dart';
import 'package:ti_soporte_accion/widgets/demo_data_notice.dart';
import 'package:ti_soporte_accion/theme/app_typography.dart';

/// Pantalla de progreso del jugador.
///
/// Muestra el crecimiento, barras de habilidades y logros desbloqueados/
/// bloqueados.
class ProgressScreen extends StatelessWidget {
  const ProgressScreen({super.key});

  static const String routeName = '/progress';

  @override
  Widget build(BuildContext context) {
    final profile = MockData.player;
    final skills = MockData.skills;
    final achievements = MockData.achievements;

    return Scaffold(
      body: Container(
        decoration: const BoxDecoration(gradient: AppColors.backgroundGradient),
        child: SafeArea(
          child: SingleChildScrollView(
            padding: const EdgeInsets.all(20),
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: <Widget>[
                Text('Progreso', style: AppTypography.heading(24)),
                const SizedBox(height: 8),
                const DemoDataNotice(
                  message:
                      'Nivel, XP, habilidades y logros '
                      'son ejemplos locales; no están verificados ni sincronizados.',
                ),
                const SizedBox(height: 12),
                const Text(
                  'Crecimiento del jugador, habilidades y logros.',
                  style: TextStyle(
                    color: AppColors.textSecondary,
                    fontSize: 14,
                  ),
                ),
                const SizedBox(height: 16),
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
                    children: <Widget>[
                      Row(
                        mainAxisAlignment: MainAxisAlignment.spaceBetween,
                        children: <Widget>[
                          Text(
                            'Nivel actual: ${profile.level}',
                            style: const TextStyle(
                              color: AppColors.textPrimary,
                              fontSize: 16,
                              fontWeight: FontWeight.w700,
                            ),
                          ),
                          Text(
                            profile.rank,
                            style: const TextStyle(
                              color: AppColors.cyan,
                              fontWeight: FontWeight.w600,
                            ),
                          ),
                        ],
                      ),
                      const SizedBox(height: 12),
                      XpBar(
                        current: profile.currentXp,
                        max: profile.xpForNextLevel,
                      ),
                    ],
                  ),
                ),
                const SizedBox(height: 20),
                Text('Habilidades', style: AppTypography.heading(19)),
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
                    children: skills.map((skill) {
                      return Padding(
                        padding: const EdgeInsets.symmetric(vertical: 10),
                        child: Column(
                          crossAxisAlignment: CrossAxisAlignment.start,
                          children: <Widget>[
                            Row(
                              mainAxisAlignment: MainAxisAlignment.spaceBetween,
                              children: <Widget>[
                                Row(
                                  children: <Widget>[
                                    Icon(
                                      skill.icon,
                                      color: skill.color,
                                      size: 18,
                                    ),
                                    const SizedBox(width: 10),
                                    Text(
                                      skill.name,
                                      style: const TextStyle(
                                        color: AppColors.textPrimary,
                                        fontWeight: FontWeight.w600,
                                      ),
                                    ),
                                  ],
                                ),
                                Text(
                                  'Nivel ${skill.level}',
                                  style: TextStyle(
                                    color: skill.color,
                                    fontWeight: FontWeight.w700,
                                  ),
                                ),
                              ],
                            ),
                            const SizedBox(height: 8),
                            ClipRRect(
                              borderRadius: BorderRadius.circular(999),
                              child: Container(
                                height: 8,
                                color: AppColors.surface,
                                child: FractionallySizedBox(
                                  alignment: Alignment.centerLeft,
                                  widthFactor: skill.progress,
                                  child: Container(color: skill.color),
                                ),
                              ),
                            ),
                          ],
                        ),
                      );
                    }).toList(),
                  ),
                ),
                const SizedBox(height: 20),
                Text('Logros', style: AppTypography.heading(19)),
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
                    children: achievements.map((Achievement a) {
                      final color = a.unlocked
                          ? AppColors.successLight
                          : AppColors.textSecondary;
                      return InkWell(
                        onTap: () {
                          Navigator.of(context)
                              .pushNamed('/achievement-detail', arguments: a);
                        },
                        borderRadius: BorderRadius.circular(12),
                        child: Padding(
                          padding: const EdgeInsets.symmetric(vertical: 8),
                          child: Row(
                            crossAxisAlignment: CrossAxisAlignment.start,
                            children: <Widget>[
                              Icon(a.icon, color: color, size: 22),
                              const SizedBox(width: 12),
                              Expanded(
                                child: Column(
                                  crossAxisAlignment: CrossAxisAlignment.start,
                                  children: <Widget>[
                                    Text(
                                      a.title,
                                      style: TextStyle(
                                        color: a.unlocked
                                            ? AppColors.textPrimary
                                            : AppColors.textSecondary,
                                        fontWeight: FontWeight.w700,
                                      ),
                                    ),
                                    const SizedBox(height: 4),
                                    Text(
                                      a.description,
                                      style: const TextStyle(
                                        color: AppColors.textSecondary,
                                        fontSize: 13,
                                        height: 1.5,
                                      ),
                                    ),
                                  ],
                                ),
                              ),
                              if (a.unlocked)
                                const Icon(
                                  Icons.verified_rounded,
                                  color: AppColors.success,
                                  size: 18,
                                ),
                            ],
                          ),
                        ),
                      );
                    }).toList(),
                  ),
                ),
                const SizedBox(height: 80),
              ],
            ),
          ),
        ),
      ),
      bottomNavigationBar: CustomBottomNav(
        currentIndex: 3,
        onTap: (int index) {
          switch (index) {
            case 0:
              Navigator.of(context).pushReplacementNamed('/home');
            case 1:
              Navigator.of(context).pushReplacementNamed('/missions');
            case 2:
              Navigator.of(context).pushReplacementNamed('/map');
            case 3:
              break;
            case 4:
              Navigator.of(context).pushReplacementNamed('/profile');
          }
        },
      ),
    );
  }
}

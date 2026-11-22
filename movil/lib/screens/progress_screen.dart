import 'package:flutter/material.dart';
import 'package:ti_soporte_accion/models/achievement.dart';
import 'package:ti_soporte_accion/services/app_data_repository.dart';
import 'package:ti_soporte_accion/theme/app_colors.dart';
import 'package:ti_soporte_accion/theme/app_typography.dart';
import 'package:ti_soporte_accion/widgets/async_content.dart';
import 'package:ti_soporte_accion/widgets/custom_bottom_nav.dart';
import 'package:ti_soporte_accion/widgets/demo_data_notice.dart';
import 'package:ti_soporte_accion/widgets/nova_panel.dart';
import 'package:ti_soporte_accion/widgets/xp_bar.dart';

class ProgressScreen extends StatelessWidget {
  const ProgressScreen({super.key});
  static const String routeName = '/progress';

  @override
  Widget build(BuildContext context) {
    final repository = AppDataRepository.instance;
    return Scaffold(
      body: Container(
        decoration: const BoxDecoration(gradient: AppColors.backgroundGradient),
        child: SafeArea(
          child: AsyncContent<ProgressPageData>(
            load: repository.getProgressPage,
            builder: (context, data) => SingleChildScrollView(
              padding: const EdgeInsets.all(20),
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  Text('Progreso', style: AppTypography.heading(24)),
                  const SizedBox(height: 12),
                  if (repository.demoMode)
                    const DemoDataNotice(
                      message:
                          'XP, niveles, habilidades y logros '
                          'son datos locales de demostración, no resultados verificados.',
                    ),
                  NovaPanel(
                    child: Column(
                      crossAxisAlignment: CrossAxisAlignment.start,
                      children: [
                        Row(
                          mainAxisAlignment: MainAxisAlignment.spaceBetween,
                          children: [
                            Text(
                              data.progress.level == null
                                  ? 'Nivel no disponible'
                                  : 'Nivel ${data.progress.level}',
                              style: AppTypography.heading(18),
                            ),
                            Text(
                              data.profile.rank,
                              style: AppTypography.label(
                                13,
                                color: AppColors.primaryCyan,
                              ),
                            ),
                          ],
                        ),
                        const SizedBox(height: 12),
                        XpBar(
                          current: data.progress.xp,
                          max: data.progress.nextLevelXp,
                        ),
                        const SizedBox(height: 16),
                        Row(
                          children: [
                            Expanded(
                              child: _ProgressCount(
                                label: 'Completadas',
                                value: data.progress.completedMissions,
                                color: AppColors.successLight,
                              ),
                            ),
                            const SizedBox(width: 12),
                            Expanded(
                              child: _ProgressCount(
                                label: 'En curso',
                                value: data.progress.inProgressMissions,
                                color: AppColors.warning,
                              ),
                            ),
                          ],
                        ),
                        if (data.progress.updatedAt != null) ...[
                          const SizedBox(height: 10),
                          Text(
                            'Actualizado ${data.progress.updatedAt!.toLocal()}',
                            style: AppTypography.label(11),
                          ),
                        ],
                      ],
                    ),
                  ),
                  const SizedBox(height: 20),
                  Text('Habilidades', style: AppTypography.heading(19)),
                  const SizedBox(height: 10),
                  if (data.skills.isEmpty)
                    const _EmptySection(
                      text: 'No hay habilidades registradas para esta cuenta.',
                    )
                  else
                    NovaPanel(
                      child: Column(
                        children: data.skills
                            .map(
                              (skill) => Padding(
                                padding: const EdgeInsets.symmetric(
                                  vertical: 9,
                                ),
                                child: Column(
                                  crossAxisAlignment: CrossAxisAlignment.start,
                                  children: [
                                    Row(
                                      children: [
                                        Icon(
                                          skill.icon,
                                          color: skill.color,
                                          size: 18,
                                        ),
                                        const SizedBox(width: 8),
                                        Expanded(
                                          child: Text(
                                            skill.name,
                                            style: AppTypography.body(14),
                                          ),
                                        ),
                                        Text(
                                          skill.level == null
                                              ? '${(skill.progress * 100).round()}%'
                                              : 'Nivel ${skill.level}',
                                          style: AppTypography.metric(
                                            12,
                                            color: skill.color,
                                          ),
                                        ),
                                      ],
                                    ),
                                    const SizedBox(height: 7),
                                    ClipRRect(
                                      borderRadius: BorderRadius.circular(999),
                                      child: LinearProgressIndicator(
                                        value: skill.progress,
                                        minHeight: 7,
                                        backgroundColor: AppColors.surfaceDark,
                                        valueColor:
                                            AlwaysStoppedAnimation<Color>(
                                              skill.color,
                                            ),
                                      ),
                                    ),
                                  ],
                                ),
                              ),
                            )
                            .toList(),
                      ),
                    ),
                  const SizedBox(height: 20),
                  Text('Logros', style: AppTypography.heading(19)),
                  const SizedBox(height: 10),
                  if (data.achievements.isEmpty)
                    const _EmptySection(
                      text: 'No hay logros configurados en el servidor.',
                    )
                  else
                    NovaPanel(
                      child: Column(
                        children: data.achievements
                            .map(
                              (achievement) =>
                                  _AchievementRow(achievement: achievement),
                            )
                            .toList(),
                      ),
                    ),
                  const SizedBox(height: 100),
                ],
              ),
            ),
          ),
        ),
      ),
      bottomNavigationBar: CustomBottomNav(
        currentIndex: 3,
        onTap: (index) {
          const routes = <String>[
            '/home',
            '/missions',
            '/map',
            '/progress',
            '/profile',
          ];
          if (index != 3) {
            Navigator.of(context).pushReplacementNamed(routes[index]);
          }
        },
      ),
    );
  }
}

class _AchievementRow extends StatelessWidget {
  const _AchievementRow({required this.achievement});
  final Achievement achievement;

  @override
  Widget build(BuildContext context) {
    final color = achievement.unlocked
        ? AppColors.successLight
        : AppColors.textSecondary;
    return Semantics(
      button: true,
      label: '${achievement.title}, ${achievement.status.label}',
      child: InkWell(
        onTap: () =>
            Navigator.of(context)
                .pushNamed('/achievement-detail', arguments: achievement),
        child: Padding(
          padding: const EdgeInsets.symmetric(vertical: 9),
          child: Row(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              Icon(
                achievement.unlocked ? achievement.icon : Icons.lock_outline,
                color: achievement.rarity?.color ?? AppColors.textSecondary,
                size: 22,
              ),
              const SizedBox(width: 12),
              Expanded(
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    Text(
                      achievement.title,
                      style: AppTypography.heading(15, weight: FontWeight.w600),
                    ),
                    const SizedBox(height: 4),
                    Text(
                      achievement.description,
                      style: AppTypography.label(12),
                    ),
                    const SizedBox(height: 4),
                    Text(
                      achievement.unlocked
                          ? 'Desbloqueado · +${achievement.xp} XP'
                          : 'Pendiente de una validación de servidor',
                      style: AppTypography.label(11, color: color),
                    ),
                  ],
                ),
              ),
            ],
          ),
        ),
      ),
    );
  }
}

class _EmptySection extends StatelessWidget {
  const _EmptySection({required this.text});
  final String text;
  @override
  Widget build(BuildContext context) =>
      NovaPanel(child: Text(text, style: AppTypography.label(13)));
}

class _ProgressCount extends StatelessWidget {
  const _ProgressCount({
    required this.label,
    required this.value,
    required this.color,
  });
  final String label;
  final int value;
  final Color color;

  @override
  Widget build(BuildContext context) => Row(
    children: [
      Text('$value', style: AppTypography.metric(16, color: color)),
      const SizedBox(width: 8),
      Expanded(child: Text(label, style: AppTypography.label(12))),
    ],
  );
}

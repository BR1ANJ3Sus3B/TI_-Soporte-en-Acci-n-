import 'package:flutter/material.dart';
import 'package:ti_soporte_accion/data/mock_data.dart';
import 'package:ti_soporte_accion/models/mission.dart';
import 'package:ti_soporte_accion/theme/app_colors.dart';
import 'package:ti_soporte_accion/widgets/custom_bottom_nav.dart';
import 'package:ti_soporte_accion/widgets/mission_card.dart';
import 'package:ti_soporte_accion/widgets/xp_bar.dart';
import 'package:ti_soporte_accion/services/auth_service.dart';
import 'package:ti_soporte_accion/widgets/demo_data_notice.dart';
import 'package:ti_soporte_accion/theme/app_typography.dart';

/// Pantalla de inicio (Dashboard).
///
/// Muestra el encabezado del jugador, la misión actual, la actividad reciente
/// y da acceso al resto de la aplicación a través del menú inferior.
class HomeScreen extends StatelessWidget {
  const HomeScreen({super.key});

  static const String routeName = '/home';

  Mission get _currentMission => MockData.missions.firstWhere(
    (Mission m) => m.status == MissionStatus.inProgress,
    orElse: () => MockData.missions.first,
  );

  @override
  Widget build(BuildContext context) {
    final profile = MockData.player;
    final mission = _currentMission;

    return Scaffold(
      body: Container(
        decoration: const BoxDecoration(gradient: AppColors.backgroundGradient),
        child: SafeArea(
          child: CustomScrollView(
            slivers: <Widget>[
              SliverPadding(
                padding: const EdgeInsets.all(20),
                sliver: SliverToBoxAdapter(
                  child: Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: <Widget>[
                      Text(
                        'Bienvenido, ${AuthService.instance.displayName}',
                        style: AppTypography.heading(24),
                      ),
                      const SizedBox(height: 8),
                      Text(
                        'NovaTech Solutions · ${AuthService.instance.roleLabel}',
                        style: AppTypography.label(14),
                      ),
                      const SizedBox(height: 12),
                      const DemoDataNotice(
                        message:
                            'La misión, nivel y XP de esta '
                            'pantalla son ejemplos locales. La API educativa todavía no está disponible.',
                      ),
                      const SizedBox(height: 16),
                      XpBar(
                        current: profile.currentXp,
                        max: profile.xpForNextLevel,
                      ),
                    ],
                  ),
                ),
              ),
              SliverPadding(
                padding: const EdgeInsets.symmetric(horizontal: 20),
                sliver: SliverToBoxAdapter(
                  child: Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: <Widget>[
                      Text('Misión actual', style: AppTypography.heading(19)),
                      const SizedBox(height: 12),
                      MissionCard(
                        mission: mission,
                        onTap: () {
                          Navigator.of(
                            context,
                          ).pushNamed('/mission-detail', arguments: mission.id);
                        },
                      ),
                    ],
                  ),
                ),
              ),
              SliverPadding(
                padding: const EdgeInsets.fromLTRB(20, 4, 20, 120),
                sliver: SliverToBoxAdapter(
                  child: Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: <Widget>[
                      Text(
                        'Actividad reciente',
                        style: AppTypography.heading(19),
                      ),
                      Padding(
                        padding: EdgeInsets.only(bottom: 8),
                        child: Text(
                          'Actividad ilustrativa, no obtenida de una cuenta.',
                          style: AppTypography.label(12),
                        ),
                      ),
                      const SizedBox(height: 12),
                      Container(
                        padding: const EdgeInsets.all(16),
                        decoration: BoxDecoration(
                          gradient: const LinearGradient(
                            begin: Alignment.topLeft,
                            end: Alignment.bottomRight,
                            colors: <Color>[
                              AppColors.card,
                              AppColors.surfaceDark,
                            ],
                          ),
                          borderRadius: BorderRadius.circular(16),
                          border: Border.all(color: AppColors.border),
                        ),
                        child: const Column(
                          children: <Widget>[
                            _ActivityItem(text: 'Ejemplo: equipo reparado'),
                            Divider(color: AppColors.border, height: 24),
                            _ActivityItem(
                              text: 'Ejemplo: contraseña restablecida',
                            ),
                            Divider(color: AppColors.border, height: 24),
                            _ActivityItem(text: 'Ejemplo: red diagnosticada'),
                          ],
                        ),
                      ),
                    ],
                  ),
                ),
              ),
            ],
          ),
        ),
      ),
      bottomNavigationBar: CustomBottomNav(
        currentIndex: 0,
        onTap: (int index) {
          switch (index) {
            case 0:
              break;
            case 1:
              Navigator.of(context).pushReplacementNamed('/missions');
            case 2:
              Navigator.of(context).pushReplacementNamed('/map');
            case 3:
              Navigator.of(context).pushReplacementNamed('/progress');
            case 4:
              Navigator.of(context).pushReplacementNamed('/profile');
          }
        },
      ),
    );
  }
}

class _ActivityItem extends StatelessWidget {
  const _ActivityItem({required this.text});

  final String text;

  @override
  Widget build(BuildContext context) {
    return Row(
      children: <Widget>[
        const Icon(
          Icons.check_circle_rounded,
          color: AppColors.success,
          size: 18,
        ),
        const SizedBox(width: 12),
        Text(
          text,
          style: const TextStyle(color: AppColors.textPrimary, fontSize: 14),
        ),
      ],
    );
  }
}

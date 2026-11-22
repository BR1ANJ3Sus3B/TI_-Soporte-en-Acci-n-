import 'package:flutter/material.dart';
import 'package:ti_soporte_accion/services/app_data_repository.dart';
import 'package:ti_soporte_accion/services/auth_service.dart';
import 'package:ti_soporte_accion/theme/app_colors.dart';
import 'package:ti_soporte_accion/theme/app_typography.dart';
import 'package:ti_soporte_accion/widgets/async_content.dart';
import 'package:ti_soporte_accion/widgets/custom_bottom_nav.dart';
import 'package:ti_soporte_accion/widgets/demo_data_notice.dart';
import 'package:ti_soporte_accion/widgets/mission_card.dart';
import 'package:ti_soporte_accion/widgets/nova_panel.dart';
import 'package:ti_soporte_accion/widgets/xp_bar.dart';

class HomeScreen extends StatelessWidget {
  const HomeScreen({super.key});
  static const String routeName = '/home';

  @override
  Widget build(BuildContext context) {
    final repository = AppDataRepository.instance;
    return Scaffold(
      body: Container(
        decoration: const BoxDecoration(gradient: AppColors.backgroundGradient),
        child: SafeArea(
          child: AsyncContent<DashboardData>(
            load: repository.getDashboard,
            builder: (context, data) => CustomScrollView(
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
                        if (repository.demoMode) ...[
                          const SizedBox(height: 12),
                          const DemoDataNotice(
                            message:
                                'Perfil, misión, nivel y XP '
                                'son datos locales de demostración.',
                          ),
                        ] else ...[
                          const SizedBox(height: 12),
                          const DemoDataNotice(
                            message:
                                'La consulta usa el backend. '
                                'La API todavía no recibe resultados verificados desde Unity.',
                          ),
                        ],
                        const SizedBox(height: 16),
                        XpBar(
                          current: data.profile.currentXp,
                          max: data.profile.xpForNextLevel,
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
                        if (data.currentMission case final mission?)
                          MissionCard(
                            mission: mission,
                            onTap: () => Navigator.of(context).pushNamed(
                              '/mission-detail',
                              arguments: mission.id,
                            ),
                          )
                        else
                          const NovaPanel(
                            child: Text(
                              'No hay misiones publicadas o en curso para esta cuenta.',
                              style: TextStyle(color: AppColors.textSecondary),
                            ),
                          ),
                      ],
                    ),
                  ),
                ),
                const SliverToBoxAdapter(child: SizedBox(height: 120)),
              ],
            ),
          ),
        ),
      ),
      bottomNavigationBar: CustomBottomNav(
        currentIndex: 0,
        onTap: (index) {
          const routes = <String>[
            '/home',
            '/missions',
            '/map',
            '/progress',
            '/profile',
          ];
          if (index != 0) {
            Navigator.of(context).pushReplacementNamed(routes[index]);
          }
        },
      ),
    );
  }
}

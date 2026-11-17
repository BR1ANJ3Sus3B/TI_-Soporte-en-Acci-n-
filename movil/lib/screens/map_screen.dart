import 'package:flutter/material.dart';
import 'package:ti_soporte_accion/data/mock_data.dart';
import 'package:ti_soporte_accion/theme/app_colors.dart';
import 'package:ti_soporte_accion/widgets/custom_bottom_nav.dart';
import 'package:ti_soporte_accion/widgets/demo_data_notice.dart';
import 'package:ti_soporte_accion/theme/app_typography.dart';

/// Pantalla que representa las áreas de la empresa (mapa).
class MapScreen extends StatelessWidget {
  const MapScreen({super.key});

  static const String routeName = '/map';

  @override
  Widget build(BuildContext context) {
    final areas = MockData.areas;

    return Scaffold(
      body: Container(
        decoration: const BoxDecoration(gradient: AppColors.backgroundGradient),
        child: SafeArea(
          child: Padding(
            padding: const EdgeInsets.all(20),
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: <Widget>[
                Text('Mapa de la empresa', style: AppTypography.heading(24)),
                const SizedBox(height: 8),
                const Text(
                  'Mapa conceptual de áreas de NovaTech; no representa niveles desbloqueables.',
                  style: TextStyle(
                    color: AppColors.textSecondary,
                    fontSize: 14,
                  ),
                ),
                const SizedBox(height: 16),
                const DemoDataNotice(
                  message:
                      'Áreas ilustrativas. La API aún no '
                      'proporciona niveles ni estados de desbloqueo.',
                ),
                const SizedBox(height: 12),
                Expanded(
                  child: GridView.builder(
                    gridDelegate:
                        const SliverGridDelegateWithFixedCrossAxisCount(
                          crossAxisCount: 2,
                          crossAxisSpacing: 12,
                          mainAxisSpacing: 12,
                          childAspectRatio: 1.1,
                        ),
                    itemCount: areas.length,
                    itemBuilder: (BuildContext context, int index) {
                      final area = areas[index];
                      return Semantics(
                        button: true,
                        label: 'Ver misiones de ${area.name}',
                        child: GestureDetector(
                          onTap: () {
                            Navigator.of(context)
                                .pushNamed('/missions', arguments: area.name);
                          },
                          child: Container(
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
                              border: Border.all(
                                color: area.color.withValues(alpha: 0.7),
                              ),
                            ),
                            child: Column(
                              mainAxisAlignment: MainAxisAlignment.center,
                              children: <Widget>[
                                Container(
                                  padding: const EdgeInsets.all(12),
                                  decoration: BoxDecoration(
                                    color: area.color.withValues(alpha: 0.2),
                                    borderRadius: BorderRadius.circular(999),
                                  ),
                                  child: Icon(
                                    area.icon,
                                    color: area.color,
                                    size: 26,
                                  ),
                                ),
                                const SizedBox(height: 12),
                                Text(
                                  area.name,
                                  textAlign: TextAlign.center,
                                  style: const TextStyle(
                                    color: AppColors.textPrimary,
                                    fontSize: 14,
                                    fontWeight: FontWeight.w700,
                                  ),
                                ),
                              ],
                            ),
                          ),
                        ),
                      );
                    },
                  ),
                ),
              ],
            ),
          ),
        ),
      ),
      bottomNavigationBar: CustomBottomNav(
        currentIndex: 2,
        onTap: (int index) {
          switch (index) {
            case 0:
              Navigator.of(context).pushReplacementNamed('/home');
            case 1:
              Navigator.of(context).pushReplacementNamed('/missions');
            case 2:
              break;
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

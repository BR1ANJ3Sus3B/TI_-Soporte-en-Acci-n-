import 'package:flutter/material.dart';
import 'package:ti_soporte_accion/models/map_area.dart';
import 'package:ti_soporte_accion/services/app_data_repository.dart';
import 'package:ti_soporte_accion/theme/app_colors.dart';
import 'package:ti_soporte_accion/theme/app_typography.dart';
import 'package:ti_soporte_accion/widgets/async_content.dart';
import 'package:ti_soporte_accion/widgets/custom_bottom_nav.dart';
import 'package:ti_soporte_accion/widgets/demo_data_notice.dart';

class MapScreen extends StatelessWidget {
  const MapScreen({super.key, this.repository});
  final AppDataRepository? repository;
  static const String routeName = '/map';

  @override
  Widget build(BuildContext context) {
    final repository = this.repository ?? AppDataRepository.instance;
    return Scaffold(
      body: Container(
        decoration: const BoxDecoration(gradient: AppColors.backgroundGradient),
        child: SafeArea(
          child: Padding(
            padding: const EdgeInsets.all(20),
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Text('Mapa de áreas', style: AppTypography.heading(24)),
                const SizedBox(height: 8),
                Text(
                  'Mapa conceptual de áreas de NovaTech, sin geolocalización.',
                  style: AppTypography.label(14),
                ),
                if (repository.demoMode) ...[
                  const SizedBox(height: 12),
                  const DemoDataNotice(
                    message:
                        'Áreas de demostración, no niveles ni '
                        'desbloqueos confirmados.',
                  ),
                ] else ...[
                  const SizedBox(height: 8),
                  Text(
                    'Áreas derivadas del catálogo publicado en la API.',
                    style: AppTypography.label(
                      12,
                      color: AppColors.primaryCyan,
                    ),
                  ),
                ],
                const SizedBox(height: 16),
                Expanded(
                  child: AsyncContent<List<MapArea>>(
                    load: repository.getAreas,
                    isEmpty: (areas) => areas.isEmpty,
                    emptyMessage: 'La API aún no ha publicado áreas asociadas a misiones.',
                    builder: (context, areas) => LayoutBuilder(
                      builder: (context, constraints) {
                        final columns = constraints.maxWidth >= 560 ? 3 : 2;
                        return GridView.builder(
                          gridDelegate:
                              SliverGridDelegateWithFixedCrossAxisCount(
                                crossAxisCount: columns,
                                crossAxisSpacing: 12,
                                mainAxisSpacing: 12,
                                childAspectRatio: 1.1,
                              ),
                          itemCount: areas.length,
                          itemBuilder: (context, index) {
                            final area = areas[index];
                            return Semantics(
                              button: true,
                              label: 'Ver misiones de ${area.name}',
                              child: Material(
                                color: Colors.transparent,
                                child: InkWell(
                                  borderRadius: BorderRadius.circular(
                                    AppColors.radiusMedium,
                                  ),
                                  onTap: () => Navigator.of(context).pushNamed(
                                    '/missions',
                                    arguments: area.name,
                                  ),
                                  child: Container(
                                    padding: const EdgeInsets.all(16),
                                    decoration: BoxDecoration(
                                      gradient: AppColors.cardGradient,
                                      borderRadius: BorderRadius.circular(
                                        AppColors.radiusMedium,
                                      ),
                                      border: Border.all(
                                        color: area.color.withValues(
                                          alpha: .65,
                                        ),
                                      ),
                                    ),
                                    child: Column(
                                      mainAxisAlignment:
                                          MainAxisAlignment.center,
                                      children: [
                                        Icon(
                                          area.icon,
                                          color: area.color,
                                          size: 28,
                                        ),
                                        const SizedBox(height: 12),
                                        Text(
                                          area.name,
                                          textAlign: TextAlign.center,
                                          style: AppTypography.heading(
                                            16,
                                            weight: FontWeight.w600,
                                          ),
                                        ),
                                      ],
                                    ),
                                  ),
                                ),
                              ),
                            );
                          },
                        );
                      },
                    ),
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
          const routes = <String>[
            '/home',
            '/missions',
            '/map',
            '/progress',
            '/profile',
          ];
          if (index != 2) {
            Navigator.of(context).pushReplacementNamed(routes[index]);
          }
        },
      ),
    );
  }
}

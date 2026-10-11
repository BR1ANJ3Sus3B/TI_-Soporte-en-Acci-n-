import 'package:flutter/material.dart';
import 'package:ti_soporte_accion/data/mock_data.dart';
import 'package:ti_soporte_accion/models/mission.dart';
import 'package:ti_soporte_accion/theme/app_colors.dart';
import 'package:ti_soporte_accion/widgets/custom_bottom_nav.dart';
import 'package:ti_soporte_accion/widgets/mission_card.dart';

/// Pantalla que muestra el listado completo de misiones.
class MissionsScreen extends StatelessWidget {
  const MissionsScreen({super.key});

  static const String routeName = '/missions';

  @override
  Widget build(BuildContext context) {
    final missions = List<Mission>.from(MockData.missions)
      ..sort((Mission a, Mission b) => a.id.compareTo(b.id));

    return Scaffold(
      body: Container(
        decoration: const BoxDecoration(gradient: AppColors.backgroundGradient),
        child: SafeArea(
          child: Padding(
            padding: const EdgeInsets.all(20),
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: <Widget>[
                const Text(
                  'Misiones',
                  style: TextStyle(
                    color: AppColors.textPrimary,
                    fontSize: 22,
                    fontWeight: FontWeight.w800,
                  ),
                ),
                const SizedBox(height: 8),
                const Text(
                  'Selecciona una misión para ver sus detalles e iniciar.',
                  style: TextStyle(
                    color: AppColors.textSecondary,
                    fontSize: 14,
                  ),
                ),
                const SizedBox(height: 16),
                Expanded(
                  child: ListView.builder(
                    itemCount: missions.length,
                    itemBuilder: (BuildContext context, int index) {
                      final mission = missions[index];
                      return MissionCard(
                        mission: mission,
                        onTap: () {
                          Navigator.of(
                            context,
                          ).pushNamed('/mission-detail', arguments: mission.id);
                        },
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
        currentIndex: 1,
        onTap: (int index) {
          switch (index) {
            case 0:
              Navigator.of(context).pushReplacementNamed('/home');
            case 1:
              break;
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

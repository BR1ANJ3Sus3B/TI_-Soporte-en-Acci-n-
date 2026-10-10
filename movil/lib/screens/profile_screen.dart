import 'package:flutter/material.dart';
import 'package:ti_soporte_accion/data/mock_data.dart';
import 'package:ti_soporte_accion/theme/app_colors.dart';
import 'package:ti_soporte_accion/widgets/character_card.dart';
import 'package:ti_soporte_accion/widgets/custom_bottom_nav.dart';
import 'package:ti_soporte_accion/widgets/stat_card.dart';

/// Pantalla de perfil del jugador.
///
/// Muestra la información personal, estadísticas, avatar y personajes
/// importantes de la historia.
class ProfileScreen extends StatelessWidget {
  const ProfileScreen({super.key});

  static const String routeName = '/profile';

  @override
  Widget build(BuildContext context) {
    final profile = MockData.player;
    final characters = MockData.characters;

    return Scaffold(
      body: Container(
        decoration: const BoxDecoration(
          gradient: AppColors.backgroundGradient,
        ),
        child: SafeArea(
          child: SingleChildScrollView(
            padding: const EdgeInsets.all(20),
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: <Widget>[
                const Text(
                  'Perfil del jugador',
                  style: TextStyle(
                    color: AppColors.textPrimary,
                    fontSize: 22,
                    fontWeight: FontWeight.w800,
                  ),
                ),
                const SizedBox(height: 16),
                Container(
                  padding: const EdgeInsets.all(20),
                  decoration: BoxDecoration(
                    gradient: const LinearGradient(
                      begin: Alignment.topLeft,
                      end: Alignment.bottomRight,
                      colors: <Color>[
                        AppColors.card,
                        Color(0xFF182235),
                      ],
                    ),
                    borderRadius: BorderRadius.circular(16),
                    border: Border.all(color: AppColors.border),
                  ),
                  child: Row(
                    children: <Widget>[
                      CircleAvatar(
                        radius: 32,
                        backgroundColor: AppColors.cyan.withValues(alpha: 0.25),
                        child: const Text(
                          'AL',
                          style: TextStyle(
                            color: AppColors.cyan,
                            fontSize: 18,
                            fontWeight: FontWeight.w800,
                          ),
                        ),
                      ),
                      const SizedBox(width: 16),
                      Expanded(
                        child: Column(
                          crossAxisAlignment: CrossAxisAlignment.start,
                          children: <Widget>[
                            Text(
                              profile.name,
                              style: const TextStyle(
                                color: AppColors.textPrimary,
                                fontSize: 18,
                                fontWeight: FontWeight.w700,
                              ),
                            ),
                            const SizedBox(height: 6),
                            Text(
                              '${profile.rank} • Nivel ${profile.level}',
                              style: const TextStyle(
                                color: AppColors.textSecondary,
                                fontSize: 13,
                              ),
                            ),
                            const SizedBox(height: 6),
                            Text(
                              'XP: ${profile.currentXp} / ${profile.xpForNextLevel}',
                              style: const TextStyle(
                                color: AppColors.cyan,
                                fontWeight: FontWeight.w600,
                              ),
                            ),
                          ],
                        ),
                      ),
                    ],
                  ),
                ),
                const SizedBox(height: 20),
                const Text(
                  'Estadísticas',
                  style: TextStyle(
                    color: AppColors.textPrimary,
                    fontSize: 18,
                    fontWeight: FontWeight.w700,
                  ),
                ),
                const SizedBox(height: 12),
                GridView.count(
                  crossAxisCount: 2,
                  shrinkWrap: true,
                  physics: const NeverScrollableScrollPhysics(),
                  crossAxisSpacing: 12,
                  mainAxisSpacing: 12,
                  childAspectRatio: 1.4,
                  children: <Widget>[
                    StatCard(
                      title: 'Misiones completadas',
                      value: profile.missionsCompleted.toString(),
                      icon: Icons.assignment_turned_in_rounded,
                      color: AppColors.success,
                    ),
                    StatCard(
                      title: 'Incidentes resueltos',
                      value: profile.incidentsResolved.toString(),
                      icon: Icons.check_circle_rounded,
                      color: AppColors.cyan,
                    ),
                    StatCard(
                      title: 'Soporte',
                      value: profile.skills['Soporte Técnico']?.toString() ?? '0',
                      icon: Icons.build_rounded,
                      color: AppColors.success,
                    ),
                    StatCard(
                      title: 'Redes',
                      value: profile.skills['Redes']?.toString() ?? '0',
                      icon: Icons.wifi_tethering_rounded,
                      color: AppColors.cyan,
                    ),
                    StatCard(
                      title: 'Programación',
                      value: profile.skills['Programación']?.toString() ?? '0',
                      icon: Icons.code_rounded,
                      color: AppColors.primary,
                    ),
                    StatCard(
                      title: 'Bases de datos',
                      value: profile.skills['Bases de Datos']?.toString() ?? '0',
                      icon: Icons.storage_rounded,
                      color: AppColors.primary,
                    ),
                  ],
                ),
                const SizedBox(height: 20),
                const Text(
                  'Personajes importantes',
                  style: TextStyle(
                    color: AppColors.textPrimary,
                    fontSize: 18,
                    fontWeight: FontWeight.w700,
                  ),
                ),
                const SizedBox(height: 12),
                SizedBox(
                  height: 170,
                  child: ListView.builder(
                    scrollDirection: Axis.horizontal,
                    itemCount: characters.length,
                    itemBuilder: (BuildContext context, int index) {
                      return CharacterCard(character: characters[index]);
                    },
                  ),
                ),
                const SizedBox(height: 80),
              ],
            ),
          ),
        ),
      ),
      bottomNavigationBar: CustomBottomNav(
        currentIndex: 4,
        onTap: (int index) {
          switch (index) {
            case 0:
              Navigator.of(context).pushReplacementNamed('/home');
            case 1:
              Navigator.of(context).pushReplacementNamed('/missions');
            case 2:
              Navigator.of(context).pushReplacementNamed('/map');
            case 3:
              Navigator.of(context).pushReplacementNamed('/progress');
            case 4:
              break;
          }
        },
      ),
    );
  }
}

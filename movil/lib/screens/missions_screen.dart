import 'package:flutter/material.dart';
import 'package:ti_soporte_accion/data/mock_data.dart';
import 'package:ti_soporte_accion/models/mission.dart';
import 'package:ti_soporte_accion/theme/app_colors.dart';
import 'package:ti_soporte_accion/widgets/custom_bottom_nav.dart';
import 'package:ti_soporte_accion/widgets/demo_data_notice.dart';
import 'package:ti_soporte_accion/widgets/mission_card.dart';

enum _MissionFilter { all, inProgress, available, completed }

class MissionsScreen extends StatefulWidget {
  const MissionsScreen({super.key});
  static const String routeName = '/missions';

  @override
  State<MissionsScreen> createState() => _MissionsScreenState();
}

class _MissionsScreenState extends State<MissionsScreen> {
  _MissionFilter _filter = _MissionFilter.all;
  String? _area;

  @override
  void didChangeDependencies() {
    super.didChangeDependencies();
    final requestedArea = ModalRoute.of(context)?.settings.arguments as String?;
    if (requestedArea != _area) _area = requestedArea;
  }

  @override
  Widget build(BuildContext context) {
    final missions = MockData.missions.where((mission) {
      final matchesArea = _area == null || mission.area == _area;
      final matchesFilter = switch (_filter) {
        _MissionFilter.all => true,
        _MissionFilter.inProgress => mission.status == MissionStatus.inProgress,
        _MissionFilter.available => mission.status == MissionStatus.available,
        _MissionFilter.completed => mission.status == MissionStatus.completed,
      };
      return matchesArea && matchesFilter;
    }).toList()..sort((a, b) => a.id.compareTo(b.id));

    return Scaffold(
      body: Container(
        decoration: const BoxDecoration(gradient: AppColors.backgroundGradient),
        child: SafeArea(
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              Padding(
                padding: const EdgeInsets.fromLTRB(20, 20, 20, 8),
                child: Text(
                  _area == null ? 'Misiones' : 'Misiones · $_area',
                  style: Theme.of(context).textTheme.headlineMedium,
                ),
              ),
              const Padding(
                padding: EdgeInsets.symmetric(horizontal: 20),
                child: DemoDataNotice(
                  message:
                      'Catálogo local de demostración. '
                      'No hay misiones disponibles confirmadas por un servidor.',
                ),
              ),
              const SizedBox(height: 12),
              SizedBox(
                height: 42,
                child: ListView(
                  padding: const EdgeInsets.symmetric(horizontal: 16),
                  scrollDirection: Axis.horizontal,
                  children: [
                    _filterChip('Todas', _MissionFilter.all),
                    _filterChip('En curso', _MissionFilter.inProgress),
                    _filterChip('Disponibles', _MissionFilter.available),
                    _filterChip('Completadas', _MissionFilter.completed),
                  ],
                ),
              ),
              const SizedBox(height: 12),
              Expanded(
                child: missions.isEmpty
                    ? Center(
                        child: Padding(
                          padding: const EdgeInsets.all(24),
                          child: Text(
                            'No hay misiones de demostración con este filtro.',
                            textAlign: TextAlign.center,
                            style: Theme.of(context).textTheme.bodyLarge
                                ?.copyWith(color: AppColors.textSecondary),
                          ),
                        ),
                      )
                    : ListView.builder(
                        padding: const EdgeInsets.fromLTRB(20, 0, 20, 20),
                        itemCount: missions.length,
                        itemBuilder: (context, index) => MissionCard(
                          mission: missions[index],
                          onTap: () => Navigator.of(context).pushNamed(
                            '/mission-detail',
                            arguments: missions[index].id,
                          ),
                        ),
                      ),
              ),
            ],
          ),
        ),
      ),
      bottomNavigationBar: CustomBottomNav(
        currentIndex: 1,
        onTap: (index) => _navigateTab(context, index),
      ),
    );
  }

  Widget _filterChip(String label, _MissionFilter filter) => Padding(
    padding: const EdgeInsets.symmetric(horizontal: 4),
    child: ChoiceChip(
      label: Text(label),
      selected: _filter == filter,
      onSelected: (_) => setState(() => _filter = filter),
      selectedColor: AppColors.primaryCyan.withValues(alpha: .16),
      labelStyle: TextStyle(
        color: _filter == filter
            ? AppColors.primaryCyan
            : AppColors.textSecondary,
      ),
      side: BorderSide(
        color: _filter == filter
            ? AppColors.primaryCyan
            : AppColors.borderSecondary,
      ),
    ),
  );

  void _navigateTab(BuildContext context, int index) {
    const routes = <String>[
      '/home',
      '/missions',
      '/map',
      '/progress',
      '/profile',
    ];
    if (index != 1) Navigator.of(context).pushReplacementNamed(routes[index]);
  }
}

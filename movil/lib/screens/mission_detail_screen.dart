import 'package:flutter/material.dart';
import 'package:ti_soporte_accion/models/mission.dart';
import 'package:ti_soporte_accion/services/app_data_repository.dart';
import 'package:ti_soporte_accion/services/auth_service.dart';
import 'package:ti_soporte_accion/theme/app_colors.dart';
import 'package:ti_soporte_accion/theme/app_typography.dart';
import 'package:ti_soporte_accion/widgets/async_content.dart';
import 'package:ti_soporte_accion/widgets/chips.dart';
import 'package:ti_soporte_accion/widgets/demo_data_notice.dart';
import 'package:ti_soporte_accion/widgets/nova_panel.dart';

class MissionDetailScreen extends StatelessWidget {
  const MissionDetailScreen({super.key, this.repository});
  final AppDataRepository? repository;
  static const String routeName = '/mission-detail';

  @override
  Widget build(BuildContext context) {
    final id = ModalRoute.of(context)?.settings.arguments as String?;
    if (id == null || id.isEmpty) {
      return const Scaffold(
        appBar: null,
        body: Center(child: Text('Falta el identificador de la misión.')),
      );
    }
    final repository = this.repository ?? AppDataRepository.instance;
    return Scaffold(
      appBar: AppBar(title: const Text('Detalle de misión')),
      body: Container(
        decoration: const BoxDecoration(gradient: AppColors.backgroundGradient),
        child: SafeArea(
          child: AsyncContent<Mission>(
            load: () => repository.getMission(id),
            builder: (context, mission) => SingleChildScrollView(
              padding: const EdgeInsets.all(20),
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  Text(mission.title, style: AppTypography.heading(24)),
                  const SizedBox(height: 12),
                  if (repository.demoMode)
                    const DemoDataNotice(
                      message:
                          'Detalle de demostración; la misión '
                          'no ha sido confirmada por el backend.',
                    )
                  else
                    Text(
                      'Datos del catálogo de la API.',
                      style: AppTypography.label(12),
                    ),
                  const SizedBox(height: 16),
                  Text(
                    mission.description,
                    style: AppTypography.body(
                      15,
                      color: AppColors.textSecondary,
                    ),
                  ),
                  const SizedBox(height: 16),
                  Wrap(
                    spacing: 8,
                    runSpacing: 8,
                    children: [
                      StatusChip(
                        status: mission.status,
                        text: _statusLabel(mission.status),
                      ),
                      DifficultyChip(
                        label: mission.difficultyLabel,
                        color: mission.difficultyColor,
                      ),
                      _InfoChip(
                        icon: Icons.schedule_rounded,
                        text: mission.estimatedTime,
                      ),
                      _InfoChip(
                        icon: Icons.rocket_launch_rounded,
                        text: '${mission.xpReward} XP · recompensa de catálogo',
                      ),
                    ],
                  ),
                  const SizedBox(height: 20),
                  NovaPanel(
                    child: Column(
                      children: [
                        _InfoRow(
                          icon: Icons.apartment_rounded,
                          title: 'Departamento',
                          value: mission.department.isEmpty
                              ? 'No especificado'
                              : mission.department,
                        ),
                        const Divider(height: 24),
                        _InfoRow(
                          icon: Icons.map_rounded,
                          title: 'Área',
                          value: mission.area,
                        ),
                        if (mission.affectedUser.isNotEmpty) ...[
                          const Divider(height: 24),
                          _InfoRow(
                            icon: Icons.person_outline,
                            title: 'Reportado por',
                            value: mission.affectedUser,
                          ),
                        ],
                      ],
                    ),
                  ),
                  const SizedBox(height: 20),
                  _sectionTitle(context, 'Objetivos educativos'),
                  const SizedBox(height: 10),
                  if (mission.objectives.isEmpty)
                    const _EmptyLine(text: 'El catálogo no publicó objetivos.')
                  else
                    _StringList(items: mission.objectives),
                  if (mission.requirements.isNotEmpty) ...[
                    const SizedBox(height: 20),
                    _sectionTitle(context, 'Requisitos'),
                    const SizedBox(height: 10),
                    _StringList(items: mission.requirements),
                  ],
                  const SizedBox(height: 24),
                  SizedBox(
                    width: double.infinity,
                    child: FilledButton.icon(
                      onPressed: mission.isCompleted ? null : () async {
                        if (AuthService.instance.isGuest) {
                          await repository.startGuestMission(mission.id);
                          if (context.mounted) {
                            ScaffoldMessenger.of(context).showSnackBar(
                              const SnackBar(content: Text('Actividad guardada en este dispositivo.')),
                            );
                          }
                        }
                        if (context.mounted) _explainUnity(context);
                      },
                      icon: const Icon(Icons.sports_esports_outlined),
                      label: Text(
                        mission.isCompleted
                            ? 'Completada · estado del servidor'
                            : mission.status == MissionStatus.inProgress
                            ? 'Continuar en videojuego'
                            : 'Iniciar en videojuego',
                      ),
                    ),
                  ),
                  const SizedBox(height: 10),
                  Text(
                    'Flutter no inicia el juego ni otorga recompensas. El estado '
                    'final debe venir de un resultado verificado en servidor.',
                    style: AppTypography.label(12),
                  ),
                  const SizedBox(height: 24),
                ],
              ),
            ),
          ),
        ),
      ),
    );
  }

  static String _statusLabel(MissionStatus status) => switch (status) {
    MissionStatus.available => 'Disponible',
    MissionStatus.inProgress => 'En curso',
    MissionStatus.completed => 'Completada',
  };

  static Widget _sectionTitle(BuildContext context, String title) =>
      Text(title, style: AppTypography.heading(19));

  static Future<void> _explainUnity(BuildContext context) => showDialog<void>(
    context: context,
    builder: (context) => AlertDialog(
      title: const Text('La misión se ejecuta en Unity'),
      content: const Text(
        'La aplicación móvil no inicia misiones ni concede XP. '
        'La sincronización depende de que Unity envíe el intento a la API y un '
        'administrador lo verifique.',
      ),
      actions: [
        TextButton(
          onPressed: () => Navigator.pop(context),
          child: const Text('Entendido'),
        ),
      ],
    ),
  );
}

class _InfoChip extends StatelessWidget {
  const _InfoChip({required this.icon, required this.text});
  final IconData icon;
  final String text;

  @override
  Widget build(BuildContext context) => Chip(
    avatar: Icon(icon, size: 16, color: AppColors.primaryCyan),
    label: Text(text),
    labelStyle: AppTypography.label(12, color: AppColors.primaryCyan),
    backgroundColor: AppColors.primaryCyan.withValues(alpha: .08),
    side: const BorderSide(color: AppColors.borderSecondary),
  );
}

class _InfoRow extends StatelessWidget {
  const _InfoRow({
    required this.icon,
    required this.title,
    required this.value,
  });
  final IconData icon;
  final String title;
  final String value;

  @override
  Widget build(BuildContext context) => Row(
    children: [
      Icon(icon, color: AppColors.primaryCyan, size: 20),
      const SizedBox(width: 12),
      Expanded(
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Text(title, style: AppTypography.label(12)),
            const SizedBox(height: 3),
            Text(value, style: AppTypography.body(14)),
          ],
        ),
      ),
    ],
  );
}

class _StringList extends StatelessWidget {
  const _StringList({required this.items});
  final List<String> items;

  @override
  Widget build(BuildContext context) => NovaPanel(
    child: Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: List.generate(
        items.length,
        (index) => Padding(
          padding: const EdgeInsets.symmetric(vertical: 7),
          child: Row(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              Text(
                '${index + 1}'.padLeft(2, '0'),
                style: AppTypography.metric(12),
              ),
              const SizedBox(width: 12),
              Expanded(
                child: Text(items[index], style: AppTypography.body(14)),
              ),
            ],
          ),
        ),
      ),
    ),
  );
}

class _EmptyLine extends StatelessWidget {
  const _EmptyLine({required this.text});
  final String text;
  @override
  Widget build(BuildContext context) =>
      Text(text, style: AppTypography.label(13));
}

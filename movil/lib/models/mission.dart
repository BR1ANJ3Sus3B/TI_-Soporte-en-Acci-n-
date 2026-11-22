import 'package:flutter/material.dart';
import 'package:ti_soporte_accion/theme/app_colors.dart';

/// Estado de una misión dentro del catálogo.
enum MissionStatus {
  /// Disponible para iniciar.
  available,

  /// En curso por el jugador.
  inProgress,

  /// Completada exitosamente.
  completed,
}

/// Representa una misión (ticket) del videojuego.
///
/// Los datos son de ejemplo para esta primera etapa; todavía no provienen de
/// una API ni de una base de datos.
class Mission {
  const Mission({
    required this.id,
    required this.title,
    required this.description,
    required this.area,
    required this.department,
    required this.affectedUser,
    required this.difficulty,
    required this.estimatedTime,
    required this.xpReward,
    required this.status,
    required this.objectives,
    this.requirements = const <String>[],
  });

  /// Identificador único de la misión.
  final String id;

  /// Título corto mostrado en las tarjetas.
  final String title;

  /// Descripción larga para la pantalla de detalle.
  final String description;

  /// Área de la empresa a la que pertenece la misión.
  final String area;

  /// Departamento afectado.
  final String department;

  /// Usuario que reporta el incidente.
  final String affectedUser;

  /// Nivel de dificultad de 1 (fácil) a 3 (difícil).
  final int difficulty;

  /// Tiempo estimado de resolución, en texto.
  final String estimatedTime;

  /// Experiencia otorgada al completar la misión.
  final int xpReward;

  /// Estado actual de la misión.
  final MissionStatus status;

  /// Lista de objetivos que debe cumplir el jugador.
  final List<String> objectives;

  /// Condiciones de acceso publicadas por la API, si existen.
  final List<String> requirements;

  /// Etiqueta legible de la dificultad.
  String get difficultyLabel => switch (difficulty) {
    1 => 'Fácil',
    2 => 'Media',
    _ => 'Difícil',
  };

  /// Color asociado a la dificultad para usar en chips e indicadores.
  Color get difficultyColor => switch (difficulty) {
    1 => AppColors.successLight,
    2 => AppColors.warning,
    _ => AppColors.error,
  };

  /// Indica si la misión ya fue completada.
  bool get isCompleted => status == MissionStatus.completed;

  factory Mission.fromJson(Map<String, dynamic> json) {
    final rawDifficulty = json['difficultyLevel'] ?? json['difficulty'];
    final difficulty = rawDifficulty is num
        ? rawDifficulty.toInt()
        : switch (rawDifficulty?.toString().toLowerCase()) {
            'básica' || 'basica' || 'basic' => 1,
            'media' || 'medium' => 2,
            _ => 3,
          };
    final rawStatus = json['status']?.toString().toLowerCase();
    final status = switch (rawStatus) {
      'completada' || 'completed' => MissionStatus.completed,
      'en progreso' ||
      'in_progress' ||
      'in progress' => MissionStatus.inProgress,
      _ => MissionStatus.available,
    };
    return Mission(
      id: json['id'] as String,
      title: json['title'] as String,
      description: json['description'] as String,
      area: (json['area'] ?? json['category']) as String,
      department: (json['department'] ?? '') as String,
      affectedUser: (json['affectedUser'] ?? '') as String,
      difficulty: difficulty,
      estimatedTime:
          (json['estimatedTime'] ?? json['duration'] ?? '') as String,
      xpReward: ((json['xpReward'] ?? json['xp']) as num).toInt(),
      status: status,
      objectives: (json['objectives'] as List<dynamic>? ?? const <dynamic>[])
          .whereType<String>()
          .toList(growable: false),
      requirements:
          (json['requirements'] as List<dynamic>? ?? const <dynamic>[])
              .whereType<String>()
              .toList(growable: false),
    );
  }
}

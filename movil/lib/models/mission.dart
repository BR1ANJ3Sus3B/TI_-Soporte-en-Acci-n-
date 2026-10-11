import 'package:flutter/material.dart';

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

  /// Etiqueta legible de la dificultad.
  String get difficultyLabel => switch (difficulty) {
    1 => 'Fácil',
    2 => 'Media',
    _ => 'Difícil',
  };

  /// Color asociado a la dificultad para usar en chips e indicadores.
  Color get difficultyColor => switch (difficulty) {
    1 => const Color(0xFF22C55E),
    2 => const Color(0xFFF59E0B),
    _ => const Color(0xFFEF4444),
  };

  /// Indica si la misión ya fue completada.
  bool get isCompleted => status == MissionStatus.completed;
}

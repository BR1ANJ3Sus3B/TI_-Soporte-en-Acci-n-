import 'package:flutter/material.dart';

/// Habilidad del jugador mostrada en la pantalla de progreso.
///
/// Se representa con bloques (por ejemplo ████████░░) para reforzar el estilo
/// RPG del proyecto.
class Skill {
  const Skill({
    required this.name,
    required this.level,
    required this.icon,
    required this.color,
    this.maxLevel = 10,
  });

  /// Nombre de la habilidad (Soporte Técnico, Redes, etc.).
  final String name;

  /// Nivel actual de la habilidad.
  final int level;

  /// Nivel máximo posible.
  final int maxLevel;

  /// Icono asociado a la habilidad.
  final IconData icon;

  /// Color de acento de la habilidad.
  final Color color;

  /// Porcentaje de avance de la habilidad (0.0 a 1.0).
  double get progress {
    if (maxLevel <= 0) return 0;
    return (level / maxLevel).clamp(0.0, 1.0);
  }
}

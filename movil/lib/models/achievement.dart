import 'package:flutter/material.dart';

/// Logro desbloqueable por el jugador.
///
/// Los logros bloqueados se muestran en gris; los desbloqueados usan el color
/// de acento definido en el tema.
class Achievement {
  const Achievement({
    required this.title,
    required this.description,
    required this.icon,
    required this.unlocked,
  });

  /// Título del logro.
  final String title;

  /// Descripción de cómo se obtiene.
  final String description;

  /// Icono representativo.
  final IconData icon;

  /// Indica si el jugador ya lo desbloqueó.
  final bool unlocked;
}

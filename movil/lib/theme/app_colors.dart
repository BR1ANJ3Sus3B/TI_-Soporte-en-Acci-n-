import 'package:flutter/material.dart';

/// Paleta de colores central del estilo "TI: Soporte en Acción".
///
/// El proyecto usa un tema oscuro, tecnológico y tipo RPG. Todos los colores
/// se declaran aquí para reutilizarlos en pantallas y widgets.
class AppColors {
  const AppColors._();

  /// Fondo principal de la aplicación.
  static const Color background = Color(0xFF0B1120);

  /// Superficies elevadas (app bars, secciones).
  static const Color surface = Color(0xFF111827);

  /// Fondo de tarjetas y paneles.
  static const Color card = Color(0xFF1F2937);

  /// Azul tecnológico principal (acciones y acentos).
  static const Color primary = Color(0xFF2563EB);

  /// Cian para acentos y datos destacados.
  static const Color cyan = Color(0xFF06B6D4);

  /// Verde de éxito (misiones completadas).
  static const Color success = Color(0xFF22C55E);

  /// Amarillo de advertencia (misiones en progreso).
  static const Color warning = Color(0xFFF59E0B);

  /// Rojo crítico (errores y dificultad alta).
  static const Color danger = Color(0xFFEF4444);

  /// Texto principal.
  static const Color textPrimary = Color(0xFFF9FAFB);

  /// Texto secundario y descripciones.
  static const Color textSecondary = Color(0xFF9CA3AF);

  /// Bordes y separadores sutiles.
  static const Color border = Color(0xFF273449);

  /// Degradado de fondo usado en toda la app.
  static const LinearGradient backgroundGradient = LinearGradient(
    begin: Alignment.topCenter,
    end: Alignment.bottomCenter,
    colors: <Color>[
      Color(0xFF0B1120),
      Color(0xFF0F172A),
      Color(0xFF0B1120),
    ],
  );

  /// Gradiente usado en barras de experiencia y botones principales.
  static const LinearGradient primaryGradient = LinearGradient(
    colors: <Color>[primary, cyan],
  );
}

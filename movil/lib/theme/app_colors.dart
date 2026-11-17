import 'package:flutter/material.dart';

/// Tokens de diseño de NovaTech basados en la especificación Stitch del proyecto.
class AppColors {
  const AppColors._();

  static const Color primaryCyan = Color(0xFF4AE4EC);
  static const Color backgroundDark = Color(0xFF06132A);
  static const Color surfaceDark = Color(0xFF020D25);
  static const Color containerDark = Color(0xFF0F1B33);
  static const Color success = Color(0xFF10B981);
  static const Color successLight = Color(0xFF4ADE80);
  static const Color warning = Color(0xFFF59E0B);
  static const Color error = Color(0xFFEF4444);
  static const Color purple = Color(0xFFA855F7);
  static const Color purpleSecondary = Color(0xFF8B5CF6);
  static const Color textPrimary = Color(0xFFF8FAFC);
  static const Color textSecondary = Color(0xFF94A3B8);
  static const Color borderPrimary = Color(0xFF1E293B);
  static const Color borderSecondary = Color(0xFF334155);

  // Alias de compatibilidad para conservar las APIs visuales existentes.
  static const Color background = backgroundDark;
  static const Color surface = surfaceDark;
  static const Color card = containerDark;
  static const Color primary = primaryCyan;
  static const Color cyan = primaryCyan;
  static const Color danger = error;
  static const Color border = borderPrimary;
  static const Color cardHighlight = surfaceDark;

  static const double radiusSmall = 12;
  static const double radiusMedium = 16;
  static const double radiusLarge = 20;

  static const LinearGradient backgroundGradient = LinearGradient(
    begin: Alignment.topLeft,
    end: Alignment.bottomCenter,
    colors: <Color>[backgroundDark, surfaceDark],
  );

  static const LinearGradient primaryGradient = LinearGradient(
    begin: Alignment.centerLeft,
    end: Alignment.centerRight,
    colors: <Color>[primaryCyan, Color(0xFF2DD4E0)],
  );

  static const LinearGradient cardGradient = LinearGradient(
    begin: Alignment.topLeft,
    end: Alignment.bottomRight,
    colors: <Color>[containerDark, surfaceDark],
  );
}

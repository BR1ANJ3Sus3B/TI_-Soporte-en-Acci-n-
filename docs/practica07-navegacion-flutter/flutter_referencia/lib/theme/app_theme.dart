import 'package:flutter/material.dart';

/// Identidad visual de TI: Soporte en Acción / NovaTech Solutions.
/// Referencia de estilo Flutter. No es una aplicación funcional completa.
abstract final class TiPalette {
  static const Color background = Color(0xFF070D1B);
  static const Color backgroundSecondary = Color(0xFF091326);
  static const Color surface = Color(0xFF10213B);
  static const Color surfaceAlt = Color(0xFF142A47);
  static const Color cyan = Color(0xFF00D9F5);
  static const Color turquoise = Color(0xFF28E6CB);
  static const Color violet = Color(0xFF906DFF);
  static const Color success = Color(0xFF38D99A);
  static const Color orange = Color(0xFFFF9E3B);
  static const Color foreground = Color(0xFFF2FBFF);
  static const Color foregroundMuted = Color(0xFFA1B7CD);
  static const Color outline = Color(0xFF275073);
  static const Color danger = Color(0xFFFF5F78);
}

abstract final class TiAppTheme {
  static ThemeData get dark {
    final scheme = ColorScheme.fromSeed(
      seedColor: TiPalette.cyan,
      brightness: Brightness.dark,
    ).copyWith(
      primary: TiPalette.cyan,
      onPrimary: TiPalette.background,
      secondary: TiPalette.turquoise,
      onSecondary: TiPalette.background,
      tertiary: TiPalette.violet,
      surface: TiPalette.surface,
      onSurface: TiPalette.foreground,
      error: TiPalette.danger,
      outline: TiPalette.outline,
    );

    return ThemeData(
      useMaterial3: true,
      brightness: Brightness.dark,
      colorScheme: scheme,
      scaffoldBackgroundColor: TiPalette.background,
      appBarTheme: const AppBarTheme(
        backgroundColor: TiPalette.backgroundSecondary,
        foregroundColor: TiPalette.foreground,
        elevation: 0,
        centerTitle: true,
      ),
      inputDecorationTheme: InputDecorationTheme(
        filled: true,
        fillColor: TiPalette.surface,
        hintStyle: const TextStyle(color: TiPalette.foregroundMuted),
        contentPadding: const EdgeInsets.symmetric(
          horizontal: 16,
          vertical: 14,
        ),
        border: OutlineInputBorder(
          borderRadius: BorderRadius.circular(12),
          borderSide: const BorderSide(color: TiPalette.outline),
        ),
        enabledBorder: OutlineInputBorder(
          borderRadius: BorderRadius.circular(12),
          borderSide: const BorderSide(color: TiPalette.outline),
        ),
        focusedBorder: OutlineInputBorder(
          borderRadius: BorderRadius.circular(12),
          borderSide: const BorderSide(color: TiPalette.cyan, width: 2),
        ),
      ),
      filledButtonTheme: FilledButtonThemeData(
        style: FilledButton.styleFrom(
          backgroundColor: TiPalette.cyan,
          foregroundColor: TiPalette.background,
          minimumSize: const Size.fromHeight(48),
          shape: RoundedRectangleBorder(
            borderRadius: BorderRadius.circular(12),
          ),
          textStyle: const TextStyle(fontWeight: FontWeight.w800),
        ),
      ),
      bottomNavigationBarTheme: const BottomNavigationBarThemeData(
        backgroundColor: TiPalette.backgroundSecondary,
        selectedItemColor: TiPalette.cyan,
        unselectedItemColor: TiPalette.foregroundMuted,
        type: BottomNavigationBarType.fixed,
      ),
      textTheme: const TextTheme(
        headlineMedium: TextStyle(
          color: TiPalette.foreground,
          fontWeight: FontWeight.w800,
        ),
        bodyMedium: TextStyle(color: TiPalette.foreground),
        bodySmall: TextStyle(color: TiPalette.foregroundMuted),
      ),
    );
  }
}

import 'package:flutter/material.dart';

import 'app_colors.dart';

/// Tema global de la aplicación.
///
/// Se construye sobre un [ThemeData] oscuro y aplica la paleta de [AppColors]
/// para dar un aspecto tecnológico y consistente en todas las pantallas.
class AppTheme {
  const AppTheme._();

  static ThemeData get dark {
    final base = ThemeData.dark(useMaterial3: true);

    return base.copyWith(
      scaffoldBackgroundColor: AppColors.background,
      colorScheme: base.colorScheme.copyWith(
        primary: AppColors.primaryCyan,
        secondary: AppColors.purpleSecondary,
        surface: AppColors.surface,
        error: AppColors.danger,
        onPrimary: AppColors.backgroundDark,
        onSurface: AppColors.textPrimary,
      ),
      appBarTheme: const AppBarTheme(
        backgroundColor: Colors.transparent,
        surfaceTintColor: Colors.transparent,
        elevation: 0,
        centerTitle: false,
        foregroundColor: AppColors.textPrimary,
        titleTextStyle: TextStyle(
          fontFamily: 'SpaceGrotesk',
          color: AppColors.textPrimary,
          fontSize: 20,
          fontWeight: FontWeight.w700,
          letterSpacing: 0.4,
        ),
      ),
      dividerColor: AppColors.border,
      inputDecorationTheme: InputDecorationTheme(
        filled: true,
        fillColor: AppColors.background.withValues(alpha: .45),
        labelStyle: const TextStyle(
          fontFamily: 'Inter',
          fontWeight: FontWeight.w500,
          fontVariations: <FontVariation>[FontVariation('wght', 500)],
          color: AppColors.textSecondary,
        ),
        prefixIconColor: AppColors.cyan,
        suffixIconColor: AppColors.textSecondary,
        errorStyle: const TextStyle(color: AppColors.error),
        enabledBorder: OutlineInputBorder(
          borderRadius: BorderRadius.circular(AppColors.radiusSmall),
          borderSide: const BorderSide(color: AppColors.border),
        ),
        focusedBorder: OutlineInputBorder(
          borderRadius: BorderRadius.circular(AppColors.radiusSmall),
          borderSide: const BorderSide(color: AppColors.cyan, width: 1.5),
        ),
        errorBorder: OutlineInputBorder(
          borderRadius: BorderRadius.circular(AppColors.radiusSmall),
          borderSide: const BorderSide(color: AppColors.danger),
        ),
        focusedErrorBorder: OutlineInputBorder(
          borderRadius: BorderRadius.circular(AppColors.radiusSmall),
          borderSide: const BorderSide(color: AppColors.danger, width: 1.5),
        ),
      ),
      textTheme: base.textTheme
          .apply(
            fontFamily: 'Inter',
            bodyColor: AppColors.textPrimary,
            displayColor: AppColors.textPrimary,
          )
          .copyWith(
            displayLarge: _heading(base.textTheme.displayLarge, 32),
            displayMedium: _heading(base.textTheme.displayMedium, 28),
            displaySmall: _heading(base.textTheme.displaySmall, 24),
            headlineLarge: _heading(base.textTheme.headlineLarge, 24),
            headlineMedium: _heading(base.textTheme.headlineMedium, 21),
            headlineSmall: _heading(base.textTheme.headlineSmall, 19),
            titleLarge: _heading(base.textTheme.titleLarge, 20),
            titleMedium: _heading(base.textTheme.titleMedium, 16),
            titleSmall: _heading(base.textTheme.titleSmall, 14),
            labelLarge: base.textTheme.labelLarge?.copyWith(
              fontFamily: 'SpaceGrotesk',
              fontWeight: FontWeight.w700,
              fontVariations: const <FontVariation>[FontVariation('wght', 700)],
            ),
          ),
      iconTheme: const IconThemeData(color: AppColors.textSecondary),
      snackBarTheme: const SnackBarThemeData(
        backgroundColor: AppColors.card,
        contentTextStyle: TextStyle(color: AppColors.textPrimary),
        behavior: SnackBarBehavior.floating,
      ),
      navigationBarTheme: NavigationBarThemeData(
        backgroundColor: AppColors.surfaceDark,
        indicatorColor: AppColors.primaryCyan.withValues(alpha: .16),
        labelTextStyle: WidgetStateProperty.resolveWith(
          (states) => TextStyle(
            color: states.contains(WidgetState.selected)
                ? AppColors.primaryCyan
                : AppColors.textSecondary,
            fontFamily: 'Inter',
            fontWeight: states.contains(WidgetState.selected)
                ? FontWeight.w700
                : FontWeight.w500,
            fontVariations: <FontVariation>[
              FontVariation(
                'wght',
                states.contains(WidgetState.selected) ? 700 : 500,
              ),
            ],
          ),
        ),
      ),
      filledButtonTheme: FilledButtonThemeData(
        style: FilledButton.styleFrom(
          foregroundColor: AppColors.backgroundDark,
          backgroundColor: AppColors.primaryCyan,
          disabledForegroundColor: AppColors.textSecondary,
          disabledBackgroundColor: AppColors.borderPrimary,
          shape: RoundedRectangleBorder(
            borderRadius: BorderRadius.circular(AppColors.radiusMedium),
          ),
          textStyle: const TextStyle(
            fontFamily: 'SpaceGrotesk',
            fontWeight: FontWeight.w700,
            fontVariations: <FontVariation>[FontVariation('wght', 700)],
          ),
        ),
      ),
      outlinedButtonTheme: OutlinedButtonThemeData(
        style: OutlinedButton.styleFrom(
          foregroundColor: AppColors.primaryCyan,
          side: const BorderSide(color: AppColors.borderSecondary),
          shape: RoundedRectangleBorder(
            borderRadius: BorderRadius.circular(AppColors.radiusMedium),
          ),
        ),
      ),
    );
  }

  static TextStyle? _heading(TextStyle? source, double size) =>
      source?.copyWith(
        fontFamily: 'SpaceGrotesk',
        fontSize: size,
        fontWeight: FontWeight.w700,
        fontVariations: const <FontVariation>[FontVariation('wght', 700)],
        color: AppColors.textPrimary,
      );
}

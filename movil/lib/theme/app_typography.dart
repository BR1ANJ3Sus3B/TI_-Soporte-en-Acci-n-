import 'package:flutter/material.dart';
import 'package:ti_soporte_accion/theme/app_colors.dart';

/// Roles tipográficos Stitch con familias empaquetadas para funcionar offline.
class AppTypography {
  const AppTypography._();

  static TextStyle heading(
    double size, {
    Color color = AppColors.textPrimary,
    FontWeight weight = FontWeight.w700,
  }) => TextStyle(
    fontFamily: 'SpaceGrotesk',
    fontSize: size,
    fontWeight: weight,
    fontVariations: <FontVariation>[
      FontVariation('wght', weight == FontWeight.w600 ? 600 : 700),
    ],
    color: color,
    height: 1.2,
  );

  static TextStyle body(double size, {Color color = AppColors.textPrimary}) =>
      TextStyle(
        fontFamily: 'Inter',
        fontSize: size,
        fontWeight: FontWeight.w400,
        fontVariations: <FontVariation>[FontVariation('wght', 400)],
        color: color,
        height: 1.5,
      );

  static TextStyle label(
    double size, {
    Color color = AppColors.textSecondary,
  }) => TextStyle(
    fontFamily: 'Inter',
    fontSize: size,
    fontWeight: FontWeight.w500,
    fontVariations: <FontVariation>[FontVariation('wght', 500)],
    color: color,
    height: 1.35,
  );

  static TextStyle metric(double size, {Color color = AppColors.primaryCyan}) =>
      TextStyle(
        fontFamily: 'SpaceMono',
        fontSize: size,
        fontWeight: FontWeight.w700,
        color: color,
        height: 1.25,
      );
}

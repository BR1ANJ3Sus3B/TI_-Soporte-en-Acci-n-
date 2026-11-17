import 'package:flutter/services.dart';
import 'package:flutter_test/flutter_test.dart';
import 'package:ti_soporte_accion/theme/app_colors.dart';
import 'package:ti_soporte_accion/theme/app_theme.dart';
import 'package:ti_soporte_accion/theme/app_typography.dart';

void main() {
  TestWidgetsFlutterBinding.ensureInitialized();

  test('usa los tokens oficiales Stitch y roles tipográficos acordados', () {
    final theme = AppTheme.dark;
    expect(AppColors.primaryCyan, const Color(0xFF4AE4EC));
    expect(AppColors.backgroundDark, const Color(0xFF06132A));
    expect(AppColors.surfaceDark, const Color(0xFF020D25));
    expect(AppColors.containerDark, const Color(0xFF0F1B33));
    expect(theme.colorScheme.primary, AppColors.primaryCyan);
    expect(theme.textTheme.bodyMedium?.fontFamily, 'Inter');
    expect(theme.textTheme.headlineMedium?.fontFamily, 'SpaceGrotesk');
    expect(AppTypography.metric(16).fontFamily, 'SpaceMono');
  });

  test('las fuentes de marca están incluidas para uso sin conexión', () async {
    for (final asset in <String>[
      'assets/fonts/SpaceGrotesk-Variable.ttf',
      'assets/fonts/Inter-Variable.ttf',
      'assets/fonts/SpaceMono-Regular.ttf',
      'assets/fonts/SpaceMono-Bold.ttf',
    ]) {
      expect((await rootBundle.load(asset)).lengthInBytes, greaterThan(1000));
    }
  });
}

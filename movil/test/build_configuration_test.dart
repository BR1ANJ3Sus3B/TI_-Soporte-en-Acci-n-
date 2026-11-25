import 'package:flutter/foundation.dart';
import 'package:flutter/material.dart';
import 'package:flutter_test/flutter_test.dart';
import 'package:ti_soporte_accion/config/app_build_config.dart';
import 'package:ti_soporte_accion/screens/login_screen.dart';
import 'package:ti_soporte_accion/services/auth_service.dart';

const _expectedIssuer = String.fromEnvironment('TEST_EXPECTED_KEYCLOAK_ISSUER');
const _expectedClientId = String.fromEnvironment(
  'TEST_EXPECTED_KEYCLOAK_CLIENT_ID',
);
const _expectedAllowHttp = bool.fromEnvironment(
  'TEST_EXPECTED_KEYCLOAK_ALLOW_HTTP',
);

void main() {
  test('AuthService reads compile-time values from Flutter dart-defines', () {
    final auth = AuthService();
    expect(auth.issuer, AppBuildConfig.keycloakIssuer);
    expect(auth.clientId, AppBuildConfig.keycloakClientId);
    expect(auth.allowHttpInDebug, AppBuildConfig.keycloakAllowHttp);

    if (AppBuildConfig.verifyKeycloakDefines) {
      expect(AppBuildConfig.hasKeycloakIssuer, isTrue);
      expect(AppBuildConfig.hasKeycloakClientId, isTrue);
      expect(AppBuildConfig.keycloakIssuer, _expectedIssuer);
      expect(AppBuildConfig.keycloakClientId, _expectedClientId);
      expect(AppBuildConfig.keycloakAllowHttp, _expectedAllowHttp);
      expect(AppBuildConfig.environment, 'development');
      expect(AppBuildConfig.buildId, isNotEmpty);
    }

    auth.dispose();
  });

  testWidgets('configured Android build enables login and account creation', (
    tester,
  ) async {
    debugDefaultTargetPlatformOverride = TargetPlatform.android;
    try {
      await tester.pumpWidget(const MaterialApp(home: LoginScreen()));

      expect(
        tester
            .widget<FilledButton>(
              find.widgetWithText(FilledButton, 'Comenzar como invitado'),
            )
            .onPressed,
        isNotNull,
      );
      expect(
        tester
            .widget<OutlinedButton>(
              find.widgetWithText(OutlinedButton, 'Iniciar sesión'),
            )
            .onPressed,
        isNotNull,
      );
      expect(
        tester
            .widget<TextButton>(
              find.widgetWithText(TextButton, 'Crear una cuenta gratis'),
            )
            .onPressed,
        isNotNull,
      );
    } finally {
      debugDefaultTargetPlatformOverride = null;
    }
  }, skip: !AppBuildConfig.verifyKeycloakDefines);
}

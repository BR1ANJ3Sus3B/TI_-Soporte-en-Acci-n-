import 'package:flutter/foundation.dart';

/// Compile-time configuration passed by Flutter's `--dart-define` options.
abstract final class AppBuildConfig {
  static const keycloakIssuer = String.fromEnvironment('KEYCLOAK_ISSUER');
  static const keycloakClientId = String.fromEnvironment('KEYCLOAK_CLIENT_ID');
  static const keycloakAllowHttp = bool.fromEnvironment('KEYCLOAK_ALLOW_HTTP');
  static const apiBaseUrl = String.fromEnvironment('API_BASE_URL');
  static const apiAllowHttp = bool.fromEnvironment('API_ALLOW_HTTP');
  static const demoMode = bool.fromEnvironment('DEMO_MODE');
  static const environment = String.fromEnvironment(
    'APP_ENV',
    defaultValue: 'development',
  );
  static const buildId = String.fromEnvironment(
    'APP_BUILD_ID',
    defaultValue: 'manual',
  );
  static const verifyKeycloakDefines = bool.fromEnvironment(
    'VERIFY_KEYCLOAK_DART_DEFINES',
  );

  static bool get hasKeycloakIssuer => keycloakIssuer.trim().isNotEmpty;
  static bool get hasKeycloakClientId => keycloakClientId.trim().isNotEmpty;

  /// Print only non-sensitive status fields, never endpoints, tokens or secrets.
  static void logDiagnostics() {
    if (!kDebugMode) return;
    debugPrint(
      '[build-config] issuerConfigured=$hasKeycloakIssuer '
      'clientIdConfigured=$hasKeycloakClientId '
      'environment=$environment buildId=$buildId',
    );
  }
}

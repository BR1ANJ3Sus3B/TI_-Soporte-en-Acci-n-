import 'dart:async';
import 'dart:convert';

import 'package:flutter/foundation.dart';
import 'package:flutter/material.dart';
import 'package:flutter_appauth/flutter_appauth.dart';
import 'package:flutter_test/flutter_test.dart';
import 'package:ti_soporte_accion/screens/login_screen.dart';
import 'package:ti_soporte_accion/services/auth_service.dart';
import 'package:ti_soporte_accion/widgets/session_gate.dart';

class FakeAppAuth extends FlutterAppAuth {
  Duration lifetime = const Duration(minutes: 5);
  bool cancel = false;
  bool failRefresh = false;
  bool failLogout = false;
  Completer<TokenResponse>? pendingRefresh;
  AuthorizationTokenRequest? request;

  @override
  Future<AuthorizationTokenResponse> authorizeAndExchangeCode(
    AuthorizationTokenRequest request,
  ) async {
    this.request = request;
    if (cancel) {
      throw FlutterAppAuthUserCancelledException(
        code: 'cancelled',
        platformErrorDetails: FlutterAppAuthPlatformErrorDetails(),
      );
    }
    final claims = base64Url.encode(
      utf8.encode(
        jsonEncode(<String, Object>{
          'name': 'Usuario de prueba',
          'email': 'test@example.com',
          'realm_access': <String, Object>{
            'roles': <String>['estudiante'],
          },
        }),
      ),
    );
    return AuthorizationTokenResponse(
      'access',
      'refresh',
      DateTime.now().add(lifetime),
      'header.$claims.signature',
      'Bearer',
      <String>['openid'],
      null,
      null,
    );
  }

  @override
  Future<TokenResponse> token(TokenRequest request) async {
    if (failRefresh) throw StateError('Sesión revocada');
    if (pendingRefresh != null) return pendingRefresh!.future;
    return refreshed();
  }

  TokenResponse refreshed() => TokenResponse(
    'renewed',
    'refresh',
    DateTime.now().add(const Duration(minutes: 5)),
    null,
    'Bearer',
    null,
    null,
  );

  @override
  Future<EndSessionResponse> endSession(EndSessionRequest request) async {
    if (failLogout) throw StateError('Sin conexión');
    return EndSessionResponse(null);
  }
}

class MemoryAuthTokenStore implements AuthTokenStore {
  String? value;
  @override
  Future<String?> read() async => value;
  @override
  Future<void> write(String value) async {
    this.value = value;
  }

  @override
  Future<void> clear() async {
    value = null;
  }
}

void main() {
  TestWidgetsFlutterBinding.ensureInitialized();
  late FakeAppAuth provider;
  late MemoryAuthTokenStore tokenStore;
  late AuthService auth;

  setUp(() {
    debugDefaultTargetPlatformOverride = TargetPlatform.android;
    provider = FakeAppAuth();
    tokenStore = MemoryAuthTokenStore();
    auth = AuthService(
      appAuth: provider,
      tokenStore: tokenStore,
      clientId: 'ti-soporte-flutter',
      issuer: 'https://id.example.com/realms/test',
    );
  });
  tearDown(() {
    auth.dispose();
    debugDefaultTargetPlatformOverride = null;
  });

  test('inicia sesión móvil y obtiene identidad y token', () async {
    expect(await auth.login(), isTrue);
    expect(auth.isAuthenticated, isTrue);
    expect(auth.displayName, 'Usuario de prueba');
    expect(auth.email, 'test@example.com');
    expect(auth.roles, contains('estudiante'));
    expect(auth.roleLabel, 'Jugador');
    expect(await auth.accessToken(), 'access');
    expect(tokenStore.value, isNotNull);
    expect(provider.request!.redirectUrl, AuthService.redirectUri);
  });

  test('cancelar el navegador no autentica al usuario', () async {
    provider.cancel = true;
    expect(await auth.login(), isFalse);
    expect(auth.isAuthenticated, isFalse);
    expect(await auth.accessToken(), isNull);
  });

  test('renueva el token próximo a vencer', () async {
    provider.lifetime = const Duration(seconds: 20);
    await auth.login();
    expect(await auth.accessToken(), 'renewed');
    expect(auth.isAuthenticated, isTrue);
  });

  test('rechaza tokens vencidos y elimina sesiones revocadas', () async {
    provider.lifetime = const Duration(seconds: -1);
    await expectLater(auth.login(), throwsStateError);
    expect(auth.isAuthenticated, isFalse);
    provider.lifetime = const Duration(seconds: 20);
    await auth.login();
    provider.failRefresh = true;
    expect(await auth.accessToken(), isNull);
    expect(auth.isAuthenticated, isFalse);
  });

  test('logout borra sesión local aunque falle el servidor', () async {
    await auth.login();
    provider.failLogout = true;
    await expectLater(auth.logout(), throwsStateError);
    expect(auth.isAuthenticated, isFalse);
    expect(await auth.accessToken(), isNull);
    expect(tokenStore.value, isNull);
  });

  test('restaura la sesión segura después de recrear el servicio', () async {
    final original = AuthService(
      appAuth: provider,
      tokenStore: tokenStore,
      clientId: 'ti-soporte-flutter',
      issuer: 'https://id.example.com/realms/test',
    );
    await original.login();
    original.dispose();
    final restarted = AuthService(
      appAuth: provider,
      tokenStore: tokenStore,
      clientId: 'ti-soporte-flutter',
      issuer: 'https://id.example.com/realms/test',
    );
    await restarted.restoreSession();
    expect(restarted.isInitialized, isTrue);
    expect(restarted.isAuthenticated, isTrue);
    expect(await restarted.accessToken(), 'access');
    expect(restarted.displayName, 'Usuario de prueba');
    restarted.dispose();
  });

  test(
    'renueva token vencido al restaurar y borra sesión si se revocó',
    () async {
      tokenStore.value = jsonEncode(<String, Object?>{
        'accessToken': 'expired',
        'refreshToken': 'refresh',
        'idToken': null,
        'expiresAt': DateTime.now()
            .subtract(const Duration(minutes: 1))
            .toIso8601String(),
      });
      await auth.restoreSession();
      expect(auth.isAuthenticated, isTrue);
      expect(await auth.accessToken(), 'renewed');

      await auth.logout();
      tokenStore.value = jsonEncode(<String, Object?>{
        'accessToken': 'expired',
        'refreshToken': 'refresh',
        'idToken': null,
        'expiresAt': DateTime.now()
            .subtract(const Duration(minutes: 1))
            .toIso8601String(),
      });
      provider.failRefresh = true;
      final revokedSession = AuthService(
        appAuth: provider,
        tokenStore: tokenStore,
        clientId: 'ti-soporte-flutter',
        issuer: 'https://id.example.com/realms/test',
      );
      await revokedSession.restoreSession();
      expect(revokedSession.isAuthenticated, isFalse);
      expect(tokenStore.value, isNull);
      revokedSession.dispose();
    },
  );

  test(
    'sesión expirada sin refresh token no conserva el access token',
    () async {
      tokenStore.value = jsonEncode(<String, Object?>{
        'accessToken': 'expired',
        'refreshToken': null,
        'idToken': null,
        'expiresAt': DateTime.now()
            .subtract(const Duration(seconds: 1))
            .toIso8601String(),
      });
      await auth.restoreSession();
      expect(auth.isAuthenticated, isFalse);
      expect(tokenStore.value, isNull);
    },
  );

  test(
    'una renovación tardía no restaura la sesión después del logout',
    () async {
      provider.lifetime = const Duration(seconds: 20);
      await auth.login();
      provider.pendingRefresh = Completer<TokenResponse>();
      final refresh = auth.accessToken();
      await auth.logout();
      provider.pendingRefresh!.complete(provider.refreshed());
      expect(await refresh, isNull);
      expect(auth.isAuthenticated, isFalse);
    },
  );

  testWidgets('una ruta privada no se muestra sin sesión', (tester) async {
    debugDefaultTargetPlatformOverride = null;
    await tester.pumpWidget(
      const MaterialApp(home: SessionGate(child: Text('Contenido privado'))),
    );
    expect(find.text('Contenido privado'), findsNothing);
    expect(find.text('Ver opciones de cuenta'), findsOneWidget);
  });

  testWidgets('la bienvenida oculta detalles de configuración de Keycloak', (
    tester,
  ) async {
    debugDefaultTargetPlatformOverride = null;
    await tester.pumpWidget(const MaterialApp(home: LoginScreen()));
    expect(find.textContaining('KEYCLOAK_ISSUER'), findsNothing);
    final button = tester.widget<FilledButton>(
      find.widgetWithText(FilledButton, 'Comenzar como invitado'),
    );
    expect(button.onPressed, isNotNull);
  });

  test('describe issuer ausente y evita activar HTTP fuera de debug permitido', () {
    final noIssuer = AuthService(
      appAuth: provider,
      tokenStore: tokenStore,
      clientId: 'ti-soporte-flutter',
    );
    expect(noIssuer.isConfigured, isFalse);
    expect(noIssuer.configurationIssue, contains('KEYCLOAK_ISSUER'));

    final httpWithoutOptIn = AuthService(
      appAuth: provider,
      tokenStore: tokenStore,
      clientId: 'ti-soporte-flutter',
      issuer: 'http://localhost:8080/realms/ti-soporte',
    );
    expect(httpWithoutOptIn.isConfigured, isFalse);
    expect(httpWithoutOptIn.configurationIssue, contains('HTTPS'));

    final discoveryUrlInsteadOfIssuer = AuthService(
      appAuth: provider,
      tokenStore: tokenStore,
      clientId: 'ti-soporte-flutter',
      issuer:
          'https://id.example.com/realms/test/.well-known/openid-configuration',
    );
    expect(discoveryUrlInsteadOfIssuer.isConfigured, isFalse);
    expect(discoveryUrlInsteadOfIssuer.configurationIssue, contains('sin'));

    final noClient = AuthService(
      appAuth: provider,
      tokenStore: tokenStore,
      issuer: 'https://id.example.com/realms/test',
      clientId: '',
    );
    expect(noClient.isConfigured, isFalse);
    expect(noClient.configurationIssue, contains('KEYCLOAK_CLIENT_ID'));

    final localDebugHttp = AuthService(
      appAuth: provider,
      tokenStore: tokenStore,
      issuer: 'http://localhost:8080/realms/test',
      clientId: 'ti-soporte-flutter',
      allowHttpInDebug: true,
    );
    expect(localDebugHttp.isConfigured, isTrue);

    noIssuer.dispose();
    httpWithoutOptIn.dispose();
    discoveryUrlInsteadOfIssuer.dispose();
    noClient.dispose();
    localDebugHttp.dispose();
  });
}

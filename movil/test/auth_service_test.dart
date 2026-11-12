import 'dart:async';
import 'dart:convert';

import 'package:flutter/foundation.dart';
import 'package:flutter/material.dart';
import 'package:flutter_appauth/flutter_appauth.dart';
import 'package:flutter_test/flutter_test.dart';
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
        jsonEncode(<String, String>{
          'name': 'Usuario de prueba',
          'email': 'test@example.com',
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

void main() {
  TestWidgetsFlutterBinding.ensureInitialized();
  late FakeAppAuth provider;
  late AuthService auth;

  setUp(() {
    debugDefaultTargetPlatformOverride = TargetPlatform.android;
    provider = FakeAppAuth();
    auth = AuthService(
      appAuth: provider,
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
    expect(await auth.accessToken(), 'access');
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
  });

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
    expect(find.text('Iniciar sesión con Keycloak'), findsOneWidget);
  });
}

import 'dart:convert';

import 'package:flutter/material.dart';
import 'package:flutter_test/flutter_test.dart';
import 'package:http/http.dart' as http;
import 'package:http/testing.dart';
import 'package:ti_soporte_accion/screens/login_screen.dart';
import 'package:ti_soporte_accion/screens/registration_screen.dart';
import 'package:ti_soporte_accion/services/api_client.dart';
import 'package:ti_soporte_accion/services/registration_service.dart';

void main() {
  test('validación de correo y contraseña coincide con los mínimos de API', () {
    expect(RegistrationService.validEmail(' player@example.test '), isTrue);
    expect(RegistrationService.validEmail('not-an-email'), isFalse);
    expect(RegistrationService.validPassword('weak'), isFalse);
    expect(RegistrationService.validPassword('Strong-Test-123!'), isTrue);
  });

  testWidgets('Crear una cuenta abre formulario nativo sin Keycloak', (
    tester,
  ) async {
    await tester.pumpWidget(
      MaterialApp(
        home: const LoginScreen(),
        routes: {'/register': (_) => const RegistrationScreen()},
      ),
    );
    await tester.ensureVisible(find.text('Crear una cuenta gratis'));
    await tester.tap(find.text('Crear una cuenta gratis'));
    await tester.pumpAndSettle();
    expect(find.text('Crea tu cuenta'), findsOneWidget);
    expect(find.byType(TextFormField), findsNWidgets(4));
  });

  for (final success in [true, false]) {
    testWidgets('formulario envía API y confirma solo al aceptar: $success', (
      tester,
    ) async {
      var requests = 0;
      final service = RegistrationService(
        api: ApiClient(
          baseUrl: 'https://api.example.test/api/v1',
          client: MockClient((request) async {
            requests++;
            expect(request.method, 'POST');
            expect(request.url.path, '/api/v1/auth/register');
            expect(request.headers.containsKey('Authorization'), isFalse);
            final body = jsonDecode(request.body) as Map;
            expect(body.keys.toSet(), {'name', 'email', 'password'});
            expect(body['email'], 'player@example.test');
            return success
                ? http.Response('{"status":"verification_pending"}', 202)
                : http.Response(
                    '{"error":{"code":"registration_unavailable","message":"Correo no disponible"}}',
                    503,
                  );
          }),
        ),
      );
      await tester.pumpWidget(
        MaterialApp(home: RegistrationScreen(service: service)),
      );
      final fields = find.byType(TextFormField);
      await tester.enterText(fields.at(0), 'Test Player');
      await tester.enterText(fields.at(1), 'Player@Example.Test');
      await tester.enterText(fields.at(2), 'Strong-Test-123!');
      await tester.enterText(fields.at(3), 'Strong-Test-123!');
      await tester.ensureVisible(find.text('Crear cuenta'));
      await tester.tap(find.text('Crear cuenta'));
      await tester.pumpAndSettle();
      expect(requests, 1);
      expect(
        find.text('Revisa tu correo'),
        success ? findsOneWidget : findsNothing,
      );
      if (!success) expect(find.text('Correo no disponible'), findsOneWidget);
      service.dispose();
    });
  }
}

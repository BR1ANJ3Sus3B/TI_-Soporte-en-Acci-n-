import 'package:flutter_test/flutter_test.dart';
import 'package:http/http.dart' as http;
import 'package:http/testing.dart';
import 'package:ti_soporte_accion/services/api_client.dart';
import 'package:ti_soporte_accion/services/auth_service.dart';

class FakeAuthService extends AuthService {
  FakeAuthService()
    : super(clientId: 'test-client', issuer: 'https://id.example.test');
  bool expired = false;
  @override
  Future<String?> accessToken() async => 'unit-test-token';
  @override
  Future<void> expireSessionForToken(String rejectedToken) async {
    expired = true;
  }
}

void main() {
  TestWidgetsFlutterBinding.ensureInitialized();

  test('adjunta access token y decodifica el contrato JSON', () async {
    final auth = FakeAuthService();
    final client = ApiClient(
      baseUrl: 'https://api.example.test/api/v1',
      authService: auth,
      client: MockClient((request) async {
        expect(request.url.path, '/api/v1/profile');
        expect(request.headers['Authorization'], 'Bearer unit-test-token');
        return http.Response(
          '{"name":"Alex","xp":0}',
          200,
          headers: <String, String>{'content-type': 'application/json'},
        );
      }),
    );
    expect(await client.getJson('/profile'), <String, dynamic>{
      'name': 'Alex',
      'xp': 0,
    });
    client.close();
    auth.dispose();
  });

  test('401 invalida la sesión local', () async {
    final auth = FakeAuthService();
    final client = ApiClient(
      baseUrl: 'https://api.example.test/api/v1',
      authService: auth,
      client: MockClient(
        (_) async => http.Response(
          '{"error":{"code":"invalid_token","message":"expired"}}',
          401,
        ),
      ),
    );
    await expectLater(client.getJson('/profile'), throwsA(isA<ApiException>()));
    expect(auth.expired, isTrue);
    client.close();
    auth.dispose();
  });

  test('401 público no invalida sesión ni envía token', () async {
    final auth = FakeAuthService();
    final client = ApiClient(
      baseUrl: 'https://api.example.test/api/v1',
      authService: auth,
      client: MockClient((request) async {
        expect(request.headers.containsKey('Authorization'), isFalse);
        return http.Response('{"error":{"code":"missing_bearer_token"}}', 401);
      }),
    );
    await expectLater(
      client.getPublicJson('/missions'),
      throwsA(
        isA<ApiException>().having(
          (e) => e.failure,
          'failure',
          ApiFailure.forbidden,
        ),
      ),
    );
    expect(auth.expired, isFalse);
    client.close();
    auth.dispose();
  });

  test('HTTP requiere habilitación explícita de debug y base /api/v1', () {
    final auth = FakeAuthService();
    final client = ApiClient(
      baseUrl: 'http://api.example.test/api/v1',
      authService: auth,
    );
    expect(client.isConfigured, isFalse);
    expect(client.configurationIssue, contains('HTTPS'));
    final wrongPath = ApiClient(
      baseUrl: 'https://api.example.test',
      authService: auth,
    );
    expect(wrongPath.isConfigured, isFalse);
    expect(wrongPath.configurationIssue, contains('/api/v1'));
    client.close();
    wrongPath.close();
    auth.dispose();
  });
}

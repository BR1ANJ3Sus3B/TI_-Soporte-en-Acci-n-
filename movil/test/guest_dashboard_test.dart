import 'package:flutter_secure_storage/flutter_secure_storage.dart';
import 'package:flutter_test/flutter_test.dart';
import 'package:http/http.dart' as http;
import 'package:http/testing.dart';
import 'package:ti_soporte_accion/services/api_client.dart';
import 'package:ti_soporte_accion/services/app_data_repository.dart';
import 'package:ti_soporte_accion/services/auth_service.dart';

void main() {
  TestWidgetsFlutterBinding.ensureInitialized();

  for (final status in [200, 401, 500]) {
    test(
      'Home invitado conserva estado sin perfil privado: HTTP $status',
      () async {
        FlutterSecureStorage.setMockInitialValues({});
        final auth = AuthService();
        await auth.continueAsGuest();
        final paths = <String>[];
        final api = ApiClient(
          authService: auth,
          baseUrl: 'https://api.example.test/api/v1',
          client: MockClient((request) async {
            paths.add(request.url.path);
            expect(request.headers.containsKey('Authorization'), isFalse);
            return http.Response(status == 200 ? '[]' : '{}', status);
          }),
        );
        final repository = AppDataRepository(
          api: api,
          authService: auth,
          demoMode: false,
        );
        for (var attempt = 0; attempt < 2; attempt++) {
          final dashboard = await repository.getDashboard();
          expect(dashboard.profile.name, 'Invitado');
          expect(auth.isGuest, isTrue);
        }
        expect(paths, ['/api/v1/missions', '/api/v1/missions']);
        final restored = AuthService();
        await restored.restoreSession();
        expect(restored.isGuest, isTrue);
        restored.dispose();
        api.close();
        auth.dispose();
      },
    );
  }
}

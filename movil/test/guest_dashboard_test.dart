import 'package:flutter_secure_storage/flutter_secure_storage.dart';
import 'package:flutter_test/flutter_test.dart';
import 'package:http/http.dart' as http;
import 'package:http/testing.dart';
import 'package:ti_soporte_accion/services/api_client.dart';
import 'package:ti_soporte_accion/services/app_data_repository.dart';
import 'package:ti_soporte_accion/services/auth_service.dart';
import 'package:ti_soporte_accion/models/mission.dart';

void main() {
  TestWidgetsFlutterBinding.ensureInitialized();

  test('catálogo remoto conserva actividad local al reiniciar', () async {
    FlutterSecureStorage.setMockInitialValues({});
    final auth = AuthService();
    await auth.continueAsGuest();
    final api = ApiClient(
      authService: auth,
      baseUrl: 'https://api.example.test/api/v1',
      client: MockClient(
        (request) async => http.Response(
          '[{"id":"public-mission","title":"Public mission","description":"Test catalog","category":"Support","xp":10,"difficulty":"basic","status":"Disponible"}]',
          200,
        ),
      ),
    );
    final repository = AppDataRepository(
      api: api,
      authService: auth,
      demoMode: false,
    );
    await repository.startGuestMission('public-mission');
    await repository.startGuestMission('public-mission');
    expect(
      (await repository.getMission('public-mission')).status,
      MissionStatus.inProgress,
    );
    final restored = AuthService();
    await restored.restoreSession();
    final reopened = AppDataRepository(
      api: api,
      authService: restored,
      demoMode: false,
    );
    final dashboard = await reopened.getDashboard();
    expect(dashboard.currentMission?.id, 'public-mission');
    expect(dashboard.currentMission?.status, MissionStatus.inProgress);
    final progress = await reopened.getProgressSummary();
    expect(progress.inProgressMissions, 1);
    expect(progress.xp, 0);
    api.close();
    auth.dispose();
    restored.dispose();
  });

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

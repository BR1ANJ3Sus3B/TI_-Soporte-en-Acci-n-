import 'package:flutter/material.dart';
import 'package:flutter_test/flutter_test.dart';
import 'package:ti_soporte_accion/screens/mission_detail_screen.dart';
import 'package:ti_soporte_accion/screens/missions_screen.dart';
import 'package:ti_soporte_accion/services/achievement_notification_service.dart';

void main() {
  TestWidgetsFlutterBinding.ensureInitialized();

  testWidgets('permite filtrar el catálogo demo por estado', (tester) async {
    await tester.pumpWidget(const MaterialApp(home: MissionsScreen()));
    expect(find.text('No puedo iniciar sesión'), findsOneWidget);
    expect(find.text('Sin conexión a Internet'), findsOneWidget);
    await tester.scrollUntilVisible(
      find.text('Servidor fuera de línea'),
      250,
      scrollable: find.byType(Scrollable).last,
    );
    expect(find.text('Servidor fuera de línea'), findsOneWidget);

    await tester.tap(find.widgetWithText(ChoiceChip, 'Completadas'));
    await tester.pumpAndSettle();
    expect(find.text('No puedo iniciar sesión'), findsOneWidget);
    expect(find.text('Sin conexión a Internet'), findsNothing);
    expect(find.text('Servidor fuera de línea'), findsNothing);
  });

  testWidgets('un identificador de misión inválido no muestra otra misión', (
    tester,
  ) async {
    await tester.pumpWidget(const MaterialApp(home: MissionDetailScreen()));
    expect(
      find.text('No se encontró esa misión de demostración.'),
      findsOneWidget,
    );
    expect(find.text('No puedo iniciar sesión'), findsNothing);
  });

  testWidgets('iniciar desde móvil no desbloquea logros', (tester) async {
    final notifications = AchievementNotificationService.instance..reset();
    await tester.pumpWidget(
      MaterialApp(
        home: Builder(
          builder: (context) => TextButton(
            onPressed: () => Navigator.of(context).push(
              MaterialPageRoute<void>(
                settings: const RouteSettings(arguments: 'm02'),
                builder: (_) => const MissionDetailScreen(),
              ),
            ),
            child: const Text('Abrir misión'),
          ),
        ),
      ),
    );
    await tester.tap(find.text('Abrir misión'));
    await tester.pumpAndSettle();
    await tester.ensureVisible(find.text('Continuar en videojuego'));
    await tester.pumpAndSettle();
    await tester.tap(find.text('Continuar en videojuego'));
    await tester.pumpAndSettle();
    expect(find.textContaining('no inicia misiones'), findsOneWidget);
    expect(notifications.current.value, isNull);
  });
}

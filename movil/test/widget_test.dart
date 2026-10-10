import 'package:flutter_test/flutter_test.dart';
import 'package:ti_soporte_accion/main.dart';

void main() {
  testWidgets('TiSoporteApp build smoke test', (WidgetTester tester) async {
    await tester.pumpWidget(const TiSoporteApp());
    await tester.pumpAndSettle(const Duration(seconds: 3));
    expect(find.text('TI: Soporte en Acción'), findsWidgets);
  });
}

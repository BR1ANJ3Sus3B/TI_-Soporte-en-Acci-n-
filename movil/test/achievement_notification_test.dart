import 'package:flutter/material.dart';
import 'package:flutter_test/flutter_test.dart';
import 'package:ti_soporte_accion/models/achievement.dart';
import 'package:ti_soporte_accion/services/achievement_notification_service.dart';

Achievement _build(String id) => Achievement(
  id: id,
  title: 'Logro $id',
  description: 'Descripción $id',
  icon: Icons.emoji_events_rounded,
  unlocked: true,
  status: AchievementStatus.unlocked,
  xp: 100,
  rarity: AchievementRarity.rare,
);

void main() {
  late AchievementNotificationService service;

  setUp(() {
    service = AchievementNotificationService.instance..reset();
  });

  test('muestra el primer logro al encolarlo', () {
    final a = _build('a01');
    service.notifyUnlock(a);
    expect(service.current.value?.id, 'a01');
  });

  test('no notifica el mismo logro dos veces', () {
    service.notifyUnlock(_build('a01'));
    service.markCompleted(_build('a01'));
    expect(service.current.value, isNull);

    service.notifyUnlock(_build('a01'));
    expect(service.current.value, isNull);
    expect(service.wasShown('a01'), isTrue);
  });

  test('encola varios logros y los entrega en orden sin superponerse', () {
    service.notifyUnlock(_build('a01'));
    service.notifyUnlock(_build('a02'));

    expect(service.current.value?.id, 'a01');

    service.markCompleted(service.current.value!);
    expect(service.current.value?.id, 'a02');

    service.markCompleted(service.current.value!);
    expect(service.current.value, isNull);
  });

  test('la rareza expone etiqueta y color', () {
    expect(AchievementRarity.legendary.label, 'Legendario');
    expect(AchievementRarity.legendary.color, const Color(0xFFF59E0B));
  });
}

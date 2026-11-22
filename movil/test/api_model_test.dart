import 'package:flutter_test/flutter_test.dart';
import 'package:ti_soporte_accion/models/achievement.dart';
import 'package:ti_soporte_accion/models/mission.dart';
import 'package:ti_soporte_accion/models/player_profile.dart';
import 'package:ti_soporte_accion/models/skill.dart';

void main() {
  test('mapea campos del contrato de misión sin conceder estado local', () {
    final mission = Mission.fromJson(<String, dynamic>{
      'id': 'm01',
      'title': 'Diagnóstico',
      'category': 'Soporte',
      'description': 'Revisar el equipo',
      'difficulty': 'Media',
      'duration': '15 min',
      'xp': 100,
      'status': 'Completada',
      'level': 1,
      'department': 'TI',
      'affectedUser': 'Sofía',
      'objectives': <String>['Aislar el problema'],
      'requirements': <String>['Hablar con el responsable'],
    });
    expect(mission.id, 'm01');
    expect(mission.status, MissionStatus.completed);
    expect(mission.xpReward, 100);
    expect(mission.requirements, contains('Hablar con el responsable'));
  });

  test('no inventa el umbral de XP que la API aún no ha definido', () {
    final profile = PlayerProfile.fromJson(<String, dynamic>{
      'id': 'keycloak-sub',
      'name': 'Usuario',
      'role': 'Jugador',
      'level': 1,
      'xp': 80,
      'nextLevelXp': null,
      'missionsCompleted': 0,
      'incidentsResolved': 0,
    });
    expect(profile.currentXp, 80);
    expect(profile.xpForNextLevel, isNull);
  });

  test('usa el progreso de habilidad devuelto por la API y no asigna rareza omitida', () {
    final skill = Skill.fromJson(<String, dynamic>{
      'name': 'Redes',
      'progress': 0.4,
      'icon': 'network',
    });
    final achievement = Achievement.fromJson(<String, dynamic>{
      'id': 'a01',
      'title': 'Logro',
      'description': 'Requisito verificado',
      'xp': 50,
      'unlocked': false,
      'icon': 'award',
    });
    expect(skill.progress, 0.4);
    expect(skill.level, isNull);
    expect(achievement.rarity, isNull);
    expect(achievement.unlockedAt, isNull);
  });
}

import 'package:ti_soporte_accion/data/mock_data.dart';
import 'package:ti_soporte_accion/models/achievement.dart';
import 'package:ti_soporte_accion/models/map_area.dart';
import 'package:ti_soporte_accion/models/mission.dart';
import 'package:ti_soporte_accion/models/player_profile.dart';
import 'package:ti_soporte_accion/models/skill.dart';
import 'package:ti_soporte_accion/services/api_client.dart';
import 'package:ti_soporte_accion/services/auth_service.dart';
import 'package:ti_soporte_accion/services/guest_progress_store.dart';

class DashboardData {
  const DashboardData({required this.profile, required this.currentMission});
  final PlayerProfile profile;
  final Mission? currentMission;
}

class ProgressSummary {
  const ProgressSummary({
    required this.level,
    required this.xp,
    required this.completedMissions,
    required this.inProgressMissions,
    this.nextLevelXp,
    this.updatedAt,
  });
  final int? level;
  final int xp;
  final int? nextLevelXp;
  final int completedMissions;
  final int inProgressMissions;
  final DateTime? updatedAt;
}

class ProgressPageData {
  const ProgressPageData({
    required this.profile,
    required this.progress,
    required this.skills,
    required this.achievements,
  });
  final PlayerProfile profile;
  final ProgressSummary progress;
  final List<Skill> skills;
  final List<Achievement> achievements;
}

/// Fuente única para pantallas móviles. Demo solo se activa explícitamente con
/// `--dart-define=DEMO_MODE=true`; por defecto consulta la API configurada.
class AppDataRepository {
  AppDataRepository({ApiClient? api, bool? demoMode})
    : _api = api ?? ApiClient(),
      demoMode = demoMode ?? const bool.fromEnvironment('DEMO_MODE');

  static final instance = AppDataRepository();
  final ApiClient _api;
  final bool demoMode;
  bool get apiConfigured => _api.isConfigured;
  String? get apiConfigurationIssue => _api.configurationIssue;
  bool get _guest => AuthService.instance.isGuest;

  Future<PlayerProfile> getProfile() async {
    if (_guest) {
      final progress = await GuestProgressStore.instance.read();
      return PlayerProfile(
        name: 'Invitado', rank: 'Progreso local', level: 1,
        currentXp: progress.xp, xpForNextLevel: 500,
        skills: MockData.player.skills,
        missionsCompleted: progress.completedMissionIds.length,
        incidentsResolved: 0,
      );
    }
    if (demoMode) return MockData.player;
    final json = _asMap(await _api.getJson('/profile'));
    return PlayerProfile.fromJson(<String, dynamic>{
      ...json,
      'name': json['name'] ?? AuthService.instance.displayName,
      'role': json['role'] ?? AuthService.instance.roleLabel,
    });
  }

  Future<PlayerProfile> updateDisplayName(String displayName) async {
    if (demoMode) {
      throw const ApiException(
        ApiFailure.configuration,
        'El perfil de demostración no se puede guardar en el servidor.',
      );
    }
    final json = _asMap(
      await _api.patchJson('/profile', <String, Object?>{
        'displayName': displayName.trim(),
      }),
    );
    return PlayerProfile.fromJson(json);
  }

  Future<List<Mission>> getMissions() async {
    if (_guest) {
      try {
        if (_api.isConfigured) {
          return _asList(await _api.getPublicJson('/missions'))
              .map(Mission.fromJson).toList(growable: false);
        }
      } on ApiException {
        // El catálogo incorporado permite seguir aprendiendo sin conexión.
      }
      return _guestMissions(await GuestProgressStore.instance.read());
    }
    if (demoMode) return List<Mission>.unmodifiable(MockData.missions);
    return _asList(await _api.getJson('/missions'))
        .map(Mission.fromJson)
        .toList(growable: false);
  }

  Future<Mission> getMission(String id) async {
    if (_guest) {
      final catalog = await getMissions();
      try {
        return catalog.firstWhere((mission) => mission.id == id);
      } on StateError {
        throw const ApiException(ApiFailure.notFound, 'La misión ya no está disponible.', statusCode: 404);
      }
    }
    if (demoMode) {
      try {
        return MockData.missions.firstWhere((mission) => mission.id == id);
      } on StateError {
        throw const ApiException(
          ApiFailure.notFound,
          'No se encontró esa misión de demostración.',
          statusCode: 404,
        );
      }
    }
    return Mission.fromJson(
      _asMap(await _api.getJson('/missions/${Uri.encodeComponent(id)}')),
    );
  }

  Future<List<Skill>> getSkills() async {
    if (_guest) return List<Skill>.unmodifiable(MockData.skills);
    if (demoMode) return List<Skill>.unmodifiable(MockData.skills);
    return _asList(await _api.getJson('/progress/specialties'))
        .map(Skill.fromJson)
        .toList(growable: false);
  }

  Future<List<Achievement>> getAchievements() async {
    if (_guest) return List<Achievement>.unmodifiable(MockData.achievements);
    if (demoMode) return List<Achievement>.unmodifiable(MockData.achievements);
    return _asList(await _api.getJson('/achievements'))
        .map(Achievement.fromJson)
        .toList(growable: false);
  }

  Future<List<MapArea>> getAreas() async {
    if (_guest) return List<MapArea>.unmodifiable(MockData.areas);
    if (demoMode) return List<MapArea>.unmodifiable(MockData.areas);
    return _asList(await _api.getJson('/areas'))
        .map(MapArea.fromJson)
        .toList(growable: false);
  }

  Future<ProgressSummary> getProgressSummary() async {
    if (_guest) {
      final progress = await GuestProgressStore.instance.read();
      return ProgressSummary(
        level: 1, xp: progress.xp, nextLevelXp: 500,
        completedMissions: progress.completedMissionIds.length,
        inProgressMissions: progress.inProgressMissionIds.length,
      );
    }
    if (demoMode) {
      final completed = MockData.missions.where((m) => m.isCompleted).length;
      final inProgress = MockData.missions
          .where((m) => m.status == MissionStatus.inProgress)
          .length;
      return ProgressSummary(
        level: MockData.player.level,
        xp: MockData.player.currentXp ?? 0,
        nextLevelXp: MockData.player.xpForNextLevel,
        completedMissions: completed,
        inProgressMissions: inProgress,
      );
    }
    final json = _asMap(await _api.getJson('/progress'));
    final completed = json['completedMissionIds'];
    final inProgress = json['inProgressMissionIds'];
    return ProgressSummary(
      level: (json['level'] as num?)?.toInt(),
      xp: (json['xp'] as num?)?.toInt() ?? 0,
      nextLevelXp: (json['nextLevelXp'] as num?)?.toInt(),
      completedMissions: completed is List ? completed.length : 0,
      inProgressMissions: inProgress is List ? inProgress.length : 0,
      updatedAt: DateTime.tryParse(json['updatedAt'] as String? ?? ''),
    );
  }

  Future<DashboardData> getDashboard() async {
    final results = await Future.wait<Object>([getProfile(), getMissions()]);
    final profile = results[0] as PlayerProfile;
    final missions = results[1] as List<Mission>;
    Mission? current;
    for (final mission in missions) {
      if (mission.status == MissionStatus.inProgress) {
        current = mission;
        break;
      }
    }
    if (current == null) {
      for (final mission in missions) {
        if (mission.status == MissionStatus.available) {
          current = mission;
          break;
        }
      }
    }
    return DashboardData(profile: profile, currentMission: current);
  }

  Future<ProgressPageData> getProgressPage() async {
    final results = await Future.wait<Object>([
      getProfile(),
      getProgressSummary(),
      getSkills(),
      getAchievements(),
    ]);
    return ProgressPageData(
      profile: results[0] as PlayerProfile,
      progress: results[1] as ProgressSummary,
      skills: results[2] as List<Skill>,
      achievements: results[3] as List<Achievement>,
    );
  }

  /// Marca actividad local para que el invitado pueda retomarla tras reiniciar.
  /// No concede XP, logros ni resultados oficiales.
  Future<void> startGuestMission(String missionId) async {
    if (!_guest) {
      return;
    }
    final current = await GuestProgressStore.instance.read();
    if (current.completedMissionIds.contains(missionId) ||
        current.inProgressMissionIds.contains(missionId)) {
      return;
    }
    await GuestProgressStore.instance.write(GuestProgress(
      completedMissionIds: current.completedMissionIds,
      inProgressMissionIds: <String>[...current.inProgressMissionIds, missionId],
      xp: current.xp,
    ));
  }

  List<Mission> _guestMissions(GuestProgress progress) => MockData.missions
      .map((mission) => Mission(
            id: mission.id, title: mission.title, description: mission.description,
            area: mission.area, department: mission.department,
            affectedUser: mission.affectedUser, difficulty: mission.difficulty,
            estimatedTime: mission.estimatedTime, xpReward: mission.xpReward,
            objectives: mission.objectives, requirements: mission.requirements,
            status: progress.completedMissionIds.contains(mission.id)
                ? MissionStatus.completed
                : progress.inProgressMissionIds.contains(mission.id)
                    ? MissionStatus.inProgress
                    : MissionStatus.available,
          ))
      .toList(growable: false);

  static Map<String, dynamic> _asMap(dynamic value) {
    if (value is Map<String, dynamic>) return value;
    throw const ApiException(
      ApiFailure.invalidResponse,
      'La API devolvió un objeto con formato inesperado.',
    );
  }

  static List<Map<String, dynamic>> _asList(dynamic value) {
    if (value is List) {
      return value.whereType<Map<String, dynamic>>().toList(growable: false);
    }
    throw const ApiException(
      ApiFailure.invalidResponse,
      'La API devolvió una lista con formato inesperado.',
    );
  }
}

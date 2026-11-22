/// Perfil del jugador usado en las pantallas de inicio, perfil y progreso.
///
/// Es un modelo de datos simulado; en etapas posteriores se cargará desde la
/// API del proyecto.
class PlayerProfile {
  const PlayerProfile({
    required this.name,
    required this.rank,
    this.level,
    this.currentXp,
    this.xpForNextLevel,
    required this.skills,
    this.missionsCompleted,
    this.incidentsResolved,
  });

  /// Nombre visible del jugador.
  final String name;

  /// Rango actual (por ejemplo, Técnico Junior).
  final String rank;

  /// Nivel actual del jugador.
  final int? level;

  /// Experiencia acumulada en el nivel actual.
  final int? currentXp;

  /// Experiencia necesaria para subir al siguiente nivel.
  final int? xpForNextLevel;

  /// Habilidades con su nivel de dominio.
  final Map<String, int> skills;

  /// Cantidad de misiones completadas.
  final int? missionsCompleted;

  /// Cantidad de incidentes resueltos.
  final int? incidentsResolved;

  /// Porcentaje de avance dentro del nivel actual (0.0 a 1.0).
  double get xpProgress {
    if (xpForNextLevel == null || xpForNextLevel! <= 0 || currentXp == null) {
      return 0;
    }
    return (currentXp! / xpForNextLevel!).clamp(0.0, 1.0);
  }

  factory PlayerProfile.fromJson(Map<String, dynamic> json) {
    final rawSkills = json['skills'];
    final skills = <String, int>{};
    if (rawSkills is Map) {
      rawSkills.forEach((key, value) {
        if (key is String && value is num) skills[key] = value.toInt();
      });
    }
    int? optionalInt(Object? value) => value is num ? value.toInt() : null;
    return PlayerProfile(
      name: json['name'] as String? ?? 'Usuario',
      rank: json['role'] as String? ?? 'Sin rol asignado',
      level: optionalInt(json['level']),
      currentXp: optionalInt(json['currentXp'] ?? json['xp']),
      xpForNextLevel: optionalInt(
        json['xpForNextLevel'] ?? json['nextLevelXp'],
      ),
      skills: skills,
      missionsCompleted: optionalInt(json['missionsCompleted']),
      incidentsResolved: optionalInt(json['incidentsResolved']),
    );
  }
}

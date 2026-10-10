/// Perfil del jugador usado en las pantallas de inicio, perfil y progreso.
///
/// Es un modelo de datos simulado; en etapas posteriores se cargará desde la
/// API del proyecto.
class PlayerProfile {
  const PlayerProfile({
    required this.name,
    required this.rank,
    required this.level,
    required this.currentXp,
    required this.xpForNextLevel,
    required this.skills,
    required this.missionsCompleted,
    required this.incidentsResolved,
  });

  /// Nombre visible del jugador.
  final String name;

  /// Rango actual (por ejemplo, Técnico Junior).
  final String rank;

  /// Nivel actual del jugador.
  final int level;

  /// Experiencia acumulada en el nivel actual.
  final int currentXp;

  /// Experiencia necesaria para subir al siguiente nivel.
  final int xpForNextLevel;

  /// Habilidades con su nivel de dominio.
  final Map<String, int> skills;

  /// Cantidad de misiones completadas.
  final int missionsCompleted;

  /// Cantidad de incidentes resueltos.
  final int incidentsResolved;

  /// Porcentaje de avance dentro del nivel actual (0.0 a 1.0).
  double get xpProgress {
    if (xpForNextLevel <= 0) return 0;
    return (currentXp / xpForNextLevel).clamp(0.0, 1.0);
  }
}

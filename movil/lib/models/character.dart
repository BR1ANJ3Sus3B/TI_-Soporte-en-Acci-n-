/// Tipo de rol que cumple un personaje dentro de la historia.
enum CharacterRole {
  /// Personaje jugable (Alex).
  player,

  /// Mentor del área de TI (Martín).
  mentor,

  /// Especialista en redes e infraestructura (Richar).
  network,

  /// Especialista en programación (Marco).
  programming,

  /// Personaje de apoyo dentro de la empresa (Sofía).
  support,

  /// Personaje de otras áreas de la organización (Elena).
  organization,

  /// Antagonista que provoca incidentes (Saboteador).
  antagonist,
}

/// Personaje de la historia de "TI: Soporte en Acción".
///
/// Los datos son de ejemplo para esta primera etapa; los personajes todavía no
/// tienen lógica interactiva, solo se muestran en la interfaz.
class GameCharacter {
  const GameCharacter({
    required this.name,
    required this.role,
    required this.description,
    required this.type,
  });

  /// Nombre del personaje.
  final String name;

  /// Rol o puesto dentro de la empresa.
  final String role;

  /// Descripción breve de su función en la historia.
  final String description;

  /// Tipo de rol, usado para el color del avatar.
  final CharacterRole type;

  /// Iniciales del personaje, usadas como avatar tipo pixel art.
  String get initials {
    final parts = name.trim().split(RegExp(r'\s+'));
    final buffer = StringBuffer();
    for (final part in parts) {
      if (part.isNotEmpty) {
        buffer.write(part[0]);
      }
    }
    return buffer.toString().toUpperCase();
  }
}

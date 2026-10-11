import 'package:flutter/material.dart';
import 'package:ti_soporte_accion/models/achievement.dart';
import 'package:ti_soporte_accion/models/mission.dart';
import 'package:ti_soporte_accion/models/player_profile.dart';
import 'package:ti_soporte_accion/models/skill.dart';
import 'package:ti_soporte_accion/models/character.dart';

/// Datos simulados para visualizar todas las pantallas de la aplicación móvil.
///
/// Estos valores se usan solo para prototipo (sketch funcional). No hay
/// conexión con API, base de datos ni autenticación real.
class MockData {
  const MockData._();

  /// Perfil del jugador (Alex).
  static const PlayerProfile player = PlayerProfile(
    name: 'Alex',
    rank: 'Técnico Junior',
    level: 1,
    currentXp: 250,
    xpForNextLevel: 500,
    skills: <String, int>{
      'Soporte Técnico': 4,
      'Redes': 2,
        'Programación': 3,
        'Bases de Datos': 2,
      },
      missionsCompleted: 5,
      incidentsResolved: 8,
    );

  /// Lista de misiones simuladas.
  static const List<Mission> missions = <Mission>[
    Mission(
      id: 'm01',
      title: 'No puedo iniciar sesión',
      description:
          'Un empleado reporta que no puede iniciar sesión en su computadora y necesita ayuda para recuperar su acceso.',
      area: 'Soporte Técnico',
      department: 'Recursos Humanos',
      affectedUser: 'Sofía',
      difficulty: 1,
      estimatedTime: '5 - 10 min',
      xpReward: 100,
      status: MissionStatus.completed,
      objectives: <String>[
        'Recopilar información del incidente',
        'Verificar estado del equipo',
        'Revisar bloqueos comunes (Bloq Mayús)',
        'Restablecer acceso',
        'Cerrar ticket correctamente',
      ],
    ),
    Mission(
      id: 'm02',
      title: 'Sin conexión a Internet',
      description:
          'Un empleado reporta que su computadora no puede conectarse a la red corporativa.',
      area: 'Redes',
      department: 'Recursos Humanos',
      affectedUser: 'Sofía',
      difficulty: 2,
      estimatedTime: '10 - 15 min',
      xpReward: 200,
      status: MissionStatus.inProgress,
      objectives: <String>[
        'Revisar conexión física',
        'Verificar configuración IP',
        'Comprobar acceso al router',
        'Realizar diagnóstico con pruebas de conectividad',
        'Resolver el problema',
      ],
    ),
    Mission(
      id: 'm03',
      title: 'Servidor fuera de línea',
      description:
          'Se detecta una caída de servicio que afecta a varios equipos de la empresa.',
      area: 'Infraestructura',
      department: 'Sala de Servidores',
      affectedUser: 'Martín',
      difficulty: 3,
      estimatedTime: '15 - 20 min',
      xpReward: 400,
      status: MissionStatus.available,
      objectives: <String>[
        'Verificar estado del servidor',
        'Revisar registros y alertas',
        'Identificar causa de la caída',
        'Aplicar solución segura',
        'Documentar el incidente',
      ],
    ),
  ];

  /// Lista de habilidades para la pantalla de progreso.
  static final List<Skill> skills = <Skill>[
    Skill(
      name: 'Soporte Técnico',
      level: player.skills['Soporte Técnico'] ?? 0,
      icon: Icons.build_rounded,
      color: const Color(0xFF22C55E),
    ),
    Skill(
      name: 'Redes',
      level: player.skills['Redes'] ?? 0,
      icon: Icons.wifi_tethering_rounded,
      color: const Color(0xFF06B6D4),
    ),
    Skill(
      name: 'Programación',
      level: player.skills['Programación'] ?? 0,
      icon: Icons.code_rounded,
      color: const Color(0xFF2563EB),
    ),
    Skill(
      name: 'Bases de Datos',
      level: player.skills['Bases de Datos'] ?? 0,
      icon: Icons.storage_rounded,
      color: const Color(0xFF7C3AED),
    ),
  ];

  /// Logros desbloqueados y bloqueados.
  static const List<Achievement> achievements = <Achievement>[
    Achievement(
      title: 'Primer ticket resuelto',
      description: 'Resuelve tu primer incidente de soporte técnico.',
      icon: Icons.verified_rounded,
      unlocked: true,
    ),
    Achievement(
      title: 'Técnico en crecimiento',
      description: 'Alcanza 250 XP en tu camino como técnico.',
      icon: Icons.trending_up_rounded,
      unlocked: true,
    ),
    Achievement(
      title: 'Experto en redes',
      description: 'Sube tu habilidad de Redes a nivel 5.',
      icon: Icons.wifi_rounded,
      unlocked: false,
    ),
    Achievement(
      title: 'Cazador de errores',
      description: 'Resuelve 10 incidentes de programación.',
      icon: Icons.bug_report_rounded,
      unlocked: false,
    ),
    Achievement(
      title: 'Guardián de la infraestructura',
      description: 'Completa una misión crítica de Sala de Servidores.',
      icon: Icons.security_rounded,
      unlocked: false,
    ),
  ];

  /// Áreas de la empresa para el mapa.
  static const List<MapArea> areas = <MapArea>[
    MapArea(
      name: 'Soporte Técnico',
      icon: Icons.build_rounded,
      color: Color(0xFF22C55E),
    ),
    MapArea(
      name: 'Recursos Humanos',
      icon: Icons.people_rounded,
      color: Color(0xFF06B6D4),
    ),
    MapArea(
      name: 'Desarrollo',
      icon: Icons.code_rounded,
      color: Color(0xFF2563EB),
    ),
    MapArea(
      name: 'Sala de Servidores',
      icon: Icons.dns_rounded,
      color: Color(0xFF7C3AED),
    ),
    MapArea(
      name: 'Administración',
      icon: Icons.apartment_rounded,
      color: Color(0xFFF59E0B),
    ),
    MapArea(
      name: 'Dirección',
      icon: Icons.account_balance_rounded,
      color: Color(0xFFEF4444),
    ),
    MapArea(
      name: 'Almacén',
      icon: Icons.inventory_2_rounded,
      color: Color(0xFF9CA3AF),
    ),
  ];

  /// Personajes importantes de la historia.
  static const List<GameCharacter> characters = <GameCharacter>[
    GameCharacter(
      name: 'Alex',
      role: 'Técnico Junior de TI',
      description: 'Personaje principal. Recién ingresa a la empresa.',
      type: CharacterRole.player,
    ),
    GameCharacter(
      name: 'Martín',
      role: 'Líder del área de TI',
      description: 'Mentor de Alex y responsable del equipo.',
      type: CharacterRole.mentor,
    ),
    GameCharacter(
      name: 'Richar',
      role: 'Especialista en Redes e Infraestructura',
      description: 'Directo y con gran experiencia en redes.',
      type: CharacterRole.network,
    ),
    GameCharacter(
      name: 'Marco',
      role: 'Especialista en Programación',
      description: 'Colabora en misiones relacionadas con código.',
      type: CharacterRole.programming,
    ),
    GameCharacter(
      name: 'Sofía',
      role: 'Soporte y Apoyo',
      description: 'Personaje de apoyo dentro de la empresa.',
      type: CharacterRole.support,
    ),
    GameCharacter(
      name: 'Elena',
      role: 'Colaboradora de la Organización',
      description: 'Actúa como contacto entre áreas.',
      type: CharacterRole.organization,
    ),
    GameCharacter(
      name: 'Saboteador',
      role: 'Agente Interno',
      description:
          'Provoca incidentes intencionalmente para dificultar el trabajo.',
      type: CharacterRole.antagonist,
    ),
  ];
}

/// Área de la empresa representada en el mapa.
class MapArea {
  const MapArea({
    required this.name,
    required this.icon,
    required this.color,
  });

  /// Nombre del área.
  final String name;

  /// Icono representativo.
  final IconData icon;

  /// Color para resaltar el área.
  final Color color;
}

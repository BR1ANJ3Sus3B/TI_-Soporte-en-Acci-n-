import 'package:flutter/material.dart';
import 'package:ti_soporte_accion/models/achievement.dart';
import 'package:ti_soporte_accion/models/character.dart';
import 'package:ti_soporte_accion/models/mission.dart';
import 'package:ti_soporte_accion/models/player_profile.dart';
import 'package:ti_soporte_accion/models/map_area.dart';
import 'package:ti_soporte_accion/models/skill.dart';

/// Datos simulados para visualizar todas las pantallas de la aplicaciÃ³n mÃ³vil.
class MockData {
  const MockData._();

  /// Perfil del jugador (Alex).
  static const PlayerProfile player = PlayerProfile(
    name: 'Alex',
    rank: 'TÃ©cnico Junior',
    level: 1,
    currentXp: 250,
    xpForNextLevel: 500,
    skills: <String, int>{
      'Soporte TÃ©cnico': 4,
      'Redes': 2,
      'ProgramaciÃ³n': 3,
      'Bases de Datos': 2,
    },
    missionsCompleted: 5,
    incidentsResolved: 8,
  );

  /// Logros desbloqueados y bloqueados.
  static const List<Achievement> achievements = <Achievement>[
    Achievement(
      id: 'a01',
      title: 'Primer Ticket',
      description: 'Has resuelto tu primer incidente de soporte tÃ©cnico.',
      icon: Icons.verified_rounded,
      unlocked: true,
      status: AchievementStatus.unlocked,
      xp: 100,
      category: 'Soporte TÃ©cnico',
      rarity: AchievementRarity.common,
    ),
    Achievement(
      id: 'a02',
      title: 'TÃ©cnico en Crecimiento',
      description: 'Alcanza 250 XP en tu camino como tÃ©cnico.',
      icon: Icons.trending_up_rounded,
      unlocked: true,
      status: AchievementStatus.unlocked,
      xp: 100,
      category: 'Progreso',
      rarity: AchievementRarity.common,
    ),
    Achievement(
      id: 'a03',
      title: 'Especialista en Redes I',
      description: 'Alcanza nivel 5 en Redes.',
      icon: Icons.wifi_rounded,
      unlocked: false,
      status: AchievementStatus.locked,
      xp: 150,
      category: 'Redes',
      rarity: AchievementRarity.rare,
    ),
    Achievement(
      id: 'a04',
      title: 'Cazador de Bugs I',
      description: 'Resuelve 10 errores de programaciÃ³n.',
      icon: Icons.bug_report_rounded,
      unlocked: false,
      status: AchievementStatus.locked,
      xp: 200,
      category: 'ProgramaciÃ³n',
      rarity: AchievementRarity.epic,
    ),
    Achievement(
      id: 'a05',
      title: 'GuardiÃ¡n del Servidor',
      description: 'Completa una misiÃ³n crÃ­tica de Sala de Servidores.',
      icon: Icons.security_rounded,
      unlocked: false,
      status: AchievementStatus.locked,
      xp: 250,
      category: 'Infraestructura',
      rarity: AchievementRarity.legendary,
    ),
    Achievement(
      id: 'a06',
      title: 'Respuesta RÃ¡pida',
      description: 'Resuelve 5 tickets en poco tiempo.',
      icon: Icons.flash_on_rounded,
      unlocked: false,
      status: AchievementStatus.locked,
      xp: 120,
      category: 'Soporte TÃ©cnico',
      rarity: AchievementRarity.uncommon,
    ),
  ];

  /// Lista de misiones simuladas.

  static final List<Skill> skills = <Skill>[
    Skill(
      name: 'Soporte Técnico',
      level: 4,
      icon: Icons.build_rounded,
      color: Color(0xFF22C55E),
    ),
    Skill(
      name: 'Redes',
      level: 2,
      icon: Icons.wifi_tethering_rounded,
      color: Color(0xFF06B6D4),
    ),
    Skill(
      name: 'Programación',
      level: 3,
      icon: Icons.code_rounded,
      color: Color(0xFF2563EB),
    ),
    Skill(
      name: 'Bases de Datos',
      level: 2,
      icon: Icons.storage_rounded,
      color: Color(0xFF7C3AED),
    ),
  ];

  static const List<Mission> missions = <Mission>[
    Mission(
      id: 'm01',
      title: 'No puedo iniciar sesión',
      description:
          'Un empleado reporta que no puede iniciar sesión en su computadora.',
      area: 'Soporte Técnico',
      department: 'Recursos Humanos',
      affectedUser: 'Sofía',
      difficulty: 1,
      estimatedTime: '5 - 10 min',
      xpReward: 100,
      status: MissionStatus.completed,
      objectives: <String>[
        'Recopilar información',
        'Verificar equipo',
        'Resolver',
        'Cerrar ticket',
      ],
    ),
    Mission(
      id: 'm02',
      title: 'Sin conexión a Internet',
      description: 'La computadora no puede conectarse a la red corporativa.',
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
        'Comprobar router',
        'Resolver',
      ],
    ),
    Mission(
      id: 'm03',
      title: 'Servidor fuera de línea',
      description: 'Se detecta caída de servicio.',
      area: 'Infraestructura',
      department: 'Sala de Servidores',
      affectedUser: 'Martín',
      difficulty: 3,
      estimatedTime: '15 - 20 min',
      xpReward: 400,
      status: MissionStatus.available,
      objectives: <String>[
        'Verificar servidor',
        'Identificar causa',
        'Aplicar solución',
      ],
    ),
  ];

  /// Áreas de la empresa.
  static final List<MapArea> areas = <MapArea>[
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

  /// Personajes importantes.
  static const List<GameCharacter> characters = <GameCharacter>[
    GameCharacter(
      name: 'Alex',
      role: 'Técnico Junior de TI',
      description: 'Personaje principal.',
      type: CharacterRole.player,
    ),
    GameCharacter(
      name: 'Martín',
      role: 'Líder del área de TI',
      description: 'Mentor de Alex.',
      type: CharacterRole.mentor,
    ),
    GameCharacter(
      name: 'Richar',
      role: 'Especialista en Redes e Infraestructura',
      description: 'Experto en redes.',
      type: CharacterRole.network,
    ),
    GameCharacter(
      name: 'Marco',
      role: 'Especialista en Programación',
      description: 'Colabora en misiones.',
      type: CharacterRole.programming,
    ),
    GameCharacter(
      name: 'Sofía',
      role: 'Soporte y Apoyo',
      description: 'Personaje de apoyo.',
      type: CharacterRole.support,
    ),
    GameCharacter(
      name: 'Saboteador',
      role: 'Agente Interno',
      description: 'Provoca incidentes intencionalmente.',
      type: CharacterRole.antagonist,
    ),
  ];
}

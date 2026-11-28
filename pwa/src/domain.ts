export type MissionStatus = 'Disponible' | 'Bloqueada' | 'Completada' | 'En progreso';
export type Mission = { id: string; title: string; category: string; description: string; objectives: string[]; requirements: string[]; difficulty: 'Básica' | 'Media' | 'Avanzada'; duration: string; reward?: string; status: MissionStatus; level: number; content: string };
export type Level = { id: string; number: number; title: string; description: string; missionIds: string[]; status: 'desbloqueado' | 'bloqueado' };
export type Achievement = { id: string; title: string; description: string; requirement: string; reward?: string; unlocked: boolean; obtainedAt?: string };
export type AuthUser = { id: string; name: string; email?: string; roles: string[] };
export const localMissions: Mission[] = [
 { id:'diagnostico-red', title:'Diagnóstico de red', category:'Redes', description:'Identifica el origen de una incidencia de conectividad.', objectives:['Reconocer capas de red', 'Documentar evidencia'], requirements:['Guía: fundamentos de red'], difficulty:'Básica', duration:'15 min', reward:'Práctica educativa', status:'Disponible', level:1, content:'Revisa síntomas, alcance e historial antes de cambiar una configuración.' },
 { id:'phishing', title:'Alerta de phishing', category:'Ciberseguridad', description:'Analiza señales de un correo sospechoso.', objectives:['Detectar indicadores', 'Escalar el incidente'], requirements:['Misión Diagnóstico de red'], difficulty:'Media', duration:'20 min', status:'Bloqueada', level:2, content:'No abras enlaces ni adjuntos; conserva la evidencia y utiliza el canal de reporte.' },
 { id:'respaldo', title:'Recuperación segura', category:'Operación', description:'Planea una restauración sin comprometer información.', objectives:['Validar respaldo', 'Comunicar impacto'], requirements:['Nivel 2'], difficulty:'Avanzada', duration:'25 min', status:'Bloqueada', level:3, content:'Una restauración debe validarse en un entorno autorizado y seguir el procedimiento vigente.' }
];
export const localLevels: Level[] = [
 {id:'nivel-1',number:1,title:'El primer día',description:'Fundamentos para atender incidentes.',missionIds:['diagnostico-red'],status:'desbloqueado'},
 {id:'nivel-2',number:2,title:'Defensa activa',description:'Clasificación y escalamiento seguro.',missionIds:['phishing'],status:'bloqueado'},
 {id:'nivel-3',number:3,title:'Continuidad',description:'Recuperación y comunicación.',missionIds:['respaldo'],status:'bloqueado'}
];
export const localAchievements: Achievement[] = [
 {id:'primer-paso',title:'Primer paso',description:'Completa tu primera misión verificada.',requirement:'Sincronización con API',unlocked:false},
 {id:'observador',title:'Observador atento',description:'Identifica un indicador de phishing.',requirement:'Completar Alerta de phishing',unlocked:false},
 {id:'continuidad',title:'Guardianía',description:'Valida una recuperación autorizada.',requirement:'Completar Recuperación segura',unlocked:false}
];

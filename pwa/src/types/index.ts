export type Difficulty = 'Básica' | 'Media' | 'Avanzada';
export type Mission = { id: string; title: string; category: string; description: string; difficulty: Difficulty; duration: string; xp: number; status: 'Disponible' | 'En progreso'; };
export type Skill = { name: string; progress: number; icon: 'headset' | 'network' | 'shield' | 'code' | 'database' };
export type Achievement = { id: string; title: string; description: string; xp: number; unlocked: boolean; date?: string; icon: 'award' | 'shield' | 'rocket' };
export type Character = { name: string; role: string; initials: string; color: string };
export type Resource = { id: string; title: string; category: string; description: string; type: string; };
export type Profile = { name: string; role: string; level: number; xp: number; nextLevelXp: number; };

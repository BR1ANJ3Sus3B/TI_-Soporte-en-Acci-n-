import { skills } from '../data/demoData'; import { apiClient, isDemoMode } from './apiClient'; import type { Skill } from '../types';
export const getSkills = () => isDemoMode ? Promise.resolve(skills) : apiClient<Skill[]>('/progress/specialties');

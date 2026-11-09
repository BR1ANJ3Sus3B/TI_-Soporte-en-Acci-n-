import { achievements } from '../data/demoData'; import { apiClient, isDemoMode } from './apiClient'; import type { Achievement } from '../types';
export const getAchievements = () => isDemoMode ? Promise.resolve(achievements) : apiClient<Achievement[]>('/achievements');

import { missions } from '../data/demoData'; import { isDemoMode, apiClient } from './apiClient'; import type { Mission } from '../types';
export const getMissions = () => isDemoMode ? Promise.resolve(missions) : apiClient<Mission[]>('/missions');
export const getMission = (id: string) => isDemoMode ? Promise.resolve(missions.find((m) => m.id === id)) : apiClient<Mission>(`/missions/${id}`);

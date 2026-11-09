import { profile } from '../data/demoData'; import { apiClient, isDemoMode } from './apiClient'; import type { Profile } from '../types';
export const getProfile = () => isDemoMode ? Promise.resolve(profile) : apiClient<Profile>('/profile');

import { getAccessToken } from './authService';
const baseUrl = import.meta.env.VITE_API_BASE_URL ?? 'http://localhost:3000/api/v1';
export const isDemoMode = import.meta.env.VITE_DEMO_MODE !== 'false';
export async function apiClient<T>(path: string): Promise<T> {
  const token = getAccessToken(); const headers: HeadersInit = { Accept: 'application/json' };
  if (token) headers.Authorization = `Bearer ${token}`;
  const response = await fetch(`${baseUrl}${path}`, { headers });
  if (!response.ok) throw new Error(`Error de API: ${response.status}`);
  return response.json() as Promise<T>;
}

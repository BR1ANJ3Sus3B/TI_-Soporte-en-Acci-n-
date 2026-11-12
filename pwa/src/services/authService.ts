// La autenticación Keycloak pertenece a la aplicación Flutter.
// La PWA no crea sesiones ni concede roles hasta implementar su backend de acceso.
export type AppRole = 'student' | 'teacher' | 'admin';
export type AuthUser = { id: string; username: string; name: string; email?: string; roles: string[]; role: AppRole | null };
export type AuthStatus = 'loading' | 'authenticated' | 'anonymous' | 'unavailable';
export const isAuthConfigured = false;
export function getAccessToken(): null { return null; }
export async function login(): Promise<void> {
  throw new Error('El acceso web está pendiente. Inicia sesión en la aplicación Flutter.');
}
export async function logout(): Promise<void> { /* No existe sesión web. */ }
export const beginLogout = logout;

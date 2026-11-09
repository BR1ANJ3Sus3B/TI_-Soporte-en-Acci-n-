import { createContext, useContext, useEffect, useMemo, useState, type ReactNode } from 'react';
import { initializeAuth, isAuthConfigured, login, logout, refreshSession, type AuthStatus, type AuthUser } from '../services/authService';

type AuthValue = { status: AuthStatus; isAuthenticated: boolean; isLoading: boolean; user: AuthUser | null; error: string | null; login: (returnTo?: string) => Promise<void>; logout: () => Promise<void>; hasRole: (role: string) => boolean; refresh: () => Promise<void>; retry: () => Promise<void> };
const AuthContext = createContext<AuthValue | null>(null);
export function AuthProvider({ children }: { children: ReactNode }) {
  const [status, setStatus] = useState<AuthStatus>(isAuthConfigured ? 'loading' : 'unavailable'); const [user, setUser] = useState<AuthUser | null>(null); const [error, setError] = useState<string | null>(null);
  async function load() { if (!isAuthConfigured) return; setStatus('loading'); setError(null); try { const result = await initializeAuth(); setUser(result.user); setStatus(result.user ? 'authenticated' : 'anonymous'); if (result.returnTo && result.returnTo !== window.location.pathname) { window.history.replaceState({}, document.title, result.returnTo); window.dispatchEvent(new PopStateEvent('popstate')); } } catch (reason) { setUser(null); setStatus('anonymous'); setError(reason instanceof Error ? reason.message : 'No fue posible conectar con Keycloak.'); } }
  async function refresh() { const nextUser = await refreshSession(); setUser(nextUser); setStatus(nextUser ? 'authenticated' : 'anonymous'); }
  useEffect(() => { const timer = window.setTimeout(() => { void load(); }, 0); return () => window.clearTimeout(timer); }, []);
  useEffect(() => { if (status !== 'authenticated') return; const timer = window.setInterval(() => { void refresh(); }, 30_000); return () => window.clearInterval(timer); }, [status]);
  const value = useMemo(() => ({ status, isAuthenticated: status === 'authenticated', isLoading: status === 'loading', user, error, login, logout, refresh, retry: load, hasRole: (role: string) => Boolean(user?.roles.includes(role) || (role === 'student' && user?.role === 'student') || (role === 'teacher' && user?.role === 'teacher') || (role === 'admin' && user?.role === 'admin')) }), [status, user, error]);
  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
}
export function useAuth(): AuthValue { const value = useContext(AuthContext); if (!value) throw new Error('AuthProvider ausente'); return value; }

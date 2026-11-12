import { createContext, useContext, type ReactNode } from 'react';
import { login, logout, type AuthStatus, type AuthUser } from '../services/authService';

type AuthValue = {
  status: AuthStatus;
  isAuthenticated: boolean;
  isLoading: boolean;
  user: AuthUser | null;
  error: string | null;
  login: () => Promise<void>;
  logout: () => Promise<void>;
  hasRole: (role: string) => boolean;
};
const unavailable: AuthValue = {
  status: 'unavailable', isAuthenticated: false, isLoading: false,
  user: null, error: null, login, logout, hasRole: () => false,
};
const AuthContext = createContext<AuthValue | null>(null);
export function AuthProvider({ children }: { children: ReactNode }) {
  return <AuthContext.Provider value={unavailable}>{children}</AuthContext.Provider>;
}
export function useAuth(): AuthValue {
  const value = useContext(AuthContext);
  if (!value) throw new Error('AuthProvider ausente');
  return value;
}

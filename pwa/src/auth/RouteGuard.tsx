import type { ReactNode } from 'react';
import { Navigate, useLocation } from 'react-router-dom';
import { LoadingState } from '../components/ui';
import type { AppRole } from '../services/authService';
import { useAuth } from './AuthContext';

export function RouteGuard({ children, roles }: { children: ReactNode; roles?: AppRole[] }) {
  const auth = useAuth(); const location = useLocation();
  if (auth.status === 'loading') return <LoadingState label="Comprobando sesión segura…" />;
  if (auth.status !== 'authenticated') return <Navigate to="/login" replace state={{ returnTo: `${location.pathname}${location.search}${location.hash}` }} />;
  if (roles && (!auth.user || !roles.some((role) => auth.hasRole(role)))) return <Navigate to="/acceso-denegado" replace />;
  return <>{children}</>;
}

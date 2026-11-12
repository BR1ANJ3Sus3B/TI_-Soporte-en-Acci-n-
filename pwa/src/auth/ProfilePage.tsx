import { useAuth } from './AuthContext';
import { AlertMessage, Card, StatusBadge } from '../components/ui';

export function AuthProfilePage() {
  const { user } = useAuth();
  return <Card><h1>Perfil</h1><StatusBadge label="Integración web pendiente" tone="pending" />
    {user && <><h2>{user.name}</h2><p>{user.email}</p></>}
    <AlertMessage title="Sin datos de servidor" tone="info">
      El perfil web y el progreso estarán disponibles al integrar la API educativa.
    </AlertMessage>
  </Card>;
}

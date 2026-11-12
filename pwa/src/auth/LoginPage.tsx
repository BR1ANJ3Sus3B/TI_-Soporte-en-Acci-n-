import type { ReactNode } from 'react';
import { Link } from 'react-router-dom';
import { BrandLogo } from '../components/brand';
import { AlertMessage } from '../components/ui';

function AuthShell({ children }: { children: ReactNode }) {
  return <main className="auth-split"><section className="auth-story">
    <BrandLogo /><div className="auth-pixel pixel-art">TI</div>
    <h1>Bienvenido a NovaTech Solutions</h1>
    <p>Aprende, resuelve incidentes y desarrolla tus habilidades tecnológicas.</p>
  </section><section className="auth-panel">{children}</section></main>;
}
export function LoginPage() {
  return <AuthShell><h1>Acceso a la plataforma</h1>
    <AlertMessage title="Inicio de sesión en la aplicación móvil" tone="info">
      Keycloak se utiliza desde la aplicación Flutter. La PWA no solicita credenciales
      ni abre una sesión con Keycloak. El acceso web autenticado está pendiente.
    </AlertMessage>
    <Link className="button" to="/demo">Explorar demostración pública</Link>
    <Link to="/recursos">Consultar recursos educativos</Link>
  </AuthShell>;
}
export function RegistrationPage() {
  return <AuthShell><h1>Registro</h1><p>Gestiona tu cuenta desde el acceso móvil de
    NovaTech. Si el servidor no permite el registro, solicita una cuenta al administrador.</p>
    <Link to="/login">Volver</Link></AuthShell>;
}
export function PasswordRecoveryPage() {
  return <AuthShell><h1>Recuperación de cuenta</h1><p>Abre el inicio de sesión de la
    aplicación Flutter y utiliza la recuperación de contraseña del proveedor, si está habilitada.</p>
    <Link to="/login">Volver</Link></AuthShell>;
}
export function AccessDeniedPage() {
  return <AuthShell><h1>Acceso restringido</h1><p>Esta sección requiere una sesión
    web y permisos válidos. El acceso web está pendiente de integración.</p>
    <Link to="/demo">Ver demostración pública</Link></AuthShell>;
}

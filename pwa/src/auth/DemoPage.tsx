import { Link } from 'react-router-dom';
import { BrandLogo } from '../components/brand';
export function DemoPage() { return <main className="auth-page"><section className="auth-card"><BrandLogo /><span className="eyebrow">Vista pública</span><h1>Demostración de contenido</h1><p>Las misiones, XP y logros de ejemplo se conservan para desarrollo, pero no representan una sesión de Alex ni conceden acceso a rutas privadas.</p><Link className="button" to="/login">Ir a iniciar sesión</Link></section></main>; }

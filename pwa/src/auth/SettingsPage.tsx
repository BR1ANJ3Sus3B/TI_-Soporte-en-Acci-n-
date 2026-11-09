import { useState } from 'react';
import { Card, StatusBadge } from '../components/ui';
import { readPreference, writePreference } from '../services/localStorageService';

export function SettingsPage() { const [reducedMotion, setReducedMotion] = useState(() => readPreference('reduced-motion', false, (value): value is boolean => typeof value === 'boolean')); function update(value: boolean) { setReducedMotion(value); writePreference('reduced-motion', value); } return <><div className="page-title"><div><span className="eyebrow">Preferencias</span><h1>Configuración</h1></div><StatusBadge label="Solo preferencias locales" tone="info" /></div><Card><h2>Accesibilidad</h2><label className="setting-option"><input type="checkbox" checked={reducedMotion} onChange={(event) => update(event.target.checked)} /> Reducir animaciones</label><p className="profile-copy">Esta preferencia no contiene información de sesión ni permisos.</p></Card></>; }

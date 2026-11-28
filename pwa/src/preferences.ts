export type Preferences = { version: 1; language: 'es'; reducedMotion: boolean; onboardingCompleted: boolean };
const key = 'ti-soporte-preferences';
export const defaults: Preferences = { version: 1, language: 'es', reducedMotion: false, onboardingCompleted: false };
export function readPreferences(): Preferences { try { const parsed = JSON.parse(localStorage.getItem(key) ?? '') as Partial<Preferences>; return parsed.version === 1 && parsed.language === 'es' ? { ...defaults, ...parsed } : defaults; } catch { return defaults; } }
export function savePreferences(value: Preferences): boolean { try { localStorage.setItem(key, JSON.stringify(value)); return true; } catch { return false; } }
export function updatePreferences(change: Partial<Preferences>) { const next = { ...readPreferences(), ...change, version: 1 as const }; savePreferences(next); return next; }
export function clearPreferences() { try { localStorage.removeItem(key); return true; } catch { return false; } }

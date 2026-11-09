const PREFIX = 'ti-soporte:';
type Envelope<T> = { version: number; value: T };

function storageAvailable(): boolean {
  return typeof window !== 'undefined' && (() => { try { const key = `${PREFIX}probe`; window.localStorage.setItem(key, '1'); window.localStorage.removeItem(key); return true; } catch { return false; } })();
}

export function readPreference<T>(key: string, fallback: T, validate: (value: unknown) => value is T, version = 1): T {
  if (!storageAvailable()) return fallback;
  try {
    const parsed: unknown = JSON.parse(window.localStorage.getItem(`${PREFIX}${key}`) ?? 'null');
    if (typeof parsed !== 'object' || parsed === null) return fallback;
    const item = parsed as Envelope<unknown>;
    return item.version === version && validate(item.value) ? item.value : fallback;
  } catch { return fallback; }
}

export function writePreference<T>(key: string, value: T, version = 1): boolean {
  if (!storageAvailable()) return false;
  try { window.localStorage.setItem(`${PREFIX}${key}`, JSON.stringify({ version, value } satisfies Envelope<T>)); return true; } catch { return false; }
}

export function removePreference(key: string): void { if (storageAvailable()) try { window.localStorage.removeItem(`${PREFIX}${key}`); } catch { /* storage disabled */ } }

export const preferences = {
  missionFilters: {
    read: () => readPreference('mission-filters', { category: 'Todas', difficulty: 'Todas' }, (value): value is { category: string; difficulty: string } => typeof value === 'object' && value !== null && typeof (value as Record<string, unknown>).category === 'string' && typeof (value as Record<string, unknown>).difficulty === 'string'),
    write: (value: { category: string; difficulty: string }) => writePreference('mission-filters', value),
  },
};

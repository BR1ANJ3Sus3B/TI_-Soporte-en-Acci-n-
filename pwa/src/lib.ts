export const endpoint = (name: string) => import.meta.env[`VITE_API_${name}_PATH`] as string | undefined;
export const apiBase = (import.meta.env.VITE_API_BASE_URL as string | undefined)?.replace(/\/$/, '');
export class ApiError extends Error { constructor(public status: number, message: string) { super(message); } }
export async function api<T>(path: string, token?: string): Promise<T> {
  if (!apiBase || !path) throw new ApiError(0, 'La integración API no está configurada.');
  const abort = new AbortController(); const timer = window.setTimeout(() => abort.abort(), 10000);
  try { const response = await fetch(`${apiBase}${path}`, { headers: { Accept:'application/json', ...(token ? { Authorization:`Bearer ${token}` } : {}) }, signal: abort.signal });
    if (!response.ok) throw new ApiError(response.status, response.status === 401 ? 'Tu sesión expiró.' : response.status === 403 ? 'No tienes permiso para esta operación.' : `La API respondió ${response.status}.`);
    return response.json() as Promise<T>;
  } finally { clearTimeout(timer); }
}
export const guestKey = 'ti-soporte-guest-progress';
export type GuestProgress = { viewed: string[]; updatedAt: string };
export function readGuest(): GuestProgress { try { return JSON.parse(localStorage.getItem(guestKey) || '{"viewed":[],"updatedAt":""}') as GuestProgress; } catch { return {viewed:[],updatedAt:''}; } }
export function saveGuest(progress: GuestProgress) { localStorage.setItem(guestKey, JSON.stringify(progress)); }
function guestDb(): Promise<IDBDatabase> { return new Promise((resolve,reject)=>{ const request=indexedDB.open('ti-soporte-local',1); request.onupgradeneeded=()=>request.result.createObjectStore('guest'); request.onsuccess=()=>resolve(request.result); request.onerror=()=>reject(request.error); }); }
export async function persistGuest(progress: GuestProgress) { try { const db=await guestDb(); await new Promise<void>((resolve,reject)=>{const tx=db.transaction('guest','readwrite');tx.objectStore('guest').put(progress,'progress');tx.oncomplete=()=>resolve();tx.onerror=()=>reject(tx.error);});db.close(); } catch { /* localStorage remains the resilient fallback */ } }
export async function restoreGuest() { try { const db=await guestDb(); const value=await new Promise<GuestProgress|undefined>((resolve,reject)=>{const request=db.transaction('guest','readonly').objectStore('guest').get('progress');request.onsuccess=()=>resolve(request.result as GuestProgress|undefined);request.onerror=()=>reject(request.error);});db.close();if(value) saveGuest(value);return value; } catch { return undefined; } }
export function markViewed(id: string) { const old = readGuest(); const next={ viewed:[...new Set([...old.viewed,id])], updatedAt:new Date().toISOString() }; saveGuest(next); void persistGuest(next); }
export function clearGuest() { localStorage.removeItem(guestKey); void (async()=>{try{const db=await guestDb();db.close();indexedDB.deleteDatabase('ti-soporte-local');}catch{/* no indexedDB */}})(); }

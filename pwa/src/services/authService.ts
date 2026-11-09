export type AppRole = 'student' | 'teacher' | 'admin';
export type AuthUser = { id: string; username: string; name: string; firstName?: string; lastName?: string; email?: string; roles: string[]; role: AppRole | null };
export type SessionUser = AuthUser;
export type AuthStatus = 'loading' | 'authenticated' | 'anonymous' | 'unavailable';
type OidcTransaction = { state: string; verifier: string; returnTo: string };
type TokenSet = { accessToken: string; refreshToken?: string; idToken?: string; expiresAt: number };
type TokenResponse = { access_token: string; refresh_token?: string; id_token?: string; expires_in: number };
type Claims = Record<string, unknown> & { sub?: string; preferred_username?: string; name?: string; given_name?: string; family_name?: string; email?: string; realm_access?: { roles?: string[] }; resource_access?: Record<string, { roles?: string[] }> };

const keycloakUrl = import.meta.env.VITE_KEYCLOAK_URL?.replace(/\/$/, '');
const realm = import.meta.env.VITE_KEYCLOAK_REALM;
const clientId = import.meta.env.VITE_KEYCLOAK_CLIENT_ID;
const transactionKey = 'ti-soporte:oidc-transaction';
let tokens: TokenSet | null = null;
export const isAuthConfigured = Boolean(keycloakUrl && realm && clientId);
export const isRegistrationEnabled = import.meta.env.VITE_KEYCLOAK_SELF_REGISTRATION === 'true';
export type SocialProvider = 'google' | 'facebook' | 'instagram';
export const enabledSocialProviders: SocialProvider[] = [];

function requireConfig(): void { if (!isAuthConfigured) throw new Error('Falta la configuración pública de Keycloak.'); }
function realmBase(): string { requireConfig(); return `${keycloakUrl}/realms/${encodeURIComponent(realm!)}/protocol/openid-connect`; }
function redirectUri(): string { return `${window.location.origin}${window.location.pathname}`; }
function base64Url(bytes: Uint8Array): string { let result = ''; bytes.forEach((byte) => { result += String.fromCharCode(byte); }); return btoa(result).replace(/\+/g, '-').replace(/\//g, '_').replace(/=+$/, ''); }
function randomValue(): string { const bytes = new Uint8Array(32); crypto.getRandomValues(bytes); return base64Url(bytes); }
async function challenge(verifier: string): Promise<string> { return base64Url(new Uint8Array(await crypto.subtle.digest('SHA-256', new TextEncoder().encode(verifier)))); }
function decodeJwt(token: string): Claims { const encoded = token.split('.')[1]; if (!encoded) throw new Error('Token de identidad inválido.'); return JSON.parse(atob(encoded.replace(/-/g, '+').replace(/_/g, '/'))) as Claims; }
function readTransaction(): OidcTransaction | null { try { const raw = window.sessionStorage.getItem(transactionKey); return raw ? JSON.parse(raw) as OidcTransaction : null; } catch { return null; } }
function clearTransaction(): void { try { window.sessionStorage.removeItem(transactionKey); } catch { /* storage no disponible */ } }
function normalizedRole(roles: string[]): AppRole | null { if (roles.includes('administrador')) return 'admin'; if (roles.includes('docente')) return 'teacher'; if (roles.includes('estudiante')) return 'student'; return null; }
function userFromTokens(nextTokens: TokenSet): AuthUser { const claims = decodeJwt(nextTokens.idToken ?? nextTokens.accessToken); const roles = [...new Set([...(claims.realm_access?.roles ?? []), ...(claims.resource_access?.[clientId!]?.roles ?? [])])]; const username = claims.preferred_username ?? claims.email ?? 'usuario'; return { id: claims.sub ?? username, username, name: (claims.name ?? [claims.given_name, claims.family_name].filter(Boolean).join(' ')) || username, firstName: claims.given_name, lastName: claims.family_name, email: claims.email, roles, role: normalizedRole(roles) }; }
async function requestTokens(params: URLSearchParams): Promise<TokenSet> { const response = await fetch(`${realmBase()}/token`, { method: 'POST', headers: { 'Content-Type': 'application/x-www-form-urlencoded', Accept: 'application/json' }, body: params.toString() }); if (!response.ok) throw new Error('Keycloak no pudo completar la autenticación.'); const payload = await response.json() as TokenResponse; return { accessToken: payload.access_token, refreshToken: payload.refresh_token, idToken: payload.id_token, expiresAt: Date.now() + payload.expires_in * 1000 }; }
async function authorizationUrl(returnTo: string, extra: Record<string, string> = {}): Promise<string> { requireConfig(); const verifier = randomValue(); const state = randomValue(); try { window.sessionStorage.setItem(transactionKey, JSON.stringify({ verifier, state, returnTo } satisfies OidcTransaction)); } catch { throw new Error('El navegador bloqueó el almacenamiento temporal necesario para el inicio de sesión.'); } const params = new URLSearchParams({ client_id: clientId!, redirect_uri: redirectUri(), response_type: 'code', scope: 'openid profile email', state, code_challenge: await challenge(verifier), code_challenge_method: 'S256', ...extra }); return `${realmBase()}/auth?${params}`; }

export async function login(returnTo = '/'): Promise<void> { if (typeof window === 'undefined') return; window.location.assign(await authorizationUrl(returnTo)); }
export async function beginLogin(returnTo = '/'): Promise<void> { return login(returnTo); }
export async function beginRegistration(): Promise<void> { if (isRegistrationEnabled) window.location.assign(await authorizationUrl('/', { kc_action: 'REGISTER' })); }
export async function beginPasswordReset(): Promise<void> { window.location.assign(await authorizationUrl('/login', { kc_action: 'UPDATE_PASSWORD' })); }
export async function beginSocialLogin(_provider: SocialProvider, returnTo = '/'): Promise<void> { return login(returnTo); }
export async function initializeAuth(): Promise<{ user: AuthUser | null; returnTo?: string }> { if (!isAuthConfigured || typeof window === 'undefined') return { user: null }; const url = new URL(window.location.href); const code = url.searchParams.get('code'); const state = url.searchParams.get('state'); const authError = url.searchParams.get('error'); if (code || authError) { const transaction = readTransaction(); clearTransaction(); ['code', 'state', 'session_state', 'iss', 'error', 'error_description'].forEach((key) => url.searchParams.delete(key)); window.history.replaceState({}, document.title, `${url.pathname}${url.search}${url.hash}`); if (authError) { if (authError === 'login_required') return { user: null }; throw new Error('Keycloak rechazó el inicio de sesión.'); } if (!transaction || !state || transaction.state !== state) throw new Error('La respuesta de inicio de sesión no pasó la validación de seguridad.'); tokens = await requestTokens(new URLSearchParams({ grant_type: 'authorization_code', client_id: clientId!, code: code!, redirect_uri: redirectUri(), code_verifier: transaction.verifier })); return { user: userFromTokens(tokens), returnTo: transaction.returnTo }; } if (!readTransaction()) { window.location.replace(await authorizationUrl(url.pathname + url.search + url.hash, { prompt: 'none' })); return new Promise(() => undefined); } return { user: null }; }
export async function refreshSession(): Promise<AuthUser | null> { if (!tokens) return null; if (tokens.expiresAt - Date.now() > 60_000) return userFromTokens(tokens); if (!tokens.refreshToken) { tokens = null; return null; } try { tokens = await requestTokens(new URLSearchParams({ grant_type: 'refresh_token', client_id: clientId!, refresh_token: tokens.refreshToken })); return userFromTokens(tokens); } catch { tokens = null; return null; } }
export function getAccessToken(): string | null { return tokens?.accessToken ?? null; }
export async function logout(): Promise<void> { const idToken = tokens?.idToken; tokens = null; if (typeof window === 'undefined' || !isAuthConfigured) return; const params = new URLSearchParams({ post_logout_redirect_uri: `${window.location.origin}/login`, client_id: clientId! }); if (idToken) params.set('id_token_hint', idToken); window.location.assign(`${realmBase()}/logout?${params}`); }
export async function beginLogout(): Promise<void> { return logout(); }

import { ApiError } from '../errors.js';

const unavailable = () => new ApiError(503, 'registration_unavailable',
  'El registro o el servicio de correo no está disponible. Inténtalo más tarde.');

export function validateRegistration(body, resend = false) {
  const fields = resend ? ['email'] : ['name', 'email', 'password'];
  if (!body || typeof body !== 'object' || Array.isArray(body) ||
      Object.keys(body).some((key) => !fields.includes(key))) {
    throw new ApiError(400, 'invalid_registration', 'Campos de registro no válidos.');
  }
  const email = typeof body.email === 'string' ? body.email.trim().toLowerCase() : '';
  if (email.length > 254 || !/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email)) {
    throw new ApiError(400, 'invalid_registration', 'Introduce un correo válido.');
  }
  if (resend) return { email };
  const name = typeof body.name === 'string' ? body.name.trim() : '';
  const password = body.password;
  if (name.length < 2 || name.length > 100 || /[\x00-\x1f]/.test(name) ||
      typeof password !== 'string' || password.length < 12 || password.length > 128 ||
      !/[a-z]/.test(password) || !/[A-Z]/.test(password) || !/[0-9]/.test(password) ||
      !/[^a-zA-Z0-9\s]/.test(password)) {
    throw new ApiError(400, 'invalid_registration',
      'Nombre de 2–100 caracteres y contraseña de 12–128 con mayúscula, minúscula, número y símbolo.');
  }
  return { name, email, password };
}

export function createRegistrationService(config = {}, fetcher = fetch) {
  async function session() {
    let base;
    try { base = new URL(config.baseUrl); } catch { throw unavailable(); }
    if (!config.clientId || !config.clientSecret ||
        (base.protocol !== 'https:' && !(config.allowHttp && base.protocol === 'http:'))) throw unavailable();
    const root = config.baseUrl.replace(/\/$/, '');
    const realm = encodeURIComponent(config.realm);
    async function call(url, options) {
      try { return await fetcher(url, { ...options, signal: AbortSignal.timeout(10000) }); }
      catch { throw unavailable(); }
    }
    const tokenResponse = await call(`${root}/realms/${realm}/protocol/openid-connect/token`, {
      method: 'POST', body: new URLSearchParams({ grant_type: 'client_credentials',
        client_id: config.clientId, client_secret: config.clientSecret }),
    });
    if (!tokenResponse.ok) throw unavailable();
    const { access_token: token } = await tokenResponse.json();
    if (!token) throw unavailable();
    async function admin(path, method = 'GET', body) {
      const response = await call(`${root}/admin/realms/${realm}${path}`, {
        method, headers: { Authorization: `Bearer ${token}`, 'Content-Type': 'application/json' },
        ...(body === undefined ? {} : { body: JSON.stringify(body) }),
      });
      return response;
    }
    const realmResponse = await admin('');
    if (!realmResponse.ok) throw unavailable();
    const settings = await realmResponse.json();
    if (!settings.smtpServer?.host || !settings.smtpServer?.from) throw unavailable();
    return admin;
  }

  async function sendVerification(admin, id) {
    const response = await admin(`/users/${encodeURIComponent(id)}/send-verify-email`, 'PUT');
    if (!response.ok) throw unavailable();
  }

  return {
    async register({ name, email, password }) {
      const admin = await session();
      const roleResponse = await admin('/roles/jugador');
      if (!roleResponse.ok) throw unavailable();
      const role = await roleResponse.json();
      // A composite role could indirectly grant administrative permissions.
      if (role.name !== 'jugador' || role.composite) throw unavailable();
      const created = await admin('/users', 'POST', { username: email, email,
        firstName: name, enabled: false, emailVerified: false,
        requiredActions: ['VERIFY_EMAIL'],
        credentials: [{ type: 'password', value: password, temporary: false }],
      });
      // Same neutral response for existing identities; never claim a new user was created.
      if (created.status === 409) return;
      if (created.status === 400) throw new ApiError(400, 'registration_rejected',
        'No se pudo completar la solicitud. Revisa los datos y los requisitos de contraseña.');
      if (created.status !== 201) throw unavailable();
      const location = created.headers.get('location');
      const id = location?.split('/').pop();
      if (!id) throw unavailable();
      const userPath = `/users/${encodeURIComponent(id)}`;
      const groupsResponse = await admin(`${userPath}/groups`);
      if (!groupsResponse.ok) throw unavailable();
      for (const group of await groupsResponse.json()) {
        if (!(await admin(`${userPath}/groups/${encodeURIComponent(group.id)}`, 'DELETE')).ok) throw unavailable();
      }
      // Remove realm defaults before assigning the one allowed role. New account
      // remains disabled if any step fails; existing accounts are never altered.
      const defaults = await admin(`${userPath}/role-mappings`);
      if (!defaults.ok) throw unavailable();
      const mappings = await defaults.json();
      const roles = mappings.realmMappings ?? [];
      if (roles.length && !(await admin(`${userPath}/role-mappings/realm`, 'DELETE', roles)).ok) throw unavailable();
      for (const client of Object.values(mappings.clientMappings ?? {})) {
        if (!(await admin(`${userPath}/role-mappings/clients/${encodeURIComponent(client.id)}`, 'DELETE', client.mappings)).ok) throw unavailable();
      }
      if (!(await admin(`${userPath}/role-mappings/realm`, 'POST', [role])).ok) throw unavailable();
      if (!(await admin(userPath, 'PUT', { enabled: true, emailVerified: false,
        requiredActions: ['VERIFY_EMAIL'] })).ok) throw unavailable();
      await sendVerification(admin, id);
    },
    async resend({ email }) {
      const admin = await session();
      const response = await admin(`/users?email=${encodeURIComponent(email)}&exact=true&max=2`);
      if (!response.ok) throw unavailable();
      const users = await response.json();
      const user = users.find((item) => item.email?.toLowerCase() === email);
      if (user?.enabled && !user.emailVerified) await sendVerification(admin, user.id);
    },
  };
}

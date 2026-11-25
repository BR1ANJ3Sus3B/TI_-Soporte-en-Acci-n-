import { Router } from 'express';
import { createHash } from 'node:crypto';
import { ApiError } from '../errors.js';
import { createRegistrationService, validateRegistration } from '../services/registration.js';

export function createRegistrationRouter({ config, service = createRegistrationService(config.registration) }) {
  const router = Router();
  const limits = new Map();
  function consume(key, maximum, duration) {
    const now = Date.now();
    for (const [k, entry] of limits) if (entry.until <= now) limits.delete(k);
    let entry = limits.get(key);
    if (!entry) {
      if (limits.size >= 10000) throw new ApiError(429, 'rate_limited', 'Inténtalo más tarde.');
      entry = { count: 0, until: now + duration };
      limits.set(key, entry);
    }
    if (++entry.count > maximum) throw new ApiError(429, 'rate_limited', 'Espera antes de volver a intentarlo.');
  }
  for (const action of ['register', 'resend-verification']) {
    router.post(`/${action}`, async (request, response) => {
      response.set('Cache-Control', 'no-store');
      if (!request.secure && !config.registration?.allowHttp) {
        throw new ApiError(400, 'https_required', 'El registro requiere HTTPS.');
      }
      consume(`ip:${request.ip}`, 10, 15 * 60 * 1000);
      consume('global', 100, 15 * 60 * 1000);
      const input = validateRegistration(request.body, action !== 'register');
      const emailKey = createHash('sha256').update(input.email).digest('hex');
      consume(`email:${emailKey}`, 1, 60 * 1000);
      if (action === 'register') await service.register(input);
      else await service.resend(input);
      // Uniform acknowledgment avoids exposing whether an email already exists.
      response.status(202).json({ status: 'verification_pending', message:
        'Solicitud recibida. Si la cuenta puede recibir verificación, revisa tu correo. Si ya tienes cuenta, inicia sesión o recupera el acceso.' });
    });
  }
  return router;
}

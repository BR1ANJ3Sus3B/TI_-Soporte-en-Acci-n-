import cors from 'cors';
import express from 'express';
import helmet from 'helmet';
import mongoose from 'mongoose';
import { env, isAuthConfigured } from './config/env.js';
import { ApiError, errorHandler } from './errors.js';
import { observeHttpRequests, registry } from './metrics.js';
import * as defaultModels from './models/index.js';
import { createApiRouter } from './routes/api.js';
import { createRegistrationRouter } from './routes/registration.js';

export function createApp({
  config = env,
  models = defaultModels,
  verifier,
  registrationService,
  databaseConnection = mongoose.connection,
} = {}) {
  const app = express();
  app.disable('x-powered-by');
  if (config.trustedProxy) app.set('trust proxy', config.trustedProxy);
  app.use(helmet());
  app.use(cors({
    origin(origin, callback) {
      if (!origin || config.corsOrigins.includes(origin)) return callback(null, true);
      return callback(null, false);
    },
    methods: ['GET', 'POST', 'PATCH', 'OPTIONS'],
    allowedHeaders: ['Authorization', 'Content-Type', 'Idempotency-Key'],
    maxAge: 600,
  }));
  app.use(express.json({ limit: '64kb', strict: true }));
  app.use(observeHttpRequests);

  app.get('/health/live', (_request, response) => {
    response.json({ status: 'ok', service: 'ti-soporte-api' });
  });

  app.get('/health/ready', (_request, response) => {
    const databaseReady = databaseConnection.readyState === 1;
    response.status(databaseReady ? 200 : 503).json({
      status: databaseReady ? 'ready' : 'not_ready',
      database: databaseReady ? 'connected' : 'disconnected',
      keycloakConfigured: isAuthConfigured(config),
    });
  });

  app.get('/metrics', async (request, response, next) => {
    try {
      if (!config.metricsBearerToken || request.get('authorization') !== `Bearer ${config.metricsBearerToken}`) {
        return response.sendStatus(404);
      }
      response.set('Content-Type', registry.contentType);
      response.end(await registry.metrics());
    } catch (error) {
      next(error);
    }
  });

  app.use('/api/v1/auth', createRegistrationRouter({ config, service: registrationService }));
  app.use('/api/v1', createApiRouter({ models, verifier, config }));
  app.use((_request, _response, next) => next(new ApiError(404, 'not_found', 'La ruta solicitada no existe.')));
  app.use(errorHandler);
  return app;
}

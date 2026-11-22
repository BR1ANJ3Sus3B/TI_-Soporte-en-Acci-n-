import assert from 'node:assert/strict';
import { once } from 'node:events';
import { createServer } from 'node:http';
import { describe, it } from 'node:test';
import mongoose from 'mongoose';
import { createApp } from '../src/app.js';

const claims = {
  sub: 'keycloak-subject-123',
  name: 'Brian Test',
  email: 'brian@example.test',
  realm_access: { roles: ['estudiante'] },
};
const config = {
  port: 0,
  corsOrigins: ['http://localhost:5173'],
  keycloakIssuer: 'https://id.example.test/realms/test',
  keycloakAudience: ['ti-soporte-api'],
  keycloakJwksUri: '',
  keycloakClientId: 'ti-soporte-flutter',
  metricsBearerToken: 'metrics-test-token',
};

function makeModels() {
  const profile = {
    subject: claims.sub,
    displayName: claims.name,
    email: claims.email,
    level: 1,
    xp: 0,
    nextLevelXp: null,
    updatedAt: new Date('2026-01-01T00:00:00Z'),
  };
  const progress = {
    subject: claims.sub,
    level: 1,
    xp: 0,
    nextLevelXp: null,
    completedMissionCodes: [],
    inProgressMissionCodes: [],
    missionStates: [],
    specialties: [],
    awardedAchievements: [],
    updatedAt: new Date('2026-01-01T00:00:00Z'),
  };
  return {
    UserProfile: { findOneAndUpdate: async () => profile },
    Progress: {
      findOneAndUpdate: async () => progress,
      findOne: async () => progress,
      exists: async () => null,
    },
    Mission: {}, Level: {}, Achievement: {}, Guide: {},
    MissionAttempt: { countDocuments: async () => 0 }, AdminAudit: {},
  };
}

async function withApi(options, run) {
  const server = createServer(createApp(options));
  server.listen(0, '127.0.0.1');
  await once(server, 'listening');
  const { port } = server.address();
  try {
    await run(`http://127.0.0.1:${port}`);
  } finally {
    server.close();
    await once(server, 'close');
  }
}

describe('API REST', () => {
  it('expone liveness y métricas sin sesión', async () => {
    await withApi({ models: makeModels(), config }, async (base) => {
      const live = await fetch(`${base}/health/live`);
      assert.equal(live.status, 200);
      assert.equal((await live.json()).status, 'ok');

      const hiddenMetrics = await fetch(`${base}/metrics`);
      assert.equal(hiddenMetrics.status, 404);
      const metrics = await fetch(`${base}/metrics`, {
        headers: { Authorization: 'Bearer metrics-test-token' },
      });
      assert.equal(metrics.status, 200);
      assert.match(await metrics.text(), /ti_soporte_api_http_requests_total/);
    });
  });

  it('protege el perfil y toma identidad/rol del token, no del body', async () => {
    const models = makeModels();
    const verifier = async (token) => {
      if (token !== 'valid-test-token') throw new Error('invalid token');
      return claims;
    };
    await withApi({ models, verifier, config }, async (base) => {
      const anonymous = await fetch(`${base}/api/v1/profile`);
      assert.equal(anonymous.status, 401);

      const authenticated = await fetch(`${base}/api/v1/profile`, {
        headers: { Authorization: 'Bearer valid-test-token' },
      });
      assert.equal(authenticated.status, 200);
      const profile = await authenticated.json();
      assert.equal(profile.id, claims.sub);
      assert.equal(profile.name, 'Brian Test');
      assert.equal(profile.email, 'brian@example.test');
      assert.equal(profile.role, 'Jugador');
      assert.equal(profile.xp, 0);
      assert.equal(profile.nextLevelXp, null);
    });
  });

  it('rechaza edición de permisos y exige admin para publicar misiones', async () => {
    const models = makeModels();
    models.Mission.create = async () => {
      throw new Error('Un usuario estudiante no debe crear misiones');
    };
    const verifier = async () => claims;
    await withApi({ models, verifier, config }, async (base) => {
      const profileEdit = await fetch(`${base}/api/v1/profile`, {
        method: 'PATCH',
        headers: { Authorization: 'Bearer token', 'Content-Type': 'application/json' },
        body: JSON.stringify({ displayName: 'Nuevo nombre', roles: ['administrador'] }),
      });
      assert.equal(profileEdit.status, 400);

      const createMission = await fetch(`${base}/api/v1/admin/missions`, {
        method: 'POST',
        headers: { Authorization: 'Bearer token', 'Content-Type': 'application/json' },
        body: JSON.stringify({ code: 'm01' }),
      });
      assert.equal(createMission.status, 403);

      const users = await fetch(`${base}/api/v1/admin/users`, {
        headers: { Authorization: 'Bearer token' },
      });
      assert.equal(users.status, 403);
    });
  });

  it('permite editar solo el nombre visible del perfil autenticado', async () => {
    const models = makeModels();
    let updatedName;
    models.UserProfile.findOneAndUpdate = async (_filter, update) => {
      updatedName = update.$set.displayName;
      return { subject: claims.sub, displayName: updatedName, email: claims.email };
    };
    const verifier = async () => claims;
    await withApi({ models, verifier, config }, async (base) => {
      const response = await fetch(`${base}/api/v1/profile`, {
        method: 'PATCH',
        headers: { Authorization: 'Bearer token', 'Content-Type': 'application/json' },
        body: JSON.stringify({ displayName: 'Nombre autorizado' }),
      });
      assert.equal(response.status, 200);
      assert.equal(updatedName, 'Nombre autorizado');
      assert.equal((await response.json()).name, 'Nombre autorizado');
    });
  });

  it('registra un intento idempotente sin acreditar XP hasta revisión', async () => {
    const models = makeModels();
    const mission = {
      code: 'm01', title: 'Misión verificada', category: 'Soporte',
      description: 'Descripción', difficulty: 'Básica', duration: '10 min',
      xpReward: 100, level: 1, department: '', affectedUser: '',
      objectives: [], requirements: [], active: true,
    };
    models.Mission.findOne = () => ({ lean: async () => mission });
    const previous = {
      _id: 'attempt-db-id', subject: claims.sub, attemptId: 'attempt-12345678',
      missionCode: 'm01', reportedOutcome: 'success', status: 'pending_verification',
    };
    let inserts = 0;
    models.MissionAttempt.create = async () => {
      inserts++;
      if (inserts > 1) throw Object.assign(new Error('duplicate'), { code: 11000 });
      return { id: previous._id, ...previous };
    };
    models.MissionAttempt.findOne = () => ({ lean: async () => previous });
    let progressCredits = 0;
    models.Progress.updateOne = async () => { progressCredits++; return { modifiedCount: 1 }; };

    await withApi({ models, verifier: async () => claims, config }, async (base) => {
      const payload = {
        attemptId: 'attempt-12345678', missionCode: 'm01', outcome: 'success',
      };
      const first = await fetch(`${base}/api/v1/attempts`, {
        method: 'POST', headers: { Authorization: 'Bearer token', 'Content-Type': 'application/json' },
        body: JSON.stringify(payload),
      });
      assert.equal(first.status, 202);
      assert.equal((await first.json()).rewardGranted, false);

      const duplicate = await fetch(`${base}/api/v1/attempts`, {
        method: 'POST', headers: { Authorization: 'Bearer token', 'Content-Type': 'application/json' },
        body: JSON.stringify(payload),
      });
      assert.equal(duplicate.status, 202);
      assert.equal((await duplicate.json()).status, 'pending_verification');
      assert.equal(progressCredits, 0);
      const metrics = await (await fetch(`${base}/metrics`, {
        headers: { Authorization: 'Bearer metrics-test-token' },
      })).text();
      assert.match(metrics, /ti_soporte_api_mission_attempt_reports_total/);
    });
  });

  it('solo un admin acredita la recompensa del catálogo y la aprobación es idempotente', async () => {
    const models = makeModels();
    const mission = { code: 'm09', xpReward: 75, active: true };
    const attemptId = new mongoose.Types.ObjectId();
    let reviewCalls = 0;
    let progressAwards = 0;
    let attempt = {
      _id: attemptId, subject: 'student-sub', missionCode: 'm09',
      reportedOutcome: 'success', status: 'pending_verification',
    };
    models.Mission.findOne = () => ({ lean: async () => mission });
    models.MissionAttempt.findOneAndUpdate = async (_filter, update) => {
      reviewCalls++;
      if (reviewCalls === 1) {
        attempt = { ...attempt, ...update.$set };
        return attempt;
      }
      if (reviewCalls === 2) {
        attempt = { ...attempt, ...update.$set };
        return attempt;
      }
      return null;
    };
    models.MissionAttempt.findById = () => ({ lean: async () => attempt });
    models.Progress.updateOne = async (_filter, update) => {
      if (update.$inc?.xp) progressAwards += update.$inc.xp;
      return { modifiedCount: 1 };
    };
    models.Progress.exists = async () => null;
    models.AdminAudit.create = async () => ({ id: 'audit' });
    const adminConfig = { ...config, keycloakClientId: 'ti-soporte-admin' };
    const verifier = async () => ({
      ...claims, sub: 'admin-sub', realm_access: { roles: ['administrador'] },
    });
    await withApi({ models, verifier, config: adminConfig }, async (base) => {
      const url = `${base}/api/v1/admin/attempts/${attemptId}/review`;
      const send = () => fetch(url, {
        method: 'POST',
        headers: { Authorization: 'Bearer admin-token', 'Content-Type': 'application/json' },
        body: JSON.stringify({ approved: true, xp: 999999 }),
      });
      const approved = await send();
      assert.equal(approved.status, 200);
      assert.equal((await approved.json()).awardedXp, 75);
      assert.equal(progressAwards, 75);

      const repeated = await send();
      assert.equal(repeated.status, 200);
      assert.equal((await repeated.json()).idempotent, true);
      assert.equal(progressAwards, 75);
    });
  });
});

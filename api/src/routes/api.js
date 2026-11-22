import { Router } from 'express';
import mongoose from 'mongoose';
import { createAuthenticate, requireRoles } from '../middleware/authenticate.js';
import { env } from '../config/env.js';
import { ApiError } from '../errors.js';
import { missionAttemptReports, missionAttemptReviews } from '../metrics.js';
import * as defaultModels from '../models/index.js';

const allowedDifficulties = new Set(['Básica', 'Media', 'Avanzada']);
const allowedAchievementIcons = new Set(['award', 'shield', 'rocket']);

function text(value, name, { min = 1, max = 1000 } = {}) {
  if (typeof value !== 'string') throw new ApiError(400, 'invalid_data', `${name} debe ser texto.`);
  const normalized = value.trim();
  if (normalized.length < min || normalized.length > max) {
    throw new ApiError(400, 'invalid_data', `${name} debe tener entre ${min} y ${max} caracteres.`);
  }
  return normalized;
}

function optionalText(value, name, max = 1000) {
  if (value == null) return '';
  return text(value, name, { min: 0, max });
}

function stringArray(value, name, maxItems = 30) {
  if (value == null) return [];
  if (!Array.isArray(value) || value.length > maxItems) {
    throw new ApiError(400, 'invalid_data', `${name} debe ser una lista de hasta ${maxItems} elementos.`);
  }
  return value.map((item, index) => text(item, `${name}[${index}]`, { max: 300 }));
}

function integer(value, name, { min = 0, max = 100000 } = {}) {
  if (!Number.isInteger(value) || value < min || value > max) {
    throw new ApiError(400, 'invalid_data', `${name} debe ser un entero entre ${min} y ${max}.`);
  }
  return value;
}

async function profileFor(subject, claims, UserProfile) {
  const fields = {};
  if (typeof claims.email === 'string') fields.email = claims.email;
  return UserProfile.findOneAndUpdate(
    { subject },
    {
      $set: fields,
      $setOnInsert: {
        displayName: String(claims.name ?? claims.preferred_username ?? ''),
      },
    },
    { new: true, upsert: true, runValidators: true, setDefaultsOnInsert: true },
  );
}

async function progressFor(subject, Progress) {
  return Progress.findOneAndUpdate(
    { subject },
    { $setOnInsert: { subject } },
    { new: true, upsert: true, setDefaultsOnInsert: true },
  );
}

function missionStatus(progress, code) {
  if (progress?.completedMissionCodes?.includes(code)) return 'completed';
  if (progress?.inProgressMissionCodes?.includes(code)) return 'in_progress';
  return 'available';
}

function missionDto(mission, status = 'available') {
  const state = {
    available: 'Disponible',
    in_progress: 'En progreso',
    completed: 'Completada',
  }[status] ?? 'Disponible';
  return {
    id: mission.code,
    title: mission.title,
    category: mission.category,
    description: mission.description,
    difficulty: mission.difficulty,
    duration: mission.duration,
    xp: mission.xpReward,
    status: state,
    level: mission.level,
    department: mission.department,
    affectedUser: mission.affectedUser,
    objectives: mission.objectives,
    requirements: mission.requirements,
  };
}

function roleLabel(roles) {
  if (roles.includes('administrador')) return 'Administrador';
  if (roles.includes('docente')) return 'Docente';
  if (roles.includes('jugador') || roles.includes('estudiante')) return 'Jugador';
  return 'Sin rol asignado';
}

function expectObject(body) {
  if (!body || typeof body !== 'object' || Array.isArray(body)) {
    throw new ApiError(400, 'invalid_data', 'El cuerpo debe ser un objeto JSON.');
  }
}

export function createApiRouter({ models = defaultModels, verifier, config = env } = {}) {
  const router = Router();
  const authenticate = createAuthenticate({ verifier, config });
  const adminOnly = requireRoles('administrador');

  // Catálogo publicado: no depende de Keycloak ni consulta progreso privado.
  // Las rutas de cuenta se registran después y siguen protegidas por completo.
  router.get('/product', (_request, response) => {
    response.json({ name: 'TI: Soporte en Acción', provider: 'NovaTech Solutions' });
  });
  router.get('/missions', async (_request, response) => {
    const missions = await models.Mission.find({ active: true }).sort({ level: 1, code: 1 }).lean();
    response.json(missions.map((mission) => missionDto(mission)));
  });
  router.get('/missions/:code', async (request, response) => {
    const mission = await models.Mission.findOne({ code: request.params.code, active: true }).lean();
    if (!mission) throw new ApiError(404, 'mission_not_found', 'No existe una misión publicada con ese código.');
    response.json(missionDto(mission));
  });
  router.get('/areas', async (_request, response) => {
    const categories = await models.Mission.distinct('category', { active: true });
    response.json(categories.filter(Boolean).sort().map((name) => ({ name })));
  });
  router.get('/levels', async (_request, response) => {
    const levels = await models.Level.find({ active: true }).sort({ order: 1 }).lean();
    response.json(levels.map((level) => ({
      id: level.code, title: level.title, description: level.description,
      order: level.order, unlocked: level.unlockedByDefault,
      requiredMissionCodes: level.requiredMissionCodes ?? [],
    })));
  });
  router.get('/achievements', async (_request, response) => {
    const catalog = await models.Achievement.find({ active: true }).sort({ code: 1 }).lean();
    response.json(catalog.map((achievement) => ({
      id: achievement.code, title: achievement.title, description: achievement.description,
      xp: achievement.xpReward, unlocked: false, icon: achievement.icon,
      criteriaDescription: achievement.criteriaDescription,
    })));
  });
  router.get('/guides', async (_request, response) => {
    const guides = await models.Guide.find({ published: true }).sort({ order: 1, code: 1 }).lean();
    response.json(guides.map(({ code, title, description, category, type }) => ({ id: code, title, description, category, type })));
  });
  router.get('/guides/:code', async (request, response) => {
    const guide = await models.Guide.findOne({ code: request.params.code, published: true }).lean();
    if (!guide) throw new ApiError(404, 'guide_not_found', 'No existe una guía publicada con ese código.');
    response.json({ id: guide.code, title: guide.title, description: guide.description, category: guide.category, type: guide.type, content: guide.content });
  });

  router.use(authenticate);

  router.get('/profile', async (request, response) => {
    const [profile, progress, incidentsResolved] = await Promise.all([
      profileFor(request.auth.subject, request.auth.claims, models.UserProfile),
      progressFor(request.auth.subject, models.Progress),
      models.MissionAttempt.countDocuments({
        subject: request.auth.subject,
        status: 'approved',
        reportedOutcome: 'success',
      }),
    ]);
    response.json({
      id: request.auth.subject,
      name: profile.displayName || String(request.auth.claims.name ?? 'Usuario'),
      email: profile.email || null,
      role: roleLabel(request.auth.roles),
      roles: request.auth.roles,
      level: progress.level,
      xp: progress.xp,
      currentXp: progress.xp,
      nextLevelXp: progress.nextLevelXp,
      missionsCompleted: progress.completedMissionCodes.length,
      incidentsResolved,
      updatedAt: progress.updatedAt,
    });
  });

  router.patch('/profile', async (request, response) => {
    expectObject(request.body);
    const keys = Object.keys(request.body);
    if (keys.some((key) => key !== 'displayName') || !keys.includes('displayName')) {
      throw new ApiError(400, 'invalid_profile_fields', 'Solo se permite actualizar displayName.');
    }
    const displayName = text(request.body.displayName, 'displayName', { max: 80 });
    const profile = await models.UserProfile.findOneAndUpdate(
      { subject: request.auth.subject },
      { $set: { displayName }, $setOnInsert: { email: request.auth.claims.email ?? '' } },
      { new: true, upsert: true, runValidators: true, setDefaultsOnInsert: true },
    );
    response.json({
      id: request.auth.subject,
      name: profile.displayName,
      email: profile.email || null,
      role: roleLabel(request.auth.roles),
      roles: request.auth.roles,
    });
  });

  router.get('/missions', async (request, response) => {
    const [missions, progress] = await Promise.all([
      models.Mission.find({ active: true }).sort({ level: 1, code: 1 }).lean(),
      models.Progress.findOne({ subject: request.auth.subject }).lean(),
    ]);
    response.json(missions.map((mission) => missionDto(mission, missionStatus(progress, mission.code))));
  });

  router.get('/missions/:code', async (request, response) => {
    const [mission, progress] = await Promise.all([
      models.Mission.findOne({ code: request.params.code, active: true }).lean(),
      models.Progress.findOne({ subject: request.auth.subject }).lean(),
    ]);
    if (!mission) throw new ApiError(404, 'mission_not_found', 'No existe una misión publicada con ese código.');
    response.json(missionDto(mission, missionStatus(progress, mission.code)));
  });

  router.get('/areas', async (_request, response) => {
    const categories = await models.Mission.distinct('category', { active: true });
    response.json(categories.filter(Boolean).sort().map((name) => ({ name })));
  });

  router.get('/levels', async (request, response) => {
    const [levels, progress] = await Promise.all([
      models.Level.find({ active: true }).sort({ order: 1 }).lean(),
      models.Progress.findOne({ subject: request.auth.subject }).lean(),
    ]);
    const completed = new Set(progress?.completedMissionCodes ?? []);
    response.json(levels.map((level) => ({
      id: level.code,
      title: level.title,
      description: level.description,
      order: level.order,
      unlocked: level.unlockedByDefault || (level.requiredMissionCodes ?? []).every((code) => completed.has(code)),
      requiredMissionCodes: level.requiredMissionCodes ?? [],
    })));
  });

  router.get('/profile/summary', async (request, response) => {
    const progress = await progressFor(request.auth.subject, models.Progress);
    const completedMissionIds = progress.completedMissionCodes ?? [];
    const inProgressMissionIds = progress.inProgressMissionCodes ?? [];
    response.json({
      userId: request.auth.subject,
      level: progress.level,
      xp: progress.xp,
      nextLevelXp: progress.nextLevelXp,
      completedMissionIds,
      inProgressMissionIds,
      missionStates: [
        ...completedMissionIds.map((code) => ({ code, status: 'completed' })),
        ...inProgressMissionIds.map((code) => ({ code, status: 'in_progress' })),
      ],
      updatedAt: progress.updatedAt,
      syncStatus: 'synchronized',
    });
  });

  router.get('/progress', async (request, response) => {
    const progress = await progressFor(request.auth.subject, models.Progress);
    response.json({
      userId: request.auth.subject,
      level: progress.level,
      xp: progress.xp,
      nextLevelXp: progress.nextLevelXp,
      missionStates: [
        ...(progress.completedMissionCodes ?? []).map((code) => ({ code, status: 'completed' })),
        ...(progress.inProgressMissionCodes ?? []).map((code) => ({ code, status: 'in_progress' })),
      ],
      completedMissionIds: progress.completedMissionCodes ?? [],
      inProgressMissionIds: progress.inProgressMissionCodes ?? [],
      updatedAt: progress.updatedAt,
    });
  });

  router.get('/progress/specialties', async (request, response) => {
    const progress = await models.Progress.findOne({ subject: request.auth.subject }).lean();
    response.json((progress?.specialties ?? []).map((skill) => ({
      name: skill.name,
      progress: skill.progress,
      icon: skill.icon,
    })));
  });

  router.get('/achievements', async (request, response) => {
    const [catalog, progress] = await Promise.all([
      models.Achievement.find({ active: true }).sort({ code: 1 }).lean(),
      models.Progress.findOne({ subject: request.auth.subject }).lean(),
    ]);
    const awards = new Map((progress?.awardedAchievements ?? [])
      .map((award) => [award.code, award.awardedAt]));
    response.json(catalog.map((achievement) => ({
      id: achievement.code,
      title: achievement.title,
      description: achievement.description,
      xp: achievement.xpReward,
      unlocked: awards.has(achievement.code),
      date: awards.get(achievement.code)?.toISOString(),
      icon: achievement.icon,
      criteriaDescription: achievement.criteriaDescription,
    })));
  });

  router.get('/guides', async (_request, response) => {
    const guides = await models.Guide.find({ published: true }).sort({ order: 1, code: 1 }).lean();
    response.json(guides.map(({ code, title, description, category, type }) => ({
      id: code, title, description, category, type,
    })));
  });

  router.get('/guides/:code', async (_request, response) => {
    const guide = await models.Guide.findOne({ code: _request.params.code, published: true }).lean();
    if (!guide) throw new ApiError(404, 'guide_not_found', 'No existe una guía publicada con ese código.');
    response.json({
      id: guide.code, title: guide.title, description: guide.description,
      category: guide.category, type: guide.type, content: guide.content,
    });
  });

  router.post('/attempts', async (request, response) => {
    expectObject(request.body);
    const attemptId = text(request.body.attemptId, 'attemptId', { max: 128 });
    const missionCode = text(request.body.missionCode, 'missionCode', { max: 80 });
    const outcome = request.body.outcome;
    if (!['success', 'failure'].includes(outcome)) {
      throw new ApiError(400, 'invalid_outcome', 'outcome debe ser success o failure.');
    }
    const durationSeconds = request.body.durationSeconds == null
      ? null : integer(request.body.durationSeconds, 'durationSeconds', { min: 0, max: 604800 });
    const mission = await models.Mission.findOne({ code: missionCode, active: true }).lean();
    if (!mission) throw new ApiError(404, 'mission_not_found', 'La misión no existe o no está publicada.');
    try {
      const attempt = await models.MissionAttempt.create({
        subject: request.auth.subject,
        attemptId,
        missionCode,
        reportedOutcome: outcome,
        durationSeconds,
        status: 'pending_verification',
      });
      missionAttemptReports.inc({ outcome, status: 'pending_verification' });
      response.status(202).json({
        id: attempt.id,
        attemptId,
        status: attempt.status,
        rewardGranted: false,
        message: 'Resultado recibido; queda pendiente de verificación. No se otorgó XP.',
      });
    } catch (error) {
      if (error?.code !== 11000) throw error;
      const previous = await models.MissionAttempt.findOne({ subject: request.auth.subject, attemptId }).lean();
      if (!previous || previous.missionCode !== missionCode || previous.reportedOutcome !== outcome) {
        throw new ApiError(409, 'idempotency_conflict', 'attemptId ya fue usado con otro resultado.');
      }
      missionAttemptReports.inc({ outcome, status: 'idempotent_replay' });
      response.status(202).json({
        id: previous._id.toString(), attemptId, status: previous.status,
        rewardGranted: previous.status === 'approved' && previous.reportedOutcome === 'success',
      });
    }
  });

  router.get('/attempts', async (request, response) => {
    const attempts = await models.MissionAttempt.find({ subject: request.auth.subject })
      .sort({ createdAt: -1 }).lean();
    response.json(attempts.map((attempt) => ({
      id: attempt._id.toString(),
      attemptId: attempt.attemptId,
      missionCode: attempt.missionCode,
      reportedOutcome: attempt.reportedOutcome,
      status: attempt.status,
      awardedXp: attempt.awardedXp,
      createdAt: attempt.createdAt,
      reviewedAt: attempt.reviewedAt,
    })));
  });

  router.get('/admin/attempts', adminOnly, async (request, response) => {
    const status = request.query.status ?? 'pending_verification';
    if (!['pending_verification', 'reviewing', 'approved', 'rejected'].includes(status)) {
      throw new ApiError(400, 'invalid_status', 'El filtro de estado no es válido.');
    }
    const attempts = await models.MissionAttempt.find({ status }).sort({ createdAt: 1 }).lean();
    response.json(attempts.map((attempt) => ({
      id: attempt._id.toString(), subject: attempt.subject,
      attemptId: attempt.attemptId, missionCode: attempt.missionCode,
      reportedOutcome: attempt.reportedOutcome, status: attempt.status,
      createdAt: attempt.createdAt,
    })));
  });

  router.get('/admin/users', adminOnly, async (_request, response) => {
    const users = await models.UserProfile.find({})
      .select('subject displayName email createdAt updatedAt')
      .sort({ createdAt: -1 })
      .lean();
    response.json(users.map((user) => ({
      id: user.subject,
      name: user.displayName,
      email: user.email || null,
      createdAt: user.createdAt,
      updatedAt: user.updatedAt,
    })));
  });

  router.get('/admin/users', adminOnly, async (_request, response) => {
    const users = await models.UserProfile.find({})
      .select('subject displayName email level createdAt updatedAt')
      .sort({ createdAt: -1 })
      .lean();
    response.json(users.map((user) => ({
      id: user.subject,
      name: user.displayName,
      email: user.email || null,
      level: user.level,
      createdAt: user.createdAt,
      updatedAt: user.updatedAt,
    })));
  });

  router.post('/admin/attempts/:id/review', adminOnly, async (request, response) => {
    expectObject(request.body);
    if (typeof request.body.approved !== 'boolean') {
      throw new ApiError(400, 'invalid_review', 'approved debe ser boolean.');
    }
    if (!mongoose.isValidObjectId(request.params.id)) {
      throw new ApiError(400, 'invalid_attempt_id', 'El identificador del intento no es válido.');
    }
    const staleReviewBefore = new Date(Date.now() - 60_000);
    const attempt = await models.MissionAttempt.findOneAndUpdate(
      {
        _id: request.params.id,
        $or: [
          { status: 'pending_verification' },
          { status: 'reviewing', reviewStartedAt: { $lte: staleReviewBefore } },
        ],
      },
      { $set: { status: 'reviewing', reviewedBy: request.auth.subject, reviewStartedAt: new Date() } },
      { new: true },
    );
    if (!attempt) {
      const current = await models.MissionAttempt.findById(request.params.id).lean();
      if (!current) throw new ApiError(404, 'attempt_not_found', 'No existe ese intento.');
      if (['approved', 'rejected'].includes(current.status)) {
        response.json({ id: current._id.toString(), status: current.status, awardedXp: current.awardedXp, idempotent: true });
        return;
      }
      throw new ApiError(409, 'attempt_under_review', 'Otro revisor está procesando el intento.');
    }

    try {
      let awardedXp = 0;
      if (request.body.approved && attempt.reportedOutcome === 'success') {
        const mission = await models.Mission.findOne({ code: attempt.missionCode, active: true }).lean();
        if (!mission) throw new ApiError(409, 'mission_unavailable', 'La misión ya no está activa; no se otorgó recompensa.');
        const alreadyCredited = await models.Progress.exists({
          subject: attempt.subject,
          awardedAttemptIds: attempt._id,
        });
        if (alreadyCredited) {
          awardedXp = attempt.awardedXp || mission.xpReward;
        } else {
          await models.Progress.updateOne(
            { subject: attempt.subject },
            { $setOnInsert: { subject: attempt.subject } },
            { upsert: true, setDefaultsOnInsert: true },
          );
          const credit = await models.Progress.updateOne(
            { subject: attempt.subject, awardedAttemptIds: { $ne: attempt._id } },
            {
              $addToSet: {
                awardedAttemptIds: attempt._id,
                completedMissionCodes: attempt.missionCode,
              },
              $pull: { inProgressMissionCodes: attempt.missionCode },
              $inc: { xp: mission.xpReward },
            },
            { runValidators: true },
          );
          awardedXp = credit.modifiedCount === 1 ? mission.xpReward : 0;
          if (awardedXp === 0 && await models.Progress.exists({
            subject: attempt.subject,
            awardedAttemptIds: attempt._id,
          })) {
            awardedXp = mission.xpReward;
          }
        }
      }

      const status = request.body.approved ? 'approved' : 'rejected';
      const reviewed = await models.MissionAttempt.findOneAndUpdate(
        { _id: attempt._id, status: 'reviewing' },
        { $set: { status, reviewedBy: request.auth.subject, reviewedAt: new Date(), reviewStartedAt: null, awardedXp } },
        { new: true },
      );
      missionAttemptReviews.inc({ decision: status });
      await models.AdminAudit.create({
        subject: request.auth.subject,
        action: request.body.approved ? 'attempt.approved' : 'attempt.rejected',
        entity: 'mission_attempt', entityId: attempt._id.toString(),
      });
      response.json({ id: reviewed._id.toString(), status: reviewed.status, awardedXp: reviewed.awardedXp });
    } catch (error) {
      await models.MissionAttempt.updateOne(
        { _id: attempt._id, status: 'reviewing' },
        { $set: { status: 'pending_verification', reviewedBy: null, reviewStartedAt: null } },
      ).catch(() => {});
      throw error;
    }
  });

  router.post('/admin/missions', adminOnly, async (request, response) => {
    expectObject(request.body);
    if (!allowedDifficulties.has(request.body.difficulty)) {
      throw new ApiError(400, 'invalid_difficulty', 'difficulty debe ser Básica, Media o Avanzada.');
    }
    const mission = await models.Mission.create({
      code: text(request.body.code, 'code', { max: 80 }),
      title: text(request.body.title, 'title', { max: 160 }),
      category: text(request.body.category, 'category', { max: 80 }),
      description: text(request.body.description, 'description', { max: 4000 }),
      difficulty: request.body.difficulty,
      duration: text(request.body.duration, 'duration', { max: 40 }),
      xpReward: integer(request.body.xpReward, 'xpReward'),
      level: integer(request.body.level, 'level', { min: 1, max: 1000 }),
      department: optionalText(request.body.department, 'department', 100),
      affectedUser: optionalText(request.body.affectedUser, 'affectedUser', 100),
      objectives: stringArray(request.body.objectives, 'objectives'),
      requirements: stringArray(request.body.requirements, 'requirements'),
      active: request.body.active === true,
    });
    await models.AdminAudit.create({ subject: request.auth.subject, action: 'mission.created', entity: 'mission', entityId: mission.code });
    response.status(201).json(missionDto(mission.toObject()));
  });

  router.post('/admin/guides', adminOnly, async (request, response) => {
    expectObject(request.body);
    const guide = await models.Guide.create({
      code: text(request.body.code, 'code', { max: 80 }),
      title: text(request.body.title, 'title', { max: 160 }),
      content: text(request.body.content, 'content', { max: 50000 }),
      category: text(request.body.category, 'category', { max: 80 }),
      type: request.body.type == null ? 'Guía' : text(request.body.type, 'type', { max: 40 }),
      description: optionalText(request.body.description, 'description', 1000),
      order: request.body.order == null ? 0 : integer(request.body.order, 'order'),
      published: request.body.published === true,
    });
    await models.AdminAudit.create({ subject: request.auth.subject, action: 'guide.created', entity: 'guide', entityId: guide.code });
    response.status(201).json({
      id: guide.code, title: guide.title, description: guide.description,
      category: guide.category, type: guide.type,
    });
  });

  router.post('/admin/levels', adminOnly, async (request, response) => {
    expectObject(request.body);
    const requiredMissionCodes = stringArray(request.body.requiredMissionCodes, 'requiredMissionCodes');
    const level = await models.Level.create({
      code: text(request.body.code, 'code', { max: 80 }),
      title: text(request.body.title, 'title', { max: 120 }),
      description: optionalText(request.body.description, 'description', 2000),
      order: integer(request.body.order, 'order', { min: 1, max: 1000 }),
      active: request.body.active === true,
      unlockedByDefault: request.body.unlockedByDefault === true,
      requiredMissionCodes,
    });
    await models.AdminAudit.create({ subject: request.auth.subject, action: 'level.created', entity: 'level', entityId: level.code });
    response.status(201).json({
      id: level.code, title: level.title, description: level.description,
      order: level.order, active: level.active,
      unlockedByDefault: level.unlockedByDefault,
      requiredMissionCodes: level.requiredMissionCodes,
    });
  });

  router.post('/admin/achievements', adminOnly, async (request, response) => {
    expectObject(request.body);
    const icon = request.body.icon ?? 'award';
    if (!allowedAchievementIcons.has(icon)) {
      throw new ApiError(400, 'invalid_icon', 'icon debe ser award, shield o rocket.');
    }
    const achievement = await models.Achievement.create({
      code: text(request.body.code, 'code', { max: 80 }),
      title: text(request.body.title, 'title', { max: 160 }),
      description: text(request.body.description, 'description', { max: 2000 }),
      xpReward: integer(request.body.xpReward ?? 0, 'xpReward'),
      icon,
      criteriaDescription: optionalText(request.body.criteriaDescription, 'criteriaDescription', 1000),
      active: request.body.active === true,
    });
    await models.AdminAudit.create({ subject: request.auth.subject, action: 'achievement.created', entity: 'achievement', entityId: achievement.code });
    response.status(201).json({
      id: achievement.code, title: achievement.title,
      description: achievement.description, xp: achievement.xpReward,
      icon: achievement.icon, active: achievement.active,
    });
  });

  return router;
}

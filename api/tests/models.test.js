import assert from 'node:assert/strict';
import { describe, it } from 'node:test';
import { Mission } from '../src/models/Mission.js';
import { MissionAttempt } from '../src/models/MissionAttempt.js';
import { Progress } from '../src/models/Progress.js';
import { UserProfile } from '../src/models/UserProfile.js';

describe('MongoDB models', () => {
  it('requires explicit mission data and reward from the catalog', async () => {
    const mission = new Mission({ code: 'm01', title: 'Demo' });
    await assert.rejects(() => mission.validate());
    assert.equal(mission.active, false);
  });

  it('keys profile and progress by stable Keycloak subject, not password', () => {
    const profileFields = Object.keys(UserProfile.schema.paths);
    assert.ok(profileFields.includes('subject'));
    assert.ok(!profileFields.some((field) => /password|secret/i.test(field)));
    assert.ok(Progress.schema.indexes().some(([fields, options]) => fields.subject === 1 && options.unique));
  });

  it('defines a per-user idempotency index for mission attempts', () => {
    assert.ok(MissionAttempt.schema.indexes().some(([fields, options]) =>
      fields.subject === 1 && fields.attemptId === 1 && options.unique));
  });
});

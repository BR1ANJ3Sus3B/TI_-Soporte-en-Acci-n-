import mongoose from 'mongoose';

const MissionAttemptSchema = new mongoose.Schema({
  subject: { type: String, required: true, index: true },
  attemptId: { type: String, required: true, trim: true, maxlength: 128 },
  missionCode: { type: String, required: true, trim: true },
  reportedOutcome: { type: String, enum: ['success', 'failure'], required: true },
  durationSeconds: { type: Number, min: 0, max: 604800, default: null },
  status: {
    type: String,
    enum: ['pending_verification', 'reviewing', 'approved', 'rejected'],
    default: 'pending_verification',
    index: true,
  },
  reviewedBy: { type: String, default: null },
  reviewStartedAt: { type: Date, default: null },
  reviewedAt: { type: Date, default: null },
  awardedXp: { type: Number, min: 0, default: 0 },
}, { timestamps: true, versionKey: false });

MissionAttemptSchema.index({ subject: 1, attemptId: 1 }, { unique: true });
MissionAttemptSchema.index({ status: 1, createdAt: 1 });

export const MissionAttempt = mongoose.models.MissionAttempt ?? mongoose.model('MissionAttempt', MissionAttemptSchema);

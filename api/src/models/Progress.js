import mongoose from 'mongoose';

const ProgressSchema = new mongoose.Schema({
  subject: { type: String, required: true, unique: true, index: true },
  xp: { type: Number, min: 0, default: 0 },
  level: { type: Number, min: 1, default: null },
  // No se calcula sin una regla de subida de nivel acordada/configurada.
  nextLevelXp: { type: Number, min: 1, default: null },
  completedMissionCodes: [{ type: String, trim: true }],
  inProgressMissionCodes: [{ type: String, trim: true }],
  specialties: [{
    name: { type: String, required: true, trim: true },
    progress: { type: Number, min: 0, max: 1, required: true },
    icon: { type: String, enum: ['headset', 'network', 'shield', 'code', 'database'], required: true },
  }],
  awardedAchievements: [{
    code: { type: String, required: true },
    awardedAt: { type: Date, required: true },
  }],
  awardedAttemptIds: [{ type: mongoose.Schema.Types.ObjectId }],
}, { timestamps: true, versionKey: false });

export const Progress = mongoose.models.Progress ?? mongoose.model('Progress', ProgressSchema);

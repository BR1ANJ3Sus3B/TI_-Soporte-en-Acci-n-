import mongoose from 'mongoose';

const LevelSchema = new mongoose.Schema({
  code: { type: String, required: true, unique: true, trim: true, maxlength: 80 },
  title: { type: String, required: true, trim: true, maxlength: 120 },
  description: { type: String, default: '', maxlength: 2000 },
  order: { type: Number, required: true, min: 1 },
  active: { type: Boolean, default: false },
  unlockedByDefault: { type: Boolean, default: false },
  requiredMissionCodes: [{ type: String, trim: true }],
}, { timestamps: true, versionKey: false });

LevelSchema.index({ active: 1, order: 1 });

export const Level = mongoose.models.Level ?? mongoose.model('Level', LevelSchema);

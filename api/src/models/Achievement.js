import mongoose from 'mongoose';

const AchievementSchema = new mongoose.Schema({
  code: { type: String, required: true, unique: true, trim: true, maxlength: 80 },
  title: { type: String, required: true, trim: true, maxlength: 160 },
  description: { type: String, required: true, maxlength: 2000 },
  xpReward: { type: Number, min: 0, max: 100000, default: 0 },
  icon: { type: String, enum: ['award', 'shield', 'rocket'], default: 'award' },
  criteriaDescription: { type: String, default: '', maxlength: 1000 },
  active: { type: Boolean, default: false },
}, { timestamps: true, versionKey: false });

export const Achievement = mongoose.models.Achievement ?? mongoose.model('Achievement', AchievementSchema);

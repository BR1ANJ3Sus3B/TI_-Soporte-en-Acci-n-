import mongoose from 'mongoose';

const MissionSchema = new mongoose.Schema({
  code: { type: String, required: true, unique: true, trim: true, maxlength: 80 },
  title: { type: String, required: true, trim: true, maxlength: 160 },
  category: { type: String, required: true, trim: true, maxlength: 80 },
  description: { type: String, required: true, maxlength: 4000 },
  difficulty: { type: String, enum: ['Básica', 'Media', 'Avanzada'], required: true },
  duration: { type: String, required: true, maxlength: 40 },
  xpReward: { type: Number, required: true, min: 0, max: 100000 },
  level: { type: Number, required: true, min: 1 },
  department: { type: String, trim: true, maxlength: 100, default: '' },
  affectedUser: { type: String, trim: true, maxlength: 100, default: '' },
  objectives: [{ type: String, trim: true, maxlength: 300 }],
  requirements: [{ type: String, trim: true, maxlength: 300 }],
  active: { type: Boolean, default: false },
}, { timestamps: true, versionKey: false });

MissionSchema.index({ active: 1, level: 1, code: 1 });

export const Mission = mongoose.models.Mission ?? mongoose.model('Mission', MissionSchema);

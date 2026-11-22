import mongoose from 'mongoose';

const GuideSchema = new mongoose.Schema({
  code: { type: String, required: true, unique: true, trim: true, maxlength: 80 },
  title: { type: String, required: true, trim: true, maxlength: 160 },
  content: { type: String, required: true, maxlength: 50000 },
  category: { type: String, required: true, trim: true, maxlength: 80 },
  type: { type: String, trim: true, maxlength: 40, default: 'Guía' },
  description: { type: String, default: '', maxlength: 1000 },
  order: { type: Number, min: 0, default: 0 },
  published: { type: Boolean, default: false },
}, { timestamps: true, versionKey: false });

GuideSchema.index({ published: 1, category: 1, order: 1 });

export const Guide = mongoose.models.Guide ?? mongoose.model('Guide', GuideSchema);

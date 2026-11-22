import mongoose from 'mongoose';

const UserProfileSchema = new mongoose.Schema({
  subject: { type: String, required: true, unique: true, index: true },
  displayName: { type: String, trim: true, maxlength: 80, default: '' },
  email: { type: String, trim: true, lowercase: true, default: '' },
}, { timestamps: true, versionKey: false });

export const UserProfile = mongoose.models.UserProfile ??
  mongoose.model('UserProfile', UserProfileSchema);

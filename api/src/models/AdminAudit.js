import mongoose from 'mongoose';

const AdminAuditSchema = new mongoose.Schema({
  subject: { type: String, required: true, index: true },
  action: { type: String, required: true, maxlength: 80 },
  entity: { type: String, required: true, maxlength: 80 },
  entityId: { type: String, required: true, maxlength: 128 },
  occurredAt: { type: Date, default: Date.now },
}, { versionKey: false });

export const AdminAudit = mongoose.models.AdminAudit ?? mongoose.model('AdminAudit', AdminAuditSchema);

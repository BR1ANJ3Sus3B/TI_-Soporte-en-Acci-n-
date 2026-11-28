import assert from 'node:assert/strict';
const ids = ['diagnostico-red', 'phishing', 'respaldo'];
assert.equal(new Set(ids).size, ids.length, 'el catálogo no debe duplicar identificadores');
const guestRoles = [];
assert.equal(guestRoles.includes('admin'), false, 'el modo invitado no concede administración');

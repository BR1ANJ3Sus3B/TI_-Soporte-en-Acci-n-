import 'dart:convert';

import 'package:flutter_secure_storage/flutter_secure_storage.dart';

/// Progreso del invitado, persistente en el dispositivo y sin valor oficial.
class GuestProgressStore {
  GuestProgressStore([FlutterSecureStorage? storage])
    : _storage = storage ?? const FlutterSecureStorage();

  static final instance = GuestProgressStore();
  static const _key = 'ti_soporte.guest.progress.v1';
  final FlutterSecureStorage _storage;

  Future<GuestProgress> read() async {
    try {
      final raw = await _storage.read(key: _key);
      if (raw == null) return const GuestProgress();
      return GuestProgress.fromJson(jsonDecode(raw));
    } catch (_) {
      return const GuestProgress();
    }
  }

  Future<void> write(GuestProgress value) =>
      _storage.write(key: _key, value: jsonEncode(value.toJson()));
}

class GuestProgress {
  const GuestProgress({
    this.completedMissionIds = const <String>[],
    this.inProgressMissionIds = const <String>[],
    this.xp = 0,
  });
  final List<String> completedMissionIds;
  final List<String> inProgressMissionIds;
  final int xp;

  factory GuestProgress.fromJson(Object? value) {
    if (value is! Map) return const GuestProgress();
    List<String> ids(Object? raw) => raw is List
        ? raw.whereType<String>().take(200).toList(growable: false)
        : const <String>[];
    final xp = value['xp'];
    return GuestProgress(
      completedMissionIds: ids(value['completedMissionIds']),
      inProgressMissionIds: ids(value['inProgressMissionIds']),
      xp: xp is num ? xp.toInt().clamp(0, 100000) : 0,
    );
  }

  Map<String, Object> toJson() => <String, Object>{
    'completedMissionIds': completedMissionIds,
    'inProgressMissionIds': inProgressMissionIds,
    'xp': xp,
  };
}

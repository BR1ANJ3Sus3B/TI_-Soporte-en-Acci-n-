import 'package:flutter/foundation.dart';
import 'package:ti_soporte_accion/models/achievement.dart';

/// Servicio encargado de encolar y entregar las notificaciones de logros
/// desbloqueados.
///
/// Mantiene una única instancia ([instance]) para que cualquier pantalla pueda
/// solicitar la notificación sin necesidad de un gestor de estado complejo.
/// El servicio garantiza que:
/// * Un logro solo se notifica una vez por sesión (evita duplicados).
/// * Si varios logros se desbloquean al mismo tiempo, se muestran en cola y no
///   se superponen.
class AchievementNotificationService {
  AchievementNotificationService._();

  /// Instancia única compartida por toda la aplicación.
  static final AchievementNotificationService instance =
      AchievementNotificationService._();

  /// Logro que debe mostrarse en pantalla en este momento (o `null`).
  final ValueNotifier<Achievement?> current = ValueNotifier<Achievement?>(null);

  /// Tiempo que permanece visible cada notificación.
  static const Duration displayDuration = Duration(milliseconds: 5000);

  final List<Achievement> _queue = <Achievement>[];
  final Set<String> _shownIds = <String>{};

  /// Encola un logro desbloqueado para mostrarlo como notificación.
  ///
  /// Si ya fue notificado en esta sesión, se ignora.
  void notifyUnlock(Achievement achievement) {
    if (_shownIds.contains(achievement.id)) {
      return;
    }
    _shownIds.add(achievement.id);
    _queue.add(achievement);
    _showNextIfIdle();
  }

  /// Marca el logro [achievement] como ya mostrado y libera el turno al
  /// siguiente de la cola. Lo invoca el overlay cuando termina la animación.
  void markCompleted(Achievement achievement) {
    if (current.value?.id == achievement.id) {
      current.value = null;
    }
    _showNextIfIdle();
  }

  /// Indica si un logro ya fue notificado en esta sesión.
  bool wasShown(String id) => _shownIds.contains(id);

  /// Reinicia por completo el estado del servicio (útil en pruebas).
  void reset() {
    _queue.clear();
    _shownIds.clear();
    current.value = null;
  }

  void _showNextIfIdle() {
    if (current.value == null && _queue.isNotEmpty) {
      current.value = _queue.removeAt(0);
    }
  }
}

import 'dart:async';

import 'package:flutter/material.dart';
import 'package:ti_soporte_accion/models/achievement.dart';
import 'package:ti_soporte_accion/screens/achievement_detail_screen.dart';
import 'package:ti_soporte_accion/services/achievement_notification_service.dart';
import 'package:ti_soporte_accion/theme/app_colors.dart';

/// Tarjeta visual de un logro desbloqueado (estilo Steam).
///
/// Se dibuja como una notificación flotante en la parte inferior de la
/// pantalla. Muestra la insignia, el nombre, la descripción, la rareza y la
/// experiencia otorgada.
class AchievementUnlockPopup extends StatelessWidget {
  const AchievementUnlockPopup({
    super.key,
    required this.achievement,
    required this.onTap,
    required this.onDismiss,
  });

  /// Logro que se está notificando.
  final Achievement achievement;

  /// Se invoca al tocar la tarjeta (abre el detalle).
  final VoidCallback onTap;

  /// Se invoca al cerrar la notificación manualmente.
  final VoidCallback onDismiss;

  @override
  Widget build(BuildContext context) {
    final rarityColor = achievement.rarity.color;

    return Material(
      color: Colors.transparent,
      child: InkWell(
        onTap: onTap,
        borderRadius: BorderRadius.circular(16),
        child: Ink(
          decoration: BoxDecoration(
            color: AppColors.card,
            borderRadius: BorderRadius.circular(16),
            border: Border.all(color: rarityColor, width: 1.5),
            boxShadow: <BoxShadow>[
              BoxShadow(
                color: rarityColor.withValues(alpha: 0.35),
                blurRadius: 18,
                spreadRadius: 1,
              ),
              const BoxShadow(
                color: Colors.black54,
                blurRadius: 14,
                offset: Offset(0, 6),
              ),
            ],
            gradient: LinearGradient(
              begin: Alignment.topLeft,
              end: Alignment.bottomRight,
              colors: <Color>[
                AppColors.card,
                Color.lerp(AppColors.card, rarityColor, 0.12)!,
              ],
            ),
          ),
          child: Padding(
            padding: const EdgeInsets.fromLTRB(14, 12, 8, 12),
            child: Row(
              children: <Widget>[
                _Badge(icon: achievement.icon, color: rarityColor),
                const SizedBox(width: 14),
                Expanded(
                  child: Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    mainAxisSize: MainAxisSize.min,
                    children: <Widget>[
                      const Text(
                        'LOGRO DESBLOQUEADO',
                        style: TextStyle(
                          color: AppColors.cyan,
                          fontSize: 10,
                          fontWeight: FontWeight.w800,
                          letterSpacing: 1.4,
                        ),
                      ),
                      const SizedBox(height: 4),
                      Text(
                        achievement.title,
                        maxLines: 1,
                        overflow: TextOverflow.ellipsis,
                        style: const TextStyle(
                          color: AppColors.textPrimary,
                          fontSize: 15,
                          fontWeight: FontWeight.w800,
                        ),
                      ),
                      const SizedBox(height: 2),
                      Text(
                        achievement.description,
                        maxLines: 2,
                        overflow: TextOverflow.ellipsis,
                        style: const TextStyle(
                          color: AppColors.textSecondary,
                          fontSize: 12,
                          height: 1.35,
                        ),
                      ),
                      const SizedBox(height: 6),
                      Row(
                        children: <Widget>[
                          _RarityTag(
                            label: achievement.rarity.label,
                            color: rarityColor,
                          ),
                          const SizedBox(width: 8),
                          Text(
                            '+${achievement.xp} XP',
                            style: const TextStyle(
                              color: AppColors.success,
                              fontSize: 12,
                              fontWeight: FontWeight.w800,
                            ),
                          ),
                        ],
                      ),
                    ],
                  ),
                ),
                IconButton(
                  onPressed: onDismiss,
                  icon: const Icon(Icons.close_rounded, size: 18),
                  color: AppColors.textSecondary,
                  splashRadius: 18,
                  tooltip: 'Cerrar',
                ),
              ],
            ),
          ),
        ),
      ),
    );
  }
}

class _Badge extends StatelessWidget {
  const _Badge({required this.icon, required this.color});

  final IconData icon;
  final Color color;

  @override
  Widget build(BuildContext context) {
    return Container(
      width: 52,
      height: 52,
      decoration: BoxDecoration(
        shape: BoxShape.circle,
        color: color.withValues(alpha: 0.18),
        border: Border.all(color: color, width: 2),
        boxShadow: <BoxShadow>[
          BoxShadow(color: color.withValues(alpha: 0.45), blurRadius: 12),
        ],
      ),
      child: Icon(icon, color: color, size: 26),
    );
  }
}

class _RarityTag extends StatelessWidget {
  const _RarityTag({required this.label, required this.color});

  final String label;
  final Color color;

  @override
  Widget build(BuildContext context) {
    return Container(
      padding: const EdgeInsets.symmetric(horizontal: 8, vertical: 2),
      decoration: BoxDecoration(
        color: color.withValues(alpha: 0.18),
        borderRadius: BorderRadius.circular(999),
        border: Border.all(color: color.withValues(alpha: 0.7)),
      ),
      child: Text(
        label,
        style: TextStyle(
          color: color,
          fontSize: 10,
          fontWeight: FontWeight.w700,
        ),
      ),
    );
  }
}

/// Capa que muestra las notificaciones de logros por encima de toda la app.
///
/// Debe colocarse en el `builder` de [MaterialApp] para cubrir todas las rutas.
/// Escucha al [AchievementNotificationService] y anima la entrada (deslizar
/// hacia arriba + aparecer), la permanencia y la salida (desvanecer + bajar).
class AchievementPopupHost extends StatefulWidget {
  const AchievementPopupHost({
    super.key,
    required this.child,
    required this.navigatorKey,
    this.service,
  });

  /// Árbol de la aplicación que queda por debajo de las notificaciones.
  final Widget child;

  /// Clave del navegador raíz para abrir el detalle desde el overlay.
  final GlobalKey<NavigatorState> navigatorKey;

  /// Servicio a utilizar (por defecto, la instancia compartida).
  final AchievementNotificationService? service;

  @override
  State<AchievementPopupHost> createState() => _AchievementPopupHostState();
}

class _AchievementPopupHostState extends State<AchievementPopupHost>
    with SingleTickerProviderStateMixin {
  late final AchievementNotificationService _service =
      widget.service ?? AchievementNotificationService.instance;
  late final AnimationController _controller = AnimationController(
    vsync: this,
    duration: const Duration(milliseconds: 300),
    reverseDuration: const Duration(milliseconds: 300),
  );
  late final Animation<Offset> _slide = Tween<Offset>(
    begin: const Offset(0, 1),
    end: Offset.zero,
  ).animate(CurvedAnimation(parent: _controller, curve: Curves.easeOutCubic));
  late final Animation<double> _fade = CurvedAnimation(
    parent: _controller,
    curve: Curves.easeOut,
  );

  Achievement? _achievement;
  Timer? _timer;
  String? _showingId;

  @override
  void initState() {
    super.initState();
    _service.current.addListener(_onCurrentChanged);
    _onCurrentChanged();
  }

  @override
  void dispose() {
    _timer?.cancel();
    _service.current.removeListener(_onCurrentChanged);
    _controller.dispose();
    super.dispose();
  }

  void _onCurrentChanged() {
    final Achievement? next = _service.current.value;
    if (next == null || next.id == _showingId) {
      return;
    }
    _showingId = next.id;
    setState(() => _achievement = next);
    _controller.forward(from: 0);
    _timer?.cancel();
    _timer = Timer(AchievementNotificationService.displayDuration, _dismiss);
  }

  Future<void> _dismiss() async {
    _timer?.cancel();
    if (!_controller.isAnimating && _controller.value == 0) {
      _finish();
      return;
    }
    await _controller.reverse();
    _finish();
  }

  void _finish() {
    final Achievement? done = _achievement;
    _showingId = null;
    if (mounted) {
      setState(() => _achievement = null);
    }
    if (done != null) {
      _service.markCompleted(done);
    }
  }

  Future<void> _openDetail() async {
    final Achievement? target = _achievement;
    await _dismiss();
    if (target != null) {
      widget.navigatorKey.currentState?.pushNamed(
        AchievementDetailScreen.routeName,
        arguments: target,
      );
    }
  }

  @override
  Widget build(BuildContext context) {
    final Achievement? achievement = _achievement;
    return Stack(
      children: <Widget>[
        widget.child,
        if (achievement != null)
          Positioned(
            left: 12,
            right: 12,
            bottom: 0,
            child: SafeArea(
              top: false,
              minimum: const EdgeInsets.only(bottom: 12),
              child: SlideTransition(
                position: _slide,
                child: FadeTransition(
                  opacity: _fade,
                  child: AchievementUnlockPopup(
                    achievement: achievement,
                    onTap: _openDetail,
                    onDismiss: _dismiss,
                  ),
                ),
              ),
            ),
          ),
      ],
    );
  }
}

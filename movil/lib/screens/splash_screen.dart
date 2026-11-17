import 'package:flutter/material.dart';
import 'package:ti_soporte_accion/services/auth_service.dart';
import 'package:ti_soporte_accion/theme/app_colors.dart';
import 'package:ti_soporte_accion/theme/app_typography.dart';

/// Pantalla inicial de NovaTech; conserva el flujo automático hacia login.
class SplashScreen extends StatefulWidget {
  const SplashScreen({super.key});

  static const String routeName = '/splash';

  @override
  State<SplashScreen> createState() => _SplashScreenState();
}

class _SplashScreenState extends State<SplashScreen>
    with SingleTickerProviderStateMixin {
  late final bool _reduceMotion = WidgetsBinding
      .instance
      .platformDispatcher
      .accessibilityFeatures
      .disableAnimations;
  late final AnimationController _controller = AnimationController(
    vsync: this,
    duration: _reduceMotion ? Duration.zero : const Duration(milliseconds: 700),
  )..forward();

  @override
  void initState() {
    super.initState();
    _restoreAndContinue();
  }

  Future<void> _restoreAndContinue() async {
    await AuthService.instance.restoreSession();
    if (!_reduceMotion) {
      await Future<void>.delayed(const Duration(milliseconds: 700));
    }
    if (!mounted) return;
    Navigator.of(context).pushReplacementNamed(
      AuthService.instance.isAuthenticated ? '/home' : '/login',
    );
  }

  @override
  void dispose() {
    _controller.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    final fade = CurvedAnimation(parent: _controller, curve: Curves.easeOut);
    return Scaffold(
      body: Container(
        decoration: const BoxDecoration(gradient: AppColors.backgroundGradient),
        child: Center(
          child: FadeTransition(
            opacity: fade,
            child: ScaleTransition(
              scale: Tween<double>(begin: .94, end: 1).animate(
                CurvedAnimation(
                  parent: _controller,
                  curve: Curves.easeOutCubic,
                ),
              ),
              child: Column(
                mainAxisAlignment: MainAxisAlignment.center,
                children: <Widget>[
                  Container(
                    width: 76,
                    height: 76,
                    decoration: BoxDecoration(
                      shape: BoxShape.circle,
                      gradient: AppColors.primaryGradient,
                      boxShadow: <BoxShadow>[
                        BoxShadow(
                          color: AppColors.primaryCyan.withValues(alpha: .14),
                          blurRadius: 14,
                        ),
                      ],
                    ),
                    child: const Icon(
                      Icons.support_agent_rounded,
                      color: AppColors.textPrimary,
                      size: 38,
                    ),
                  ),
                  const SizedBox(height: 22),
                  Text(
                    'TI: Soporte en Acción',
                    style: AppTypography.heading(24),
                  ),
                  const SizedBox(height: 10),
                  const Text(
                    'NOVATECH SOLUTIONS',
                    style: TextStyle(
                      color: AppColors.cyan,
                      fontSize: 12,
                      fontWeight: FontWeight.w700,
                      letterSpacing: 1.2,
                    ),
                  ),
                  const SizedBox(height: 10),
                  const Text(
                    'Preparando entorno de soporte…',
                    style: TextStyle(color: AppColors.textSecondary),
                  ),
                  const SizedBox(height: 28),
                  const SizedBox(
                    width: 32,
                    height: 32,
                    child: CircularProgressIndicator(
                      strokeWidth: 2,
                      valueColor: AlwaysStoppedAnimation<Color>(AppColors.cyan),
                    ),
                  ),
                ],
              ),
            ),
          ),
        ),
      ),
    );
  }
}

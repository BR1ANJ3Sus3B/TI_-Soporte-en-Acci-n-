import 'package:flutter/material.dart';
import 'package:ti_soporte_accion/services/api_client.dart';
import 'package:ti_soporte_accion/services/auth_service.dart';
import 'package:ti_soporte_accion/theme/app_colors.dart';
import 'package:ti_soporte_accion/theme/app_typography.dart';
import 'package:ti_soporte_accion/widgets/nova_panel.dart';

/// Estados comunes de carga, error y vacío con reintento accesible.
class AsyncContent<T> extends StatefulWidget {
  const AsyncContent({
    required this.load,
    required this.builder,
    this.isEmpty,
    this.emptyMessage = 'No hay información disponible.',
    super.key,
  });

  final Future<T> Function() load;
  final Widget Function(BuildContext context, T value) builder;
  final bool Function(T value)? isEmpty;
  final String emptyMessage;

  @override
  State<AsyncContent<T>> createState() => _AsyncContentState<T>();
}

class _AsyncContentState<T> extends State<AsyncContent<T>> {
  late Future<T> _future = widget.load();

  void _retry() => setState(() => _future = widget.load());

  @override
  Widget build(BuildContext context) => FutureBuilder<T>(
    future: _future,
    builder: (context, snapshot) {
      if (snapshot.connectionState != ConnectionState.done) {
        return const Center(
          child: CircularProgressIndicator(color: AppColors.primaryCyan),
        );
      }
      if (snapshot.hasError) {
        final error = snapshot.error;
        final message = error is ApiException
            ? error.message
            : 'No se pudieron cargar los datos. Inténtalo de nuevo.';
        return Center(
          child: Padding(
            padding: const EdgeInsets.all(24),
            child: NovaPanel(
              child: Column(
                mainAxisSize: MainAxisSize.min,
                children: [
                  const Icon(
                    Icons.cloud_off_rounded,
                    color: AppColors.error,
                    size: 32,
                  ),
                  const SizedBox(height: 12),
                  Text(
                    message,
                    textAlign: TextAlign.center,
                    style: AppTypography.body(
                      14,
                      color: AppColors.textSecondary,
                    ),
                  ),
                  const SizedBox(height: 12),
                  if (error is ApiException &&
                      error.failure == ApiFailure.unauthorized) ...[
                    OutlinedButton(
                      onPressed: () =>
                          Navigator.of(context)
                              .pushNamedAndRemoveUntil('/login', (_) => false),
                      child: const Text('Iniciar sesión'),
                    ),
                    TextButton(
                      onPressed: () async {
                        await AuthService.instance.continueAsGuest();
                        if (context.mounted) {
                          Navigator.of(context)
                              .pushNamedAndRemoveUntil('/home', (_) => false);
                        }
                      },
                      child: const Text('Continuar como invitado'),
                    ),
                  ] else
                    OutlinedButton.icon(
                      onPressed: _retry,
                      icon: const Icon(Icons.refresh_rounded),
                      label: const Text('Reintentar'),
                    ),
                ],
              ),
            ),
          ),
        );
      }
      final value = snapshot.data as T;
      if (widget.isEmpty?.call(value) ?? false) {
        return Center(
          child: Padding(
            padding: const EdgeInsets.all(24),
            child: Text(
              widget.emptyMessage,
              textAlign: TextAlign.center,
              style: AppTypography.body(14, color: AppColors.textSecondary),
            ),
          ),
        );
      }
      return widget.builder(context, value);
    },
  );
}

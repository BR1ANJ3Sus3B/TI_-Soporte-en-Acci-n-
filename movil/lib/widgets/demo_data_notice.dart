import 'package:flutter/material.dart';
import 'package:ti_soporte_accion/theme/app_colors.dart';

/// Identifica datos de ejemplo para que no parezcan resultados del servidor.
class DemoDataNotice extends StatelessWidget {
  const DemoDataNotice({
    super.key,
    this.message =
        'Datos locales de demostración; '
        'no están sincronizados con el servidor.',
  });

  final String message;

  @override
  Widget build(BuildContext context) => Semantics(
    label: 'Aviso: $message',
    child: Container(
      width: double.infinity,
      padding: const EdgeInsets.all(12),
      decoration: BoxDecoration(
        color: AppColors.warning.withValues(alpha: .08),
        border: Border.all(color: AppColors.warning.withValues(alpha: .45)),
        borderRadius: BorderRadius.circular(AppColors.radiusSmall),
      ),
      child: Row(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          const Icon(
            Icons.info_outline_rounded,
            color: AppColors.warning,
            size: 18,
          ),
          const SizedBox(width: 8),
          Expanded(
            child: Text(
              message,
              style: const TextStyle(
                color: AppColors.textSecondary,
                fontSize: 12,
                height: 1.45,
              ),
            ),
          ),
        ],
      ),
    ),
  );
}

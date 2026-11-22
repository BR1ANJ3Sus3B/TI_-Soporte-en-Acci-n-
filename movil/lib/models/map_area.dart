import 'package:flutter/material.dart';
import 'package:ti_soporte_accion/theme/app_colors.dart';

/// Área de la empresa representada en el mapa.
class MapArea {
  const MapArea({required this.name, required this.icon, required this.color});

  final String name;
  final IconData icon;
  final Color color;

  factory MapArea.fromJson(Map<String, dynamic> json) {
    final name = json['name'] as String;
    final icon = switch ((json['icon'] as String?) ?? '') {
      'network' => Icons.hub_rounded,
      'security' => Icons.shield_rounded,
      'database' => Icons.storage_rounded,
      'code' => Icons.code_rounded,
      _ => Icons.business_rounded,
    };
    return MapArea(name: name, icon: icon, color: AppColors.primaryCyan);
  }
}

import 'package:flutter/material.dart';
import 'package:ti_soporte_accion/models/character.dart';
import 'package:ti_soporte_accion/theme/app_colors.dart';

/// Tarjeta reutilizable para mostrar un personaje importante de la historia.
class CharacterCard extends StatelessWidget {
  const CharacterCard({super.key, required this.character});

  /// Personaje a mostrar.
  final GameCharacter character;

  Color _getColor(CharacterRole type) {
    switch (type) {
      case CharacterRole.player:
        return AppColors.cyan;
      case CharacterRole.mentor:
        return AppColors.primary;
      case CharacterRole.network:
        return AppColors.success;
      case CharacterRole.programming:
        return AppColors.primary;
      case CharacterRole.support:
        return AppColors.cyan;
      case CharacterRole.organization:
        return AppColors.warning;
      case CharacterRole.antagonist:
        return AppColors.danger;
    }
  }

  @override
  Widget build(BuildContext context) {
    final color = _getColor(character.type);
    return Container(
      width: 220,
      margin: const EdgeInsets.only(right: 12),
      padding: const EdgeInsets.all(16),
      decoration: BoxDecoration(
        gradient: LinearGradient(
          begin: Alignment.topLeft,
          end: Alignment.bottomRight,
          colors: <Color>[AppColors.card, const Color(0xFF182235)],
        ),
        borderRadius: BorderRadius.circular(16),
        border: Border.all(color: color.withValues(alpha: 0.6)),
      ),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: <Widget>[
          CircleAvatar(
            backgroundColor: color.withValues(alpha: 0.25),
            radius: 24,
            child: Text(
              character.initials,
              style: TextStyle(
                color: color,
                fontWeight: FontWeight.w700,
                fontSize: 16,
              ),
            ),
          ),
          const SizedBox(height: 12),
          Text(
            character.name,
            style: const TextStyle(
              color: AppColors.textPrimary,
              fontSize: 16,
              fontWeight: FontWeight.w700,
            ),
          ),
          const SizedBox(height: 4),
          Text(
            character.role,
            style: TextStyle(
              color: color,
              fontSize: 12,
              fontWeight: FontWeight.w600,
            ),
          ),
          const SizedBox(height: 8),
          Text(
            character.description,
            style: const TextStyle(
              color: AppColors.textSecondary,
              fontSize: 12,
              height: 1.5,
            ),
            maxLines: 2,
            overflow: TextOverflow.ellipsis,
          ),
        ],
      ),
    );
  }
}

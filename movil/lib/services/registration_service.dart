import 'package:ti_soporte_accion/services/api_client.dart';

class RegistrationService {
  RegistrationService({ApiClient? api}) : _api = api ?? ApiClient();
  final ApiClient _api;
  static bool validEmail(String value) =>
      value.trim().length <= 254 &&
      RegExp(r'^[^\s@]+@[^\s@]+\.[^\s@]+$').hasMatch(value.trim());
  static bool validPassword(String value) =>
      value.length >= 12 &&
      value.length <= 128 &&
      RegExp('[a-z]').hasMatch(value) &&
      RegExp('[A-Z]').hasMatch(value) &&
      RegExp('[0-9]').hasMatch(value) &&
      RegExp(r'[^a-zA-Z0-9\s]').hasMatch(value);
  Future<void> register(String name, String email, String password) async {
    _validate(
      await _api.requestJson(
        'POST',
        '/auth/register',
        requiresAuthentication: false,
        body: {
          'name': name.trim(),
          'email': email.trim().toLowerCase(),
          'password': password,
        },
      ),
    );
  }

  Future<void> resend(String email) async {
    _validate(
      await _api.requestJson(
        'POST',
        '/auth/resend-verification',
        requiresAuthentication: false,
        body: {'email': email.trim().toLowerCase()},
      ),
    );
  }

  void _validate(dynamic result) {
    if (result is! Map || result['status'] != 'verification_pending') {
      throw const ApiException(
        ApiFailure.invalidResponse,
        'No se pudo confirmar la solicitud.',
      );
    }
  }

  void dispose() => _api.close();
}

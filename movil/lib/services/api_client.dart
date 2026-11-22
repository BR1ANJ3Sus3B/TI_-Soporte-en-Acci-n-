import 'dart:async';
import 'dart:convert';

import 'package:flutter/foundation.dart';
import 'package:http/http.dart' as http;
import 'package:ti_soporte_accion/services/auth_service.dart';

enum ApiFailure {
  configuration,
  unauthorized,
  forbidden,
  notFound,
  offline,
  server,
  invalidResponse,
}

class ApiException implements Exception {
  const ApiException(this.failure, this.message, {this.statusCode});
  final ApiFailure failure;
  final String message;
  final int? statusCode;
  @override
  String toString() => message;
}

/// Cliente HTTP autenticado. No contiene URI ni credenciales de MongoDB.
class ApiClient {
  ApiClient({
    http.Client? client,
    AuthService? authService,
    String? baseUrl,
    bool? allowHttp,
  }) : _client = client ?? http.Client(),
       _authService = authService ?? AuthService.instance,
       _configuredBaseUrl =
           baseUrl ?? const String.fromEnvironment('API_BASE_URL'),
       _configuredAllowHttp =
           allowHttp ?? const bool.fromEnvironment('API_ALLOW_HTTP');

  final http.Client _client;
  final AuthService _authService;
  final String _configuredBaseUrl;
  final bool _configuredAllowHttp;

  String get baseUrl => _configuredBaseUrl.trim();
  String? get configurationIssue {
    if (baseUrl.isEmpty) {
      return 'Falta API_BASE_URL; configura el origen y `/api/v1`.';
    }
    final uri = Uri.tryParse(baseUrl);
    if (uri == null || uri.host.isEmpty || uri.hasQuery || uri.hasFragment) {
      return 'API_BASE_URL debe ser una URL absoluta sin query ni fragmento.';
    }
    final secureScheme =
        uri.scheme == 'https' ||
        (uri.scheme == 'http' && kDebugMode && _configuredAllowHttp);
    if (!secureScheme) {
      return 'La API requiere HTTPS; HTTP solo se permite en debug con API_ALLOW_HTTP=true.';
    }
    if (!uri.path.replaceAll(RegExp(r'/$'), '').endsWith('/api/v1')) {
      return 'API_BASE_URL debe terminar en /api/v1.';
    }
    return null;
  }

  bool get isConfigured => configurationIssue == null;

  Future<dynamic> getJson(String path) => requestJson('GET', path);
  Future<dynamic> getPublicJson(String path) =>
      requestJson('GET', path, requiresAuthentication: false);

  Future<dynamic> postJson(String path, Map<String, Object?> body) =>
      requestJson('POST', path, body: body);

  Future<dynamic> patchJson(String path, Map<String, Object?> body) =>
      requestJson('PATCH', path, body: body);

  Future<dynamic> requestJson(
    String method,
    String path, {
    Map<String, Object?>? body,
    bool requiresAuthentication = true,
  }) async {
    final configIssue = configurationIssue;
    if (configIssue != null) {
      throw ApiException(ApiFailure.configuration, configIssue);
    }
    final token = requiresAuthentication ? await _authService.accessToken() : null;
    if (requiresAuthentication && token == null) {
      throw const ApiException(
        ApiFailure.unauthorized,
        'La sesión expiró. Inicia sesión otra vez.',
        statusCode: 401,
      );
    }
    final normalizedBase = baseUrl.endsWith('/') ? baseUrl : '$baseUrl/';
    final normalizedPath = path.startsWith('/') ? path.substring(1) : path;
    final uri = Uri.parse(normalizedBase).resolve(normalizedPath);
    try {
      final headers = <String, String>{
        'Accept': 'application/json',
      };
      if (token != null) headers['Authorization'] = 'Bearer $token';
      if (body != null) headers['Content-Type'] = 'application/json';
      final request = http.Request(method, uri)..headers.addAll(headers);
      if (body != null) request.body = jsonEncode(body);
      final streamed = await _client
          .send(request)
          .timeout(const Duration(seconds: 12));
      final response = await http.Response.fromStream(streamed)
          .timeout(const Duration(seconds: 12));
      if (response.statusCode == 401) {
        await _authService.expireSession();
        throw const ApiException(
          ApiFailure.unauthorized,
          'El servidor rechazó la sesión. Vuelve a iniciar sesión.',
          statusCode: 401,
        );
      }
      if (response.statusCode == 403) {
        throw const ApiException(
          ApiFailure.forbidden,
          'Tu cuenta no tiene permiso para consultar este recurso.',
          statusCode: 403,
        );
      }
      if (response.statusCode == 404) {
        throw const ApiException(
          ApiFailure.notFound,
          'El recurso ya no está disponible.',
          statusCode: 404,
        );
      }
      if (response.statusCode < 200 || response.statusCode >= 300) {
        final serverMessage = _readError(response.body);
        throw ApiException(
          ApiFailure.server,
          serverMessage ?? 'El servidor respondió ${response.statusCode}.',
          statusCode: response.statusCode,
        );
      }
      if (response.bodyBytes.isEmpty) return null;
      try {
        return jsonDecode(utf8.decode(response.bodyBytes));
      } on FormatException {
        throw const ApiException(
          ApiFailure.invalidResponse,
          'El servidor devolvió una respuesta JSON inválida.',
        );
      }
    } on ApiException {
      rethrow;
    } on TimeoutException {
      throw const ApiException(
        ApiFailure.offline,
        'La API tardó demasiado. Comprueba la conexión e inténtalo de nuevo.',
      );
    } on http.ClientException {
      throw const ApiException(
        ApiFailure.offline,
        'No se pudo conectar a la API. Comprueba API_BASE_URL y la red.',
      );
    } catch (_) {
      throw const ApiException(
        ApiFailure.offline,
        'La API no está disponible. Comprueba la conexión e inténtalo de nuevo.',
      );
    }
  }

  String? _readError(String body) {
    try {
      final decoded = jsonDecode(body);
      if (decoded is Map<String, dynamic> && decoded['error'] is Map) {
        final error = decoded['error'] as Map;
        final message = error['message'];
        if (message is String && message.isNotEmpty) return message;
      }
    } catch (_) {
      // No se propaga contenido arbitrario del servidor a la interfaz.
    }
    return null;
  }

  void close() => _client.close();
}

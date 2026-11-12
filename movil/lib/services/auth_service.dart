import 'dart:async';
import 'dart:convert';

import 'package:flutter/foundation.dart';
import 'package:flutter_appauth/flutter_appauth.dart';

/// Sesión móvil OIDC. AppAuth gestiona PKCE, state y el navegador del sistema.
/// Los tokens viven únicamente en memoria; la API deberá validarlos por su cuenta.
class AuthService extends ChangeNotifier {
  AuthService({
    FlutterAppAuth? appAuth,
    this.issuer = const String.fromEnvironment('KEYCLOAK_ISSUER'),
    this.clientId = const String.fromEnvironment(
      'KEYCLOAK_CLIENT_ID',
      defaultValue: 'ti-soporte-flutter',
    ),
    this.allowInsecure = const bool.fromEnvironment('KEYCLOAK_ALLOW_HTTP'),
  }) : _appAuth = appAuth ?? const FlutterAppAuth();

  static final instance = AuthService();
  static const redirectUri = 'com.tisoporte.accion:/oauthredirect';
  static const _scopes = <String>['openid', 'profile', 'email'];
  final FlutterAppAuth _appAuth;
  final String issuer;
  final String clientId;
  final bool allowInsecure;
  String? _accessToken;
  String? _refreshToken;
  String? _idToken;
  DateTime? _expiresAt;
  Timer? _refreshTimer;
  Future<String?>? _refreshing;
  int _generation = 0;
  Map<String, dynamic> _identity = <String, dynamic>{};

  bool get isConfigured {
    final uri = Uri.tryParse(issuer);
    return clientId.isNotEmpty &&
        uri != null &&
        uri.host.isNotEmpty &&
        (uri.scheme == 'https' || (allowInsecure && uri.scheme == 'http'));
  }

  bool get isSupported =>
      !kIsWeb &&
      (defaultTargetPlatform == TargetPlatform.android ||
          defaultTargetPlatform == TargetPlatform.iOS);
  bool get isAuthenticated =>
      _accessToken != null &&
      _expiresAt != null &&
      _expiresAt!.isAfter(DateTime.now());
  String get displayName =>
      (_identity['name'] ?? _identity['preferred_username'] ?? 'Usuario')
          .toString();
  String get email => (_identity['email'] ?? '').toString();
  String get discoveryUrl =>
      '${issuer.replaceAll(RegExp(r'/$'), '')}/.well-known/openid-configuration';

  Future<bool> login() async {
    if (!isSupported) {
      throw StateError('El acceso está disponible en Android e iOS.');
    }
    if (!isConfigured) {
      throw StateError('Configura KEYCLOAK_ISSUER y KEYCLOAK_CLIENT_ID.');
    }
    final generation = ++_generation;
    try {
      final response = await _appAuth.authorizeAndExchangeCode(
        AuthorizationTokenRequest(
          clientId,
          redirectUri,
          discoveryUrl: discoveryUrl,
          scopes: _scopes,
          allowInsecureConnections: allowInsecure,
          promptValues: const <String>['select_account'],
        ),
      );
      if (generation != _generation) return false;
      _accept(response);
      return true;
    } on FlutterAppAuthUserCancelledException {
      return false;
    }
  }

  void _accept(TokenResponse response) {
    final expiry = response.accessTokenExpirationDateTime;
    if (response.accessToken == null ||
        response.accessToken!.isEmpty ||
        expiry == null ||
        !expiry.isAfter(DateTime.now())) {
      throw StateError('El servidor no devolvió una sesión válida.');
    }
    _accessToken = response.accessToken;
    _refreshToken = response.refreshToken ?? _refreshToken;
    _idToken = response.idToken ?? _idToken;
    _expiresAt = expiry;
    // Estos claims solo se usan para presentación, nunca para autorizar una API.
    try {
      _identity = jsonDecode(
        utf8.decode(
          base64Url.decode(base64Url.normalize(_idToken!.split('.')[1])),
        ),
      ) as Map<String, dynamic>;
    } catch (_) {
      _identity = <String, dynamic>{};
    }
    _refreshTimer?.cancel();
    final remaining = expiry.difference(DateTime.now());
    final delay = remaining > const Duration(seconds: 60)
        ? remaining - const Duration(seconds: 30)
        : remaining;
    _refreshTimer = Timer(delay, () {
      unawaited(accessToken());
    });
    notifyListeners();
  }

  /// Usar antes de cada llamada futura a la API. Nunca retorna un token vencido.
  Future<String?> accessToken() async {
    if (_accessToken == null) return null;
    if (_expiresAt!.isAfter(DateTime.now().add(const Duration(seconds: 45)))) {
      return _accessToken;
    }
    return _refreshing ??= _refresh().whenComplete(() => _refreshing = null);
  }

  Future<String?> _refresh() async {
    final generation = _generation;
    if (_refreshToken == null) {
      _clear();
      return null;
    }
    try {
      final response = await _appAuth.token(
        TokenRequest(
          clientId,
          redirectUri,
          discoveryUrl: discoveryUrl,
          refreshToken: _refreshToken,
          scopes: _scopes,
          allowInsecureConnections: allowInsecure,
        ),
      );
      if (generation != _generation) return null;
      _accept(response);
      return _accessToken;
    } catch (_) {
      if (generation == _generation) _clear();
      return null;
    }
  }

  Future<void> logout() async {
    final idToken = _idToken;
    _clear();
    if (idToken == null) return;
    await _appAuth.endSession(
      EndSessionRequest(
        idTokenHint: idToken,
        postLogoutRedirectUrl: redirectUri,
        discoveryUrl: discoveryUrl,
        allowInsecureConnections: allowInsecure,
      ),
    );
  }

  void _clear() {
    _generation++;
    _refreshTimer?.cancel();
    _accessToken = _refreshToken = _idToken = null;
    _expiresAt = null;
    _identity = <String, dynamic>{};
    notifyListeners();
  }

  @override
  void dispose() {
    _refreshTimer?.cancel();
    super.dispose();
  }
}

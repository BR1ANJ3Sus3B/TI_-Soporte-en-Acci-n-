import 'dart:async';
import 'dart:convert';

import 'package:flutter/foundation.dart';
import 'package:flutter_appauth/flutter_appauth.dart';
import 'package:flutter_secure_storage/flutter_secure_storage.dart';
import 'package:ti_soporte_accion/config/app_build_config.dart';

/// Persistencia de la sesión a través del almacenamiento seguro del dispositivo.
abstract interface class AuthTokenStore {
  Future<String?> read();
  Future<void> write(String value);
  Future<void> clear();
}

/// Un invitado usa únicamente datos locales; nunca recibe un token OIDC.
enum SessionState { initializing, guest, authenticated, expired, signedOut }

class PlatformAuthTokenStore implements AuthTokenStore {
  PlatformAuthTokenStore([FlutterSecureStorage? storage])
    : _storage = storage ?? const FlutterSecureStorage();

  static const _key = 'ti_soporte.oidc.session.v1';
  final FlutterSecureStorage _storage;

  @override
  Future<String?> read() => _storage.read(key: _key);

  @override
  Future<void> write(String value) => _storage.write(key: _key, value: value);

  @override
  Future<void> clear() => _storage.delete(key: _key);
}

/// Sesión OIDC. AppAuth realiza Code + PKCE en el navegador del sistema.
/// Los tokens se guardan cifrados por Keychain (iOS) / Keystore (Android).
class AuthService extends ChangeNotifier {
  AuthService({
    FlutterAppAuth? appAuth,
    AuthTokenStore? tokenStore,
    this.issuer = AppBuildConfig.keycloakIssuer,
    this.clientId = AppBuildConfig.keycloakClientId,
    this.allowHttpInDebug = AppBuildConfig.keycloakAllowHttp,
  }) : _appAuth = appAuth ?? const FlutterAppAuth(),
       _tokenStore = tokenStore ?? PlatformAuthTokenStore();

  static final instance = AuthService();
  static const redirectUri = 'com.tisoporte.accion:/oauthredirect';
  // Discovery advertises this API scope; request its audience mapper as well.
  static const _scopes = <String>['openid', 'ti-soporte-api'];
  final FlutterAppAuth _appAuth;
  final AuthTokenStore _tokenStore;
  final String issuer;
  final String clientId;
  final bool allowHttpInDebug;
  String? _accessToken;
  String? _refreshToken;
  String? _idToken;
  DateTime? _expiresAt;
  Timer? _refreshTimer;
  Future<String?>? _refreshing;
  Future<void>? _restoreFuture;
  int _generation = 0;
  bool _initialized = false;
  bool _guest = false;
  SessionState _sessionState = SessionState.initializing;
  Map<String, dynamic> _identity = <String, dynamic>{};
  List<String> _roles = <String>[];

  bool get isInitialized => _initialized;
  bool get isGuest => _guest;
  SessionState get sessionState => _sessionState;
  bool get allowInsecure => kDebugMode && allowHttpInDebug;

  String? get configurationIssue {
    if (!isSupported) {
      return 'El acceso con Keycloak está disponible en Android e iOS.';
    }
    if (issuer.trim().isEmpty) {
      return 'Falta KEYCLOAK_ISSUER. Flutter no lee .env automáticamente; '
          'edita keycloak.local.json con el issuer real y ejecuta '
          'flutter run --dart-define-from-file=keycloak.local.json.';
    }
    if (issuer != issuer.trim()) {
      return 'KEYCLOAK_ISSUER contiene espacios al inicio o al final.';
    }
    final uri = Uri.tryParse(issuer);
    if (uri == null ||
        uri.host.isEmpty ||
        (uri.scheme != 'https' && uri.scheme != 'http')) {
      return 'KEYCLOAK_ISSUER debe ser una URL absoluta https://.../realms/<realm> '
          'copiada del issuer del servidor.';
    }
    if (uri.path.endsWith('/.well-known/openid-configuration')) {
      return 'KEYCLOAK_ISSUER debe ser el issuer del realm, sin '
          '/.well-known/openid-configuration.';
    }
    if (uri.scheme == 'http' && !allowInsecure) {
      return 'HTTP solo está permitido en compilaciones debug si '
          'KEYCLOAK_ALLOW_HTTP=true. Para producción usa HTTPS.';
    }
    if (clientId.trim().isEmpty) {
      return 'Falta KEYCLOAK_CLIENT_ID. Indica el Client ID público confirmado '
          'en Keycloak (la documentación del proyecto espera ti-soporte-flutter) '
          'y vuelve a ejecutar con --dart-define-from-file=keycloak.local.json.';
    }
    return null;
  }

  bool get isConfigured => configurationIssue == null;

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
  List<String> get roles => List<String>.unmodifiable(_roles);
  String get roleLabel {
    if (_roles.contains('administrador')) return 'Administrador';
    if (_roles.contains('docente')) return 'Docente';
    if (_roles.contains('jugador') || _roles.contains('estudiante')) {
      return 'Jugador';
    }
    return 'Sin rol asignado';
  }

  String get discoveryUrl =>
      '${issuer.replaceAll(RegExp(r'/$'), '')}/.well-known/openid-configuration';

  /// Restaura una sesión segura y renueva tokens vencidos antes de habilitar rutas.
  Future<void> restoreSession() => _restoreFuture ??= _restore();

  Future<void> _restore() async {
    try {
      // Algunos runners de escritorio no registran el canal seguro. No deben
      // bloquear la bienvenida ni el contenido público por ello.
      final raw = await _tokenStore.read().timeout(
        const Duration(milliseconds: 250),
        onTimeout: () => null,
      );
      if (raw == null) return;
      final record = jsonDecode(raw) as Map<String, dynamic>;
      if (record['guest'] == true) {
        _guest = true;
        _sessionState = SessionState.guest;
        return;
      }
      if (!isSupported || !isConfigured) return;
      _accessToken = record['accessToken'] as String?;
      _refreshToken = record['refreshToken'] as String?;
      _idToken = record['idToken'] as String?;
      _expiresAt = DateTime.tryParse(record['expiresAt'] as String? ?? '');
      if (_accessToken == null || _expiresAt == null) {
        await _clearSession();
        return;
      }
      _readIdentity();
      if (_expiresAt!.isAfter(
        DateTime.now().add(const Duration(seconds: 45)),
      )) {
        _sessionState = SessionState.authenticated;
        _scheduleRefresh();
        notifyListeners();
        return;
      }
      if (await _refresh() == null) await _clearSession();
    } catch (_) {
      _clearMemory();
      try {
        await _tokenStore.clear();
      } catch (_) {
        // Storage plugin unavailable (for example, a non-mobile test runner).
      }
    } finally {
      _initialized = true;
      if (!_guest &&
          !isAuthenticated &&
          _sessionState == SessionState.initializing) {
        _sessionState = SessionState.signedOut;
      }
      notifyListeners();
    }
  }

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
      await _accept(response);
      _initialized = true;
      return true;
    } on FlutterAppAuthUserCancelledException {
      return false;
    }
  }

  /// Persiste la elección de invitado sin crear ni simular una sesión remota.
  Future<void> continueAsGuest() async {
    _clearMemory();
    _guest = true;
    _sessionState = SessionState.guest;
    await _tokenStore.write(jsonEncode(<String, Object?>{'guest': true}));
    notifyListeners();
  }

  Future<void> _accept(TokenResponse response) async {
    final expiry = response.accessTokenExpirationDateTime;
    final accessToken = response.accessToken;
    if (accessToken == null ||
        accessToken.isEmpty ||
        expiry == null ||
        !expiry.isAfter(DateTime.now())) {
      throw StateError('El servidor no devolvió una sesión válida.');
    }
    final refreshToken = response.refreshToken ?? _refreshToken;
    final idToken = response.idToken ?? _idToken;
    final identity = _decodeIdentity(idToken);
    final roles = _readRoles(identity);
    await _tokenStore.write(
      jsonEncode(<String, Object?>{
        'accessToken': accessToken,
        'refreshToken': refreshToken,
        'idToken': idToken,
        'expiresAt': expiry.toIso8601String(),
      }),
    );
    _accessToken = accessToken;
    _refreshToken = refreshToken;
    _idToken = idToken;
    _expiresAt = expiry;
    _identity = identity;
    _roles = roles;
    _initialized = true;
    _guest = false;
    _sessionState = SessionState.authenticated;
    _scheduleRefresh();
    notifyListeners();
  }

  Map<String, dynamic> _decodeIdentity(String? token) {
    try {
      if (token == null) return <String, dynamic>{};
      final parts = token.split('.');
      if (parts.length != 3) return <String, dynamic>{};
      return jsonDecode(
        utf8.decode(base64Url.decode(base64Url.normalize(parts[1]))),
      ) as Map<String, dynamic>;
    } catch (_) {
      return <String, dynamic>{};
    }
  }

  List<String> _readRoles(Map<String, dynamic> claims) {
    final roles = <String>{};
    final realmAccess = claims['realm_access'];
    if (realmAccess is Map && realmAccess['roles'] is List) {
      roles.addAll((realmAccess['roles'] as List).whereType<String>());
    }
    final resourceAccess = claims['resource_access'];
    if (resourceAccess is Map) {
      final client = resourceAccess[clientId];
      if (client is Map && client['roles'] is List) {
        roles.addAll((client['roles'] as List).whereType<String>());
      }
    }
    return roles.toList()..sort();
  }

  void _readIdentity() {
    _identity = _decodeIdentity(_idToken);
    _roles = _readRoles(_identity);
  }

  void _scheduleRefresh() {
    _refreshTimer?.cancel();
    final remaining = _expiresAt!.difference(DateTime.now());
    final delay = remaining > const Duration(seconds: 60)
        ? remaining - const Duration(seconds: 30)
        : remaining;
    _refreshTimer = Timer(delay, () {
      unawaited(accessToken());
    });
  }

  /// Usar antes de una futura llamada a API; nunca entrega tokens vencidos.
  Future<String?> accessToken() async {
    if (_accessToken == null) return null;
    if (_expiresAt!.isAfter(DateTime.now().add(const Duration(seconds: 45)))) {
      return _accessToken;
    }
    return _refreshing ??= _refresh().whenComplete(() => _refreshing = null);
  }

  Future<String?> _refresh() async {
    final generation = _generation;
    if (_refreshToken == null || !isConfigured) {
      try {
        await _clearSession();
      } catch (_) {
        // The in-memory session is already removed even if secure storage fails.
      }
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
      await _accept(response);
      return _accessToken;
    } catch (_) {
      if (generation == _generation) {
        try {
          await _clearSession();
        } catch (_) {
          // Never leave an expired credential usable in memory.
        }
      }
      return null;
    }
  }

  Future<void> logout() async {
    final idToken = _idToken;
    await _clearSession(state: SessionState.signedOut);
    if (idToken == null || !isConfigured || !isSupported) return;
    await _appAuth.endSession(
      EndSessionRequest(
        idTokenHint: idToken,
        postLogoutRedirectUrl: redirectUri,
        discoveryUrl: discoveryUrl,
        allowInsecureConnections: allowInsecure,
      ),
    );
  }

  /// Limpia la sesión local si la API responde 401; no abre el navegador.
  Future<void> expireSession() => _clearSession();

  /// Ignore late responses from a previous account/session or a guest request.
  Future<void> expireSessionForToken(String rejectedToken) async {
    if (!_guest && _accessToken == rejectedToken) await _clearSession();
  }

  Future<void> _clearSession({
    SessionState state = SessionState.expired,
  }) async {
    _clearMemory();
    _sessionState = state;
    try {
      await _tokenStore.clear();
    } finally {
      notifyListeners();
    }
  }

  void _clearMemory() {
    _generation++;
    _refreshTimer?.cancel();
    _accessToken = _refreshToken = _idToken = null;
    _expiresAt = null;
    _identity = <String, dynamic>{};
    _roles = <String>[];
    _initialized = true;
    _guest = false;
    _sessionState = SessionState.expired;
  }

  @override
  void dispose() {
    _refreshTimer?.cancel();
    super.dispose();
  }
}

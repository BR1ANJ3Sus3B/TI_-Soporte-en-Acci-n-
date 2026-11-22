param([ValidateSet('android','web')][string]$Target = 'android')
$ErrorActionPreference = 'Stop'
if (-not (Test-Path movil/keycloak.local.json)) { Copy-Item movil/keycloak.local.json.example movil/keycloak.local.json }
Push-Location movil
try {
  flutter pub get
  if ($Target -eq 'web') { flutter run -d chrome --dart-define-from-file=keycloak.local.json }
  else { flutter run --dart-define-from-file=keycloak.local.json }
} finally { Pop-Location }

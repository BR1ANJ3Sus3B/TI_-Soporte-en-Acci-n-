$ErrorActionPreference = 'Stop'
$RepositoryRoot = (Resolve-Path (Join-Path $PSScriptRoot '..')).Path
if ((git -C $RepositoryRoot branch --show-current) -ne 'feature/mobile-flutter') {
  throw "Expected feature/mobile-flutter at '$RepositoryRoot'."
}
if (-not (Test-Path -LiteralPath (Join-Path $RepositoryRoot 'infra/keycloak/.env')) -or
    -not (Test-Path -LiteralPath (Join-Path $RepositoryRoot 'infra/api/.env'))) {
  throw 'Local .env files are missing; their contents will not be shown or changed.'
}
docker info *> $null
if ($LASTEXITCODE -ne 0) { throw 'Docker Desktop is unavailable to the current user.' }

docker compose -f (Join-Path $RepositoryRoot 'infra/keycloak/compose.yaml') ps
if ($LASTEXITCODE -ne 0) { throw 'No se pudo consultar el estado de Keycloak.' }
docker compose -f (Join-Path $RepositoryRoot 'infra/api/compose.yaml') ps
if ($LASTEXITCODE -ne 0) { throw 'No se pudo consultar el estado de API/MongoDB.' }

$FlutterConfigPath = Join-Path $RepositoryRoot 'movil/keycloak.local.json'
if (-not (Test-Path -LiteralPath $FlutterConfigPath -PathType Leaf)) {
  throw "Falta el archivo local de defines: $FlutterConfigPath"
}
try {
  $FlutterConfig = Get-Content -LiteralPath $FlutterConfigPath -Raw | ConvertFrom-Json
} catch {
  throw 'keycloak.local.json is not valid JSON.'
}
if ([string]::IsNullOrWhiteSpace([string]$FlutterConfig.KEYCLOAK_ISSUER) -or
    [string]::IsNullOrWhiteSpace([string]$FlutterConfig.KEYCLOAK_CLIENT_ID)) {
  throw 'KEYCLOAK_ISSUER y KEYCLOAK_CLIENT_ID deben estar configurados.'
}
try {
  $Discovery = Invoke-RestMethod "$($FlutterConfig.KEYCLOAK_ISSUER.TrimEnd('/'))/.well-known/openid-configuration" -TimeoutSec 10
} catch {
  throw 'Keycloak OIDC Discovery did not respond for the configured issuer.'
}
if ($Discovery.issuer -cne $FlutterConfig.KEYCLOAK_ISSUER) {
  throw 'The Discovery issuer does not match keycloak.local.json.'
}
Write-Host 'Keycloak OIDC Discovery: reachable and matches Flutter config.'

$ApiUri = [Uri]$FlutterConfig.API_BASE_URL
try {
  $ApiHealth = Invoke-RestMethod "$($ApiUri.GetLeftPart([UriPartial]::Authority))/health/ready" -TimeoutSec 10
} catch {
  throw 'API /health/ready did not respond.'
}
if ($ApiHealth.status -ne 'ready' -or -not $ApiHealth.keycloakConfigured) {
  throw 'API is not ready or Keycloak is not configured.'
}
Write-Host 'API: ready; MongoDB connected; OIDC configuration present.'

if (Get-Command adb -ErrorAction SilentlyContinue) {
  adb devices -l
  if ($LASTEXITCODE -ne 0) { throw 'Could not query adb.' }
}

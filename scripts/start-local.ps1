$ErrorActionPreference = 'Stop'
$RepositoryRoot = (Resolve-Path (Join-Path $PSScriptRoot '..')).Path
if ((git -C $RepositoryRoot branch --show-current) -ne 'feature/mobile-flutter') {
  throw "Expected feature/mobile-flutter at '$RepositoryRoot'."
}
if (-not (Get-Command docker -ErrorAction SilentlyContinue)) {
  throw 'Docker CLI is not installed.'
}
docker info *> $null
if ($LASTEXITCODE -ne 0) {
  throw 'Docker Desktop is not running.'
}

$KeycloakEnv = Join-Path $RepositoryRoot 'infra/keycloak/.env'
$ApiEnv = Join-Path $RepositoryRoot 'infra/api/.env'
if (-not (Test-Path -LiteralPath $KeycloakEnv) -or -not (Test-Path -LiteralPath $ApiEnv)) {
  throw 'Local .env files are missing. This script does not create/replace secrets or provision configuration.'
}

# `docker compose start` solo arranca contenedores que ya existen; no crea,
# rebuilds images or replaces services or volumes.
docker compose -f (Join-Path $RepositoryRoot 'infra/keycloak/compose.yaml') --env-file $KeycloakEnv start
if ($LASTEXITCODE -ne 0) { throw 'Could not start existing Keycloak containers.' }
docker compose -f (Join-Path $RepositoryRoot 'infra/api/compose.yaml') --env-file $ApiEnv start
if ($LASTEXITCODE -ne 0) { throw 'Could not start existing API/MongoDB containers.' }

Write-Host 'Started existing containers only; no service was created or rebuilt.'
Write-Host 'Run scripts/check-services.ps1, then scripts/run-flutter.ps1.'

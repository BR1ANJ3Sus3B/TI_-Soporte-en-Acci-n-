param(
  [Parameter(Mandatory = $false)][string]$KeycloakAdminPassword,
  [Parameter(Mandatory = $false)][string]$MetricsBearerToken
)

$ErrorActionPreference = 'Stop'
if (-not (Get-Command docker -ErrorAction SilentlyContinue)) { throw 'Docker CLI no está instalado.' }
docker info *> $null
if ($LASTEXITCODE -ne 0) { throw 'Docker Desktop no está en ejecución.' }

if (-not (Test-Path infra/keycloak/.env)) {
  if ([string]::IsNullOrWhiteSpace($KeycloakAdminPassword)) { throw 'Indica -KeycloakAdminPassword para crear infra/keycloak/.env.' }
  "KC_ADMIN_PASSWORD=$KeycloakAdminPassword" | Set-Content infra/keycloak/.env -NoNewline
}
if (-not (Test-Path infra/api/.env)) {
  if ([string]::IsNullOrWhiteSpace($MetricsBearerToken)) { throw 'Indica -MetricsBearerToken para crear infra/api/.env.' }
  Copy-Item infra/api/.env.example infra/api/.env
  Add-Content infra/api/.env "`nMETRICS_BEARER_TOKEN=$MetricsBearerToken"
}

docker network inspect ti-soporte-monitoring_default *> $null
if ($LASTEXITCODE -ne 0) { docker network create ti-soporte-monitoring_default | Out-Null }
Push-Location infra/keycloak; docker compose up -d; Pop-Location
Push-Location infra/api; docker compose up -d; Pop-Location
Write-Host 'Servicios iniciados. Espera healthchecks y ejecuta scripts/run-flutter.ps1.'

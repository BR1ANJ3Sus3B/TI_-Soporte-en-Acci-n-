$ErrorActionPreference = 'Stop'
$RepositoryRoot = (Resolve-Path (Join-Path $PSScriptRoot '..')).Path
Set-Location $RepositoryRoot
if (Test-Path infra/api/.env) { docker compose -f infra/api/compose.yaml --env-file infra/api/.env stop }
if (Test-Path infra/keycloak/.env) { docker compose -f infra/keycloak/compose.yaml --env-file infra/keycloak/.env stop }
Write-Host 'Servicios detenidos; no se eliminaron contenedores, redes ni volúmenes.'

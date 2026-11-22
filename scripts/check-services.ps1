$ErrorActionPreference = 'Stop'
$RepositoryRoot = (Resolve-Path (Join-Path $PSScriptRoot '..')).Path
Set-Location $RepositoryRoot
if (-not (Test-Path infra/keycloak/.env) -or -not (Test-Path infra/api/.env)) { throw 'Faltan archivos .env locales; ejecuta start-local.ps1.' }
docker info *> $null
if ($LASTEXITCODE -ne 0) { throw 'Docker Desktop no está disponible para el usuario actual.' }
docker compose -f infra/keycloak/compose.yaml --env-file infra/keycloak/.env ps
docker compose -f infra/api/compose.yaml --env-file infra/api/.env ps
try { (Invoke-RestMethod 'http://localhost:8080/realms/ti-soporte/.well-known/openid-configuration').issuer } catch { throw 'Keycloak discovery no respondió en http://localhost:8080.' }
try { (Invoke-RestMethod 'http://localhost:3000/health/ready').status } catch { throw 'API health no respondió en http://localhost:3000.' }

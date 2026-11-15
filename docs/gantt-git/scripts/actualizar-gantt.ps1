[CmdletBinding()]
param(
  [string]$RepositoryPath = (Resolve-Path (Join-Path $PSScriptRoot '../../..')).Path,
  [string]$OutputPath = (Join-Path $PSScriptRoot '../datos/gantt-git.json'),
  [string]$JavaScriptOutputPath = (Join-Path $PSScriptRoot '../datos/gantt-git.js')
)

$ErrorActionPreference = 'Stop'
$utf8 = [System.Text.UTF8Encoding]::new($false)
[Console]::OutputEncoding = $utf8
$OutputEncoding = $utf8
$acuteO = [char]0x00F3
$acuteI = [char]0x00ED
Set-Location $RepositoryPath

function Invoke-GitText([string[]]$Arguments) {
  # Algunos assets Unity tienen atributos personalizados no disponibles en esta
  # instalación de Git. Son avisos de atributos, no fallos del historial.
  $nativePreference = $PSNativeCommandUseErrorActionPreference
  $errorPreference = $ErrorActionPreference
  $PSNativeCommandUseErrorActionPreference = $false
  $ErrorActionPreference = 'Continue'
  $result = & git @Arguments 2>$null
  $ErrorActionPreference = $errorPreference
  $PSNativeCommandUseErrorActionPreference = $nativePreference
  if ($LASTEXITCODE -ne 0) { throw "git $($Arguments -join ' ') failed." }
  return ($result -join "`n")
}

function Get-Module([string[]]$Paths, [string]$Subject) {
  $first = @($Paths | Where-Object { $_ } | ForEach-Object { $_.Trim('"') }) | Select-Object -First 1
  if ($first) {
    $top = ($first -split '/')[0]
    $map = @{ videojuego = 'Videojuego Unity'; movil = "Aplicaci${acuteO}n m${acuteO}vil"; pwa = 'PWA'; web = 'Web'; api = 'API'; database = 'Base de datos'; infra = 'Infraestructura'; Docker = 'Contenedores'; docs = "Documentaci${acuteO}n"; Logs = 'Monitoreo' }
    if ($map.ContainsKey($top)) { return $map[$top] }
    return "Ra${acuteI}z: $top"
  }
  if ($Subject -match 'docs') { return "Documentaci${acuteO}n" }
  return 'Repositorio'
}

function Get-Phase([string]$Subject, [string]$Module) {
  $text = $Subject.ToLowerInvariant()
  if ($text -match 'fix|repair|repar|corrig|bug') { return "Correcci${acuteO}n" }
  if ($text -match 'test|prueba') { return 'Pruebas' }
  if ($text -match 'docs|document') { return "Documentaci${acuteO}n" }
  if ($text -match 'chore|config|setup|estructura|init') { return "Configuraci${acuteO}n" }
  if ($text -match 'feat|agrega|integra|implement|crear|anade') { return "Implementaci${acuteO}n" }
  if ($Module -eq "Documentaci${acuteO}n") { return "Documentaci${acuteO}n" }
  return 'Cambio registrado'
}

function Get-ActivityClassification([string[]]$Paths, [string]$Subject, [string]$Module) {
  $evidence = (($Paths -join "`n") + "`n" + $Subject).ToLowerInvariant()
  if ($evidence -match 'animacion|animation|alexrunn|idle_|walk_|run_') { return [ordered]@{ key = 'unity-animaciones'; label = 'Animaciones y recursos Unity'; module = 'Videojuego Unity' } }
  if ($evidence -match 'mission|mision') { return [ordered]@{ key = 'unity-misiones'; label = 'Sistemas de misiones'; module = 'Videojuego Unity' } }
  if ($evidence -match 'novatech|nivel|level|scene|escena|mapa') { return [ordered]@{ key = 'unity-escenarios'; label = 'Escenarios y niveles'; module = 'Videojuego Unity' } }
  if ($evidence -match 'mainmenu|menu|/ui/|gameui|interfaz') { return [ordered]@{ key = 'unity-interfaces'; label = 'Interfaces del videojuego'; module = 'Videojuego Unity' } }
  if ($evidence -match 'player|character|personaje|npc') { return [ordered]@{ key = 'unity-personajes'; label = 'Personajes y movimiento'; module = 'Videojuego Unity' } }
  if ($evidence -match '^movil/|\(movil\)|flutter') { return [ordered]@{ key = 'movil'; label = 'Aplicacion movil'; module = 'Aplicacion movil' } }
  if ($evidence -match '^pwa/|\(pwa\)|keycloak') { return [ordered]@{ key = 'pwa-acceso'; label = 'PWA y autenticacion'; module = 'PWA' } }
  if ($evidence -match 'grafana|monitoreo|telemetria') { return [ordered]@{ key = 'monitoreo'; label = 'Monitoreo y telemetria'; module = 'Monitoreo' } }
  if ($evidence -match 'docker|infra/|api/') { return [ordered]@{ key = 'plataforma'; label = 'Infraestructura y servicios'; module = 'Infraestructura' } }
  if ($evidence -match '^docs/|^doc/|\bdocs:|documentacion|canvas|wireframe|navegacion') { return [ordered]@{ key = 'documentacion'; label = 'Documentacion y diseno'; module = 'Documentacion' } }
  if ($evidence -match 'estructura|initial|\bmain\b|\.gitignore|config|chore') { return [ordered]@{ key = 'configuracion'; label = 'Configuracion del proyecto'; module = 'Repositorio' } }
  return [ordered]@{ key = ('modulo-' + ($Module -replace '[^a-zA-Z0-9]+','-').Trim('-').ToLowerInvariant()); label = $Module; module = $Module }
}

$remoteUrl = (& git config --get remote.origin.url 2>$null | Select-Object -First 1)
$baseUrl = $null
if ($remoteUrl -match 'github\.com[/:](.+?)(?:\.git)?$') { $baseUrl = "https://github.com/$($Matches[1].TrimEnd('.git'))" }
elseif ($remoteUrl -match '^https?://.+') { $baseUrl = $remoteUrl.TrimEnd('/') }

$refs = @{}
$refLines = Invoke-GitText @('for-each-ref', '--format=%(refname:short)%x1f%(objectname)', 'refs/heads', 'refs/remotes')
foreach ($line in ($refLines -split "`n")) {
  $parts = $line -split [char]0x1f, 2
  if ($parts.Count -eq 2 -and $parts[0] -and $parts[0] -notmatch '/HEAD$') {
    if (-not $refs.ContainsKey($parts[1])) { $refs[$parts[1]] = @() }
    $refs[$parts[1]] += $parts[0]
  }
}

$format = '%H%x1f%P%x1f%an%x1f%ae%x1f%aI%x1f%cI%x1f%s'
$raw = Invoke-GitText @('log', '--branches', '--remotes', '--no-notes', '--date=iso-strict', "--format=$format", '--name-status')
# PowerShell normaliza el separador de registros de control de Git. En vez de
# depender de él, las cabeceras se reconocen por el hash y sus seis campos.
$headerPattern = '^[0-9a-f]{40}' + [char]0x1f
$entries = [System.Collections.Generic.List[object]]::new()
$currentEntry = [System.Collections.Generic.List[string]]::new()
foreach ($line in ($raw -split "`n")) {
  if ($line -match $headerPattern) {
    if ($currentEntry.Count) { $entries.Add(@($currentEntry)) }
    $currentEntry = [System.Collections.Generic.List[string]]::new()
  }
  if ($currentEntry.Count -or $line -match $headerPattern) { $currentEntry.Add($line) }
}
if ($currentEntry.Count) { $entries.Add(@($currentEntry)) }
$commits = [System.Collections.Generic.List[object]]::new()
Write-Verbose "Entradas Git detectadas: $(@($entries).Count)"

foreach ($entry in $entries) {
  $lines = @($entry | Where-Object { $_ -ne '' })
  if ($lines.Count -eq 0) { continue }
  $meta = $lines[0] -split [char]0x1f, 7
  if ($meta.Count -lt 7) { Write-Verbose "Entrada omitida sin cabecera Git: $($lines[0])"; continue }
  $files = [System.Collections.Generic.List[object]]::new()
  foreach ($fileLine in ($lines | Select-Object -Skip 1)) {
    if (-not $fileLine.Trim()) { continue }
    $columns = $fileLine -split "`t"
    $status = $columns[0]
    $path = $columns[$columns.Count - 1]
    $oldPath = if ($columns.Count -gt 2) { $columns[1] } else { $null }
    $files.Add([ordered]@{ status = $status; path = $path; oldPath = $oldPath })
  }
  $paths = @($files | ForEach-Object { $_.path })
  $module = Get-Module $paths $meta[6]
  $phase = Get-Phase $meta[6] $module
  $activity = Get-ActivityClassification $paths $meta[6] $module
  $branches = @(Invoke-GitText @('branch', '--all', '--no-color', '--contains', $meta[0]) -split "`n" | ForEach-Object { $_.Trim().TrimStart('*').Trim() } | Where-Object { $_ -and $_ -notmatch 'HEAD ->' })
  $commitUrl = if ($baseUrl) { "$baseUrl/commit/$($meta[0])" } else { $null }
  $commits.Add([ordered]@{
    hash = $meta[0]; shortHash = $meta[0].Substring(0, 12); parents = @($meta[1] -split ' ' | Where-Object { $_ }); author = [ordered]@{ name = $meta[2]; email = $meta[3] }
    authoredAt = $meta[4]; committedAt = $meta[5]; subject = $meta[6]; branches = $branches; files = @($files); module = $activity.module; phase = $phase; activityKey = $activity.key; activityLabel = $activity.label; commitUrl = $commitUrl
  })
}

$orderedCommits = @($commits | Sort-Object -Property @{ Expression = { $_['authoredAt'] } }, @{ Expression = { $_['hash'] } })
$activities = @($orderedCommits | Group-Object -Property @{ Expression = { $_['activityKey'] } } | ForEach-Object {
  $activityCommits = @($_.Group | Sort-Object -Property @{ Expression = { $_['authoredAt'] } })
  $first = $activityCommits[0]; $last = $activityCommits[-1]
  $isMilestone = $activityCommits.Count -eq 1 -or $first.authoredAt.Substring(0,10) -eq $last.authoredAt.Substring(0,10)
  [ordered]@{
    id = "activity-$($first.activityKey)"; kind = if ($isMilestone) { 'milestone' } else { 'observed-period' }; label = $first.activityLabel; start = $first.authoredAt; end = $last.authoredAt
    module = $first.module; phases = @($activityCommits | ForEach-Object phase | Select-Object -Unique); authors = @($activityCommits | ForEach-Object { $_.author.name } | Select-Object -Unique); branches = @($activityCommits | ForEach-Object branches | Select-Object -Unique); commitHashes = @($activityCommits | ForEach-Object hash)
    realData = $true; estimated = $false; durationNote = if ($isMilestone) { "Hito temporal: un commit o actividad registrada el mismo dia; no representa duraci${acuteO}n ni avance." } else { "Periodo observado entre el primer y ultimo commit asociado; no representa duraci${acuteO}n planificada ni avance." }
  }
} | Sort-Object -Property @{ Expression = { $_['start'] } })

$groups = @($orderedCommits | Group-Object -Property @{ Expression = { $_['module'] } }, @{ Expression = { $_['phase'] } } | ForEach-Object {
  $groupCommits = @($_.Group | Sort-Object -Property @{ Expression = { $_['authoredAt'] } })
  [ordered]@{
    id = ('group-' + ($_.Name -replace '[^a-zA-Z0-9]+','-').Trim('-').ToLowerInvariant()); module = $groupCommits[0].module; phase = $groupCommits[0].phase
    firstAuthoredAt = $groupCommits[0].authoredAt; lastAuthoredAt = $groupCommits[-1].authoredAt; commitHashes = @($groupCommits | ForEach-Object hash); realData = $true; estimated = $false
  }
})

$payload = [ordered]@{
  schemaVersion = 1; generatedAt = (Get-Date).ToUniversalTime().ToString('o'); source = [ordered]@{ repository = (Invoke-GitText @('rev-parse','--show-toplevel')); head = (Invoke-GitText @('rev-parse','HEAD')); remote = $remoteUrl; scope = 'git log --all (commits alcanzables desde ramas locales y remotas; excluye stashes)'; realDataOnly = $true }
  commits = $orderedCommits; activities = $activities; groups = $groups
}

$directory = Split-Path -Parent $OutputPath
New-Item -ItemType Directory -Force -Path $directory | Out-Null
$payloadJson = $payload | ConvertTo-Json -Depth 10
$payloadJson | Set-Content -Path $OutputPath -Encoding utf8
"window.GANTT_GIT_DATA = $payloadJson;" | Set-Content -Path $JavaScriptOutputPath -Encoding utf8
$teamJsonPath = Join-Path (Split-Path -Parent $OutputPath) 'equipo-proyecto.json'
$teamJavaScriptPath = Join-Path (Split-Path -Parent $OutputPath) 'equipo-proyecto.js'
if (Test-Path $teamJsonPath) { "window.GANTT_TEAM_CONFIG = $(Get-Content -Raw $teamJsonPath);" | Set-Content -Path $teamJavaScriptPath -Encoding utf8 }
Write-Host "Datos actualizados: $OutputPath ($($orderedCommits.Count) commits, $($activities.Count) actividades)"

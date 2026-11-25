param(
  [ValidateSet('android', 'web')][string]$Target = 'android',
  [string]$DeviceId,
  [switch]$GuestOnly,
  [switch]$ValidateOnly
)

$ErrorActionPreference = 'Stop'
$RepositoryRoot = (Resolve-Path (Join-Path $PSScriptRoot '..')).Path
$FlutterRoot = Join-Path $RepositoryRoot 'movil'
$ConfigPath = Join-Path $FlutterRoot 'keycloak.local.json'

function Invoke-CheckedNative {
  param([string]$FilePath, [string[]]$Arguments, [string]$FailureMessage)
  & $FilePath @Arguments
  if ($LASTEXITCODE -ne 0) { throw $FailureMessage }
}

if ((git -C $RepositoryRoot branch --show-current) -ne 'feature/mobile-flutter') {
  throw "Expected branch feature/mobile-flutter at '$RepositoryRoot'; refusing to build another branch."
}
if (-not (Get-Command flutter -ErrorAction SilentlyContinue)) {
  throw 'No se encontro Flutter en PATH.'
}
if ($Target -eq 'web' -and -not $GuestOnly) {
  throw 'Keycloak is configured for Android/iOS. For web use -GuestOnly or run the mobile target.'
}
if ($Target -eq 'android' -and -not (Get-Command adb -ErrorAction SilentlyContinue)) {
  throw 'No se encontro adb en PATH; instala Android Platform Tools.'
}

$BuildId = "$(Get-Date -Format 'yyyyMMdd-HHmmss')-$(git -C $RepositoryRoot rev-parse --short HEAD)"
$DartDefines = @(
  "--dart-define=APP_ENV=development",
  "--dart-define=APP_BUILD_ID=$BuildId"
)

if ($GuestOnly) {
  $DartDefines += '--dart-define=DEMO_MODE=true'
  Write-Host "Explicit guest mode; no Keycloak defines or login will be used. BuildId=$BuildId"
} else {
  if (-not (Test-Path -LiteralPath $ConfigPath -PathType Leaf)) {
    throw "Missing '$ConfigPath'. Copy keycloak.local.json.example and configure the real issuer/Client ID before an authenticated build."
  }
  try {
    $Config = Get-Content -LiteralPath $ConfigPath -Raw | ConvertFrom-Json
  } catch {
    throw "'$ConfigPath' is not valid JSON: $($_.Exception.Message)"
  }
  if ($null -eq $Config -or $Config -isnot [System.Management.Automation.PSCustomObject]) {
    throw "'$ConfigPath' must contain a JSON object of dart-define keys."
  }
  $ConfigNames = @($Config.PSObject.Properties | ForEach-Object { $_.Name })
  foreach ($Name in @('KEYCLOAK_ISSUER', 'KEYCLOAK_CLIENT_ID')) {
    if ($ConfigNames -notcontains $Name -or [string]::IsNullOrWhiteSpace([string]$Config.$Name)) {
      throw "'$ConfigPath' requires non-empty '$Name'; Flutter will not run."
    }
  }
  $SensitiveKeys = @($ConfigNames | Where-Object { $_ -match '(?i)password|secret|token' })
  if ($SensitiveKeys.Count -gt 0) {
    throw "Do not put credentials/tokens in '$ConfigPath'; Flutter uses a public OIDC client."
  }

  $Issuer = [string]$Config.KEYCLOAK_ISSUER
  $IssuerUri = $null
  if (-not [Uri]::TryCreate($Issuer, [UriKind]::Absolute, [ref]$IssuerUri) -or
      $IssuerUri.Scheme -notin @('http', 'https') -or
      $IssuerUri.AbsolutePath -match '/\.well-known/openid-configuration/?$') {
    throw 'KEYCLOAK_ISSUER must be the absolute realm issuer URL, not the Discovery URL.'
  }
  if ($IssuerUri.Scheme -eq 'http' -and [string]$Config.KEYCLOAK_ALLOW_HTTP -ne 'true') {
    throw 'HTTP issuer is allowed only for flutter run debug with KEYCLOAK_ALLOW_HTTP=true.'
  }

  try {
    $Discovery = Invoke-RestMethod -Uri "$($Issuer.TrimEnd('/'))/.well-known/openid-configuration" -TimeoutSec 10
  } catch {
    throw 'OIDC Discovery is unreachable from this host; check issuer and Keycloak status.'
  }
  if ($Discovery.issuer -cne $Issuer) {
    throw 'KEYCLOAK_ISSUER does not exactly match the issuer field from OpenID Discovery.'
  }
  if ([string]::IsNullOrWhiteSpace([string]$Config.API_BASE_URL) -or
      -not ([string]$Config.API_BASE_URL).EndsWith('/api/v1')) {
    throw 'API_BASE_URL must point to a reachable origin and end in /api/v1.'
  }
  $ApiUri = [Uri]$Config.API_BASE_URL
  try {
    $ApiHealth = Invoke-RestMethod -Uri "$($ApiUri.GetLeftPart([UriPartial]::Authority))/health/ready" -TimeoutSec 10
  } catch {
    throw 'API_BASE_URL does not respond at /health/ready; check the API before running Flutter.'
  }
  if ($ApiHealth.status -ne 'ready' -or -not $ApiHealth.keycloakConfigured) {
    throw 'The API is not ready or Keycloak validation is not configured.'
  }

  Write-Host 'Flutter config: KEYCLOAK_ISSUER=present; KEYCLOAK_CLIENT_ID=present; Discovery and API verified.'
  Write-Host "APP_ENV=development; APP_BUILD_ID=$BuildId"
}

$SelectedDeviceId = $null
if ($Target -eq 'android') {
  $DeviceJson = (& flutter devices --machine | Out-String)
  if ($LASTEXITCODE -ne 0) { throw 'No se pudieron enumerar los dispositivos Flutter.' }
  $Devices = ConvertFrom-Json -InputObject $DeviceJson
  $AndroidDevices = @($Devices | Where-Object { $_.isSupported -and $_.targetPlatform -like 'android-*' })
  if (-not [string]::IsNullOrWhiteSpace($DeviceId)) {
    $Selected = @($AndroidDevices | Where-Object { $_.id -eq $DeviceId })
    if ($Selected.Count -ne 1) { throw "Device '$DeviceId' is not listed by Flutter as a connected Android device." }
    $SelectedDeviceId = $Selected[0].id
  } elseif ($AndroidDevices.Count -eq 1) {
    $SelectedDeviceId = $AndroidDevices[0].id
  } elseif ($AndroidDevices.Count -gt 1) {
    throw 'Multiple Android devices are connected; rerun with -DeviceId <id from flutter devices>.'
  } elseif (-not $ValidateOnly) {
    throw 'No Android device is connected. Run flutter devices and connect/unlock the phone.'
  } else {
    Write-Host 'No Android connected; validation/test will run without device installation.'
  }

  if ($SelectedDeviceId) {
    $DeviceState = (& adb -s $SelectedDeviceId get-state 2>$null | Out-String).Trim()
    if ($LASTEXITCODE -ne 0 -or $DeviceState -ne 'device') {
      throw "ADB did not confirm device state for '$SelectedDeviceId'."
    }
    if (-not $GuestOnly) {
      Invoke-CheckedNative 'adb' @('-s', $SelectedDeviceId, 'reverse', 'tcp:8080', 'tcp:8080') 'Could not configure adb reverse for Keycloak (8080).'
      Invoke-CheckedNative 'adb' @('-s', $SelectedDeviceId, 'reverse', 'tcp:3000', 'tcp:3000') 'Could not configure adb reverse for API (3000).'
      $ReverseList = (& adb -s $SelectedDeviceId reverse --list | Out-String)
      if ($ReverseList -notmatch 'tcp:8080\s+tcp:8080' -or $ReverseList -notmatch 'tcp:3000\s+tcp:3000') {
        throw 'ADB Reverse did not confirm ports 8080/3000.'
      }
    }
    $SelectedName = $AndroidDevices | Where-Object { $_.id -eq $SelectedDeviceId } | Select-Object -First 1 -ExpandProperty name
    Write-Host "Android selected: $SelectedName ($SelectedDeviceId)"
  }
}

Push-Location $FlutterRoot
try {
  Invoke-CheckedNative 'flutter' @('pub', 'get') 'flutter pub get failed.'

  if (-not $GuestOnly) {
    $TestDefines = @(
      '--dart-define-from-file=keycloak.local.json',
      '--dart-define=VERIFY_KEYCLOAK_DART_DEFINES=true',
      "--dart-define=TEST_EXPECTED_KEYCLOAK_ISSUER=$($Config.KEYCLOAK_ISSUER)",
      "--dart-define=TEST_EXPECTED_KEYCLOAK_CLIENT_ID=$($Config.KEYCLOAK_CLIENT_ID)",
      "--dart-define=TEST_EXPECTED_KEYCLOAK_ALLOW_HTTP=$($Config.KEYCLOAK_ALLOW_HTTP)"
    ) + $DartDefines
    Invoke-CheckedNative 'flutter' @('test') 'flutter test failed.'
    Invoke-CheckedNative 'flutter' (@('test', 'test/build_configuration_test.dart') + $TestDefines) 'Dart-define test did not receive JSON values; the app will not be run.'
    $DartDefines += '--dart-define-from-file=keycloak.local.json'
  }

  $FlutterArguments = @('run')
  if ($Target -eq 'android') {
    if ($SelectedDeviceId) { $FlutterArguments += @('-d', $SelectedDeviceId) }
  } else {
    $FlutterArguments += @('-d', 'chrome')
  }
  $FlutterArguments += $DartDefines

  Write-Host "Flutter project directory: $FlutterRoot"
  Write-Host "Dart-define file: $(if ($GuestOnly) { 'none (explicit guest mode)' } else { $ConfigPath })"
  Write-Host "Comando: flutter $($FlutterArguments -join ' ')"
  if ($ValidateOnly) {
    Write-Host 'Preflight complete; Flutter was not started and the installed app was not changed.'
    return
  }
  Write-Host 'Flutter will run with verified dart-defines and rebuild for the selected device.'
  & flutter @FlutterArguments
  if ($LASTEXITCODE -ne 0) { throw "flutter run exited with code $LASTEXITCODE." }
} finally {
  Pop-Location
}

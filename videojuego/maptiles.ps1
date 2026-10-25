$ErrorActionPreference = 'Stop'
$metaPath = 'Assets\Art\Characters\Scenes\Imagen de ChatGPT 9 oct 2026, 04_16_21 p.m-1.png.meta'
$meta = Get-Content $metaPath -Raw
$pairs = [regex]::Matches($meta, '(?m)^\s+213: (-?\d+)\r?\n\s+second: (.+)$')
$map = @{}
foreach ($p in $pairs) { $map[$p.Groups[1].Value.Trim()] = $p.Groups[2].Value.Trim() }
"present sprite ids: " + $map.Count

$rects = [regex]::Matches($meta, '(?ms)rect:\s*\r?\n\s+serializedVersion: 2\r?\n\s+x: (-?\d+)\r?\n\s+y: (-?\d+)\r?\n\s+width: (\d+)\r?\n\s+height: (\d+)')

$folder = 'Assets\Art\Characters\Scenes\Nivel01_Empresa'
Get-ChildItem $folder -Filter *.asset | ForEach-Object {
    $t = Get-Content $_.FullName -Raw
    $m = [regex]::Match($t, 'm_Sprite: \{fileID: (-?\d+)')
    if (-not $m.Success) { return }
    $id = $m.Groups[1].Value
    $present = $map.ContainsKey($id)
    $nm = if ($present) { $map[$id] } else { 'MISSING' }
    $idx = -1
    if ($present) { $idx = [int][regex]::Match($nm, '-1_(\d+)$').Groups[1].Value }
    $rect = ''
    if ($idx -ge 0 -and $idx -lt $rects.Count) {
        $r = $rects[$idx]
        $rect = 'rect x={0} y={1} w={2} h={3}' -f $r.Groups[1].Value, $r.Groups[2].Value, $r.Groups[3].Value, $r.Groups[4].Value
    }
    '{0,-26} id={1,-22} sprite#{2,-3} {3}' -f $_.BaseName, $id, $idx, $rect
}

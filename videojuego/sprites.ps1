$ErrorActionPreference = 'Stop'
$metaPath = 'Assets\Art\Characters\Scenes\Imagen de ChatGPT 9 oct 2026, 04_16_21 p.m-1.png.meta'
$meta = Get-Content $metaPath -Raw
$rects = [regex]::Matches($meta, '(?ms)rect:\s*\r?\n\s+serializedVersion: 2\r?\n\s+x: (-?\d+)\r?\n\s+y: (-?\d+)\r?\n\s+width: (\d+)\r?\n\s+height: (\d+)')
$H = 1086
$out = @()
for ($i = 0; $i -lt $rects.Count; $i++) {
    $r = $rects[$i]
    $x = [int]$r.Groups[1].Value
    $y = [int]$r.Groups[2].Value
    $w = [int]$r.Groups[3].Value
    $h = [int]$r.Groups[4].Value
    $top = $H - ($y + $h)
    $col = [math]::Floor(($x + $w / 2.0 - 8) / 144.0)
    $row = [math]::Floor(($top + $h / 2.0 - 10) / 134.0)
    $out += [pscustomobject]@{ Idx = $i; Row = [int]$row; Col = [int]$col; Left = $x; Top = $top; W = $w; H = $h }
}
$out | Sort-Object Row, Col, Left | ForEach-Object {
    '{0,3}  celda({1},{2})  img x={3,-5} y={4,-5} {5}x{6}' -f $_.Idx, $_.Col, $_.Row, $_.Left, $_.Top, $_.W, $_.H
}

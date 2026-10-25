$ErrorActionPreference = 'Stop'
Add-Type -AssemblyName System.Drawing

$pngPath = 'Assets\Art\Characters\Scenes\Imagen de ChatGPT 9 oct 2026, 04_16_21 p.m-1.png'
$metaPath = "$pngPath.meta"
$img = [System.Drawing.Image]::FromFile((Resolve-Path $pngPath))
$W = $img.Width
$imgH = $img.Height
$meta = Get-Content $metaPath -Raw
$rects = [regex]::Matches($meta, '(?ms)rect:\s*\r?\n\s+serializedVersion: 2\r?\n\s+x: (-?\d+)\r?\n\s+y: (-?\d+)\r?\n\s+width: (\d+)\r?\n\s+height: (\d+)')

$map = @{}
for ($i = 0; $i -lt $rects.Count; $i++) {
    $r = $rects[$i]
    $x = [int]$r.Groups[1].Value
    $y = [int]$r.Groups[2].Value
    $w = [int]$r.Groups[3].Value
    $h = [int]$r.Groups[4].Value
    $map[$i] = @{ Left = $x; Top = ($imgH - ($y + $h)); W = $w; H = $h }
}

New-Item -ItemType Directory -Force -Path 'Tools\ref' | Out-Null

$indices = @()
if ($args.Count -gt 0) { $indices = $args | ForEach-Object { [int]$_ } }
else { $indices = @(0, 1, 2, 3, 4) }

$src = New-Object System.Drawing.Bitmap $img

$cell = 150
$cols = 5
$rows = [math]::Ceiling($indices.Count / $cols)
$sheet = New-Object System.Drawing.Bitmap (($cell * $cols) + 2), (($cell * $rows) + 2), ([System.Drawing.Imaging.PixelFormat]::Format32bppArgb)
$g = [System.Drawing.Graphics]::FromImage($sheet)
$g.Clear([System.Drawing.Color]::FromArgb(255, 30, 30, 40))
$g.InterpolationMode = [System.Drawing.Drawing2D.InterpolationMode]::NearestNeighbor
$g.PixelOffsetMode = [System.Drawing.Drawing2D.PixelOffsetMode]::Half
$font = New-Object System.Drawing.Font('Consolas', 13, [System.Drawing.FontStyle]::Bold)
$brush = [System.Drawing.Brushes]::Yellow

for ($k = 0; $k -lt $indices.Count; $k++) {
    $idx = $indices[$k]
    $cx = ($k % $cols) * $cell
    $cy = [math]::Floor($k / $cols) * $cell
    $g.DrawString(("#{0}" -f $idx), $font, $brush, ($cx + 3), ($cy + 1))
    if (-not $map.ContainsKey($idx)) { continue }
    $r = $map[$idx]
    $srcRect = New-Object System.Drawing.Rectangle $r.Left, $r.Top, $r.W, $r.H
    $dstRect = New-Object System.Drawing.Rectangle ($cx + 2), ($cy + 20), ($cell - 4), ($cell - 24)
    $g.DrawImage($src, $dstRect, $srcRect, [System.Drawing.GraphicsUnit]::Pixel)
}

$g.Dispose()
$src.Dispose()
$img.Dispose()
$outName = if ($env:SHEET_OUT) { $env:SHEET_OUT } else { 'sheet2.png' }
$outPath = Join-Path (Join-Path (Get-Location) 'Tools\ref') $outName
$sheet.Save($outPath, [System.Drawing.Imaging.ImageFormat]::Png)
$sheet.Dispose()
'saved ' + $outPath

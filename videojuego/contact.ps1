$ErrorActionPreference = 'Stop'
Add-Type -AssemblyName System.Drawing

# args: sheetIndex(0..3) fromIndex toIndex outName
$sheetIndex = [int]$args[0]
$from = [int]$args[1]
$to = [int]$args[2]
$outName = $args[3]

$dir = 'Assets\Art\Characters\Scenes'
$all = Get-ChildItem (Join-Path $dir '*.png') | Sort-Object Name
$pngPath = $all[$sheetIndex].FullName
$img = [System.Drawing.Image]::FromFile($pngPath)
$imgW = $img.Width
$imgH = $img.Height
$meta = Get-Content ($pngPath + '.meta') -Raw
$rects = [regex]::Matches($meta, '(?ms)rect:\s*\r?\n\s+serializedVersion: 2\r?\n\s+x: (-?\d+)\r?\n\s+y: (-?\d+)\r?\n\s+width: (\d+)\r?\n\s+height: (\d+)')

$rectList = @()
for ($i = 0; $i -lt $rects.Count; $i++) {
    $g = $rects[$i]
    $rectList += @{ Idx = $i
        Left = [int]$g.Groups[1].Value
        Top = ($imgH - ([int]$g.Groups[2].Value + [int]$g.Groups[4].Value))
        W = [int]$g.Groups[3].Value
        H = [int]$g.Groups[4].Value }
}

$cell = 230
$cols = 5
$indices = $from..$to
$rows = [int][math]::Ceiling($indices.Count / $cols)
$bmp = New-Object System.Drawing.Bitmap (($cell * $cols) + 2), (($cell * $rows) + 2), ([System.Drawing.Imaging.PixelFormat]::Format32bppArgb)
$g = [System.Drawing.Graphics]::FromImage($bmp)
$g.Clear([System.Drawing.Color]::FromArgb(255, 24, 26, 34))
$g.InterpolationMode = [System.Drawing.Drawing2D.InterpolationMode]::NearestNeighbor
$g.PixelOffsetMode = [System.Drawing.Drawing2D.PixelOffsetMode]::Half
$font = New-Object System.Drawing.Font('Consolas', 15, [System.Drawing.FontStyle]::Bold)
$font2 = New-Object System.Drawing.Font('Consolas', 11, [System.Drawing.FontStyle]::Regular)

$k = 0
foreach ($idx in $indices) {
    if ($idx -ge $rectList.Count) { break }
    $r = $rectList[$idx]
    $cx = ($k % $cols) * $cell
    $cy = [int][math]::Floor($k / $cols) * $cell
    $g.DrawString(("#{0}" -f $idx), $font, [System.Drawing.Brushes]::Yellow, ($cx + 4), ($cy + 2))
    $g.DrawString(("{0}x{1}" -f $r.W, $r.H), $font2, [System.Drawing.Brushes]::Gainsboro, ($cx + 4), ($cy + 22))

    # scale to fill the cell while keeping aspect
    $availW = $cell - 10
    $availH = $cell - 48
    $scale = [math]::Min($availW / $r.W, $availH / $r.H)
    $dw = [int]($r.W * $scale)
    $dh = [int]($r.H * $scale)
    $dx = $cx + [int](($cell - $dw) / 2)
    $dy = $cy + 42 + [int](($availH - $dh) / 2)
    $srcRect = New-Object System.Drawing.Rectangle $r.Left, $r.Top, $r.W, $r.H
    $dstRect = New-Object System.Drawing.Rectangle $dx, $dy, $dw, $dh
    $g.DrawImage($img, $dstRect, $srcRect, [System.Drawing.GraphicsUnit]::Pixel)
    $k++
}

$g.Dispose()
$img.Dispose()
$out = Join-Path (Join-Path (Get-Location) 'Tools\ref') $outName
$bmp.Save($out, [System.Drawing.Imaging.ImageFormat]::Png)
$bmp.Dispose()
'saved ' + $out

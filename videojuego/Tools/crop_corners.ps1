Add-Type -AssemblyName System.Drawing

$sheet = "C:\Users\jesus\Documents\Video_Juego\TI_-Soporte-en-Acci-n-\videojuego\Assets\Art\Characters\Scenes\Imagen de ChatGPT 9 oct 2026, 04_16_21 p.m-1.png"
$out   = "C:\Users\jesus\Documents\Video_Juego\TI_-Soporte-en-Acci-n-\videojuego\Tools\ref\corners.png"

# index, x, yPng, w, h
$items = @(
    @(6,853,37,220,105),
    @(8,1106,9,144,134),
    @(9,1274,9,144,134),
    @(20,7,335,133,127),
    @(21,154,336,137,115),
    @(22,296,341,134,110),
    @(23,436,332,131,122),
    @(24,568,333,130,127),
    @(25,699,334,148,118),
    @(26,850,330,88,120),
    @(27,1012,329,93,120),
    @(28,1149,314,271,140),
    @(29,1107,634,49,124),
    @(30,1,478,144,134),
    @(31,150,478,92,129),
    @(32,245,469,144,134),
    @(33,390,471,119,129),
    @(42,313,618,144,134)
)

$scale = 2
$cellW = 320
$cellH = 320
$cols  = 6
$rows  = [math]::Ceiling($items.Count / $cols)

$src = [System.Drawing.Image]::FromFile($sheet)
$bmp = New-Object System.Drawing.Bitmap (($cols * $cellW), ($rows * $cellH))
$g   = [System.Drawing.Graphics]::FromImage($bmp)
$g.Clear([System.Drawing.Color]::FromArgb(40,40,40))
$g.InterpolationMode = [System.Drawing.Drawing2D.InterpolationMode]::NearestNeighbor
$g.PixelOffsetMode   = [System.Drawing.Drawing2D.PixelOffsetMode]::Half
$font = New-Object System.Drawing.Font("Consolas", 20, [System.Drawing.FontStyle]::Bold)
$brush = [System.Drawing.Brushes]::Yellow
$pen = New-Object System.Drawing.Pen([System.Drawing.Color]::FromArgb(0,255,255), 1)

$i = 0
foreach ($it in $items) {
    $idx = $it[0]; $x = $it[1]; $y = $it[2]; $w = $it[3]; $h = $it[4]
    $cx = ($i % $cols) * $cellW
    $cy = [math]::Floor($i / $cols) * $cellH

    $dw = $w * $scale
    $dh = $h * $scale
    $srcRect = New-Object System.Drawing.Rectangle($x, $y, $w, $h)
    $dstRect = New-Object System.Drawing.Rectangle(($cx + 10), ($cy + 40), $dw, $dh)
    $g.DrawImage($src, $dstRect, $srcRect, [System.Drawing.GraphicsUnit]::Pixel)
    # cell-size reference square (the cell is ~144 px on this sheet)
    $g.DrawRectangle($pen, ($cx + 10), ($cy + 40), (144 * $scale), (144 * $scale))
    $g.DrawString("SPR $idx", $font, $brush, ($cx + 10), ($cy + 6))
    $i++
}

$g.Dispose()
$bmp.Save($out, [System.Drawing.Imaging.ImageFormat]::Png)
$bmp.Dispose()
$src.Dispose()
Write-Output "SAVED $out"

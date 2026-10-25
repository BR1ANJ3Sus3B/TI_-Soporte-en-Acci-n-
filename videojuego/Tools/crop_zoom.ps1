Add-Type -AssemblyName System.Drawing

$sheet = "C:\Users\jesus\Documents\Video_Juego\TI_-Soporte-en-Acci-n-\videojuego\Assets\Art\Characters\Scenes\Imagen de ChatGPT 9 oct 2026, 04_16_21 p.m-1.png"
$out   = "C:\Users\jesus\Documents\Video_Juego\TI_-Soporte-en-Acci-n-\videojuego\Tools\ref\corners_zoom.png"

# label, x, yPng, w, h, scale
$items = @(
    @("SPR 23  (L, arms up+right)", 430, 325, 145, 135, 5),
    @("SPR 26  (used as WallCorner)", 840, 320, 115, 140, 5),
    @("SPR 27", 1000, 320, 115, 140, 5),
    @("SPR 9   (band + end post)", 1265, 0, 155, 150, 4)
)

$src = [System.Drawing.Image]::FromFile($sheet)
$bmp = New-Object System.Drawing.Bitmap 1920, 1100
$g   = [System.Drawing.Graphics]::FromImage($bmp)
$g.Clear([System.Drawing.Color]::FromArgb(0,0,0))
$g.InterpolationMode = [System.Drawing.Drawing2D.InterpolationMode]::NearestNeighbor
$font  = New-Object System.Drawing.Font("Consolas", 18, [System.Drawing.FontStyle]::Bold)
$brush = [System.Drawing.Brushes]::Yellow

$i = 0
foreach ($it in $items) {
    $dx = 20 + ($i % 2) * 950
    $dy = 40 + [math]::Floor($i / 2) * 540
    $srcRect = New-Object System.Drawing.Rectangle($it[1], $it[2], $it[3], $it[4])
    $dstRect = New-Object System.Drawing.Rectangle($dx, $dy, ($it[3] * $it[5]), ($it[4] * $it[5]))
    $g.DrawImage($src, $dstRect, $srcRect, [System.Drawing.GraphicsUnit]::Pixel)
    $g.DrawString($it[0], $font, $brush, $dx, ($dy - 28))
    $i++
}

$g.Dispose()
$bmp.Save($out, [System.Drawing.Imaging.ImageFormat]::Png)
$bmp.Dispose()
$src.Dispose()
Write-Output "SAVED $out"

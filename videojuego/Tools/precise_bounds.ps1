Add-Type -AssemblyName System.Drawing

$sheet = "C:\Users\jesus\Documents\Video_Juego\TI_-Soporte-en-Acci-n-\videojuego\Assets\Art\Characters\Scenes\Imagen de ChatGPT 9 oct 2026, 04_16_21 p.m-1.png"
$bmp = [System.Drawing.Bitmap]::FromFile($sheet)

# index, x, yPng, w, h
$items = @(
    @(8,1106,9,144,134),
    @(21,154,336,137,115),
    @(23,436,332,131,122),
    @(24,568,333,130,127),
    @(25,699,334,148,118),
    @(26,850,330,88,120),
    @(70,0,914,144,134),
    @(76,872,922,93,133),
    @(77,967,920,126,136)
)

$lines = @("index  rectW x rectH   contentPx(left,bottom,w,h)   fraction(x0,y0,w,h)   fill%")

foreach ($it in $items) {
    $idx = $it[0]; $x = $it[1]; $y = $it[2]; $w = $it[3]; $h = $it[4]

    $minX = $w; $maxX = -1; $minYTop = $h; $maxYTop = -1; $opaque = 0
    for ($py = 0; $py -lt $h; $py++) {
        for ($px = 0; $px -lt $w; $px++) {
            $p = $bmp.GetPixel($x + $px, $y + $py)
            if ($p.A -lt 24) { continue }
            $opaque++
            if ($px -lt $minX) { $minX = $px }
            if ($px -gt $maxX) { $maxX = $px }
            if ($py -lt $minYTop) { $minYTop = $py }
            if ($py -gt $maxYTop) { $maxYTop = $py }
        }
    }

    if ($maxX -lt 0) { $lines += "SPR $idx  EMPTY"; continue }

    $cw = $maxX - $minX + 1
    $ch = $maxYTop - $minYTop + 1
    # Unity rect origin is bottom-left; PNG y grows downward.
    $leftPx   = $minX
    $bottomPx = $h - ($maxYTop + 1)

    $fx0 = $leftPx / $w
    $fy0 = $bottomPx / $h
    $fw  = $cw / $w
    $fh  = $ch / $h
    $fill = 100.0 * $opaque / ($cw * $ch)

    $lines += ("SPR {0,-3} {1,3}x{2,-3}  content=({3},{4},{5},{6})  frac=({7:F4},{8:F4},{9:F4},{10:F4})  fill={11:F1}%" -f `
        $idx, $w, $h, $leftPx, $bottomPx, $cw, $ch, $fx0, $fy0, $fw, $fh, $fill)
}

$bmp.Dispose()
$lines | Out-File -FilePath "Tools\ref\bounds.txt" -Encoding UTF8 -Force
$lines -join "`r`n"

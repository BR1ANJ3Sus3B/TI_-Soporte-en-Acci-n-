Add-Type -AssemblyName System.Drawing

$sheet = "C:\Users\jesus\Documents\Video_Juego\TI_-Soporte-en-Acci-n-\videojuego\Assets\Art\Characters\Scenes\Imagen de ChatGPT 9 oct 2026, 04_16_21 p.m-1.png"

# index, x, yPng, w, h
$items = @(
    @(10,17,170,144,134),
    @(11,169,173,143,129),
    @(12,321,169,173,125),
    @(16,866,173,144,134),
    @(17,1010,171,144,134),
    @(18,1154,166,144,134),
    @(19,1301,165,144,134),
    @(20,7,335,133,127),
    @(21,154,336,137,115),
    @(22,296,341,134,110)
)

$bmp = [System.Drawing.Bitmap]::FromFile($sheet)
$GRID = 16
$lines = @()

foreach ($it in $items) {
    $idx = $it[0]; $x = $it[1]; $y = $it[2]; $w = $it[3]; $h = $it[4]
    $lines += ""
    $lines += "=== SPR $idx  rect ${w}x${h}  (16x16 occupancy: '#'=opaque, ':'=see-through, 'O'=opaque+light)"
    for ($r = 0; $r -lt $GRID; $r++) {
        $row = ""
        for ($c = 0; $c -lt $GRID; $c++) {
            $x0 = $x + [math]::Floor($c * $w / $GRID); $x1 = $x + [math]::Floor(($c + 1) * $w / $GRID) - 1
            $y0 = $y + [math]::Floor($r * $h / $GRID); $y1 = $y + [math]::Floor(($r + 1) * $h / $GRID) - 1
            if ($x1 -lt $x0) { $x1 = $x0 }
            if ($y1 -lt $y0) { $y1 = $y0 }
            $sumA = 0.0; $sumL = 0.0; $cnt = 0
            for ($py = $y0; $py -le $y1; $py++) {
                for ($px = $x0; $px -le $x1; $px++) {
                    $p = $bmp.GetPixel($px, $py)
                    $sumA += $p.A
                    $sumL += (0.30 * $p.R + 0.59 * $p.G + 0.11 * $p.B)
                    $cnt++
                }
            }
            $a = $sumA / $cnt
            $l = $sumL / $cnt
            if ($a -lt 128) { $row += "." }
            elseif ($l -gt 140) { $row += "O" }
            else { $row += "#" }
        }
        $lines += "  $row"
    }
}

$bmp.Dispose()
$lines | Out-File -FilePath "Tools\ref\occupancy.txt" -Encoding UTF8
$lines -join "`n"

Add-Type -AssemblyName System.Drawing

$path = "C:\Users\jesus\Documents\Video_Juego\TI_-Soporte-en-Acci-n-\videojuego\Assets\Art\Characters\Scenes\Imagen de ChatGPT 9 oct 2026, 04_16_21 p.m-1.png"
$bmp = [System.Drawing.Bitmap]::FromFile($path)

function Opaque($rx, $ry, $rw, $rh, $label) {
  $minX = $rw; $maxX = -1; $minY = $rh; $maxY = -1
  for ($y = 0; $y -lt $rh; $y++) {
    for ($x = 0; $x -lt $rw; $x++) {
      if ($bmp.GetPixel($rx + $x, $ry + $y).A -le 10) { continue }
      if ($x -lt $minX) { $minX = $x }
      if ($x -gt $maxX) { $maxX = $x }
      if ($y -lt $minY) { $minY = $y }
      if ($y -gt $maxY) { $maxY = $y }
    }
  }
  $cw = $maxX - $minX + 1
  $ch = $maxY - $minY + 1
  Write-Output ("{0}: rect={1}x{2} opaco=({3},{4}) {5}x{6} margenes L={7} R={8} T={9} B={10}  fillX={11:N3} fillY={12:N3}" -f `
    $label, $rw, $rh, $minX, $minY, $cw, $ch, $minX, ($rw - 1 - $maxX), $minY, ($rh - 1 - $maxY), ($rw / $cw), ($rh / $ch))
}

Opaque 295 (1086 - (935 + 110)) 144 110 "FLOOR_2"
Opaque 559 (1086 - (927 + 113)) 126 113 "COREFLOOR_4"

$bmp.Dispose()

Add-Type -AssemblyName System.Drawing

$path = "C:\Users\jesus\Documents\Video_Juego\TI_-Soporte-en-Acci-n-\videojuego\Assets\Art\Characters\Scenes\Imagen de ChatGPT 9 oct 2026, 04_16_21 p.m-1.png"
$bmp = [System.Drawing.Bitmap]::FromFile($path)

# index x y(width,height) from Unity sprite.rect converted to top-down rows
$sprites = @(
  @(8,  1106, 9,   144, 134),
  @(24, 568,  333, 130, 127),
  @(25, 699,  334, 148, 118),
  @(26, 850,  330, 88,  120),
  @(42, 313,  618, 144, 134),
  @(36, 897,  478, 103, 131),
  @(2,  295,  41,  144, 110),
  @(4,  559,  46,  126, 113)
)

foreach ($s in $sprites) {
  $idx = $s[0]; $rx = $s[1]; $ry = $s[2]; $rw = $s[3]; $rh = $s[4]
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
  Write-Output ("sprite_{0,-2} rect={1}x{2} opaco=({3},{4}) {5}x{6} margenes L={7} R={8} T={9} B={10}  fillX={11:N3} fillY={12:N3}" -f `
    $idx, $rw, $rh, $minX, $minY, $cw, $ch, $minX, ($rw - 1 - $maxX), $minY, ($rh - 1 - $maxY), ($rw / $cw), ($rh / $ch))
}

$bmp.Dispose()

Add-Type -AssemblyName System.Drawing

$path = "C:\Users\jesus\Documents\Video_Juego\TI_-Soporte-en-Acci-n-\videojuego\Assets\Art\Characters\Scenes\Imagen de ChatGPT 9 oct 2026, 04_16_21 p.m-1.png"
$bmp = [System.Drawing.Bitmap]::FromFile($path)

function Analyze($rx, $ry, $rw, $rh, $label) {
  Write-Output "===== $label  rect=($rx,$ry) ${rw}x${rh} ====="

  $stepX = 4; $stepY = 4
  for ($cy = $ry; $cy -lt ($ry + $rh); $cy += $stepY) {
    $line = ""
    for ($cx = $rx; $cx -lt ($rx + $rw); $cx += $stepX) {
      $sum = 0.0; $n = 0
      for ($py = $cy; $py -lt [Math]::Min($cy + $stepY, $ry + $rh); $py++) {
        for ($px = $cx; $px -lt [Math]::Min($cx + $stepX, $rx + $rw); $px++) {
          $c = $bmp.GetPixel($px, $py)
          if ($c.A -gt 10) { $sum += (0.299 * $c.R + 0.587 * $c.G + 0.114 * $c.B); $n++ }
        }
      }
      if ($n -eq 0) { $line += " " }
      else {
        $v = $sum / $n
        $ramp = " .:-=+*#%@"
        $idx = [Math]::Min(9, [int]($v / 25.6))
        $line += [string]$ramp[$idx]
      }
    }
    Write-Output $line
  }

  # Per-column and per-row mean luminance, first and last 18 lines of each axis.
  $colVals = @()
  for ($cx = 0; $cx -lt $rw; $cx++) {
    $s = 0.0; $n = 0
    for ($py = $ry; $py -lt ($ry + $rh); $py++) {
      $c = $bmp.GetPixel($rx + $cx, $py)
      if ($c.A -gt 10) { $s += (0.299 * $c.R + 0.587 * $c.G + 0.114 * $c.B); $n++ }
    }
    if ($n -gt 0) { $colVals += [int]($s / $n) } else { $colVals += -1 }
  }
  $rowVals = @()
  for ($cy = 0; $cy -lt $rh; $cy++) {
    $s = 0.0; $n = 0
    for ($px = $rx; $px -lt ($rx + $rw); $px++) {
      $c = $bmp.GetPixel($px, $ry + $cy)
      if ($c.A -gt 10) { $s += (0.299 * $c.R + 0.587 * $c.G + 0.114 * $c.B); $n++ }
    }
    if ($n -gt 0) { $rowVals += [int]($s / $n) } else { $rowVals += -1 }
  }

  Write-Output ("col[0..17]    = " + (($colVals[0..17]) -join ","))
  Write-Output ("col[ult-17..] = " + (($colVals[($rw - 18)..($rw - 1)]) -join ","))
  Write-Output ("row[0..17]    = " + (($rowVals[0..17]) -join ","))
  Write-Output ("row[ult-17..] = " + (($rowVals[($rh - 18)..($rh - 1)]) -join ","))
  Write-Output ""
}

# Unity sprite.rect has a bottom-left origin; PNG rows are top-down.
Analyze 295 (1086 - (935 + 110)) 144 110 "FLOOR sprite_2"
Analyze 559 (1086 - (927 + 113)) 126 113 "CORE FLOOR sprite_4"

$bmp.Dispose()

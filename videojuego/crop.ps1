$ErrorActionPreference = 'Stop'
Add-Type -AssemblyName System.Drawing
$src = Resolve-Path 'Assets\Art\Characters\Scenes\Imagen de ChatGPT 9 oct 2026, 04_16_21 p.m-1.png'
$img = [System.Drawing.Image]::FromFile($src)
New-Item -ItemType Directory -Force -Path 'Tools\ref' | Out-Null

function Save-Crop($x, $y, $w, $h, $scale, $out) {
    $bmp = New-Object System.Drawing.Bitmap ([int]($w * $scale)), ([int]($h * $scale))
    $g = [System.Drawing.Graphics]::FromImage($bmp)
    $g.InterpolationMode = [System.Drawing.Drawing2D.InterpolationMode]::NearestNeighbor
    $g.PixelOffsetMode = [System.Drawing.Drawing2D.PixelOffsetMode]::Half
    $g.DrawImage($img, (New-Object System.Drawing.Rectangle 0, 0, $bmp.Width, $bmp.Height), (New-Object System.Drawing.Rectangle $x, $y, $w, $h), [System.Drawing.GraphicsUnit]::Pixel)
    $g.Dispose()
    $bmp.Save((Join-Path (Get-Location) $out), [System.Drawing.Imaging.ImageFormat]::Png)
    $bmp.Dispose()
    "saved $out"
}

Save-Crop 0 0 724 543 2 'Tools\ref\q1.png'
Save-Crop 724 0 724 543 2 'Tools\ref\q2.png'
Save-Crop 0 543 724 543 2 'Tools\ref\q3.png'
Save-Crop 724 543 724 543 2 'Tools\ref\q4.png'
$img.Dispose()

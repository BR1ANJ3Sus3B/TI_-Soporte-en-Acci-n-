$ErrorActionPreference = 'Stop'
Add-Type -AssemblyName System.Drawing
$png = 'Assets\Art\Characters\Scenes\Imagen de ChatGPT 9 oct 2026, 04_16_21 p.m-1.png'
$img = [System.Drawing.Image]::FromFile((Resolve-Path $png))
$w = [int]($img.Width * 0.45)
$h = [int]($img.Height * 0.45)
$bmp = New-Object System.Drawing.Bitmap $w, $h, ([System.Drawing.Imaging.PixelFormat]::Format32bppArgb)
$g = [System.Drawing.Graphics]::FromImage($bmp)
$g.Clear([System.Drawing.Color]::FromArgb(255, 30, 30, 40))
$g.InterpolationMode = [System.Drawing.Drawing2D.InterpolationMode]::HighQualityBicubic
$g.DrawImage($img, (New-Object System.Drawing.Rectangle 0, 0, $w, $h))
$g.Dispose()
$img.Dispose()
$out = Join-Path (Get-Location) 'Tools\ref\whole.png'
$bmp.Save($out, [System.Drawing.Imaging.ImageFormat]::Png)
$bmp.Dispose()
"saved $out"

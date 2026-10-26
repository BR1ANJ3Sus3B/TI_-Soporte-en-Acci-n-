$ErrorActionPreference = 'Stop'
Add-Type -AssemblyName System.Drawing
$dir = 'Assets\Art\Characters\Scenes'
$all = Get-ChildItem (Join-Path $dir '*.png') | Sort-Object Name
New-Item -ItemType Directory -Force -Path 'Tools\ref' | Out-Null
for ($k = 0; $k -lt $all.Count; $k++) {
    $f = $all[$k]
    $img = [System.Drawing.Image]::FromFile($f.FullName)
    $scale = 0.45
    $w = [int]($img.Width * $scale)
    $h = [int]($img.Height * $scale)
    $bmp = New-Object System.Drawing.Bitmap $w, $h, ([System.Drawing.Imaging.PixelFormat]::Format32bppArgb)
    $g = [System.Drawing.Graphics]::FromImage($bmp)
    $g.Clear([System.Drawing.Color]::FromArgb(255, 30, 30, 40))
    $g.InterpolationMode = [System.Drawing.Drawing2D.InterpolationMode]::HighQualityBicubic
    $g.DrawImage($img, (New-Object System.Drawing.Rectangle 0, 0, $w, $h))
    $g.Dispose()
    $img.Dispose()
    $out = Join-Path (Join-Path (Get-Location) 'Tools\ref') ("s$k`_whole.png")
    $bmp.Save($out, [System.Drawing.Imaging.ImageFormat]::Png)
    $bmp.Dispose()
    'saved ' + $out
}

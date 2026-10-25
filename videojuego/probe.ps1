$ErrorActionPreference = 'Stop'
try {
    Add-Type -AssemblyName System.Drawing
    $img = [System.Drawing.Image]::FromFile((Resolve-Path 'Assets\Art\Characters\Scenes\Imagen de ChatGPT 9 oct 2026, 04_16_21 p.m-1.png'))
    "OK $($img.Width)x$($img.Height)"
    $img.Dispose()
} catch {
    "FAIL: " + $_.Exception.Message
}

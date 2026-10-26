$ErrorActionPreference = 'Stop'
Add-Type -AssemblyName System.Drawing
Get-ChildItem 'Assets\Art\Characters\Scenes\*.png' | ForEach-Object {
    $img = [System.Drawing.Image]::FromFile($_.FullName)
    $m = Get-Content ($_.FullName + '.meta') -Raw
    $rects = ([regex]::Matches($m, '(?ms)rect:\s*\r?\n\s+serializedVersion: 2')).Count
    $ppu = [regex]::Match($m, 'spritePixelsToUnits:\s*([0-9.]+)').Groups[1].Value
    $spriteMode = [regex]::Match($m, '(?m)^\s+spriteMode:\s*(\d+)').Groups[1].Value
    $names = ([regex]::Matches($m, '(?m)^\s+name: ')).Count
    '{0} | {1}x{2} | rects={3} | ppu={4} | spriteMode={5} | names={6}' -f $_.Name, $img.Width, $img.Height, $rects, $ppu, $spriteMode, $names
    $img.Dispose()
}

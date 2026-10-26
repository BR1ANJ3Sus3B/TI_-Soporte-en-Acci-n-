using System.Text;
using UnityEditor;
using UnityEngine;

public static class FloorSpriteInfo
{
    private const string SheetPath =
        "Assets/Art/Characters/Scenes/Imagen de ChatGPT 9 oct 2026, 04_16_21 p.m-1.png";

    public static string Rects()
    {
        var sb = new StringBuilder();
        foreach (var asset in AssetDatabase.LoadAllAssetsAtPath(SheetPath))
        {
            if (!(asset is Sprite sprite)) continue;
            int cut = sprite.name.LastIndexOf('_');
            int index;
            if (cut < 0 || !int.TryParse(sprite.name.Substring(cut + 1), out index)) continue;
            if (index != 2 && index != 4) continue;
            sb.AppendLine(string.Format("{0}: rect=({1},{2},{3},{4}) ppu={5} sheet={6}x{7}",
                sprite.name, (int)sprite.rect.x, (int)sprite.rect.y,
                (int)sprite.rect.width, (int)sprite.rect.height,
                sprite.pixelsPerUnit, sprite.texture.width, sprite.texture.height));
        }
        return sb.ToString();
    }

    public static string Execute()
    {
        var sb = new StringBuilder();
        var assets = AssetDatabase.LoadAllAssetsAtPath(SheetPath);
        foreach (var asset in assets)
        {
            if (!(asset is Sprite sprite)) continue;

            int index;
            int cut = sprite.name.LastIndexOf('_');
            if (cut < 0 || !int.TryParse(sprite.name.Substring(cut + 1), out index)) continue;
            if (index != 2 && index != 4) continue;

            var rect = sprite.rect;
            var texture = sprite.texture;
            if (!texture.isReadable)
            {
                sb.AppendLine($"{sprite.name}: textura NO legible (isReadable=false) rect={rect.width}x{rect.height}");
                continue;
            }

            int minX = (int)rect.width, minY = (int)rect.height, maxX = -1, maxY = -1;
            var pixels = texture.GetPixels(
                (int)rect.x, (int)rect.y, (int)rect.width, (int)rect.height);
            for (int y = 0; y < (int)rect.height; y++)
                for (int x = 0; x < (int)rect.width; x++)
                {
                    if (pixels[y * (int)rect.width + x].a <= 0.01f) continue;
                    if (x < minX) minX = x;
                    if (x > maxX) maxX = x;
                    if (y < minY) minY = y;
                    if (y > maxY) maxY = y;
                }

            sb.AppendLine(string.Format(
                "{0}: rect={1}x{2} ppu={3}  opaco x[{4}..{5}] y[{6}..{7}]  margenes L={8} R={9} B={10} T={11}",
                sprite.name, rect.width, rect.height, sprite.pixelsPerUnit,
                minX, maxX, minY, maxY,
                minX, rect.width - 1 - maxX, minY, rect.height - 1 - maxY));

            // Sample the outermost opaque pixel rows for a seam-colour read.
            int row = minY;
            var left = pixels[row * (int)rect.width + minX];
            var right = pixels[row * (int)rect.width + maxX];
            var middle = pixels[row * (int)rect.width + (int)(rect.width / 2)];
            sb.AppendLine(string.Format(
                "   fila inferior opaca: izq={0} centro={1} der={2}", left, middle, right));
        }

        return sb.ToString();
    }
}

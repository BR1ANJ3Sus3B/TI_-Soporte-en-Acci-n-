using System.Text;
using UnityEditor;
using UnityEngine;

public static class SpriteRects
{
    private const string SheetPath =
        "Assets/Art/Characters/Scenes/Imagen de ChatGPT 9 oct 2026, 04_16_21 p.m-1.png";

    public static string Execute()
    {
        var wanted = new[] { 8, 24, 25, 26, 42, 36 };
        var sb = new StringBuilder();
        sb.AppendLine("SHEET " + new Texture2D(1, 1).width);

        foreach (var asset in AssetDatabase.LoadAllAssetsAtPath(SheetPath))
        {
            if (!(asset is Sprite sprite)) continue;
            int cut = sprite.name.LastIndexOf('_');
            int index;
            if (cut < 0 || !int.TryParse(sprite.name.Substring(cut + 1), out index)) continue;
            if (System.Array.IndexOf(wanted, index) < 0) continue;

            var r = sprite.rect;
            int pngTop = sprite.texture.height - (int)(r.y + r.height);
            sb.AppendLine(string.Format("{0} {1} {2} {3} {4}",
                index, (int)r.x, pngTop, (int)r.width, (int)r.height));
        }

        return sb.ToString();
    }
}

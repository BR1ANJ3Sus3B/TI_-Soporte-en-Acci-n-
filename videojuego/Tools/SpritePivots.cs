using System.Text;
using UnityEditor;
using UnityEngine;

public static class SpritePivots
{
    private const string SheetPath =
        "Assets/Art/Characters/Scenes/Imagen de ChatGPT 9 oct 2026, 04_16_21 p.m-1.png";

    public static string Execute()
    {
        var wanted = new[] { 2, 4, 5, 8, 24, 25, 26, 36, 42, 49 };
        var sb = new StringBuilder();

        foreach (var asset in AssetDatabase.LoadAllAssetsAtPath(SheetPath))
        {
            if (!(asset is Sprite sprite)) continue;
            int cut = sprite.name.LastIndexOf('_');
            int index;
            if (cut < 0 || !int.TryParse(sprite.name.Substring(cut + 1), out index)) continue;
            if (System.Array.IndexOf(wanted, index) < 0) continue;

            var r = sprite.rect;
            var pivotPx = new Vector2(sprite.pivot.x - r.x, sprite.pivot.y - r.y);
            sb.AppendLine(string.Format(
                "{0,-3} rect={1}x{2} pivot={3} pivotRel=({4:0.0},{5:0.0}) centro=({6:0.0},{7:0.0}) {8}",
                index, (int)r.width, (int)r.height, sprite.pivot, pivotPx.x, pivotPx.y,
                r.width * 0.5f, r.height * 0.5f,
                Mathf.Approximately(pivotPx.x, r.width * 0.5f) &&
                Mathf.Approximately(pivotPx.y, r.height * 0.5f) ? "CENTRO" : "OTRO"));
        }

        return sb.ToString();
    }
}

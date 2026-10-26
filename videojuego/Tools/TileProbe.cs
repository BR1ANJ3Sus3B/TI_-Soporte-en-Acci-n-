using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class TileProbe
{
    private static readonly string[] Names =
    {
        "NovaTech_RoadDash", "NovaTech_BayLine", "NovaTech_Plaza", "NovaTech_Asphalt",
        "NovaTech_Grass_A", "NovaTech_Furn_4_68",
    };

    public static string Execute()
    {
        var sb = new StringBuilder();
        const string folder = "Assets/Art/Characters/Scenes/Nivel01_Empresa/Tiles";

        foreach (var name in Names)
        {
            var tile = AssetDatabase.LoadAssetAtPath<Tile>(folder + "/" + name + ".asset");
            if (tile == null) { sb.AppendLine(name + ": MISSING"); continue; }
            var sprite = tile.sprite;
            if (sprite == null) { sb.AppendLine(name + ": NO SPRITE"); continue; }
            var m = tile.transform;
            sb.AppendLine(string.Format(
                "{0}: sprite={1} rect={2}x{3} ppu={4} units={5:F3}x{6:F3} color={7} scale=({8:F3},{9:F3})",
                name, sprite.name, (int)sprite.rect.width, (int)sprite.rect.height,
                sprite.pixelsPerUnit,
                sprite.rect.width / sprite.pixelsPerUnit, sprite.rect.height / sprite.pixelsPerUnit,
                tile.color, m.m00, m.m11));
        }

        // Also confirm what the tilemap actually shows in a sample cell.
        var dash = GameObject.Find("Tilemap_Paving")?.GetComponent<Tilemap>();
        if (dash != null)
        {
            var t = dash.GetTile(new Vector3Int(0, -26, 0));
            sb.AppendLine("cell(0,-26) tile=" + (t != null ? t.name : "null"));
            var bay = dash.GetTile(new Vector3Int(-10, -21, 0));
            sb.AppendLine("cell(-10,-21) tile=" + (bay != null ? bay.name : "null"));
        }

        var ground = GameObject.Find("Tilemap_Ground")?.GetComponent<Tilemap>();
        if (ground != null)
        {
            int count = 0;
            foreach (var pos in ground.cellBounds.allPositionsWithin)
                if (ground.GetTile(pos) != null) count++;
            sb.AppendLine($"ground tiles={count} bounds={ground.cellBounds}");
        }

        return sb.ToString();
    }
}

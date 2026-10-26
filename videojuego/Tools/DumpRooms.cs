using System.Text;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class DumpRooms
{
    private static void Row(StringBuilder sb, Tilemap solids, string label, int x0, int x1, int y0, int y1)
    {
        sb.AppendLine(label);
        for (int y = y1; y >= y0; y--)
        {
            sb.Append(y.ToString().PadLeft(4)).Append(' ');
            for (int x = x0; x <= x1; x++)
            {
                var c = new Vector3Int(x, y, 0);
                if (solids.HasTile(c)) sb.Append('#');
                else if (Physics2D.OverlapPointAll(new Vector2(x + 0.5f, y + 0.5f)).Length > 0) sb.Append('W');
                else sb.Append('.');
            }
            sb.AppendLine();
        }
        sb.AppendLine();
    }

    public static string Execute()
    {
        var sb = new StringBuilder();
        var go = GameObject.Find("Grid/Tilemap_Solidos");
        if (go == null) return "Tilemap_Solidos no existe";
        var solids = go.GetComponent<Tilemap>();
        var col = go.GetComponent<TilemapCollider2D>();
        if (col != null) col.ProcessTilemapChanges();

        Row(sb, solids, "Hall NO (-21..-14, 11..15)", -21, -14, 11, 15);
        Row(sb, solids, "Oficina S1 (-9..-3, -7..-3)", -9, -3, -7, -3);
        Row(sb, solids, "Sala Oeste A (-21..-14, 5..6)", -21, -14, 5, 6);
        Row(sb, solids, "Sala Este A (14..21, 5..6)", 14, 21, 5, 6);
        Row(sb, solids, "Hall SE (14..21, -7..-3)", 14, 21, -7, -3);
        return sb.ToString();
    }
}

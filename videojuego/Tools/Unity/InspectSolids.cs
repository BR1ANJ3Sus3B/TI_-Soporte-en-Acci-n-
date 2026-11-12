using System.Text;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class InspectSolids
{
    public static string Execute()
    {
        var sb = new StringBuilder();

        foreach (var name in new[] { "Tilemap_Solidos", "Tilemap_Walls", "Tilemap_Windows", "Tilemap_Doors", "Tilemap_Decoracion" })
        {
            var go = GameObject.Find("Grid/" + name);
            if (go == null) { sb.AppendLine(name + ": NO ENCONTRADO"); continue; }

            var map = go.GetComponent<Tilemap>();
            var col = go.GetComponent<TilemapCollider2D>();
            var rend = go.GetComponent<TilemapRenderer>();
            var box = go.GetComponent<BoxCollider2D>();

            int tiles = 0;
            foreach (var p in map.cellBounds.allPositionsWithin)
                if (map.HasTile(p)) tiles++;

            sb.AppendLine(string.Format(
                "{0}: tiles={1} bounds={2} tilemapCollider={3} box={4} renderer={5}",
                name, tiles, map.cellBounds,
                col == null ? "none" : col.enabled + "/trigger=" + col.isTrigger,
                box == null ? "none" : "yes",
                rend == null ? "none" : rend.enabled.ToString()));
        }

        // Rows of the cafeteria (cells 14..21, y -7..-3) as a footprint sanity check.
        var solids = GameObject.Find("Grid/Tilemap_Solidos").GetComponent<Tilemap>();
        sb.AppendLine();
        sb.AppendLine("Cafeteria (Hall SE) footprint, '#' = caja solida:");
        for (int y = -2; y >= -8; y--)
        {
            sb.Append(string.Format("y={0,3} ", y));
            for (int x = 13; x <= 22; x++)
                sb.Append(solids.HasTile(new Vector3Int(x, y, 0)) ? '#' : '.');
            sb.AppendLine();
        }

        sb.AppendLine();
        sb.AppendLine("Nucleo central (x -10..10, y 1..7):");
        for (int y = 7; y >= 1; y--)
        {
            sb.Append(string.Format("y={0,3} ", y));
            for (int x = -10; x <= 10; x++)
                sb.Append(solids.HasTile(new Vector3Int(x, y, 0)) ? '#' : '.');
            sb.AppendLine();
        }

        return sb.ToString();
    }
}

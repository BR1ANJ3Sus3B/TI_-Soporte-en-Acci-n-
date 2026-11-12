using System.Text;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class WallCollisionAudit
{
    public static string Execute()
    {
        var sb = new StringBuilder();

        var wallsGo = GameObject.Find("Grid/Tilemap_Walls");
        var windowsGo = GameObject.Find("Grid/Tilemap_Windows");

        var walls = wallsGo.GetComponent<Tilemap>();
        var windows = windowsGo.GetComponent<Tilemap>();

        var wallCol = wallsGo.GetComponent<TilemapCollider2D>();
        var winCol = windowsGo.GetComponent<TilemapCollider2D>();
        if (wallCol != null) wallCol.ProcessTilemapChanges();
        if (winCol != null) winCol.ProcessTilemapChanges();

        sb.AppendLine($"Tilemap_Walls collider={(wallCol != null ? wallCol.enabled + "/trigger=" + wallCol.isTrigger : "NINGUNO")}");
        sb.AppendLine($"Tilemap_Windows collider={(winCol != null ? winCol.enabled + "/trigger=" + winCol.isTrigger : "NINGUNO")}");
        sb.AppendLine();

        Audit(sb, "MUROS", walls);
        Audit(sb, "VENTANAS", windows);

        // Player collider geometry, to see whether the body can visually sink in.
        var player = GameObject.Find("Player");
        if (player != null)
        {
            sb.AppendLine();
            foreach (var col in player.GetComponents<Collider2D>())
            {
                sb.AppendLine($"Player {col.GetType().Name}: size={col.bounds.size} centro={col.bounds.center} offset/radius segun tipo");
                if (col is CapsuleCollider2D cap)
                    sb.AppendLine($"   capsule size={cap.size} offset={cap.offset} direction={cap.direction}");
                if (col is BoxCollider2D box)
                    sb.AppendLine($"   box size={box.size} offset={box.offset}");
            }
            var renderer = player.GetComponentInChildren<SpriteRenderer>();
            if (renderer != null)
                sb.AppendLine($"Player sprite bounds={renderer.bounds.size} centro={renderer.bounds.center}");
        }

        return sb.ToString();
    }

    private static void Audit(StringBuilder sb, string label, Tilemap map)
    {
        int total = 0, missing = 0;
        var firstMissing = new StringBuilder();

        foreach (var pos in map.cellBounds.allPositionsWithin)
        {
            if (!map.HasTile(pos)) continue;
            total++;
            if (Physics2D.OverlapPointAll(new Vector2(pos.x + 0.5f, pos.y + 0.5f)).Length == 0)
            {
                missing++;
                if (missing <= 8) firstMissing.Append($" ({pos.x},{pos.y})");
            }
        }

        sb.AppendLine($"{label}: celdas={total} sin colision={missing}{firstMissing}");
    }
}

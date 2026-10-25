using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class Reachability
{
    // Room interiors, same rects the builder uses.
    private static readonly (string name, int x0, int x1, int y0, int y1)[] Rooms =
    {
        ("Hall NO",      -21, -14,  11,  15),
        ("Oficina N1",    -9,  -3,  11,  15),
        ("Oficina N2",     3,   9,  11,  15),
        ("Hall NE",       14,  21,  11,  15),
        ("Sala Oeste A", -21, -14,   5,   6),
        ("Sala Oeste B", -21, -14,   2,   3),
        ("Sala Este A",   14,  21,   5,   6),
        ("Sala Este B",   14,  21,   2,   3),
        ("Nucleo",        -9,   9,   2,   6),
        ("Hall SO",      -21, -14,  -7,  -3),
        ("Oficina S1",    -9,  -3,  -7,  -3),
        ("Oficina S2",     3,   9,  -7,  -3),
        ("Hall SE",       14,  21,  -7,  -3),
    };

    private static bool Free(Vector2 p) => Physics2D.OverlapPointAll(p).Length == 0;

    public static string Execute()
    {
        var sb = new StringBuilder();
        var start = new Vector2Int(0, -9);

        var solidsGo = GameObject.Find("Grid/Tilemap_Solidos");
        if (solidsGo != null)
        {
            var col = solidsGo.GetComponent<TilemapCollider2D>();
            if (col != null) col.ProcessTilemapChanges();
        }

        if (!Free(new Vector2(start.x + 0.5f, start.y + 0.5f)))
            return "El punto de aparicion esta bloqueado: " + start;

        var seen = new HashSet<Vector2Int> { start };
        var queue = new Queue<Vector2Int>();
        queue.Enqueue(start);

        while (queue.Count > 0)
        {
            var cell = queue.Dequeue();
            foreach (var step in new[]
                     {
                         Vector2Int.up, Vector2Int.down,
                         Vector2Int.left, Vector2Int.right,
                     })
            {
                var next = cell + step;
                if (seen.Contains(next)) continue;
                if (Mathf.Abs(next.x) > 30 || Mathf.Abs(next.y) > 30) continue;
                if (!Free(new Vector2(next.x + 0.5f, next.y + 0.5f))) continue;
                seen.Add(next);
                queue.Enqueue(next);
            }
        }

        sb.AppendLine($"Celdas alcanzables desde el spawn: {seen.Count}");
        sb.AppendLine();

        int sealedRooms = 0;
        foreach (var room in Rooms)
        {
            int free = 0, reachable = 0;
            for (int y = room.y0; y <= room.y1; y++)
                for (int x = room.x0; x <= room.x1; x++)
                {
                    if (!Free(new Vector2(x + 0.5f, y + 0.5f))) continue;
                    free++;
                    if (seen.Contains(new Vector2Int(x, y))) reachable++;
                }

            bool ok = free > 0 && reachable == free;
            if (!ok) sealedRooms++;
            sb.AppendLine(string.Format("{0,-14} libres={1,3} alcanzables={2,3} {3}",
                room.name, free, reachable, ok ? "OK" : "BLOQUEADA"));
        }

        sb.AppendLine();
        sb.AppendLine(sealedRooms == 0
            ? "RESULTADO: todas las salas son accesibles"
            : $"RESULTADO: {sealedRooms} salas con celdas inaccesibles");
        return sb.ToString();
    }
}

using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class Unreachable
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
        ("Sala oculta",   23,  29,  11,  15),
    };

    private static bool Free(Vector2 p) => Physics2D.OverlapPointAll(p).Length == 0;

    public static string Execute()
    {
        var sb = new StringBuilder();
        // El spawn real del jugador es la celda (0,3); se comprueba tambien la (0,2),
        // donde cae el collider de los pies.
        var spawnCell = new Vector2Int(0, 3);

        var solidsGo = GameObject.Find("Grid/Tilemap_Solidos");
        if (solidsGo != null)
        {
            var col = solidsGo.GetComponent<TilemapCollider2D>();
            if (col != null) col.ProcessTilemapChanges();
        }

        sb.AppendLine($"spawn (0,3) libre={Free(new Vector2(0.5f, 3.5f))}  " +
                      $"pies (0,2) libre={Free(new Vector2(0.5f, 2.5f))}  " +
                      $"computadora (-8,-3) libre={Free(new Vector2(-7.5f, -2.5f))}  " +
                      $"cable (-7,-3) libre={Free(new Vector2(-6.5f, -2.5f))}  " +
                      $"entrada pared falsa (23,13) libre={Free(new Vector2(23.5f, 13.5f))}");
        sb.AppendLine();

        if (!Free(new Vector2(spawnCell.x + 0.5f, spawnCell.y + 0.5f)))
            return "El punto de aparicion esta bloqueado: " + spawnCell;

        var seen = new HashSet<Vector2Int> { spawnCell };
        var queue = new Queue<Vector2Int>();
        queue.Enqueue(spawnCell);

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

        foreach (var room in Rooms)
        {
            var cortadas = new List<string>();
            int free = 0, reachable = 0;
            for (int y = room.y0; y <= room.y1; y++)
                for (int x = room.x0; x <= room.x1; x++)
                {
                    if (!Free(new Vector2(x + 0.5f, y + 0.5f))) continue;
                    free++;
                    if (seen.Contains(new Vector2Int(x, y))) reachable++;
                    else cortadas.Add($"({x},{y})");
                }

            bool ok = free > 0 && reachable == free;
            sb.AppendLine(string.Format("{0,-14} libres={1,3} alcanzables={2,3} {3}",
                room.name, free, reachable, ok ? "OK" : "CORTADAS: " + string.Join(" ", cortadas)));
        }

        return sb.ToString();
    }
}

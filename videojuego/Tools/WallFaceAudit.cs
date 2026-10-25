using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class WallFaceAudit
{
    public static string Execute()
    {
        var sb = new StringBuilder();

        var walls = GameObject.Find("Grid/Tilemap_Walls").GetComponent<Tilemap>();
        var windows = GameObject.Find("Grid/Tilemap_Windows").GetComponent<Tilemap>();
        var floor = GameObject.Find("Grid/Tilemap_Floor").GetComponent<Tilemap>();
        var doors = GameObject.Find("Grid/Tilemap_Doors").GetComponent<Tilemap>();
        var solidsGo = GameObject.Find("Grid/Tilemap_Solidos");
        var solids = solidsGo.GetComponent<Tilemap>();

        var solidCol = solidsGo.GetComponent<Collider2D>();
        var wallsCol = GameObject.Find("Grid/Tilemap_Walls").GetComponent<Collider2D>();
        var windowsCol = GameObject.Find("Grid/Tilemap_Windows").GetComponent<Collider2D>();

        sb.AppendLine($"Walls collider={(wallsCol != null ? wallsCol.GetType().Name : "NINGUNO")}" +
                      $" | Windows collider={(windowsCol != null ? windowsCol.GetType().Name : "NINGUNO")}" +
                      $" | Solidos collider={(solidCol != null ? solidCol.GetType().Name + " trigger=" + solidCol.isTrigger : "NINGUNO")}");
        sb.AppendLine();

        // 1) Every wall / window cell must be covered by a collider.
        int blockedTotal = 0, blockedMissing = 0;
        var missingSample = new StringBuilder();
        foreach (var map in new[] { walls, windows })
        {
            foreach (var pos in map.cellBounds.allPositionsWithin)
            {
                if (!map.HasTile(pos)) continue;
                blockedTotal++;
                if (Physics2D.OverlapPointAll(new Vector2(pos.x + 0.5f, pos.y + 0.5f)).Length == 0)
                {
                    blockedMissing++;
                    if (blockedMissing <= 6) missingSample.Append($" ({pos.x},{pos.y})");
                }
            }
        }
        sb.AppendLine($"Celdas muro+ventana={blockedTotal} sin colision={blockedMissing}{missingSample}");

        // 2) Doors and floor must stay passable.
        int doorTotal = 0, doorBlocked = 0;
        foreach (var pos in doors.cellBounds.allPositionsWithin)
        {
            if (!doors.HasTile(pos)) continue;
            doorTotal++;
            if (Physics2D.OverlapPointAll(new Vector2(pos.x + 0.5f, pos.y + 0.5f)).Length > 0) doorBlocked++;
        }
        int floorTotal = 0, floorBlocked = 0;
        foreach (var pos in floor.cellBounds.allPositionsWithin)
        {
            if (!floor.HasTile(pos)) continue;
            floorTotal++;
            if (Physics2D.OverlapPointAll(new Vector2(pos.x + 0.5f, pos.y + 0.5f)).Length > 0) floorBlocked++;
        }
        sb.AppendLine($"Puertas={doorTotal} con colision={doorBlocked} (deben ser 0)");
        sb.AppendLine($"Suelo={floorTotal} con colision={floorBlocked} (solo muebles)");
        sb.AppendLine();

        // 3) Cast from floor cells into their wall neighbours: the hit must land on the
        //    shared cell edge (0.5 from the floor cell centre).
        var deviations = new List<float>();
        int samples = 0, wrongCollider = 0;
        float worst = 0f;
        Vector2 worstAt = Vector2.zero;

        var dirs = new[] { Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left };
        foreach (var pos in floor.cellBounds.allPositionsWithin)
        {
            if (!floor.HasTile(pos)) continue;
            foreach (var dir in dirs)
            {
                var neighbour = pos + (Vector3Int)dir;
                if (!walls.HasTile(neighbour) && !windows.HasTile(neighbour)) continue;
                if (solids.HasTile(pos)) continue; // furniture in the way

                var origin = new Vector2(pos.x + 0.5f, pos.y + 0.5f);
                var hit = Physics2D.Raycast(origin, dir, 3f);
                if (hit.collider == null) { wrongCollider++; continue; }

                samples++;
                float dev = hit.distance - 0.5f;
                deviations.Add(dev);
                if (Mathf.Abs(dev) > Mathf.Abs(worst)) { worst = dev; worstAt = origin; }
            }
        }

        if (deviations.Count > 0)
        {
            float sum = 0f, min = float.MaxValue, max = float.MinValue;
            foreach (var d in deviations) { sum += d; if (d < min) min = d; if (d > max) max = d; }
            sb.AppendLine($"Muestras de cara de muro={samples} sin impacto={wrongCollider}");
            sb.AppendLine($"  desviacion media={sum / deviations.Count:0.0000}  min={min:0.0000}  max={max:0.0000}  peor={worst:0.0000} en {worstAt}");
            sb.AppendLine($"  dentro de +-0.02: {Count(deviations, 0.02f)}/{deviations.Count}");
        }

        return sb.ToString();
    }

    private static int Count(List<float> values, float tolerance)
    {
        int n = 0;
        foreach (var v in values) if (Mathf.Abs(v) <= tolerance) n++;
        return n;
    }
}

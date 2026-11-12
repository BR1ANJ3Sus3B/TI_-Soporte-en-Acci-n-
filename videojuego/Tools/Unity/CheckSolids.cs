using System.Text;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class CheckSolids
{
    public static string Execute()
    {
        var sb = new StringBuilder();
        foreach (var name in new[]
                 {
                     "Tilemap_Floor", "Tilemap_Walls", "Tilemap_WallTrim", "Tilemap_Windows",
                     "Tilemap_Doors", "Tilemap_Decoracion", "Tilemap_Solidos",
                 })
        {
            var go = GameObject.Find(name);
            if (go == null) { sb.AppendLine($"{name} MISSING"); continue; }

            var map = go.GetComponent<Tilemap>();
            var renderer = go.GetComponent<TilemapRenderer>();
            var collider = go.GetComponent<TilemapCollider2D>();
            int filled = 0;
            var bounds = map.cellBounds;
            foreach (var cell in bounds.allPositionsWithin)
                if (map.GetTile(cell) != null) filled++;

            int shapes = collider != null ? collider.shapeCount : 0;
            sb.AppendLine($"{name,-20} order={(renderer != null ? renderer.sortingOrder : 999),-3} " +
                          $"tiles={filled,-4} collider={(collider != null ? "YES" : "no"),-4} " +
                          $"shapes={shapes} enabled={(renderer != null && renderer.enabled)}");
        }
        return sb.ToString();
    }
}

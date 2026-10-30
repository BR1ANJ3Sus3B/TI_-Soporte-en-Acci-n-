using System.Text;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>Comprueba qué capas tienen tile en las celdas de la entrada sur.</summary>
public static class Tools_ProbarEntrada
{
    public static string Execute()
    {
        var sb = new StringBuilder();
        var grid = GameObject.Find("Grid");
        if (grid == null) return "sin Grid";

        var celdas = new[]
        {
            new Vector3Int(0, -8, 0), new Vector3Int(0, -9, 0),
            new Vector3Int(0, -10, 0), new Vector3Int(0, -11, 0),
            new Vector3Int(0, -12, 0), new Vector3Int(0, -13, 0)
        };

        foreach (var tm in grid.GetComponentsInChildren<Tilemap>(true))
        {
            var partes = new StringBuilder();
            foreach (var c in celdas)
            {
                var tile = tm.GetTile(c);
                partes.Append($" ({c.x},{c.y})={tile?.name ?? "-"}");
            }
            sb.AppendLine($"{tm.name}:{partes}");
        }

        var solidos = grid.transform.Find("Tilemap_Solidos")?.GetComponent<Tilemap>();
        sb.AppendLine("--- sólidos en la columna x=0 ---");
        if (solidos != null)
        {
            for (int y = -14; y <= -6; y++)
                sb.AppendLine($"  (0,{y}) solido={solidos.HasTile(new Vector3Int(0, y, 0))}");
        }

        var col = solidos?.GetComponent<TilemapCollider2D>();
        sb.AppendLine("collider sólidos = " + (col != null));

        return sb.ToString();
    }
}

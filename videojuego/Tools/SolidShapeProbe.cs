using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class SolidShapeProbe
{
    public static string Execute()
    {
        var sb = new StringBuilder();

        var solidTile = AssetDatabase.LoadAssetAtPath<Tile>("Assets/Art/Characters/Scenes/Nivel01_Empresa/Tiles/NovaTech_Solido.asset");
        if (solidTile == null)
        {
            sb.AppendLine("NovaTech_Solido.asset NO EXISTE");
        }
        else
        {
            sb.AppendLine("NovaTech_Solido colliderType=" + solidTile.colliderType);

            if (solidTile.sprite != null)
            {
                Sprite sp = solidTile.sprite;
                sb.AppendLine("  sprite=" + sp.name + " rect=" + sp.rect + " ppu=" + sp.pixelsPerUnit);
                sb.AppendLine("  physicsShapes=" + sp.GetPhysicsShapeCount());

                Matrix4x4 m = solidTile.transform;
                Vector3 pos = new Vector3(m.m03, m.m13, m.m23);
                Vector3 scl = new Vector3(m.m00, m.m11, m.m22);
                sb.AppendLine("  transform traslacion=" + pos + " escala=" + scl);
            }
        }
        sb.AppendLine();

        Vector2Int[] cells = { new Vector2Int(-4, -6), new Vector2Int(12, -7), new Vector2Int(5, 8), new Vector2Int(-2, 5) };
        Vector2Int[] dirs = { Vector2Int.up, Vector2Int.left, Vector2Int.up, Vector2Int.left };

        for (int i = 0; i < cells.Length; i++)
        {
            Vector2 origin = new Vector2(cells[i].x + 0.5f, cells[i].y + 0.5f);
            Vector2 dir = new Vector2(dirs[i].x, dirs[i].y);
            RaycastHit2D hit = Physics2D.Raycast(origin, dir, 3f);

            if (hit.collider == null)
            {
                sb.AppendLine(cells[i] + " dir=" + dirs[i] + ": sin impacto");
                continue;
            }

            sb.AppendLine(cells[i] + " dir=" + dirs[i] + ": impacto a " + hit.distance.ToString("0.000") +
                          " punto=" + hit.point + " collider='" + hit.collider.name + "' bounds=" + hit.collider.bounds);

            TilemapCollider2D tmc = hit.collider as TilemapCollider2D;
            if (tmc != null)
            {
                Tilemap map = tmc.GetComponent<Tilemap>();
                Vector3 back = hit.point - (Vector3)(dir * 0.02f);
                Vector3Int cell = map.WorldToCell(back);
                Tile tile = map.GetTile(cell) as Tile;
                sb.AppendLine("   celda=" + cell + " tile=" + (tile != null ? tile.name + " tipo=" + tile.colliderType : "null"));
            }
        }

        return sb.ToString();
    }
}

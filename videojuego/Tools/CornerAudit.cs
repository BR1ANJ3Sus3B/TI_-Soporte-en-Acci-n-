using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class CornerAudit
{
    private const string SheetPath =
        "Assets/Art/Characters/Scenes/Imagen de ChatGPT 9 oct 2026, 04_16_21 p.m-1.png";

    public static string Execute()
    {
        var sb = new StringBuilder();

        // --- every slice of the tileset: index + rect in PNG (top-left) coordinates
        var list = new List<(int idx, Rect r, float ppu)>();
        foreach (var asset in AssetDatabase.LoadAllAssetsAtPath(SheetPath))
        {
            if (!(asset is Sprite s)) continue;
            int cut = s.name.LastIndexOf('_');
            if (cut < 0 || !int.TryParse(s.name.Substring(cut + 1), out int idx)) continue;
            list.Add((idx, s.rect, s.pixelsPerUnit));
        }
        list.Sort((a, b) => a.idx.CompareTo(b.idx));
        foreach (var e in list)
        {
            int top = (int)(1086 - (e.r.y + e.r.height));
            sb.AppendLine($"SPR {e.idx}\tx={e.r.x:F0} yPng={top} w={e.r.width:F0} h={e.r.height:F0} ppu={e.ppu:F0}");
        }

        // --- which tile asset sits on each wall cell, and how those cells are surrounded
        var wallMap = GameObject.Find("Tilemap_Walls")?.GetComponent<Tilemap>();
        var floorMap = GameObject.Find("Tilemap_Floor")?.GetComponent<Tilemap>();
        var winMap = GameObject.Find("Tilemap_Windows")?.GetComponent<Tilemap>();
        var doorMap = GameObject.Find("Tilemap_Doors")?.GetComponent<Tilemap>();
        if (wallMap == null || floorMap == null) return sb.Append("NO_TILEMAP").ToString();

        var perTile = new Dictionary<string, int>();
        var cornerSig = new Dictionary<string, int>();
        var samples = new Dictionary<string, Vector3Int>();

        var bounds = wallMap.cellBounds;
        sb.AppendLine($"WALLBOUNDS {bounds}");
        foreach (var cell in bounds.allPositionsWithin)
        {
            var tile = wallMap.GetTile(cell);
            if (tile == null) continue;
            string path = AssetDatabase.GetAssetPath(tile);
            string name = System.IO.Path.GetFileNameWithoutExtension(path);
            perTile.TryGetValue(name, out int c);
            perTile[name] = c + 1;

            bool Solid(Vector3Int p) =>
                wallMap.GetTile(p) != null || winMap.GetTile(p) != null || doorMap.GetTile(p) != null;
            bool FloorAt(Vector3Int p) => floorMap.GetTile(p) != null;

            if (!name.Contains("Corner") && !name.Contains("_T_")) continue;

            string Sig(Vector3Int p) =>
                (FloorAt(p) ? "F" : ".") + (Solid(p) ? "S" : "-");

            string sig = Sig(cell + Vector3Int.up) + Sig(cell + Vector3Int.right) +
                         Sig(cell + Vector3Int.down) + Sig(cell + Vector3Int.left) + "|" +
                         Sig(cell + new Vector3Int(1, 1, 0)) + Sig(cell + new Vector3Int(1, -1, 0)) +
                         Sig(cell + new Vector3Int(-1, -1, 0)) + Sig(cell + new Vector3Int(-1, 1, 0));
            string key = name + " " + sig;
            cornerSig.TryGetValue(key, out int k);
            cornerSig[key] = k + 1;
            if (!samples.ContainsKey(key)) samples[key] = cell;
        }

        sb.AppendLine("--- tiles used on Tilemap_Walls");
        foreach (var kv in perTile) sb.AppendLine($"  {kv.Key} = {kv.Value}");

        sb.AppendLine("--- corners / tees, neighbour signature up,right,down,left|NE,SE,SW,NW  (F=floor S=solid .=void)");
        foreach (var kv in cornerSig)
            sb.AppendLine($"  {kv.Value,4}  {kv.Key}   sample={samples[kv.Key]}");

        return sb.ToString();
    }
}

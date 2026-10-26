using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class MapInspect
{
    public static void Execute()
    {
        var sb = new StringBuilder();

        sb.AppendLine("=== TILEMAPS EN ESCENA ===");
        foreach (var tm in UnityEngine.Object.FindObjectsByType<Tilemap>(FindObjectsSortMode.None))
        {
            var bounds = tm.cellBounds;
            int count = tm.GetUsedTilesCount();
            sb.AppendLine($"- {tm.name} | tiles usados={count} | bounds={bounds.min}..{bounds.max} | sortOrder={(tm.GetComponent<TilemapRenderer>() != null ? tm.GetComponent<TilemapRenderer>().sortingOrder : 999)} | collider={(tm.GetComponent<TilemapCollider2D>() != null)}");
            var sprites = new HashSet<string>();
            foreach (var pos in bounds.allPositionsWithin)
            {
                var t = tm.GetTile(pos);
                if (t == null) continue;
                if (t is Tile tile)
                {
                    sprites.Add(tile.sprite == null ? "NULL_SPRITE(" + tile.name + ")" : tile.sprite.name + " " + tile.sprite.rect.size);
                }
                else sprites.Add(t.GetType().Name);
            }
            foreach (var s in sprites) sb.AppendLine("      " + s);
        }

        sb.AppendLine();
        sb.AppendLine("=== TILE ASSETS EN CARPETA ===");
        var guids = AssetDatabase.FindAssets("t:Tile", new[] { "Assets/Art/Characters/Scenes/Nivel01_Empresa" });
        foreach (var g in guids)
        {
            var path = AssetDatabase.GUIDToAssetPath(g);
            var tile = AssetDatabase.LoadAssetAtPath<TileBase>(path);
            if (tile is Tile t)
            {
                var sp = t.sprite;
                sb.AppendLine($"{t.name,-26} sprite={(sp == null ? "NULL" : sp.name + " " + sp.rect.size)} scale=({t.transform.e00:0.###},{t.transform.e11:0.###}) collider={t.colliderType}");
            }
            else sb.AppendLine($"{System.IO.Path.GetFileName(path)} -> {tile?.GetType().Name ?? "null"}");
        }

        sb.AppendLine();
        sb.AppendLine("=== TEXTURA DEL TILESET ===");
        var texPath = "Assets/Art/Characters/Scenes/Imagen de ChatGPT 9 oct 2026, 04_16_21 p.m-1.png";
        var importer = AssetImporter.GetAtPath(texPath) as TextureImporter;
        if (importer != null)
        {
            sb.AppendLine($"spriteImportMode={importer.spriteImportMode} PPU={importer.spritePixelsPerUnit} filter={importer.filterMode} texType={importer.textureType} readable={importer.isReadable}");
            var sheet = new TextureImporterSettings();
            importer.ReadTextureSettings(sheet);
            var tex = AssetDatabase.LoadAssetAtPath<Texture2D>(texPath);
            sb.AppendLine($"tamaño textura={(tex != null ? tex.width + "x" + tex.height : "?")}");
        }
        var sprites = AssetDatabase.LoadAllAssetsAtPath(texPath).OfType<Sprite>().ToArray();
        sb.AppendLine($"sprites={sprites.Length}");
        foreach (var s in sprites.OrderByDescending(x => x.rect.y).ThenBy(x => x.rect.x))
            sb.AppendLine($"   {s.name,-46} rect={s.rect}");

        Debug.Log(sb.ToString());
    }
}

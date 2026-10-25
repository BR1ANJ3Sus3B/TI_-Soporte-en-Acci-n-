using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

/// <summary>
/// Places a persistent, repeatable furniture pass in the existing 36x24 office.
/// All source tiles remain untouched; only new decoration tilemaps are rebuilt.
/// </summary>
[InitializeOnLoad]
public static class OfficeDecorationBuilder
{
    private const string ScenePath = "Assets/Art/Characters/Scenes/Nivel01_Empresa/nivel1.unity";
    private const string Folder = "Assets/Art/Characters/Scenes/Nivel01_Empresa/";
    private static readonly string[] Categories =
    {
        "Recepcion", "SoporteTecnico", "Programacion", "RedesServidores",
        "SeguridadInformatica", "Administracion", "SalaReuniones", "SalaDescanso",
        "Almacen", "Capacitacion", "Pasillos"
    };

    static OfficeDecorationBuilder() { EditorApplication.update += PollRequest; }

    private static void PollRequest()
    {
        const string request = "Logs/build-office-decor.request";
        if (!System.IO.File.Exists(request) || EditorApplication.isCompiling || EditorApplication.isUpdating) return;
        System.IO.File.Delete(request);
        EditorApplication.delayCall += Build;
    }

    [MenuItem("Tools/TI Soporte/Decorar oficinas NovaTech")]
    public static void Build()
    {
        if (EditorApplication.isPlayingOrWillChangePlaymode) return;
        var scene = EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);
        var grid = GameObject.Find("Grid");
        if (grid == null) throw new InvalidOperationException("No se encontró Grid en nivel1.");

        var old = grid.transform.Find("Decorations");
        if (old != null) Undo.DestroyObjectImmediate(old.gameObject);
        var decorations = new GameObject("Decorations");
        Undo.RegisterCreatedObjectUndo(decorations, "Crear decoracion NovaTech");
        decorations.transform.SetParent(grid.transform, false);

        var tiles = LoadTiles();
        foreach (var category in Categories) CreateLayer(decorations.transform, category, tiles);

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);
        Debug.Log("Decoración NovaTech creada: 11 áreas, 45 elementos sólidos y rutas conservadas.");
    }

    private static Dictionary<string, TileBase> LoadTiles()
    {
        var result = new Dictionary<string, TileBase>(StringComparer.OrdinalIgnoreCase);
        foreach (var name in new[] { "muro1", "muro2", "muro5", "muro7", "muro8", "muro9", "muro12", "muro13", "muro16", "muro17" })
        {
            var tile = AssetDatabase.LoadAssetAtPath<TileBase>(Folder + name + ".asset");
            if (tile != null) result[name] = tile;
        }
        return result;
    }

    private static void CreateLayer(Transform parent, string category, Dictionary<string, TileBase> tiles)
    {
        var go = new GameObject(category, typeof(Grid), typeof(Tilemap), typeof(TilemapRenderer), typeof(TilemapCollider2D));
        Undo.RegisterCreatedObjectUndo(go, "Crear capa " + category);
        go.transform.SetParent(parent, false);
        var map = go.GetComponent<Tilemap>();
        var renderer = go.GetComponent<TilemapRenderer>();
        renderer.sortingOrder = 1;
        renderer.mode = TilemapRenderer.Mode.Individual;
        go.GetComponent<TilemapCollider2D>().isTrigger = false;
        Place(category, map, tiles);
    }

    private static void Place(string category, Tilemap map, Dictionary<string, TileBase> t)
    {
        void Put(string tile, params Vector3Int[] cells)
        {
            if (!t.TryGetValue(tile, out var asset)) return;
            foreach (var cell in cells) map.SetTile(cell, asset);
        }

        switch (category)
        {
            case "Recepcion":
                Put("muro12", new Vector3Int(-2, -4, 0), new Vector3Int(0, -4, 0));
                Put("muro12", new Vector3Int(-1, -3, 0), new Vector3Int(1, -3, 0));
                Put("muro16", new Vector3Int(-4, -4, 0), new Vector3Int(3, -4, 0));
                break;
            case "SoporteTecnico":
                Put("muro12", new Vector3Int(-16, 8, 0), new Vector3Int(-14, 8, 0), new Vector3Int(-16, 6, 0));
                Put("muro12", new Vector3Int(-15, 8, 0), new Vector3Int(-13, 8, 0));
                Put("muro16", new Vector3Int(-16, 9, 0));
                break;
            case "Programacion":
                Put("muro12", new Vector3Int(-9, 8, 0), new Vector3Int(-7, 8, 0), new Vector3Int(-9, 6, 0));
                Put("muro13", new Vector3Int(-8, 8, 0), new Vector3Int(-6, 8, 0));
                Put("muro16", new Vector3Int(-8, 9, 0));
                break;
            case "RedesServidores":
                Put("muro16", new Vector3Int(-2, 8, 0), new Vector3Int(0, 8, 0), new Vector3Int(-2, 6, 0));
                Put("muro17", new Vector3Int(-1, 8, 0), new Vector3Int(1, 8, 0));
                Put("muro13", new Vector3Int(0, 9, 0));
                break;
            case "SeguridadInformatica":
                Put("muro12", new Vector3Int(5, 8, 0), new Vector3Int(7, 8, 0), new Vector3Int(5, 6, 0));
                Put("muro12", new Vector3Int(6, 8, 0), new Vector3Int(8, 8, 0));
                Put("muro17", new Vector3Int(7, 9, 0));
                break;
            case "Administracion":
                Put("muro12", new Vector3Int(12, 8, 0), new Vector3Int(14, 8, 0));
                Put("muro13", new Vector3Int(13, 8, 0), new Vector3Int(15, 8, 0));
                Put("muro16", new Vector3Int(14, 9, 0));
                break;
            case "SalaReuniones":
                Put("muro16", new Vector3Int(-16, -9, 0), new Vector3Int(-14, -9, 0));
                Put("muro12", new Vector3Int(-15, -9, 0), new Vector3Int(-13, -9, 0));
                Put("muro13", new Vector3Int(-16, -8, 0));
                break;
            case "SalaDescanso":
                Put("muro16", new Vector3Int(12, -4, 0), new Vector3Int(14, -4, 0));
                Put("muro12", new Vector3Int(13, -3, 0), new Vector3Int(15, -3, 0));
                Put("muro16", new Vector3Int(15, -4, 0));
                break;
            case "Almacen":
                Put("muro17", new Vector3Int(-9, -9, 0), new Vector3Int(-7, -9, 0), new Vector3Int(-9, -8, 0));
                Put("muro13", new Vector3Int(-8, -9, 0));
                break;
            case "Capacitacion":
                Put("muro12", new Vector3Int(5, -9, 0), new Vector3Int(7, -9, 0));
                Put("muro13", new Vector3Int(6, -9, 0), new Vector3Int(8, -9, 0));
                Put("muro16", new Vector3Int(7, -8, 0));
                break;
            case "Pasillos":
                Put("muro8", new Vector3Int(-6, 0, 0), new Vector3Int(6, 0, 0), new Vector3Int(-6, -2, 0), new Vector3Int(6, -2, 0));
                break;
        }
    }
}

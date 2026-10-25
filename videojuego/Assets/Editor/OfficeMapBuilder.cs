using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

/// <summary>
/// Builds the NovaTech office level from the existing tile assets.
/// It is deliberately scoped to nivel1.unity and can be run repeatedly.
/// </summary>
[InitializeOnLoad]
public static class OfficeMapBuilder
{
    private const string ScenePath = "Assets/Art/Characters/Scenes/Nivel01_Empresa/nivel1.unity";
    private const string AssetFolder = "Assets/Art/Characters/Scenes/Nivel01_Empresa/";
    private static string ProjectRoot => Directory.GetParent(Application.dataPath).FullName;
    private static string RequestPath => Path.Combine(ProjectRoot, "Logs/build-office-map.request");
    private static string ResultPath => Path.Combine(ProjectRoot, "Logs/build-office-map.result");
    private static bool requestScheduled;

    private const int MinX = -18;
    private const int MaxX = 17;
    private const int MinY = -12;
    private const int MaxY = 11;

    static OfficeMapBuilder()
    {
        // Logs is ignored by Unity's asset importer, so polling also handles a
        // request created while the editor is already open.
        EditorApplication.update += PollRequest;
    }

    private static void PollRequest()
    {
        if (!File.Exists(RequestPath)) return;
        if (requestScheduled) return;
        requestScheduled = true;
        EditorApplication.delayCall += ProcessRequest;
    }

    [InitializeOnLoadMethod]
    private static void RunRequestedBuild()
    {
        if (File.Exists(RequestPath)) EditorApplication.delayCall += ProcessRequest;
    }

    [MenuItem("Tools/TI Soporte/Construir mapa NovaTech")]
    public static void BuildFromMenu()
    {
        Build();
    }

    [MenuItem("Tools/TI Soporte/Construir mapa NovaTech", true)]
    private static bool CanBuild() => !EditorApplication.isPlayingOrWillChangePlaymode;

    private static void ProcessRequest()
    {
        if (!File.Exists(RequestPath)) return;
        if (EditorApplication.isPlayingOrWillChangePlaymode)
        {
            EditorApplication.isPlaying = false;
            EditorApplication.delayCall += ProcessRequest;
            return;
        }
        if (EditorApplication.isCompiling || EditorApplication.isUpdating)
        {
            EditorApplication.delayCall += ProcessRequest;
            return;
        }

        File.Delete(RequestPath);
        requestScheduled = false;
        try { Build(); }
        catch (Exception exception)
        {
            File.WriteAllText(ResultPath, "ERROR " + exception);
            Debug.LogException(exception);
        }
    }

    private static void Build()
    {
        var scene = EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);
        var grid = FindOrCreateGrid(scene);
        var floor = FindOrCreateTilemap(grid, "Tilemap_Floor", -20);
        var walls = FindOrCreateTilemap(grid, "Tilemap_Walls", 0);
        var doors = FindOrCreateTilemap(grid, "Tilemap_Doors", 2);
        var windows = FindOrCreateTilemap(grid, "Tilemap_Windows", 1);
        var decor = FindOrCreateTilemap(grid, "Tilemap_Decoracion", 3);

        TileBase floorTile = LoadTile("Imagen de ChatGPT 9 oct 2026, 04_16_21 p.m-1_1.asset");
        TileBase wallHorizontal = LoadTile("Empresa_ParedHorizontal.asset");
        TileBase wallVertical = LoadTile("Empresa_ParedVertical.asset");
        TileBase doorTile = LoadTile("muro6.asset");
        TileBase windowTile = LoadTile("muro18.asset");
        TileBase consoleTile = LoadTile("muro12.asset");

        floor.ClearAllTiles();
        walls.ClearAllTiles();
        doors.ClearAllTiles();
        windows.ClearAllTiles();
        decor.ClearAllTiles();

        // Full technological floor, with one consistent 1-unit grid cell.
        for (int y = MinY; y <= MaxY; y++)
            for (int x = MinX; x <= MaxX; x++)
                floor.SetTile(new Vector3Int(x, y, 0), floorTile);

        var wallCells = new HashSet<Vector3Int>();
        var doorCells = new HashSet<Vector3Int>();
        var windowCells = new HashSet<Vector3Int>();

        // Building shell. The central lower entrance stays open.
        Horizontal(wallCells, MinY, MinX, MaxX, -1, 0, 1);
        Horizontal(wallCells, MaxY, MinX, MaxX);
        Vertical(wallCells, MinX, MinY, MaxY);
        Vertical(wallCells, MaxX, MinY, MaxY);

        // Five upper offices.
        Room(wallCells, doorCells, -17, -12, 5, 10, DoorSide.Bottom);
        Room(wallCells, doorCells, -10, -5, 5, 10, DoorSide.Bottom);
        Room(wallCells, doorCells, -3, 2, 5, 10, DoorSide.Bottom);
        Room(wallCells, doorCells, 4, 9, 5, 10, DoorSide.Bottom);
        Room(wallCells, doorCells, 11, 16, 5, 10, DoorSide.Bottom);

        // Symmetric side rooms connected to the cross corridor.
        Room(wallCells, doorCells, -17, -12, 0, 4, DoorSide.Right);
        Room(wallCells, doorCells, -17, -12, -6, -1, DoorSide.Right);
        Room(wallCells, doorCells, 11, 16, 0, 4, DoorSide.Left);
        Room(wallCells, doorCells, 11, 16, -6, -1, DoorSide.Left);

        // Four lower rooms around the central entrance.
        Room(wallCells, doorCells, -17, -12, -10, -7, DoorSide.Top);
        Room(wallCells, doorCells, -10, -5, -10, -7, DoorSide.Top);
        Room(wallCells, doorCells, 4, 9, -10, -7, DoorSide.Top);
        Room(wallCells, doorCells, 11, 16, -10, -7, DoorSide.Top);

        // Central technical core: two access structures with a walkable ring.
        Room(wallCells, doorCells, -4, 0, 1, 4, DoorSide.Bottom);
        Room(wallCells, doorCells, 1, 5, 1, 4, DoorSide.Bottom);

        // Door cells always take priority over room-wall corners and shared
        // boundaries, preventing visible overlaps in the compact layout.
        wallCells.ExceptWith(doorCells);

        // Windows replace selected wall cells, so they remain visible and passable
        // only where the architectural plan calls for them.
        WindowRun(windowCells, wallCells, -16, -13, 10);
        WindowRun(windowCells, wallCells, -9, -6, 10);
        WindowRun(windowCells, wallCells, 5, 8, 10);
        WindowRun(windowCells, wallCells, 12, 15, 10);
        WindowRun(windowCells, wallCells, -18, -18, 1);
        WindowRun(windowCells, wallCells, 17, 17, 1);

        foreach (Vector3Int cell in wallCells)
            walls.SetTile(cell, Mathf.Abs(cell.y) % 2 == 0 ? wallHorizontal : wallVertical);
        foreach (Vector3Int cell in doorCells)
            doors.SetTile(cell, doorTile);
        foreach (Vector3Int cell in windowCells)
            windows.SetTile(cell, windowTile);

        // Small technical consoles make the central access structures readable.
        decor.SetTile(new Vector3Int(-1, 2, 0), consoleTile);
        decor.SetTile(new Vector3Int(1, 2, 0), consoleTile);

        ConfigureWallCollisions(walls);
        ConfigureCamera(scene);
        PlacePlayer(scene);
        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);
        AssetDatabase.SaveAssets();
        OfficeDecorationBuilder.Build();
        File.WriteAllText(ResultPath,
            $"OK rooms=11 walls={wallCells.Count} doors={doorCells.Count} windows={windowCells.Count} scene={ScenePath}");
        Debug.Log("NovaTech map built: 11 rooms, central corridors, doors, windows and TilemapCollider2D wall collision.");
    }

    private enum DoorSide { Top, Bottom, Left, Right }

    private static void Room(HashSet<Vector3Int> walls, HashSet<Vector3Int> doors,
        int left, int right, int bottom, int top, DoorSide side)
    {
        var gaps = new HashSet<int>();
        int middle = side == DoorSide.Left || side == DoorSide.Right ? (bottom + top) / 2 : (left + right) / 2;
        gaps.Add(middle);
        Horizontal(walls, bottom, left, right, side == DoorSide.Bottom ? middle - 1 : int.MinValue,
            side == DoorSide.Bottom ? middle : int.MinValue, side == DoorSide.Bottom ? middle + 1 : int.MinValue);
        Horizontal(walls, top, left, right, side == DoorSide.Top ? middle - 1 : int.MinValue,
            side == DoorSide.Top ? middle : int.MinValue, side == DoorSide.Top ? middle + 1 : int.MinValue);
        Vertical(walls, left, bottom, top, side == DoorSide.Left ? middle - 1 : int.MinValue,
            side == DoorSide.Left ? middle : int.MinValue, side == DoorSide.Left ? middle + 1 : int.MinValue);
        Vertical(walls, right, bottom, top, side == DoorSide.Right ? middle - 1 : int.MinValue,
            side == DoorSide.Right ? middle : int.MinValue, side == DoorSide.Right ? middle + 1 : int.MinValue);
        if (side == DoorSide.Bottom)
            for (int x = middle - 1; x <= middle + 1; x++) doors.Add(new Vector3Int(x, bottom, 0));
        if (side == DoorSide.Top)
            for (int x = middle - 1; x <= middle + 1; x++) doors.Add(new Vector3Int(x, top, 0));
        if (side == DoorSide.Left)
            for (int y = middle - 1; y <= middle + 1; y++) doors.Add(new Vector3Int(left, y, 0));
        if (side == DoorSide.Right)
            for (int y = middle - 1; y <= middle + 1; y++) doors.Add(new Vector3Int(right, y, 0));
    }

    private static void Horizontal(HashSet<Vector3Int> cells, int y, int left, int right, params int[] gaps)
    {
        var excluded = new HashSet<int>(gaps);
        for (int x = left; x <= right; x++) if (!excluded.Contains(x)) cells.Add(new Vector3Int(x, y, 0));
    }

    private static void Vertical(HashSet<Vector3Int> cells, int x, int bottom, int top, params int[] gaps)
    {
        var excluded = new HashSet<int>(gaps);
        for (int y = bottom; y <= top; y++) if (!excluded.Contains(y)) cells.Add(new Vector3Int(x, y, 0));
    }

    private static void WindowRun(HashSet<Vector3Int> windows, HashSet<Vector3Int> walls, int left, int right, int y)
    {
        for (int x = left; x <= right; x++)
        {
            var cell = new Vector3Int(x, y, 0);
            if (!walls.Remove(cell)) continue;
            windows.Add(cell);
        }
    }

    private static TileBase LoadTile(string fileName)
    {
        var tile = AssetDatabase.LoadAssetAtPath<TileBase>(AssetFolder + fileName);
        if (tile == null) throw new InvalidOperationException("Missing tile asset: " + fileName);
        return tile;
    }

    private static GameObject FindOrCreateGrid(Scene scene)
    {
        foreach (var root in scene.GetRootGameObjects())
            if (root.name == "Grid") return root;
        var grid = new GameObject("Grid", typeof(Grid));
        SceneManager.MoveGameObjectToScene(grid, scene);
        return grid;
    }

    private static Tilemap FindOrCreateTilemap(GameObject grid, string name, int sortingOrder)
    {
        var child = grid.transform.Find(name);
        GameObject go = child != null ? child.gameObject : new GameObject(name, typeof(Grid), typeof(Tilemap), typeof(TilemapRenderer));
        if (child == null) go.transform.SetParent(grid.transform, false);
        var renderer = go.GetComponent<TilemapRenderer>();
        renderer.sortingOrder = sortingOrder;
        return go.GetComponent<Tilemap>();
    }

    private static void ConfigureWallCollisions(Tilemap walls)
    {
        foreach (var old in walls.GetComponents<BoxCollider2D>()) UnityEngine.Object.DestroyImmediate(old);
        var collider = walls.GetComponent<TilemapCollider2D>();
        if (collider == null) collider = walls.gameObject.AddComponent<TilemapCollider2D>();
        // A TilemapCollider2D is enough for this one-cell wall grid and avoids
        // adding a second rigidbody to the imported scene's environment root.
        collider.isTrigger = false;
    }

    private static void ConfigureCamera(Scene scene)
    {
        var camera = GameObject.FindWithTag("MainCamera");
        if (camera == null) return;
        var framing = camera.GetComponent<MapCameraFrame>() ?? camera.AddComponent<MapCameraFrame>();
        var serialized = new SerializedObject(framing);
        serialized.FindProperty("mapCenter").vector2Value = Vector2.zero;
        serialized.FindProperty("mapSize").vector2Value = new Vector2(MaxX - MinX + 1, MaxY - MinY + 1);
        serialized.FindProperty("padding").floatValue = 3f;
        serialized.ApplyModifiedPropertiesWithoutUndo();
        camera.GetComponent<Camera>().orthographic = true;
        camera.GetComponent<Camera>().backgroundColor = new Color(0.015f, 0.035f, 0.08f, 1f);
    }

    private static void PlacePlayer(Scene scene)
    {
        foreach (var root in scene.GetRootGameObjects())
            if (root.name == "Player")
            {
                root.transform.position = new Vector3(0, -9, 0);
                return;
            }
        var prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Scripts/Player.prefab");
        var player = (GameObject)PrefabUtility.InstantiatePrefab(prefab, scene);
        player.name = "Player";
        player.transform.position = new Vector3(0, -9, 0);
    }
}

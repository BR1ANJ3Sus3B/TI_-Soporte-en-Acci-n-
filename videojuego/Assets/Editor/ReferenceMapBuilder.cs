using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

/// <summary>
/// Rebuilds nivel1 as the symmetric NovaTech facility layout taken from the design
/// reference: four corner halls, two offices north and south of the spine, four
/// side rooms, a central technical core and a full circulation ring.
///
/// The builder is deterministic and re-runnable: it owns every tilemap it touches,
/// so running it twice produces the same map.
/// </summary>
[InitializeOnLoad]
public static class ReferenceMapBuilder
{
    private const string ScenePath = "Assets/Art/Characters/Scenes/Nivel01_Empresa/nivel1.unity";
    private const string TileFolder = "Assets/Art/Characters/Scenes/Nivel01_Empresa/Tiles";
    private const string TilesetPath =
        "Assets/Art/Characters/Scenes/Imagen de ChatGPT 9 oct 2026, 04_16_21 p.m-1.png";
    private const string FurnitureSheetPath =
        "Assets/Art/Characters/Scenes/Imagen de ChatGPT 9 oct 2026, 04_16_22 p.m-2.png";
    private const string TechSheetPath =
        "Assets/Art/Characters/Scenes/Imagen de ChatGPT 9 oct 2026, 04_16_25 p.m-3.png";
    private const string ExteriorSheetPath =
        "Assets/Art/Characters/Scenes/Imagen de ChatGPT 9 oct 2026, 04_16_26 p.m-4.png";

    // The prop sheets draw one grid cell as roughly 110 px, so furniture keeps its
    // drawn proportions instead of being squashed into a single cell.
    private const float PropCellPixels = 110f;
    private const int FurnitureSheet = 2;
    private const int TechSheet = 3;
    private const int ExteriorSheet = 4;

    // A cell becomes solid once a prop covers at least this much of it on both axes.
    // Anything thinner stays walkable so furniture never plugs a doorway.
    private const float MinSolidCoverage = 0.5f;

    // Building shell in grid cells (1 cell = 1 world unit).
    private const int MinX = -22, MaxX = 22, MinY = -8, MaxY = 16;

    // Sprite indices of the NovaTech tileset (auto-sliced sheet).
    private const int SpriteFloor      = 2;   // dark slate floor, low contrast seams
    private const int SpriteFloorCore  = 4;   // blue deck plating
    private const int SpriteWallPanel  = 8;   // solid wall panel, fills its whole cell
    private const int SpriteWallCorner = 23;  // L corner, arms to up + right
    private const int SpriteWindow     = 42;  // three pane glass run
    private const int SpriteDoor       = 36;  // amber door in a frame
    private const int SpriteElevator   = 33;  // elevator doors
    private const int SpriteStairs     = 49;  // staircase
    private const int SpriteHazard     = 5;   // yellow/black threshold plate
    private const int SpriteSolid      = 28;  // tiny dot, used only as a collider carrier

    // Every slice sits inside a transparent margin, and some are strongly off centre:
    // the wall panel, for instance, is an 81 px band inside a 134 px cell with 49 px
    // of empty space above it. Fitting the rect to a cell therefore draws a thin bar
    // adrift from the floor, which reads as a gap against the walls. These are the
    // measured opaque areas of the source PNG, as a fraction of each sprite rect
    // (bottom-left origin); tiles are scaled and shifted so the opaque area itself
    // lands exactly on the cell. Indices missing here fall back to the full rect.
    private static readonly Dictionary<int, Rect> SpriteContent = new Dictionary<int, Rect>
    {
        { SpriteFloor,     new Rect( 16f / 144f,   5f / 110f,  98f / 144f,  96f / 110f) },
        { SpriteFloorCore, new Rect( 16f / 126f,  13f / 113f,  99f / 126f,  96f / 113f) },
        { SpriteWallPanel, new Rect(  3f / 144f,   4f / 134f, 136f / 144f,  81f / 134f) },
        { SpriteWallCorner, new Rect(25f / 131f, 11f / 122f,  84f / 131f, 111f / 122f) },
        { SpriteWindow,    new Rect(  0f / 144f,   7f / 134f, 137f / 144f,  96f / 134f) },
        { SpriteDoor,      new Rect( 19f / 103f,  10f / 131f,  79f / 103f, 110f / 131f) },
    };

    // ---------------------------------------------------------------- exterior

    // The grounds around the facility: lawn, road, parking bays and street kit.
    private const int ExtMinX = -30, ExtMaxX = 30, ExtMinY = -27, ExtMaxY = 21;

    // Sprite indices of the exterior sheet.
    private const int ExtBench      = 75;  // wooden park bench
    private const int ExtLamp       = 45;  // street lamp post
    private const int ExtTraffic    = 41;  // twin traffic light
    private const int ExtBollard    = 40;  // steel bollard
    private const int ExtFence      = 12;  // chain link fence run
    private const int ExtBooth      = 4;   // guard booth with barrier arm
    private const int ExtGatePost   = 16;  // gate pillar with hedge
    private const int ExtDumpster   = 51;  // site skip
    private const int ExtPlanterTall = 20; // potted shrub
    private const int ExtPlanterLong = 112;// long flower trough
    private const int ExtPlanterPink = 114;// long pink flower trough

    private static readonly int[] ExtBushes = { 89, 90, 91, 98, 99, 110, 113, 107, 111 };
    private static readonly int[] ExtCars   = { 3, 5, 6, 7, 8 };

    // Hard surfaces. Laid on the paving layer, above the lawn and below the floors.
    // The south elevation reads downward from the building: apron, steps, sidewalk,
    // parking bays, kerb, carriageway.
    private static readonly (int x0, int x1, int y0, int y1)[] ExtPlaza =
    {
        (-14, 14, -15,  -9),  // apron between the south portico and the sidewalk
        (-26, 26, -18, -16),  // public sidewalk running the width of the plot
        ( 24, 29,  -2,   4),  // east garden patio
        ( -4,  4,  20,  21),  // north service path behind the building
        (-29, -23, -24, -19), // entrance lane past the guard booth
    };

    private static readonly (int x0, int x1, int y0, int y1)[] ExtRoad =
    {
        (-26, 26, -24, -19),  // parking bays
        (-28, 28, -24, -24),  // kerb between the bays and the carriageway
        (-30, 30, -27, -25),  // carriageway
    };

    private static readonly (int x0, int x1, int y0, int y1)[] ExtSteps = { (-3, 3, -14, -12) };

    // Where the lawn is allowed to grow trees: the plot border plus the strip behind
    // the building.
    private static readonly (int x0, int x1, int y0, int y1)[] ExtGreen =
    {
        (-30, -23, -27, 21),
        ( 23,  30, -27, 21),
        (-30,  30,  17, 21),
        (-24, -15, -16,  -9),  // garden strip west of the entrance plaza
        ( 15,  24, -16,  -9),  // garden strip east of the entrance plaza
    };

    // Small overscale so neighbouring tiles overlap by a couple of percent instead of
    // meeting on an exact edge, which would show a hairline seam.
    private const float TileOverlap = 1.02f;

    private static string ProjectRoot => Directory.GetParent(Application.dataPath).FullName;
    private static string RequestPath => Path.Combine(ProjectRoot, "Logs/build-novatech-map.request");
    private static string ResultPath => Path.Combine(ProjectRoot, "Logs/build-novatech-map.result");
    private static bool requestScheduled;

    // Room interiors (inclusive). Walls are the ring one cell outside each rect.
    private static readonly (int x0, int x1, int y0, int y1)[] Rooms =
    {
        (-21, -14,  11,  15), // Hall noreste-oeste (esquina superior izquierda)
        ( -9,  -3,  11,  15), // Oficina norte 1
        (  3,   9,  11,  15), // Oficina norte 2
        ( 14,  21,  11,  15), // Hall superior derecho
        (-21, -14,   5,   6), // Sala oeste A
        (-21, -14,   2,   3), // Sala oeste B
        ( 14,  21,   5,   6), // Sala este A
        ( 14,  21,   2,   3), // Sala este B
        ( -9,   9,   2,   6), // Nucleo tecnico central
        (-21, -14,  -7,  -3), // Hall inferior izquierdo
        ( -9,  -3,  -7,  -3), // Oficina sur 1
        (  3,   9,  -7,  -3), // Oficina sur 2
        ( 14,  21,  -7,  -3), // Hall inferior derecho
    };

    // Corridors (inclusive). Corridor floor is drawn first and stays walkable.
    private static readonly (int x0, int x1, int y0, int y1)[] Corridors =
    {
        (-21, 21,   8,   9), // Pasillo horizontal norte
        (-21, 21,  -1,   0), // Pasillo horizontal sur
        (-12, -11, -7,  15), // Pasillo vertical oeste
        ( 11,  12, -7,  15), // Pasillo vertical este
        ( -1,   1,  10,  15), // Eje central norte
        ( -1,   1,  -7,  -2), // Eje central sur
        ( -1,   1,  17,  18), // Vestibulo de entrada superior
        ( -1,   1, -10,  -9), // Vestibulo de entrada inferior
    };

    // Building outline. Rooms flush with it reuse the same cells.
    private static readonly (int x0, int x1, int y0, int y1)[] ShellWalls =
    {
        (-22, 22,  16,  16),
        (-22, 22,  -8,  -8),
        (-22, -22, -8,  16),
        ( 22,  22, -8,  16),
        // Entrance vestibules.
        ( -2,  -2,  17,  18), (  2,   2,  17,  18), ( -2,   2,  19,  19),
        ( -2,  -2, -10,  -9), (  2,   2, -10,  -9), ( -2,   2, -11, -11),
    };

    // Glass runs. Orientation is derived from neighbours at build time.
    private static readonly (int x0, int x1, int y0, int y1)[] Windows =
    {
        (-20, -15,  16,  16), ( -8,  -4,  16,  16), (  4,   8,  16,  16), ( 15,  20,  16,  16),
        (-20, -15,  -8,  -8), ( -8,  -4,  -8,  -8), (  4,   8,  -8,  -8), ( 15,  20,  -8,  -8),
        (-22, -22,  12,  14), (-22, -22,   5,   6), (-22, -22,   2,   3), (-22, -22,  -6,  -4),
        ( 22,  22,  12,  14), ( 22,  22,   5,   6), ( 22,  22,   2,   3), ( 22,  22,  -6,  -4),
    };

    // Doorways. Every room reachable from the ring, plus the four entrances.
    private static readonly (int x0, int x1, int y0, int y1)[] Doors =
    {
        (-18, -17,  10,  10), ( -6,  -6,  10,  10), (  6,   6,  10,  10), ( 17,  18,  10,  10),
        (-18, -17,  -2,  -2), ( -6,  -6,  -2,  -2), (  6,   6,  -2,  -2), ( 17,  18,  -2,  -2),
        (-13, -13,   6,   6), (-13, -13,   3,   3), ( 13,  13,   6,   6), ( 13,  13,   3,   3),
        ( -1,   1,   1,   1),  // Nucleo, acceso sur
        ( -1,   1,  16,  16), ( -1,   1,  19,  19),  // Entrada superior
        ( -1,   1,  -8,  -8), ( -1,   1, -11, -11),  // Entrada inferior
    };

    private static readonly HashSet<Vector2Int> Floor = new HashSet<Vector2Int>();
    private static readonly HashSet<Vector2Int> Wall = new HashSet<Vector2Int>();
    private static readonly HashSet<Vector2Int> Glass = new HashSet<Vector2Int>();
    private static readonly HashSet<Vector2Int> Door = new HashSet<Vector2Int>();

    private static readonly Dictionary<string, TileBase> Tiles = new Dictionary<string, TileBase>();
    private static Dictionary<int, Sprite> sprites;
    private static Dictionary<int, Sprite> furniture;
    private static Dictionary<int, Sprite> tech;
    private static Dictionary<int, Sprite> exterior;

    static ReferenceMapBuilder()
    {
        EditorApplication.update += PollRequest;
    }

    private static void PollRequest()
    {
        if (requestScheduled || !File.Exists(RequestPath)) return;
        requestScheduled = true;
        EditorApplication.delayCall += ProcessRequest;
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
        if (!File.Exists(RequestPath)) { requestScheduled = false; return; }
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
        try
        {
            var report = Build();
            File.WriteAllText(ResultPath, report);
            Debug.Log(report);
        }
        catch (Exception exception)
        {
            File.WriteAllText(ResultPath, "ERROR " + exception);
            Debug.LogException(exception);
        }
    }

    public static string Build()
    {
        if (EditorApplication.isPlayingOrWillChangePlaymode)
            throw new InvalidOperationException("Sal del modo Play antes de reconstruir el mapa.");

        var scene = EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);
        EnsureTileFolder();
        Tiles.Clear();
        LoadSprites();

        var grid = FindOrCreateGrid(scene);
        var ground = FindOrCreateTilemap(grid, "Tilemap_Ground", -40);
        var paving = FindOrCreateTilemap(grid, "Tilemap_Paving", -35);
        var exteriorMap = FindOrCreateTilemap(grid, "Tilemap_Exterior", -30);
        var floor = FindOrCreateTilemap(grid, "Tilemap_Floor", -20);
        var walls = FindOrCreateTilemap(grid, "Tilemap_Walls", 0);
        var wallTrim = FindOrCreateTilemap(grid, "Tilemap_WallTrim", 1);
        var windows = FindOrCreateTilemap(grid, "Tilemap_Windows", 2);
        var doors = FindOrCreateTilemap(grid, "Tilemap_Doors", 3);
        var deco = FindOrCreateTilemap(grid, "Tilemap_Decoracion", 4);

        RemoveLegacyDecor(grid);

        BuildCellSets();

        floor.ClearAllTiles();
        walls.ClearAllTiles();
        wallTrim.ClearAllTiles();
        windows.ClearAllTiles();
        doors.ClearAllTiles();
        deco.ClearAllTiles();
        ground.ClearAllTiles();
        paving.ClearAllTiles();
        exteriorMap.ClearAllTiles();

        var floorTile = GetTile("NovaTech_Floor", SpriteFloor, 0, Tile.ColliderType.None);
        var coreFloorTile = GetTile("NovaTech_FloorCore", SpriteFloorCore, 0, Tile.ColliderType.None);
        var hazardTile = GetTile("NovaTech_Hazard", SpriteHazard, 0, Tile.ColliderType.None);

        // Every wall cell is covered edge to edge by this solid panel. The corner art is
        // an L, so on its own it would leave the inside of the bend transparent and the
        // camera background would show through as a notch.
        var wallBaseTile = GetTile("NovaTech_Wall", SpriteWallPanel, 0, Tile.ColliderType.None);

        foreach (var cell in Floor)
            floor.SetTile(new Vector3Int(cell.x, cell.y, 0), floorTile);

        // Deck plating marks the technical core so it reads as a distinct zone.
        var core = Rooms[8];
        for (int y = core.y0; y <= core.y1; y++)
            for (int x = core.x0; x <= core.x1; x++)
                floor.SetTile(new Vector3Int(x, y, 0), coreFloorTile);

        // The northeast hall houses the server racks, so it shares the plating.
        var serverHall = Rooms[3];
        for (int y = serverHall.y0; y <= serverHall.y1; y++)
            for (int x = serverHall.x0; x <= serverHall.x1; x++)
                floor.SetTile(new Vector3Int(x, y, 0), coreFloorTile);

        // Threshold plates under the two main entrances.
        floor.SetTile(new Vector3Int(0, 19, 0), hazardTile);
        floor.SetTile(new Vector3Int(0, -11, 0), hazardTile);

        foreach (var cell in Door)
        {
            var tile = DoorTile(cell);
            doors.SetTile(new Vector3Int(cell.x, cell.y, 0), tile);
        }

        foreach (var cell in Glass)
        {
            var vertical = !IsSolid(cell + Vector2Int.left) && !IsSolid(cell + Vector2Int.right);
            var tile = GetTile(vertical ? "NovaTech_Window_V" : "NovaTech_Window_H",
                SpriteWindow, vertical ? 1 : 0, Tile.ColliderType.Grid);
            windows.SetTile(new Vector3Int(cell.x, cell.y, 0), tile);
        }

        foreach (var cell in Wall)
        {
            var position = new Vector3Int(cell.x, cell.y, 0);
            walls.SetTile(position, wallBaseTile);
            var corner = WallCornerTile(cell);
            if (corner != null) wallTrim.SetTile(position, corner);
        }

        // Two staircase runs flank the core, then the whole furniture pass.
        var stairsTile = GetTile("NovaTech_Stairs", SpriteStairs, 0, Tile.ColliderType.None);
        foreach (var cell in new[]
                 {
                     new Vector3Int(-7, 3, 0), new Vector3Int(-7, 4, 0), new Vector3Int(-7, 5, 0),
                     new Vector3Int(7, 3, 0), new Vector3Int(7, 4, 0), new Vector3Int(7, 5, 0),
                 })
            deco.SetTile(cell, stairsTile);

        PlaceProps(deco);

        // The site around the building: lawn, paving, road, parking and greenery.
        var exteriorSolids = BuildExterior(ground, paving, exteriorMap);

        // Hidden map that carries one collider per cell a piece of furniture covers.
        var solids = FindOrCreateTilemap(grid, "Tilemap_Solidos", 5);
        var solidsRenderer = solids.GetComponent<TilemapRenderer>();
        if (solidsRenderer != null) solidsRenderer.enabled = false;
        solids.ClearAllTiles();
        BuildWallSolids(solids);
        BuildPropSolids(solids);
        BuildExteriorSolids(solids, exteriorSolids);

        ConfigureCollision(walls, wallTrim, windows, doors, deco, floor, solids,
            ground, paving, exteriorMap);
        PlacePlayer(scene);
        ConfigureCamera(scene);

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);
        AssetDatabase.SaveAssets();

        return $"OK mapa NovaTech reconstruido | suelo={Floor.Count} muros={Wall.Count} " +
               $"ventanas={Glass.Count} puertas={Door.Count} exterior={ExtMaxX - ExtMinX + 1}x{ExtMaxY - ExtMinY + 1} " +
               $"| escena={ScenePath}";
    }

    // ---------------------------------------------------------------- geometry

    private static void BuildCellSets()
    {
        Floor.Clear(); Wall.Clear(); Glass.Clear(); Door.Clear();

        foreach (var room in Rooms)
            for (int y = room.y0; y <= room.y1; y++)
                for (int x = room.x0; x <= room.x1; x++)
                    Floor.Add(new Vector2Int(x, y));

        foreach (var corridor in Corridors)
            for (int y = corridor.y0; y <= corridor.y1; y++)
                for (int x = corridor.x0; x <= corridor.x1; x++)
                    Floor.Add(new Vector2Int(x, y));

        // Room perimeter rings.
        foreach (var room in Rooms)
            Ring(room.x0 - 1, room.x1 + 1, room.y0 - 1, room.y1 + 1);

        foreach (var shell in ShellWalls)
            Ring(shell.x0, shell.x1, shell.y0, shell.y1);

        // Walls never sit on walkable floor: corridors punch their own openings.
        Wall.ExceptWith(Floor);

        foreach (var run in Windows)
            for (int y = run.y0; y <= run.y1; y++)
                for (int x = run.x0; x <= run.x1; x++)
                {
                    var cell = new Vector2Int(x, y);
                    if (Wall.Remove(cell)) Glass.Add(cell);
                }

        foreach (var run in Doors)
            for (int y = run.y0; y <= run.y1; y++)
                for (int x = run.x0; x <= run.x1; x++)
                {
                    var cell = new Vector2Int(x, y);
                    if (Wall.Remove(cell) || Glass.Remove(cell)) Door.Add(cell);
                }
    }

    /// <summary>Adds the rectangle outline (not the filled rect) to the wall set.</summary>
    private static void Ring(int x0, int x1, int y0, int y1)
    {
        if (x0 == x1 && y0 == y1) { Wall.Add(new Vector2Int(x0, y0)); return; }
        for (int x = x0; x <= x1; x++)
        {
            Wall.Add(new Vector2Int(x, y0));
            Wall.Add(new Vector2Int(x, y1));
        }
        for (int y = y0; y <= y1; y++)
        {
            Wall.Add(new Vector2Int(x0, y));
            Wall.Add(new Vector2Int(x1, y));
        }
    }

    private static bool IsSolid(Vector2Int cell) => Wall.Contains(cell) || Glass.Contains(cell);

    private static bool IsWalkable(Vector2Int cell) => Floor.Contains(cell) || Door.Contains(cell);

    // ------------------------------------------------------------ tile picking

    /// <summary>
    /// Corner art for a cell where the wall bends, or null on a straight run. The
    /// sprite's arms point up + right, so each mask gets a quarter turn that lands
    /// the arms on the two neighbours that really are wall.
    /// </summary>
    private static TileBase WallCornerTile(Vector2Int cell)
    {
        bool up = IsSolid(cell + Vector2Int.up);
        bool right = IsSolid(cell + Vector2Int.right);
        bool down = IsSolid(cell + Vector2Int.down);
        bool left = IsSolid(cell + Vector2Int.left);

        switch ((up ? 1 : 0) | (right ? 2 : 0) | (down ? 4 : 0) | (left ? 8 : 0))
        {
            case 3: return Corner(0);   // up + right
            case 9: return Corner(1);   // up + left
            case 12: return Corner(2);  // down + left
            case 6: return Corner(3);   // down + right
        }

        // Ends, straights and T junctions keep the plain panel: the sheet has no three
        // armed piece, so a four armed junction sprite would push a stub into the floor.
        return null;
    }

    private static TileBase Corner(int quarterTurns) =>
        GetTile("NovaTech_Wall_Corner_" + quarterTurns * 90, SpriteWallCorner, quarterTurns,
            Tile.ColliderType.None);

    private static TileBase DoorTile(Vector2Int cell)
    {
        // The door art faces along a horizontal wall; a doorway in a vertical
        // wall uses the same tile turned a quarter turn.
        bool inHorizontalWall = IsWallRun(cell + Vector2Int.left) || IsWallRun(cell + Vector2Int.right);
        return inHorizontalWall
            ? GetTile("NovaTech_Door_H", SpriteDoor, 0, Tile.ColliderType.None)
            : GetTile("NovaTech_Door_V", SpriteDoor, 1, Tile.ColliderType.None);
    }

    private static bool IsWallRun(Vector2Int cell) => Wall.Contains(cell) || Glass.Contains(cell);

    // ------------------------------------------------------------------ props

    /// <summary>
    /// Furniture layout. Every entry is (sheet, sprite, cell x, cell y) and the prop
    /// is centred on that cell; the sheets are indexed 2 = furniture, 3 = IT kit.
    /// Placements follow the reference floor plan: open offices with paired
    /// workstations, a board room, a private office, a server hall, storage rooms,
    /// a lounge per side, a reception core and a cafeteria.
    /// </summary>
    private static readonly (int sheet, int sprite, int x, int y)[] Props =
    {
        // Hall NO: open office, four paired workstations either side of a centre aisle.
        (FurnitureSheet,  2, -20, 14), (FurnitureSheet,  2, -15, 14),
        (FurnitureSheet,  2, -20, 12), (FurnitureSheet,  2, -15, 12),
        (FurnitureSheet, 22, -21, 11),
        (FurnitureSheet, 36, -21, 15), (FurnitureSheet, 36, -14, 15),

        // Oficina N1: board room with a long table and a free standing whiteboard.
        (FurnitureSheet,  6,  -6, 13),
        (TechSheet,       8,  -8, 14),
        (FurnitureSheet, 36,  -3, 15), (FurnitureSheet, 36,  -9, 11),

        // Oficina N2: private office, L desk plus a wall map screen.
        (TechSheet,       0,   5, 14),
        (TechSheet,       9,   8, 11),
        (FurnitureSheet, 22,   3, 15), (FurnitureSheet, 36,   9, 15),

        // Hall NE: server hall, rack rows plus a router and a lone rack.
        (FurnitureSheet, 26,  17, 14),
        (TechSheet,      17,  17, 12),
        (TechSheet,      23,  14, 15), (TechSheet,      19,  21, 15),
        (FurnitureSheet, 36,  14, 11),

        // Sala Oeste A / Este A: storage on the north row, the south row stays clear
        // so the walkway between the door and the room is never cut.
        (FurnitureSheet, 25, -20,  5), (FurnitureSheet, 25, -18,  5),
        (FurnitureSheet, 25, -16,  5), (FurnitureSheet, 25, -14,  5),
        (FurnitureSheet, 25,  15,  5), (FurnitureSheet, 25,  17,  5),
        (FurnitureSheet, 25,  19,  5), (FurnitureSheet, 25,  21,  5),

        // Sala Oeste B / Este B: break nooks.
        (FurnitureSheet, 16, -19,  2), (FurnitureSheet, 10, -16,  3),
        (FurnitureSheet, 36, -21,  3),
        (FurnitureSheet, 16,  18,  2), (FurnitureSheet, 10,  15,  3),
        (FurnitureSheet, 36,  21,  3),

        // Nucleo central: reception facing the south entrance, elevator bank on the
        // north wall, bench seating and two glass meeting tables either side.
        (FurnitureSheet, 44,   0,  6),
        (TechSheet,       6,   0,  3),
        (FurnitureSheet, 20,  -6,  5), (FurnitureSheet, 20,   6,  5),
        (TechSheet,      43,  -4,  3), (TechSheet,      43,   4,  3),
        (FurnitureSheet, 37,  -8,  6), (FurnitureSheet, 37,   8,  6),
        (FurnitureSheet, 36,  -9,  2), (FurnitureSheet, 36,   9,  2),

        // Hall SO: meeting lounge, long table plus a sofa group.
        (FurnitureSheet,  6, -18, -5),
        (FurnitureSheet, 15, -19, -3),
        (FurnitureSheet, 13, -15, -6),
        (FurnitureSheet, 22, -21, -3), (FurnitureSheet, 36, -14, -7),

        // Oficina S1: open office, desks either side of the aisle that lines up with
        // the doorway at x -6.
        (FurnitureSheet,  2,  -8, -4), (FurnitureSheet,  2,  -4, -4),
        (FurnitureSheet,  2,  -8, -6), (FurnitureSheet,  2,  -4, -6),
        (FurnitureSheet, 36,  -9, -3), (FurnitureSheet, 36,  -3, -7),

        // Oficina S2: open office, desks on the north row and L desks on the south.
        (FurnitureSheet,  4,   4, -4), (FurnitureSheet,  4,   7, -4),
        (TechSheet,       0,   4, -6), (TechSheet,       0,   7, -6),
        (FurnitureSheet, 36,   3, -3), (FurnitureSheet, 36,   9, -7),

        // Hall SE: cafeteria, round tables leaving the row below them walkable, and
        // the service wall along the south edge.
        (FurnitureSheet, 10,  15, -3), (FurnitureSheet, 10,  19, -3),
        (FurnitureSheet, 10,  15, -5), (FurnitureSheet, 10,  19, -5),
        (FurnitureSheet, 35,  17, -7), (FurnitureSheet, 34,  21, -7),
        (FurnitureSheet, 31,  14, -7),
    };

    /// <summary>
    /// Creates (or refreshes) a furniture tile. Props keep the proportions they were
    /// drawn at instead of being normalised to one cell, so a two-cell conference
    /// table stays two cells wide.
    /// </summary>
    private static TileBase Prop(int sheet, int spriteIndex)
    {
        string key = "F" + sheet + "_" + spriteIndex;
        if (Tiles.TryGetValue(key, out var cached)) return cached;

        var sprite = PropSprite(sheet, spriteIndex);

        string path = $"{TileFolder}/NovaTech_Furn_{sheet}_{spriteIndex}.asset";
        var tile = AssetDatabase.LoadAssetAtPath<Tile>(path);
        bool created = false;
        if (tile == null)
        {
            tile = ScriptableObject.CreateInstance<Tile>();
            created = true;
        }

        tile.sprite = sprite;
        tile.colliderType = Tile.ColliderType.None;
        tile.color = Color.white;
        tile.flags = TileFlags.LockColor;
        tile.transform = PropTransform(sprite);

        if (created) AssetDatabase.CreateAsset(tile, path);
        else EditorUtility.SetDirty(tile);

        Tiles[key] = tile;
        return tile;
    }

    /// <summary>Uniform scale from source pixels to grid cells; no normalisation.</summary>
    private static Matrix4x4 PropTransform(Sprite sprite)
    {
        float unit = sprite.pixelsPerUnit <= 0f ? 100f : sprite.pixelsPerUnit;
        float scale = unit / PropCellPixels;
        return Matrix4x4.Scale(new Vector3(scale, scale, 1f));
    }

    private static Sprite PropSprite(int sheet, int spriteIndex)
    {
        var source = sheet == FurnitureSheet ? furniture
                   : sheet == TechSheet ? tech
                   : exterior;
        if (!source.TryGetValue(spriteIndex, out var sprite))
            throw new InvalidOperationException($"La hoja {sheet} no tiene el sprite {spriteIndex}.");
        return sprite;
    }

    private static void PlaceProps(Tilemap deco)
    {
        foreach (var prop in Props)
            deco.SetTile(new Vector3Int(prop.x, prop.y, 0), Prop(prop.sheet, prop.sprite));
    }

    // ---------------------------------------------------------------- exterior

    /// <summary>
    /// Builds the site around the building: lawn, paving, road and parking on their
    /// own layers, then greenery and street furniture on top. Returns the cells that
    /// must collide, so the player cannot walk through a tree, a car or the fence.
    /// </summary>
    private static HashSet<Vector2Int> BuildExterior(Tilemap ground, Tilemap paving, Tilemap props)
    {
        var solids = new HashSet<Vector2Int>();

        var lawn = new[]
        {
            TerrainTile("NovaTech_Lawn_A", TerrainSprite("NovaTech_Ground_Lawn", LawnPixel), Color.white),
            TerrainTile("NovaTech_Lawn_B", TerrainSprite("NovaTech_Ground_Lawn", LawnPixel), new Color(0.90f, 1f, 0.84f)),
            TerrainTile("NovaTech_Lawn_C", TerrainSprite("NovaTech_Ground_Lawn", LawnPixel), new Color(1f, 0.97f, 0.86f)),
        };
        var concrete = TerrainTile("NovaTech_Plaza", TerrainSprite("NovaTech_Ground_Paving", PavingPixel), Color.white);
        var asphalt = TerrainTile("NovaTech_Asphalt", TerrainSprite("NovaTech_Ground_AsphaltV2", AsphaltPixel), Color.white);
        var dash = TerrainTile("NovaTech_RoadDash", TerrainSprite("NovaTech_Ground_DashV2", DashPixel), Color.white);
        var bayLine = TerrainTile("NovaTech_BayLine", TerrainSprite("NovaTech_Ground_BayLineV2", BayPixel), Color.white);
        var steps = GetTile("NovaTech_Stairs", SpriteStairs, 0, Tile.ColliderType.None);

        var random = new System.Random(20251025);

        // Lawn over the whole plot: no cell can ever show the camera clear colour.
        // The tint is picked per 3x3 block so the lawn varies without a checkerboard.
        for (int y = ExtMinY; y <= ExtMaxY; y++)
            for (int x = ExtMinX; x <= ExtMaxX; x++)
            {
                int block = (((x / 3) * 73856093) ^ ((y / 3) * 19349663)) & 0x7fffffff;
                ground.SetTile(new Vector3Int(x, y, 0), lawn[block % 3]);
            }

        foreach (var rect in ExtPlaza) Fill(paving, rect, concrete);
        foreach (var rect in ExtRoad) Fill(paving, rect, asphalt);
        foreach (var rect in ExtSteps) Fill(paving, rect, steps);

        // Dashed centre line down the carriageway.
        for (int x = ExtMinX + 2; x <= ExtMaxX - 2; x += 2)
            paving.SetTile(new Vector3Int(x, -26, 0), dash);

        // Eight cars nose in toward the building, a painted line between each bay.
        for (int i = 0; i < 8; i++)
        {
            int bay = -14 + i * 4;
            PlaceExt(props, ExtCars[(i * 3 + 1) % ExtCars.Length], bay, -21);
            solids.Add(new Vector2Int(bay, -21));
        }
        for (int x = -16; x <= 16; x += 4)
            for (int y = -23; y <= -19; y++)
                paving.SetTile(new Vector3Int(x, y, 0), bayLine);

        // Guard booth and barrier on the south-west entrance lane.
        PlaceExt(props, ExtBooth, -26, -21);
        solids.Add(new Vector2Int(-27, -21));
        solids.Add(new Vector2Int(-26, -21));
        PlaceExt(props, ExtGatePost, -22, -19);
        solids.Add(new Vector2Int(-22, -19));

        // Lamps, benches and signals along the sidewalk.
        foreach (int x in new[] { -21, -13, 13, 21 })
        {
            PlaceExt(props, ExtLamp, x, -17);
            solids.Add(new Vector2Int(x, -17));
        }
        foreach (int x in new[] { -17, 17 })
        {
            PlaceExt(props, ExtBench, x, -17);
            solids.Add(new Vector2Int(x, -17));
        }
        foreach (int x in new[] { -11, 11 })
        {
            PlaceExt(props, ExtTraffic, x, -16);
            solids.Add(new Vector2Int(x, -16));
        }

        // Planters framing the entrance steps.
        foreach (int x in new[] { -10, -9, 9, 10 })
        {
            PlaceExt(props, ExtPlanterTall, x, -13);
            solids.Add(new Vector2Int(x, -13));
        }
        foreach (int x in new[] { -16, 16 })
            PlaceExt(props, ExtPlanterLong, x, -11);
        foreach (int x in new[] { -20, 20 })
            PlaceExt(props, ExtPlanterPink, x, -11);
        foreach (int x in new[] { -6, 6 })
        {
            PlaceExt(props, ExtBollard, x, -12);
            solids.Add(new Vector2Int(x, -12));
        }

        // Rear service yard off the north path.
        PlaceExt(props, ExtDumpster, -8, 20);
        solids.Add(new Vector2Int(-8, 20));
        PlaceExt(props, ExtDumpster, 8, 20);
        solids.Add(new Vector2Int(8, 20));
        foreach (int x in new[] { -12, 12 })
        {
            PlaceExt(props, ExtBench, x, 20);
            solids.Add(new Vector2Int(x, 20));
        }

        // East garden patio: cafe tables under the trees, fence along the boundary.
        props.SetTile(new Vector3Int(26, 1, 0), Prop(FurnitureSheet, 10));
        props.SetTile(new Vector3Int(26, -1, 0), Prop(FurnitureSheet, 10));
        solids.Add(new Vector2Int(26, 1));
        solids.Add(new Vector2Int(26, -1));
        PlaceExt(props, ExtBench, 28, 3);
        solids.Add(new Vector2Int(28, 3));
        PlaceExt(props, ExtPlanterLong, 27, 4);
        for (int y = -7; y <= 8; y += 2)
        {
            PlaceExt(props, ExtFence, 30, y);
            solids.Add(new Vector2Int(30, y));
        }

        ScatterGreenery(props, solids, random);

        // Invisible wall around the plot so the player can never leave the grounds.
        for (int x = ExtMinX; x <= ExtMaxX; x++)
        {
            solids.Add(new Vector2Int(x, ExtMinY));
            solids.Add(new Vector2Int(x, ExtMaxY));
        }
        for (int y = ExtMinY; y <= ExtMaxY; y++)
        {
            solids.Add(new Vector2Int(ExtMinX, y));
            solids.Add(new Vector2Int(ExtMaxX, y));
        }

        return solids;
    }

    /// <summary>Scatters low planting over the lawn areas, deterministically.</summary>
    private static void ScatterGreenery(Tilemap props, HashSet<Vector2Int> solids, System.Random random)
    {
        // Shrubs and flower clumps only: the tree layer was removed so the grounds stay
        // open and nothing tall crowds the plot border.
        foreach (var area in ExtGreen)
        {
            for (int y = area.y0; y <= area.y1; y++)
            {
                for (int x = area.x0; x <= area.x1; x++)
                {
                    var cell = new Vector2Int(x, y);
                    if (IsBuildingCell(cell) || IsPaved(cell)) continue;
                    if (HasProp(props, cell)) continue;
                    if (random.Next(100) >= 46) continue;
                    PlaceExt(props, ExtBushes[random.Next(ExtBushes.Length)], x, y);
                }
            }
        }
    }

    private static bool HasProp(Tilemap props, Vector2Int cell) =>
        props.GetTile(new Vector3Int(cell.x, cell.y, 0)) != null;

    /// <summary>Cells taken by the building shell itself, with a one cell margin.</summary>
    private static bool IsBuildingCell(Vector2Int cell)
    {
        if (cell.x >= -23 && cell.x <= 23 && cell.y >= -9 && cell.y <= 17) return true;
        return cell.x >= -3 && cell.x <= 3 && cell.y >= -12 && cell.y <= 20;
    }

    /// <summary>Cells already carrying a hard surface, so nothing is planted on them.</summary>
    private static bool IsPaved(Vector2Int cell)
    {
        if (IsBuildingCell(cell)) return true;
        foreach (var rect in ExtPlaza) if (Inside(cell, rect)) return true;
        foreach (var rect in ExtRoad) if (Inside(cell, rect)) return true;
        foreach (var rect in ExtSteps) if (Inside(cell, rect)) return true;
        return false;
    }

    private static bool Inside(Vector2Int cell, (int x0, int x1, int y0, int y1) rect) =>
        cell.x >= rect.x0 && cell.x <= rect.x1 && cell.y >= rect.y0 && cell.y <= rect.y1;

    private static void Fill(Tilemap map, (int x0, int x1, int y0, int y1) rect, TileBase tile)
    {
        for (int y = rect.y0; y <= rect.y1; y++)
            for (int x = rect.x0; x <= rect.x1; x++)
                map.SetTile(new Vector3Int(x, y, 0), tile);
    }

    private static void PlaceExt(Tilemap props, int spriteIndex, int x, int y) =>
        props.SetTile(new Vector3Int(x, y, 0), Prop(ExteriorSheet, spriteIndex));

    private static void BuildExteriorSolids(Tilemap solids, HashSet<Vector2Int> cells)
    {
        var solidTile = GetTile("NovaTech_Solido", SpriteSolid, 0, Tile.ColliderType.Grid);
        foreach (var cell in cells)
            solids.SetTile(new Vector3Int(cell.x, cell.y, 0), solidTile);
    }

    /// <summary>
    /// A terrain tile built from one of the generated ground textures. The sprite is
    /// authored at exactly one cell per slice, so the tile transform stays identity
    /// and neighbouring cells always meet edge to edge.
    /// </summary>
    private static TileBase TerrainTile(string assetName, Sprite sprite, Color tint)
    {
        if (Tiles.TryGetValue(assetName, out var cached)) return cached;

        string path = $"{TileFolder}/{assetName}.asset";
        var tile = AssetDatabase.LoadAssetAtPath<Tile>(path);
        bool created = false;
        if (tile == null)
        {
            tile = ScriptableObject.CreateInstance<Tile>();
            created = true;
        }

        tile.sprite = sprite;
        tile.colliderType = Tile.ColliderType.None;
        tile.color = tint;
        tile.flags = TileFlags.LockColor;
        tile.transform = Matrix4x4.identity;

        if (created) AssetDatabase.CreateAsset(tile, path);
        else EditorUtility.SetDirty(tile);

        Tiles[assetName] = tile;
        return tile;
    }

    private const int GroundSize = 128;

    /// <summary>
    /// Generates (once) a square ground texture measuring exactly one cell. The
    /// exterior sheet only ships object art — its terrain slices are portrait
    /// panels with baked corner shading — so the lawn, paving and road markings are
    /// painted here instead of being stretched out of a slab sprite.
    /// </summary>
    private static Sprite TerrainSprite(string fileName, Func<int, int, Color> paint)
    {
        string path = $"{TileFolder}/{fileName}.png";
        var existing = AssetDatabase.LoadAssetAtPath<Sprite>(path);
        if (existing != null) return existing;

        var texture = new Texture2D(GroundSize, GroundSize, TextureFormat.RGBA32, false);
        var pixels = new Color[GroundSize * GroundSize];
        for (int y = 0; y < GroundSize; y++)
            for (int x = 0; x < GroundSize; x++)
                pixels[y * GroundSize + x] = paint(x, y);
        texture.SetPixels(pixels);
        texture.Apply();
        File.WriteAllBytes(path, texture.EncodeToPNG());
        UnityEngine.Object.DestroyImmediate(texture);

        AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
        var importer = (TextureImporter)AssetImporter.GetAtPath(path);
        if (importer != null)
        {
            importer.textureType = TextureImporterType.Sprite;
            importer.spriteImportMode = SpriteImportMode.Single;
            importer.spritePixelsPerUnit = GroundSize;
            importer.filterMode = FilterMode.Bilinear;
            importer.mipmapEnabled = false;
            importer.alphaIsTransparency = true;
            importer.wrapMode = TextureWrapMode.Clamp;
            importer.SaveAndReimport();
        }

        return AssetDatabase.LoadAssetAtPath<Sprite>(path);
    }

    /// <summary>Deterministic value noise, so every rebuild paints the same ground.</summary>
    private static float Hash(int x, int y, int seed)
    {
        unchecked
        {
            int h = x * 374761393 + y * 668265263 + seed * 1442695041;
            h = (h ^ (h >> 13)) * 1274126177;
            return ((h ^ (h >> 16)) & 0x7fffffff) / (float)0x7fffffff;
        }
    }

    private static Color LawnPixel(int x, int y)
    {
        float grain = (Hash(x, y, 11) - 0.5f) * 0.055f;
        float blade = (Hash(x / 2, y / 5, 12) - 0.5f) * 0.05f;
        var c = new Color(0.24f, 0.47f, 0.19f);
        return new Color(c.r + grain, c.g + grain + blade, c.b + grain * 0.5f, 1f);
    }

    private static Color PavingPixel(int x, int y)
    {
        const int grout = 3, bevel = 9;
        float grain = (Hash(x, y, 21) - 0.5f) * 0.035f;
        Color c;
        if (x < grout || y < grout) c = new Color(0.26f, 0.27f, 0.29f);
        else if (x < grout + bevel || y < grout + bevel) c = new Color(0.56f, 0.57f, 0.59f);
        else if (x > GroundSize - bevel || y > GroundSize - bevel) c = new Color(0.43f, 0.44f, 0.46f);
        else c = new Color(0.50f, 0.51f, 0.53f);
        return new Color(c.r + grain, c.g + grain, c.b + grain, 1f);
    }

    private static Color AsphaltPixel(int x, int y)
    {
        float grain = (Hash(x, y, 31) - 0.5f) * 0.045f;
        float speck = Hash(x / 2, y / 2, 32) > 0.985f ? 0.10f : 0f;
        var c = new Color(0.26f, 0.27f, 0.29f);
        return new Color(c.r + grain + speck, c.g + grain + speck, c.b + grain + speck, 1f);
    }

    private static Color DashPixel(int x, int y)
    {
        bool inside = x > GroundSize * 0.16f && x < GroundSize * 0.84f &&
                      y > GroundSize * 0.44f && y < GroundSize * 0.56f;
        return inside ? new Color(0.95f, 0.80f, 0.22f, 1f) : AsphaltPixel(x, y);
    }

    private static Color BayPixel(int x, int y)
    {
        bool inside = x >= 3 && x < 9 && y > GroundSize * 0.06f && y < GroundSize * 0.94f;
        return inside ? new Color(0.90f, 0.91f, 0.93f, 1f) : AsphaltPixel(x, y);
    }

    /// <summary>
    /// One exact cell box per wall and window cell. The decorative tiles cannot own
    /// the collision themselves: centring their art on the cell puts a translation in
    /// the tile transform, and TilemapCollider2D bakes that into the shape, so wall
    /// faces ended up offset from the cell edge — stopping short on one side and
    /// letting the body sink into the wall on the other.
    /// </summary>
    private static void BuildWallSolids(Tilemap solids)
    {
        var solidTile = GetTile("NovaTech_Solido", SpriteSolid, 0, Tile.ColliderType.Grid);

        foreach (var cell in Wall)
            solids.SetTile(new Vector3Int(cell.x, cell.y, 0), solidTile);

        foreach (var cell in Glass)
            solids.SetTile(new Vector3Int(cell.x, cell.y, 0), solidTile);
    }

    /// <summary>
    /// wider than the single cell it is anchored on, so the cells it really covers
    /// each get a Grid collider. Cells outside the walkable floor are skipped, so a
    /// prop that overhangs a wall never throws collision into the room next door.
    /// </summary>
    private static void BuildPropSolids(Tilemap solids)
    {
        var solidTile = GetTile("NovaTech_Solido", SpriteSolid, 0, Tile.ColliderType.Grid);

        foreach (var prop in Props)
        {
            var sprite = PropSprite(prop.sheet, prop.sprite);
            float width = Mathf.Max(sprite.rect.width, 1f) / PropCellPixels;
            float height = Mathf.Max(sprite.rect.height, 1f) / PropCellPixels;
            float left = prop.x + 0.5f - width * 0.5f;
            float right = left + width;
            float bottom = prop.y + 0.5f - height * 0.5f;
            float top = bottom + height;

            int x0 = Mathf.FloorToInt(left);
            int x1 = Mathf.FloorToInt(right);
            int y0 = Mathf.FloorToInt(bottom);
            int y1 = Mathf.FloorToInt(top);

            for (int y = y0; y <= y1; y++)
                for (int x = x0; x <= x1; x++)
                {
                    float overlapX = Mathf.Min(right, x + 1f) - Mathf.Max(left, x);
                    float overlapY = Mathf.Min(top, y + 1f) - Mathf.Max(bottom, y);
                    if (overlapX < MinSolidCoverage || overlapY < MinSolidCoverage) continue;
                    if (!Floor.Contains(new Vector2Int(x, y))) continue;
                    solids.SetTile(new Vector3Int(x, y, 0), solidTile);
                }
        }
    }

    // ------------------------------------------------------------- tile assets

    private static void LoadSprites()
    {
        sprites = LoadSheet(TilesetPath);
        furniture = LoadSheet(FurnitureSheetPath);
        tech = LoadSheet(TechSheetPath);
        exterior = LoadSheet(ExteriorSheetPath);
    }

    private static Dictionary<int, Sprite> LoadSheet(string path)
    {
        var result = new Dictionary<int, Sprite>();
        string prefix = Path.GetFileNameWithoutExtension(path);
        foreach (var sprite in AssetDatabase.LoadAllAssetsAtPath(path).OfType<Sprite>())
        {
            int underscore = sprite.name.LastIndexOf('_');
            if (underscore < 0) continue;
            if (!sprite.name.StartsWith(prefix, StringComparison.Ordinal)) continue;
            if (int.TryParse(sprite.name.Substring(underscore + 1), out int index))
                result[index] = sprite;
        }
        if (result.Count == 0)
            throw new InvalidOperationException(
                $"No se encontraron sprites en {Path.GetFileName(path)}.");
        return result;
    }

    private static TileBase GetTile(string assetName, int spriteIndex, int quarterTurns,
        Tile.ColliderType collider)
    {
        string key = assetName;
        if (Tiles.TryGetValue(key, out var cached)) return cached;

        if (!sprites.TryGetValue(spriteIndex, out var sprite))
            throw new InvalidOperationException($"El tileset no tiene el sprite {spriteIndex} ({assetName}).");

        string path = $"{TileFolder}/{assetName}.asset";
        var tile = AssetDatabase.LoadAssetAtPath<Tile>(path);
        bool created = false;
        if (tile == null)
        {
            tile = ScriptableObject.CreateInstance<Tile>();
            created = true;
        }

        tile.sprite = sprite;
        tile.colliderType = collider;
        tile.color = Color.white;
        tile.flags = TileFlags.LockColor;
        // TilemapCollider2D bakes the tile transform into the shape it generates, so a
        // scaled or shifted tile collides as a shrunken rectangle instead of the cell.
        // The collision-only tile must therefore keep an identity transform.
        tile.transform = collider == Tile.ColliderType.Grid && spriteIndex == SpriteSolid
            ? Matrix4x4.identity
            : FitToCell(sprite, quarterTurns, ContentOf(spriteIndex));

        if (created) AssetDatabase.CreateAsset(tile, path);
        else EditorUtility.SetDirty(tile);

        Tiles[key] = tile;
        return tile;
    }

    /// <summary>
    /// Scales and shifts a slice so its opaque area — not its rect — lands on a
    /// one-cell square centred on the tile pivot, then turns it. Transparent margins
    /// in the source art therefore never show up as gaps between tiles.
    /// </summary>
    private static Matrix4x4 FitToCell(Sprite sprite, int quarterTurns, Rect content)
    {
        float unit = sprite.pixelsPerUnit <= 0f ? 100f : sprite.pixelsPerUnit;
        float width = Mathf.Max(sprite.rect.width, 1f) / unit;
        float height = Mathf.Max(sprite.rect.height, 1f) / unit;

        float contentWidth = Mathf.Max(content.width * width, 0.0001f);
        float contentHeight = Mathf.Max(content.height * height, 0.0001f);
        var scale = new Vector3(
            TileOverlap / contentWidth, TileOverlap / contentHeight, 1f);

        // Where the content's centre sits in the sprite's own space (pivot = centre).
        var offset = new Vector3(
            (content.x + content.width * 0.5f - 0.5f) * width * scale.x,
            (content.y + content.height * 0.5f - 0.5f) * height * scale.y,
            0f);

        var rotation = Matrix4x4.Rotate(Quaternion.Euler(0f, 0f, 90f * quarterTurns));
        return rotation * Matrix4x4.Translate(-offset) * Matrix4x4.Scale(scale);
    }

    private static Rect ContentOf(int spriteIndex) =>
        SpriteContent.TryGetValue(spriteIndex, out var content)
            ? content
            : new Rect(0f, 0f, 1f, 1f);

    private static void EnsureTileFolder()
    {
        if (!AssetDatabase.IsValidFolder(TileFolder))
        {
            var parent = Path.GetDirectoryName(TileFolder).Replace('\\', '/');
            AssetDatabase.CreateFolder(parent, Path.GetFileName(TileFolder));
        }
    }

    // ------------------------------------------------------------------ scene

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
        GameObject go = child != null
            ? child.gameObject
            : new GameObject(name, typeof(Tilemap), typeof(TilemapRenderer));
        if (child == null) go.transform.SetParent(grid.transform, false);

        var renderer = go.GetComponent<TilemapRenderer>();
        if (renderer == null) renderer = go.AddComponent<TilemapRenderer>();
        renderer.sortingOrder = sortingOrder;

        var map = go.GetComponent<Tilemap>();
        if (map == null) map = go.AddComponent<Tilemap>();
        return map;
    }

    /// <summary>Removes the decoration groups authored for the previous layout.</summary>
    private static void RemoveLegacyDecor(GameObject grid)
    {
        var decorations = grid.transform.Find("Decorations");
        if (decorations != null) UnityEngine.Object.DestroyImmediate(decorations.gameObject);

        var details = grid.transform.Find("Tilemap_Details");
        if (details != null) UnityEngine.Object.DestroyImmediate(details.gameObject);

        foreach (var stale in new[] { "Tilemap_Windows_OLD" })
        {
            var child = grid.transform.Find(stale);
            if (child != null) UnityEngine.Object.DestroyImmediate(child.gameObject);
        }
    }

    private static void ConfigureCollision(Tilemap walls, Tilemap wallTrim, Tilemap windows,
        Tilemap doors, Tilemap deco, Tilemap floor, Tilemap solids,
        Tilemap ground, Tilemap paving, Tilemap exteriorMap)
    {
        SetCollider(walls, false);
        SetCollider(wallTrim, false);
        SetCollider(windows, false);
        SetCollider(doors, false);
        SetCollider(deco, false);
        SetCollider(floor, false);
        SetCollider(ground, false);
        SetCollider(paving, false);
        SetCollider(exteriorMap, false);
        SetCollider(solids, true);
    }

    private static void SetCollider(Tilemap map, bool solid)
    {
        var collider = map.GetComponent<TilemapCollider2D>();
        if (!solid)
        {
            if (collider != null) UnityEngine.Object.DestroyImmediate(collider);
            foreach (var box in map.GetComponents<BoxCollider2D>())
                UnityEngine.Object.DestroyImmediate(box);
            return;
        }

        if (collider == null) collider = map.gameObject.AddComponent<TilemapCollider2D>();
        collider.isTrigger = false;

        // Batch edits in the editor leave the baked collider shape stale, so the
        // footprint would only be correct after entering Play mode.
        collider.ProcessTilemapChanges();
    }

    private static void ConfigureCamera(Scene scene)
    {
        var camera = GameObject.FindWithTag("MainCamera");
        if (camera == null) return;

        var framing = camera.GetComponent<MapCameraFrame>();
        if (framing == null) framing = camera.AddComponent<MapCameraFrame>();

        var player = GameObject.Find("Player");

        var serialized = new SerializedObject(framing);
        serialized.FindProperty("target").objectReferenceValue = player != null ? player.transform : null;
        serialized.FindProperty("mapCenter").vector2Value = new Vector2(
            (ExtMinX + ExtMaxX + 1) * 0.5f, (ExtMinY + ExtMaxY + 1) * 0.5f);
        serialized.FindProperty("mapSize").vector2Value = new Vector2(
            ExtMaxX - ExtMinX + 1, ExtMaxY - ExtMinY + 1);
        serialized.FindProperty("zoomSize").floatValue = 6.5f;
        serialized.FindProperty("followSmoothing").floatValue = 0.18f;
        serialized.ApplyModifiedPropertiesWithoutUndo();

        var cam = camera.GetComponent<Camera>();
        if (cam == null) return;
        cam.orthographic = true;
        cam.orthographicSize = 6.5f;
        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.backgroundColor = new Color(0.015f, 0.03f, 0.06f, 1f);

        // Start already framed on the player instead of flying in from the old centre.
        if (player != null)
            camera.transform.position = new Vector3(
                player.transform.position.x, player.transform.position.y, camera.transform.position.z);
    }

    private static void PlacePlayer(Scene scene)
    {
        var spawn = new Vector3(0f, -9f, 0f);
        foreach (var root in scene.GetRootGameObjects())
            if (root.name == "Player")
            {
                root.transform.position = spawn;
                return;
            }

        var prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Scripts/Player.prefab");
        if (prefab == null) return;
        var player = (GameObject)PrefabUtility.InstantiatePrefab(prefab, scene);
        player.name = "Player";
        player.transform.position = spawn;
    }
}

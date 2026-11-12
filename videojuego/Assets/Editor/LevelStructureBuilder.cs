using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

/// <summary>
/// Builds a readable, collision-enabled layout layer for missions 02-04.  The base
/// NovaTech facility remains intact; each scene receives its own rooms, work areas
/// and circulation route under a single root so this operation is repeatable.
/// </summary>
public static class LevelStructureBuilder
{
    private const string LevelsFolder = "Assets/Scenes/Niveles";
    private const string TilesFolder = "Assets/Scenes/Nivel01_Empresa/Tiles";

    [MenuItem("TI Soporte/Construir estructuras de niveles 02 a 04")]
    public static void BuildAll()
    {
        Build("02_ProblemasDeHardware", BuildHardware);
        Build("03_SinConexion", BuildNetwork);
        Build("04_CodigoRojo", BuildIncidentCommand);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("Estructuras de los niveles 02, 03 y 04 construidas.");
    }

    private static void Build(string sceneName, Action<Layout> design)
    {
        var scene = EditorSceneManager.OpenScene($"{LevelsFolder}/{sceneName}.unity", OpenSceneMode.Single);
        foreach (var existingRoot in scene.GetRootGameObjects())
            if (existingRoot.name.StartsWith("EstructuraDeNivel", StringComparison.Ordinal))
                UnityEngine.Object.DestroyImmediate(existingRoot);

        var root = new GameObject("EstructuraDeNivel");
        SceneManager.MoveGameObjectToScene(root, scene);
        var layout = new Layout(root, sceneName);
        design(layout);
        layout.Finish();
        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);
    }

    // Nivel 02: taller con dos bahias de diagnostico y un almacen de refacciones.
    private static void BuildHardware(Layout l)
    {
        l.FloorRect(-20, 5, 13, 9, "Taller_Hardware");
        l.Room(-20, 5, 6, 9, "Bahia_Diagnostico_A", DoorSide.Right, 4);
        l.Room(-13, 5, 6, 9, "Bahia_Diagnostico_B", DoorSide.Left, 4);
        l.Room(-20, 15, 13, 4, "Almacen_Refacciones", DoorSide.Bottom, 6);
        l.FurnitureLine(-18, 7, 4, true, "Bancos_de_prueba");
        l.FurnitureLine(-11, 7, 4, true, "Mesas_de_reparacion");
        l.FurnitureLine(-18, 16, 9, false, "Estanterias_de_componentes");
        l.HazardLine(-14, 5, 3, "Zona_de_seguridad");
        l.Label("NIVEL 02 · TALLER DE HARDWARE", new Vector3(-13.5f, 13.5f, 0));
    }

    // Nivel 03: sala de comunicaciones, racks y punto de distribucion de red.
    private static void BuildNetwork(Layout l)
    {
        l.FloorRect(8, 5, 14, 10, "Centro_de_Red");
        l.Room(8, 5, 8, 10, "Sala_de_Servidores", DoorSide.Right, 5);
        l.Room(17, 5, 5, 10, "Centro_de_Monitoreo", DoorSide.Left, 5);
        l.Room(8, 16, 14, 3, "Canal_de_Cableado", DoorSide.Bottom, 7);
        l.FurnitureLine(10, 7, 5, true, "Racks_A");
        l.FurnitureLine(13, 7, 5, true, "Racks_B");
        l.FurnitureLine(18, 7, 4, true, "Consolas_de_red");
        l.HazardLine(9, 16, 11, "Canalizaciones");
        l.Label("NIVEL 03 · CENTRO DE RED", new Vector3(15f, 13.5f, 0));
    }

    // Nivel 04: puesto de mando, sala de aislamiento y area de documentacion.
    private static void BuildIncidentCommand(Layout l)
    {
        l.FloorRect(-7, -6, 15, 11, "Centro_de_Respuesta");
        l.Room(-7, -6, 5, 11, "Aislamiento", DoorSide.Right, 5);
        l.Room(-1, -6, 9, 7, "Puesto_de_Mando", DoorSide.Bottom, 4);
        l.Room(-1, 2, 9, 3, "Registro_del_Incidente", DoorSide.Bottom, 4);
        l.FurnitureLine(-5, -4, 4, true, "Estaciones_de_cuarentena");
        l.FurnitureLine(1, -4, 5, false, "Mesa_de_coordinacion");
        l.FurnitureLine(1, 3, 5, false, "Panel_de_registro");
        l.HazardLine(-6, -6, 12, "Perimetro_rojo");
        l.Label("NIVEL 04 · CENTRO DE RESPUESTA", new Vector3(0.5f, 0.2f, 0));
    }

    private enum DoorSide { Left, Right, Bottom }

    private sealed class Layout
    {
        private readonly Tilemap floor, walls, solids, furniture, hazard;
        private readonly Transform labels;
        private readonly TileBase floorTile, wallTile, solidTile, furnitureTile, hazardTile;

        public Layout(GameObject root, string sceneName)
        {
            floor = Map(root, "Pisos", 1, false);
            furniture = Map(root, "Mobiliario", 3, false);
            hazard = Map(root, "Senalizacion", 4, false);
            walls = Map(root, "Muros", 5, false);
            solids = Map(root, "Colisiones", 0, true);
            labels = new GameObject("Rotulos").transform;
            labels.SetParent(root.transform, false);
            floorTile = Tile("NovaTech_Floor.asset");
            wallTile = Tile("NovaTech_Wall.asset");
            solidTile = Tile("NovaTech_Solido.asset");
            furnitureTile = Tile("NovaTech_Furn_3_24.asset");
            hazardTile = Tile("NovaTech_Hazard.asset");
            root.name = "EstructuraDeNivel_" + sceneName;
        }

        public void FloorRect(int x, int y, int width, int height, string _) => Fill(floor, x, y, width, height, floorTile);

        public void Room(int x, int y, int width, int height, string _, DoorSide door, int doorOffset)
        {
            for (int px = x; px < x + width; px++)
            for (int py = y; py < y + height; py++)
            {
                bool edge = px == x || px == x + width - 1 || py == y || py == y + height - 1;
                bool opening = (door == DoorSide.Left && px == x && py == y + doorOffset) ||
                               (door == DoorSide.Right && px == x + width - 1 && py == y + doorOffset) ||
                               (door == DoorSide.Bottom && py == y && px == x + doorOffset);
                if (!edge || opening) continue;
                PutWall(px, py);
            }
        }

        public void FurnitureLine(int x, int y, int length, bool vertical, string _)
        {
            for (int i = 0; i < length; i++) furniture.SetTile(new Vector3Int(x + (vertical ? 0 : i), y + (vertical ? i : 0)), furnitureTile);
        }

        public void HazardLine(int x, int y, int length, string _)
        {
            for (int i = 0; i < length; i++) hazard.SetTile(new Vector3Int(x + i, y), hazardTile);
        }

        public void Label(string value, Vector3 position)
        {
            var go = new GameObject(value);
            go.transform.SetParent(labels, false);
            go.transform.position = position;
        }

        public void Finish()
        {
            var collider = solids.GetComponent<TilemapCollider2D>();
            collider.ProcessTilemapChanges();
        }

        private void PutWall(int x, int y)
        {
            var position = new Vector3Int(x, y);
            walls.SetTile(position, wallTile);
            solids.SetTile(position, solidTile);
        }

        private static void Fill(Tilemap map, int x, int y, int width, int height, TileBase tile)
        {
            for (int px = x; px < x + width; px++)
            for (int py = y; py < y + height; py++) map.SetTile(new Vector3Int(px, py), tile);
        }

        private static Tilemap Map(GameObject parent, string name, int order, bool collidable)
        {
            var go = new GameObject(name, typeof(Tilemap), typeof(TilemapRenderer));
            go.transform.SetParent(parent.transform, false);
            go.GetComponent<TilemapRenderer>().sortingOrder = order;
            if (collidable) go.AddComponent<TilemapCollider2D>();
            return go.GetComponent<Tilemap>();
        }

        private static TileBase Tile(string file) => AssetDatabase.LoadAssetAtPath<TileBase>($"{TilesFolder}/{file}");
    }
}

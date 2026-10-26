using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;

public static class Probe1
{
    static string F(float v) => v.ToString("0.###", System.Globalization.CultureInfo.InvariantCulture);

    public static string Execute()
    {
        var sb = new System.Text.StringBuilder();

        // ---- characters
        foreach (var name in new[] { "Martin.png", "saboteador.png" })
        {
            string p = "Assets/Art/Characters/" + name;
            var ti = AssetImporter.GetAtPath(p) as TextureImporter;
            sb.AppendLine("CHAR " + name + " -> " + (ti == null ? "NO_IMPORTER" : ti.textureType + " mode=" + ti.spriteImportMode + " ppu=" + F(ti.spritePixelsPerUnit)));
            foreach (var a in AssetDatabase.LoadAllAssetsAtPath(p))
                sb.AppendLine("   asset " + a.GetType().Name + " '" + a.name + "'");
        }

        // ---- scene objects
        var grid = GameObject.Find("Grid");
        sb.AppendLine("Grid=" + (grid != null));
        var solidGo = GameObject.Find("Grid/Tilemap_Solidos");
        var solidMap = solidGo != null ? solidGo.GetComponent<Tilemap>() : null;
        var decoGo = GameObject.Find("Grid/Tilemap_Decoracion");
        var decoMap = decoGo != null ? decoGo.GetComponent<Tilemap>() : null;
        var floorGo = GameObject.Find("Grid/Tilemap_Floor");
        var floorMap = floorGo != null ? floorGo.GetComponent<Tilemap>() : null;

        int[][] probes = new int[][]
        {
            new[] { 0, 1 }, new[] { 0, 2 }, new[] { 0, 3 }, new[] { 1, 4 }, new[] { 2, 4 },
            new[] { -1, 4 }, new[] { 0, 5 }, new[] { 0, 4 }, new[] { 3, 4 }, new[] { -3, 4 },
            new[] { -6, 3 }, new[] { 0, 6 },
        };
        foreach (var c in probes)
        {
            bool solid = solidMap != null && solidMap.GetTile(new Vector3Int(c[0], c[1], 0)) != null;
            bool floor = floorMap != null && floorMap.GetTile(new Vector3Int(c[0], c[1], 0)) != null;
            bool deco = decoMap != null && decoMap.GetTile(new Vector3Int(c[0], c[1], 0)) != null;
            sb.AppendLine($"CELL ({c[0]},{c[1]}) solid={solid} floor={floor} deco={deco}");
        }

        // ---- canvases / event systems
        sb.AppendLine("Canvas count=" + UnityEngine.Object.FindObjectsByType<Canvas>(FindObjectsSortMode.None).Length);
        sb.AppendLine("EventSystem count=" + UnityEngine.Object.FindObjectsByType<UnityEngine.EventSystems.EventSystem>(FindObjectsSortMode.None).Length);
        sb.AppendLine("GameBootstrap count=" + UnityEngine.Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).Count(m => m.GetType().Name == "GameBootstrap"));

        // ---- camera
        var cam = Camera.main;
        sb.AppendLine("Camera=" + (cam != null) + " ortho=" + (cam != null && cam.orthographic) + " size=" + (cam != null ? F(cam.orthographicSize) : "-"));
        if (cam != null)
            foreach (var c in cam.GetComponents<Component>())
                sb.AppendLine("   cam comp " + c.GetType().Name);

        // ---- Light2D availability
        var lightType = AppDomain.CurrentDomain.GetAssemblies()
            .Select(a => a.GetType("UnityEngine.Rendering.Universal.Light2D", false))
            .FirstOrDefault(t => t != null);
        sb.AppendLine("Light2D type=" + (lightType != null ? lightType.FullName : "MISSING"));

        // ---- input handling
        var ps = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/ProjectSettings.asset").FirstOrDefault();
        var so = new SerializedObject(ps);
        var activeInput = so.FindProperty("activeInputHandler");
        sb.AppendLine("activeInputHandler=" + (activeInput != null ? activeInput.intValue.ToString() : "?"));

        // ---- fonts
        try { var f = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf"); sb.AppendLine("font LegacyRuntime=" + (f != null)); }
        catch (Exception e) { sb.AppendLine("font LegacyRuntime FAIL " + e.GetType().Name); }
        try { var f2 = Resources.GetBuiltinResource<Font>("Arial.ttf"); sb.AppendLine("font Arial=" + (f2 != null)); }
        catch (Exception e) { sb.AppendLine("font Arial FAIL " + e.GetType().Name); }

        // ---- render pipeline
        sb.AppendLine("renderPipeline=" + (UnityEngine.Rendering.GraphicsSettings.defaultRenderPipeline != null
            ? UnityEngine.Rendering.GraphicsSettings.defaultRenderPipeline.GetType().Name : "null"));

        // ---- player
        var player = GameObject.Find("Player");
        if (player != null)
        {
            sb.AppendLine("Player pos=" + F(player.transform.position.x) + "," + F(player.transform.position.y));
            foreach (var c in player.GetComponents<Component>())
                sb.AppendLine("   player comp " + c.GetType().Name);
        }

        // ---- lights in scene
        foreach (var l in UnityEngine.Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None))
            if (l.GetType().Name == "Light2D")
                sb.AppendLine("Light2D obj=" + l.gameObject.name + " pos=" + l.transform.position);
        sb.AppendLine("GlobalLight obj=" + (GameObject.Find("Global Light 2D") != null));

        // ---- scene root objects
        var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        foreach (var root in scene.GetRootGameObjects())
            sb.AppendLine("ROOT " + root.name + " children=" + root.transform.childCount);

        // ---- tags & layers
        sb.AppendLine("tags=" + string.Join(",", UnityEditorInternal.InternalEditorUtility.tags));
        var layers = new List<string>();
        for (int i = 0; i < 32; i++) { var n = LayerMask.LayerToName(i); if (!string.IsNullOrEmpty(n)) layers.Add(i + ":" + n); }
        sb.AppendLine("layers=" + string.Join(",", layers));

        return sb.ToString();
    }
}

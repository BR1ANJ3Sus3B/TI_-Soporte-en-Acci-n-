using System;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>Crea los cuatro niveles jugables desde el escenario base y guarda los NPCs en escena.</summary>
public static class CharacterLevelIntegrator
{
    private const string BaseScene = "Assets/Scenes/Nivel01_Empresa/nivel1.unity";
    private const string LevelsFolder = "Assets/Scenes/Niveles";
    private const string PlayerPrefab = "Assets/Scripts/Player.prefab";

    private readonly struct LevelSpec
    {
        public readonly string file;
        public readonly string mission;
        public readonly string title;
        public readonly string starter;
        public readonly string completer;
        public LevelSpec(string file, string mission, string title, string starter, string completer)
        {
            this.file = file; this.mission = mission; this.title = title;
            this.starter = starter; this.completer = completer;
        }
    }

    [MenuItem("TI Soporte/Integrar personajes en los cuatro niveles")]
    public static void Integrar()
    {
        if (!AssetDatabase.IsValidFolder(LevelsFolder))
            AssetDatabase.CreateFolder("Assets/Scenes", "Niveles");

        var levels = new[]
        {
            new LevelSpec("01_ElPrimerDia", "M01", "EL PRIMER D\u00cdA", "Mart\u00edn", "Mart\u00edn"),
            new LevelSpec("02_ProblemasDeHardware", "M02", "PROBLEMAS DE HARDWARE", "Marco", "Mart\u00edn"),
            new LevelSpec("03_SinConexion", "M03", "SIN CONEXI\u00d3N", "Sof\u00eda", "Richard"),
            new LevelSpec("04_CodigoRojo", "M04", "C\u00d3DIGO ROJO", "Richard", "Mart\u00edn")
        };

        CrearMisiones();
        foreach (var level in levels)
        {
            var path = $"{LevelsFolder}/{level.file}.unity";
            if (!AssetDatabase.LoadAssetAtPath<SceneAsset>(path) && !AssetDatabase.CopyAsset(BaseScene, path))
                throw new InvalidOperationException("No se pudo copiar la escena base: " + BaseScene);
            ConfigurarEscena(path, level);
        }

        EditorBuildSettings.scenes = new[]
        {
            new EditorBuildSettingsScene("Assets/Scenes/MainMenu.unity", true),
            new EditorBuildSettingsScene($"{LevelsFolder}/01_ElPrimerDia.unity", true),
            new EditorBuildSettingsScene($"{LevelsFolder}/02_ProblemasDeHardware.unity", true),
            new EditorBuildSettingsScene($"{LevelsFolder}/03_SinConexion.unity", true),
            new EditorBuildSettingsScene($"{LevelsFolder}/04_CodigoRojo.unity", true)
        };
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("Personajes y niveles integrados correctamente.");
    }

    [MenuItem("TI Soporte/Ubicar NPCs en sus puestos de trabajo")]
    public static void UbicarNpcsEnPuestos()
    {
        Integrar();
        Debug.Log("NPCs ubicados en sus estaciones de trabajo.");
    }

    [MenuItem("TI Soporte/Verificar personajes en los cuatro niveles")]
    public static void Verificar()
    {
        foreach (var scenePath in new[]
        {
            $"{LevelsFolder}/01_ElPrimerDia.unity",
            $"{LevelsFolder}/02_ProblemasDeHardware.unity",
            $"{LevelsFolder}/03_SinConexion.unity",
            $"{LevelsFolder}/04_CodigoRojo.unity"
        })
        {
            var scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
            var player = UnityEngine.Object.FindFirstObjectByType<PlayerMovement>();
            var npcs = UnityEngine.Object.FindObjectsByType<NpcInteractable>(FindObjectsSortMode.None);
            var names = string.Join(", ", npcs.Select(npc => npc.nombre));
            if (player == null || npcs.Length != 4)
                throw new InvalidOperationException($"Verificaci\u00f3n fallida en {scenePath}: jugador={player != null}, NPCs={npcs.Length}.");

            VerificarSprite(player.GetComponent<SpriteRenderer>()?.sprite,
                "Assets/Art/Characters/Alex Pixel Character Sprite Sheet 1.png", "Alex", scenePath);

            foreach (var npc in npcs)
            {
                var sprite = npc.GetComponentInChildren<SpriteRenderer>()?.sprite;
                VerificarSprite(sprite, RutaPersonaje(npc.nombre), npc.nombre, scenePath);
            }
            Debug.Log($"VERIFICADO {scene.name}: Alex y 4 NPCs ({names}).");
        }
    }

    private static string RutaPersonaje(string name)
    {
        if (name.StartsWith("Mart", StringComparison.OrdinalIgnoreCase)) return "Assets/Art/Characters/Martin.png";
        if (name.StartsWith("Richard", StringComparison.OrdinalIgnoreCase)) return "Assets/Art/Characters/Richard.png";
        if (name.StartsWith("Marco", StringComparison.OrdinalIgnoreCase)) return "Assets/Art/Characters/Marco.png";
        if (name.StartsWith("Sof", StringComparison.OrdinalIgnoreCase)) return "Assets/Art/Characters/Sofia.png";
        throw new InvalidOperationException("NPC sin sprite original asignado: " + name);
    }

    private static void VerificarSprite(Sprite sprite, string expectedPath, string character, string scenePath)
    {
        string currentPath = sprite == null ? string.Empty : AssetDatabase.GetAssetPath(sprite);
        if (currentPath != expectedPath)
            throw new InvalidOperationException($"Sprite incorrecto para {character} en {scenePath}: {currentPath}.");
    }

    private static void ConfigurarEscena(string path, LevelSpec level)
    {
        var scene = EditorSceneManager.OpenScene(path, OpenSceneMode.Single);
        var oldRoot = GameObject.Find("IntegracionPersonajes");
        if (oldRoot != null) UnityEngine.Object.DestroyImmediate(oldRoot);

        var root = new GameObject("IntegracionPersonajes");
        var sprites = CargarSprites();
        var registry = root.AddComponent<DialoguePortraitRegistry>();
        registry.Configurar(sprites.alex, sprites.martin, sprites.richard, sprites.marco, sprites.sofia);

        var player = UnityEngine.Object.FindFirstObjectByType<PlayerMovement>();
        if (player == null)
        {
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(PlayerPrefab);
            if (prefab == null) throw new InvalidOperationException("No se encontr\u00f3 el prefab del jugador.");
            var playerObject = PrefabUtility.InstantiatePrefab(prefab, scene) as GameObject;
            playerObject.name = "Alex";
            playerObject.transform.position = Vector3.zero;
            player = playerObject.GetComponent<PlayerMovement>();
        }

        if (level.mission != "M01")
        {
            var bootstrap = UnityEngine.Object.FindFirstObjectByType<GameBootstrap>();
            if (bootstrap != null) bootstrap.enabled = false;
            AsegurarServicios(root.transform);
        }

        var spawn = player.transform.position;
        ConfigurarMartinExistente(level, sprites.martin, PosicionNpc(level.mission, "Mart\u00edn", spawn + new Vector3(2f, 0.6f)));
        CrearNpc(root.transform, "Richard", sprites.richard, PosicionNpc(level.mission, "Richard", spawn + new Vector3(4f, 1.2f)), level, player);
        CrearNpc(root.transform, "Marco", sprites.marco, PosicionNpc(level.mission, "Marco", spawn + new Vector3(-2.4f, -1.1f)), level, player);
        CrearNpc(root.transform, "Sof\u00eda", sprites.sofia, PosicionNpc(level.mission, "Sof\u00eda", spawn + new Vector3(1.1f, -2.8f)), level, player);

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);
    }

    private static void AsegurarServicios(Transform parent)
    {
        if (UnityEngine.Object.FindFirstObjectByType<ProgressService>() == null) parent.gameObject.AddComponent<ProgressService>();
        if (UnityEngine.Object.FindFirstObjectByType<MissionService>() == null) parent.gameObject.AddComponent<MissionService>();
        if (UnityEngine.Object.FindFirstObjectByType<DialogueRunner>() == null) parent.gameObject.AddComponent<DialogueRunner>();
        if (UnityEngine.Object.FindFirstObjectByType<GameUI>() == null) parent.gameObject.AddComponent<GameUI>();
    }

    private static void ConfigurarMartinExistente(LevelSpec level, Sprite sprite, Vector3 fallbackPosition)
    {
        var martin = UnityEngine.Object.FindObjectsByType<NpcInteractable>(FindObjectsSortMode.None)
            .FirstOrDefault(npc => npc.nombre.IndexOf("Martin", StringComparison.OrdinalIgnoreCase) >= 0 || npc.nombre.IndexOf("Mart\u00edn", StringComparison.OrdinalIgnoreCase) >= 0);
        if (martin == null)
        {
            var root = GameObject.Find("IntegracionPersonajes").transform;
            CrearNpc(root, "Mart\u00edn", sprite, fallbackPosition, level, UnityEngine.Object.FindFirstObjectByType<PlayerMovement>());
            return;
        }

        martin.nombre = "Mart\u00edn";
        var visual = martin.GetComponentInChildren<SpriteRenderer>();
        if (visual != null) visual.sprite = sprite;
        if (level.mission != "M01")
        {
            martin.transform.position = fallbackPosition;
            ConfigurarDialogo(martin.gameObject, "Mart\u00edn", level, "Mart\u00edn");
        }
    }

    private static void CrearNpc(Transform parent, string name, Sprite sprite, Vector3 position, LevelSpec level, PlayerMovement player)
    {
        var go = new GameObject("NPC_" + name, typeof(SpriteRenderer), typeof(NpcInteractable), typeof(LevelNpcDialogue));
        go.transform.SetParent(parent);
        go.transform.position = position;
        var renderer = go.GetComponent<SpriteRenderer>();
        renderer.sprite = sprite;
        // Personajes sobre el mobiliario (orden 4-5), pero por debajo de los
        // marcadores de objetivo (orden 10).
        renderer.sortingOrder = 6;
        AjustarEscala(go.transform, renderer, player);
        var npc = go.GetComponent<NpcInteractable>();
        npc.nombre = name;
        ConfigurarDialogo(go, name, level, name);
    }

    private static void ConfigurarDialogo(GameObject go, string name, LevelSpec level, string role)
    {
        var dialogue = go.GetComponent<LevelNpcDialogue>();
        if (dialogue == null) dialogue = go.AddComponent<LevelNpcDialogue>();
        bool starts = string.Equals(role, level.starter, StringComparison.OrdinalIgnoreCase);
        bool completes = string.Equals(role, level.completer, StringComparison.OrdinalIgnoreCase);
        string text = Texto(level.mission, name, starts, completes);
        dialogue.Configurar(name, text, level.mission, starts, completes);
    }

    private static string Texto(string mission, string name, bool starts, bool completes)
    {
        if (starts) return mission switch
        {
            "M02" => "Tenemos un equipo con fallas de hardware. Revisa primero la energ\u00eda, los cables y los componentes antes de reemplazar nada.",
            "M03" => "La oficina perdi\u00f3 conexi\u00f3n. Comprueba el cableado, el punto de red y la configuraci\u00f3n antes de escalar el incidente.",
            "M04" => "C\u00f3digo rojo: necesitamos aislar el problema, documentar los hallazgos y avisar al equipo sin poner en riesgo los datos.",
            _ => "Bienvenido a NovaTech. Habla conmigo para recibir tu primer reporte de soporte."
        };
        if (completes) return "Buen trabajo. Tu diagn\u00f3stico fue claro y el incidente qued\u00f3 registrado como resuelto.";
        return $"Soy {name}. Mant\u00e9n la calma, observa los s\u00edntomas y documenta cada paso antes de actuar.";
    }

    private static void AjustarEscala(Transform target, SpriteRenderer npc, PlayerMovement player)
    {
        var playerRenderer = player != null ? player.GetComponent<SpriteRenderer>() : null;
        if (playerRenderer == null || playerRenderer.sprite == null || npc.sprite == null) return;
        float height = Mathf.Max(0.01f, npc.sprite.bounds.size.y);
        float desired = playerRenderer.sprite.bounds.size.y * player.transform.localScale.y * 0.95f;
        float scale = Mathf.Clamp(desired / height, 0.35f, 2f);
        target.localScale = new Vector3(scale, scale, 1f);
    }

    /// <summary>Puestos fijos, dentro de las áreas de trabajo de cada nivel.</summary>
    private static Vector3 PosicionNpc(string mission, string npc, Vector3 fallback)
    {
        if (mission == "M02") return npc switch
        {
            "Marco" => new Vector3(-17f, 8f, 0f),       // asigna el diagnóstico
            "Martín" => new Vector3(-10f, 8f, 0f),
            "Richard" => new Vector3(-17f, 16f, 0f),
            "Sofía" => new Vector3(-10f, 16f, 0f),
            _ => fallback
        };
        if (mission == "M03") return npc switch
        {
            "Sofía" => new Vector3(19f, 8f, 0f),        // asigna la red
            "Richard" => new Vector3(12f, 8f, 0f),
            "Marco" => new Vector3(12f, 12f, 0f),
            "Martín" => new Vector3(19f, 12f, 0f),
            _ => fallback
        };
        if (mission == "M04") return npc switch
        {
            "Richard" => new Vector3(2f, -3f, 0f),      // asigna el incidente
            "Martín" => new Vector3(2f, 3f, 0f),
            "Marco" => new Vector3(-5f, -3f, 0f),
            "Sofía" => new Vector3(-5f, 2f, 0f),
            _ => fallback
        };
        return fallback;
    }

    private static (Sprite alex, Sprite martin, Sprite richard, Sprite marco, Sprite sofia) CargarSprites() =>
        (PrimerSprite("Assets/Art/Characters/Alex Pixel Character Sprite Sheet 1.png", "Alex-player"),
         PrimerSprite("Assets/Art/Characters/Martin.png", "Martin_0"),
         PrimerSprite("Assets/Art/Characters/Richard.png", "Richard_0"),
         PrimerSprite("Assets/Art/Characters/Marco.png", "Marco_0"),
         PrimerSprite("Assets/Art/Characters/Sofia.png", "Sofia_0"));

    private static Sprite PrimerSprite(string path, string preferred) =>
        AssetDatabase.LoadAllAssetsAtPath(path).OfType<Sprite>().FirstOrDefault(sprite => sprite.name == preferred)
        ?? AssetDatabase.LoadAllAssetsAtPath(path).OfType<Sprite>().FirstOrDefault();

    private static void CrearMisiones()
    {
        CrearMision("M02", "02", "PROBLEMAS DE HARDWARE", "Revisa el equipo y aplica un diagn\u00f3stico de hardware responsable.");
        CrearMision("M03", "03", "SIN CONEXI\u00d3N", "Restaura la conectividad siguiendo el procedimiento de red.");
        CrearMision("M04", "04", "C\u00d3DIGO ROJO", "Atiende el incidente cr\u00edtico y documenta la respuesta del equipo.");
    }

    private static void CrearMision(string id, string number, string title, string objective)
    {
        const string folder = "Assets/ScriptableObjects/Missions";
        string path = $"{folder}/Mision{id.Substring(1)}.asset";
        var mission = AssetDatabase.LoadAssetAtPath<MissionDefinition>(path);
        if (mission == null)
        {
            mission = ScriptableObject.CreateInstance<MissionDefinition>();
            AssetDatabase.CreateAsset(mission, path);
        }
        mission.id = id;
        mission.numero = number;
        mission.titulo = title;
        mission.objetivo = objective;
        mission.recompensaXp = 125;
        mission.insigniaId = "mision_" + number;
        mission.insigniaNombre = title;
        mission.registro = "Misi\u00f3n " + number + " completada.";
        EditorUtility.SetDirty(mission);
    }
}

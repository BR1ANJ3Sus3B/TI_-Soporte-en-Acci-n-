using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

/// <summary>
/// Instancia en la escena la capa de gameplay de la misión 01: Martín, la computadora
/// reportada, el cable de alimentación, la pared falsa de la sala secreta y el objeto
/// que arranca los servicios (<see cref="GameBootstrap"/>).
///
/// Lo llama <see cref="ReferenceMapBuilder"/> al final de cada reconstrucción del mapa,
/// y es idempotente: reutiliza los objetos que ya existen en vez de duplicarlos, así que
/// el progreso guardado y las referencias serializadas sobreviven a un rebuild.
/// </summary>
public static class GameplayBuilder
{
    private const string TileFolder = "Assets/Art/Characters/Scenes/Nivel01_Empresa/Tiles";
    private const string TechSheetPath =
        "Assets/Art/Characters/Scenes/Imagen de ChatGPT 9 oct 2026, 04_16_25 p.m-3.png";
    private const string MartinPath = "Assets/Art/Characters/Martin.png";
    private const string MisionFolder = "Assets/ScriptableObjects/Missions";
    private const string MisionPath = MisionFolder + "/Mision01_ElPrimerIncidente.asset";

    // Por encima de Tilemap_Decoracion (4) y de los sólidos ocultos (5), para que los
    // personajes y el equipo nunca queden tapados por una pieza de mobiliario.
    private const int OrdenSprite = 6;

    // El mobiliario del tilemap se dibuja a 110 px por celda; un SpriteRenderer suelto
    // tiene que usar la misma escala o el equipo aparece un 10% más grande que sus vecinos.
    private const float PropCellPixels = 110f;

    // Alto en mundo del personaje jugable (Alex): su fotograma mide 103x128 px a 100 ppu.
    // Martín se escala a esta misma altura para que los dos personajes se vean iguales;
    // es una constante y no una medición en vivo para que el rebuild sea determinista
    // aunque el Animator de Alex esté mostrando otro fotograma.
    private const float AlturaPersonaje = 1.28f;

    // Celdas del layout del mapa (1 celda = 1 unidad de mundo).
    private static readonly Vector2Int CeldaComputadora = new Vector2Int(-8, -3);
    private static readonly Vector2Int CeldaCable = new Vector2Int(-7, -3);
    private static readonly Vector2Int CeldaMartin = new Vector2Int(2, 4);
    private static readonly Vector2Int CeldaParedFalsa = new Vector2Int(22, 13);

    /// <summary>Construye (o refresca) toda la capa de gameplay dentro de la escena dada.</summary>
    public static void Build(Scene scene)
    {
        var raiz = ObtenerRaiz(scene, "Gameplay");
        raiz.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);

        PrepararJugador(scene);

        var spriteComputadora = CargarSpriteDeHoja(TechSheetPath, 0);
        var spriteMartin = CargarSprite(MartinPath, "Martin_0");

        var computadora = ConstruirComputadora(raiz, spriteComputadora);
        var cable = ConstruirCable(raiz);
        var martin = ConstruirMartin(raiz, spriteMartin);
        var paredFalsa = ConstruirParedFalsa(scene, raiz);
        ConstruirBootstrap(raiz, computadora, cable, martin, paredFalsa);

        AssetDatabase.SaveAssets();
    }

    // ------------------------------------------------------------------ personajes

    /// <summary>El jugador necesita su detector de objetos interactivos (tecla E).</summary>
    private static void PrepararJugador(Scene scene)
    {
        var jugador = BuscarRaiz(scene, "Player");
        if (jugador == null)
        {
            Debug.LogWarning("[GameplayBuilder] No hay un objeto Player en la escena; " +
                             "se omite el detector de interacciones.");
            return;
        }

        if (jugador.GetComponent<PlayerInteractor>() == null)
            jugador.AddComponent<PlayerInteractor>();
    }

    // ------------------------------------------------------------------ elementos

    private static BrokenComputer ConstruirComputadora(GameObject raiz, Sprite sprite)
    {
        var go = ObtenerHijo(raiz, "Computadora_Incidente");
        go.transform.localPosition = CentroDeCelda(CeldaComputadora);

        var visual = Asegurar<SpriteRenderer>(go);
        visual.sprite = sprite;
        visual.sortingOrder = OrdenSprite;
        if (sprite != null)
            go.transform.localScale = Vector3.one * (sprite.pixelsPerUnit / PropCellPixels);

        // El marcador flotante indica al jugador dónde está el equipo reportado.
        var marcadorGo = ObtenerHijo(go, "Marcador");
        marcadorGo.transform.localPosition = new Vector3(0f, 0.9f, 0f);
        var marcadorVisual = Asegurar<SpriteRenderer>(marcadorGo);
        marcadorVisual.sortingOrder = OrdenSprite + 2;
        marcadorVisual.color = new Color(1f, 0.28f, 0.28f, 1f);
        var marcador = Asegurar<WorldMarker>(marcadorGo);

        var computadora = Asegurar<BrokenComputer>(go);
        Asignar(computadora, "marcador", marcador);
        Asignar(computadora, "visual", visual);
        return computadora;
    }

    private static PowerCable ConstruirCable(GameObject raiz)
    {
        var go = ObtenerHijo(raiz, "Cable_Alimentacion");
        go.transform.localPosition = new Vector3(
            CeldaCable.x + 0.5f, CeldaCable.y + 0.35f, 0f);

        // El sprite lo pone PowerCable.Awake con Iconos.Cable: el proyecto no tiene
        // arte de cable suelto como asset.
        var visual = Asegurar<SpriteRenderer>(go);
        visual.sortingOrder = OrdenSprite + 1;   // por encima del equipo que lo tapa

        var cable = Asegurar<PowerCable>(go);
        Asignar(cable, "visual", visual);
        return cable;
    }

    private static NpcInteractable ConstruirMartin(GameObject raiz, Sprite sprite)
    {
        var go = ObtenerHijo(raiz, "Martin");

        var visual = Asegurar<SpriteRenderer>(go);
        visual.sprite = sprite;
        visual.sortingOrder = OrdenSprite;

        // Misma altura que Alex: el sprite de Martín es más pequeño que su fotograma.
        float altoNativo = sprite != null ? sprite.bounds.size.y : AlturaPersonaje;
        float escala = AlturaPersonaje / altoNativo;
        go.transform.localScale = Vector3.one * escala;

        // Los pies del personaje se apoyan en el borde inferior de su celda, no su centro.
        float alto = altoNativo * escala;
        go.transform.localPosition = new Vector3(
            CeldaMartin.x + 0.5f, CeldaMartin.y + 0.12f + alto * 0.5f, 0f);

        var npc = Asegurar<NpcInteractable>(go);
        npc.nombre = "Martín";
        npc.siempreDisponible = true;
        AsignarTexto(npc, "textoInteraccion", "Hablar con Martín");
        AsignarFlotante(npc, "radioInteraccion", 2.2f);
        return npc;
    }

    /// <summary>
    /// La pared falsa son dos objetos: el tilemap que la dibuja —hijo de Grid, para que
    /// el arte caiga exactamente sobre la celda del hueco— y el interactivo que la
    /// bloquea, colocado en el centro de esa celda para que la distancia al jugador sea
    /// la correcta.
    /// </summary>
    private static SecretRoomDoor ConstruirParedFalsa(Scene scene, GameObject raiz)
    {
        Tilemap tilemap = null;
        var grid = BuscarRaiz(scene, "Grid");
        if (grid == null)
        {
            Debug.LogWarning("[GameplayBuilder] No hay un Grid en la escena; la pared falsa " +
                             "se quedará sin arte.");
        }
        else
        {
            var mapaGo = ObtenerHijo(grid, "Tilemap_ParedFalsa");
            var renderer = Asegurar<TilemapRenderer>(mapaGo);
            renderer.sortingOrder = 0;   // el mismo que Tilemap_Walls

            tilemap = Asegurar<Tilemap>(mapaGo);
            tilemap.ClearAllTiles();

            var tileMuro = AssetDatabase.LoadAssetAtPath<Tile>($"{TileFolder}/NovaTech_Wall.asset");
            if (tileMuro == null)
                throw new InvalidOperationException(
                    "Falta el tile NovaTech_Wall: reconstruye antes el mapa base.");
            tilemap.SetTile(new Vector3Int(CeldaParedFalsa.x, CeldaParedFalsa.y, 0), tileMuro);
        }

        var go = ObtenerHijo(raiz, "Pared_Falsa");
        go.transform.localPosition = CentroDeCelda(CeldaParedFalsa);

        var bloqueo = Asegurar<BoxCollider2D>(go);
        bloqueo.size = Vector2.one;
        bloqueo.offset = Vector2.zero;
        bloqueo.isTrigger = false;

        var pared = Asegurar<SecretRoomDoor>(go);
        Asignar(pared, "bloqueo", bloqueo);
        Asignar(pared, "paredFalsa", tilemap);
        return pared;
    }

    private static void ConstruirBootstrap(GameObject raiz, BrokenComputer computadora,
        PowerCable cable, NpcInteractable martin, SecretRoomDoor paredFalsa)
    {
        var go = ObtenerHijo(raiz, "Bootstrap");
        var bootstrap = Asegurar<GameBootstrap>(go);

        Asignar(bootstrap, "mision01", AsegurarMision());
        Asignar(bootstrap, "computadora", computadora);
        Asignar(bootstrap, "cable", cable);
        Asignar(bootstrap, "martin", martin);
        Asignar(bootstrap, "paredFalsa", paredFalsa);
    }

    /// <summary>Crea el asset de datos de la misión si todavía no existe.</summary>
    private static MissionDefinition AsegurarMision()
    {
        if (!AssetDatabase.IsValidFolder("Assets/ScriptableObjects"))
            AssetDatabase.CreateFolder("Assets", "ScriptableObjects");
        if (!AssetDatabase.IsValidFolder(MisionFolder))
            AssetDatabase.CreateFolder("Assets/ScriptableObjects", "Missions");

        var definicion = AssetDatabase.LoadAssetAtPath<MissionDefinition>(MisionPath);
        if (definicion == null)
        {
            definicion = ScriptableObject.CreateInstance<MissionDefinition>();
            AssetDatabase.CreateAsset(definicion, MisionPath);
        }

        definicion.id = "M01";
        definicion.numero = "01";
        definicion.titulo = "EL PRIMER INCIDENTE";
        definicion.objetivo =
            "Dirígete a la oficina de soporte y revisa la computadora reportada.";
        definicion.recompensaXp = 100;
        definicion.insigniaId = "primer_diagnostico";
        definicion.insigniaNombre = "Primer Diagnóstico";
        definicion.registro = "Incidente 001 resuelto: cable de alimentación desconectado.";
        EditorUtility.SetDirty(definicion);

        return definicion;
    }

    // --------------------------------------------------------------------- apoyo

    private static Vector3 CentroDeCelda(Vector2Int celda) =>
        new Vector3(celda.x + 0.5f, celda.y + 0.5f, 0f);

    private static GameObject ObtenerRaiz(Scene scene, string nombre)
    {
        var existente = BuscarRaiz(scene, nombre);
        if (existente != null) return existente;

        var go = new GameObject(nombre);
        SceneManager.MoveGameObjectToScene(go, scene);
        return go;
    }

    private static GameObject BuscarRaiz(Scene scene, string nombre)
    {
        foreach (var root in scene.GetRootGameObjects())
            if (root.name == nombre) return root;
        return null;
    }

    private static GameObject ObtenerHijo(GameObject padre, string nombre)
    {
        var hijo = padre.transform.Find(nombre);
        if (hijo != null) return hijo.gameObject;

        var go = new GameObject(nombre);
        go.transform.SetParent(padre.transform, false);
        return go;
    }

    private static T Asegurar<T>(GameObject go) where T : Component
    {
        var componente = go.GetComponent<T>();
        return componente != null ? componente : go.AddComponent<T>();
    }

    private static void Asignar(UnityEngine.Object destino, string campo, UnityEngine.Object valor)
    {
        if (destino == null) return;

        var serializado = new SerializedObject(destino);
        var propiedad = serializado.FindProperty(campo);
        if (propiedad == null)
        {
            Debug.LogWarning($"[GameplayBuilder] {destino.GetType().Name} no tiene el campo {campo}.");
            return;
        }

        propiedad.objectReferenceValue = valor;
        serializado.ApplyModifiedPropertiesWithoutUndo();
    }

    private static void AsignarFlotante(UnityEngine.Object destino, string campo, float valor)
    {
        if (destino == null) return;

        var serializado = new SerializedObject(destino);
        var propiedad = serializado.FindProperty(campo);
        if (propiedad == null)
        {
            Debug.LogWarning($"[GameplayBuilder] {destino.GetType().Name} no tiene el campo {campo}.");
            return;
        }

        propiedad.floatValue = valor;
        serializado.ApplyModifiedPropertiesWithoutUndo();
    }

    private static void AsignarTexto(UnityEngine.Object destino, string campo, string valor)
    {
        if (destino == null) return;

        var serializado = new SerializedObject(destino);
        var propiedad = serializado.FindProperty(campo);
        if (propiedad == null)
        {
            Debug.LogWarning($"[GameplayBuilder] {destino.GetType().Name} no tiene el campo {campo}.");
            return;
        }

        propiedad.stringValue = valor;
        serializado.ApplyModifiedPropertiesWithoutUndo();
    }

    private static Sprite CargarSprite(string path, string nombre)
    {
        var sprite = AssetDatabase.LoadAllAssetsAtPath(path)
            .OfType<Sprite>()
            .FirstOrDefault(s => s.name == nombre);

        if (sprite == null)
            Debug.LogWarning($"[GameplayBuilder] {nombre} no existe en {path}.");

        return sprite;
    }

    /// <summary>
    /// Los índices de sprite son el contrato estable de las hojas: el nombre termina en
    /// _N, y los rects auto-sliced no sirven para deducir el orden.
    /// </summary>
    private static Sprite CargarSpriteDeHoja(string path, int indice)
    {
        var sprites = new Dictionary<int, Sprite>();
        string prefijo = System.IO.Path.GetFileNameWithoutExtension(path);

        foreach (var sprite in AssetDatabase.LoadAllAssetsAtPath(path).OfType<Sprite>())
        {
            if (!sprite.name.StartsWith(prefijo, StringComparison.Ordinal)) continue;
            int guion = sprite.name.LastIndexOf('_');
            if (guion < 0) continue;
            if (int.TryParse(sprite.name.Substring(guion + 1), out int valor))
                sprites[valor] = sprite;
        }

        if (sprites.TryGetValue(indice, out var encontrado)) return encontrado;

        Debug.LogWarning($"[GameplayBuilder] {System.IO.Path.GetFileName(path)} no tiene el sprite {indice}.");
        return null;
    }
}

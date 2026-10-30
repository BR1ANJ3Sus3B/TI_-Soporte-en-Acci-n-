using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Events;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Construye la escena del menú principal de *TI: Soporte en Acción* con la
/// estética arcade neón de la referencia: fondo de oficina nocturna, título en
/// dos líneas con contorno, tres orbes brillantes (verde · naranja · morado)
/// sobre placas de nombre, y columnas laterales de orbes azules de icono.
///
/// Es re-ejecutable: sobrescribe la escena y reimporta el arte recortado que falte.
/// </summary>
public static class ConstructorMenuPrincipal
{
    private const string CarpetaGeneradas = "Assets/Generated_Images";
    private const string CarpetaMenu = "Assets/Generated_Images/Menu";
    private const string RutaEscena = "Assets/Scenes/MainMenu.unity";
    private const string EscenaJuego = "nivel1";
    private const string VersionArte = "2";

    // Paleta arcade
    private static readonly Color Cian = new Color(0.35f, 0.93f, 1f, 1f);
    private static readonly Color CianClaro = new Color(0.74f, 0.98f, 1f, 1f);
    private static readonly Color Verde = new Color(0.30f, 0.95f, 0.45f, 1f);
    private static readonly Color Naranja = new Color(1f, 0.68f, 0.14f, 1f);
    private static readonly Color Morado = new Color(0.78f, 0.42f, 1f, 1f);
    private static readonly Color Azul = new Color(0.30f, 0.62f, 1f, 1f);
    private static readonly Color Panel = new Color(0.010f, 0.026f, 0.055f, 1f);
    private static readonly Color PanelClaro = new Color(0.024f, 0.059f, 0.110f, 0.94f);
    private static readonly Color Blanco = new Color(0.97f, 0.99f, 1f, 1f);
    private static readonly Color TintaContorno = new Color(0.02f, 0.05f, 0.13f, 1f);

    private static Font fuente;
    private static bool forzarRecorte;

    private enum Modo { Fondo, Orbe, Linea, Croma }

    public static string Execute()
    {
        Directory.CreateDirectory(CarpetaMenu);
        AssetDatabase.Refresh();

        // Si cambia la clave de alfa hay que volver a recortar el arte ya copiado.
        string marca = $"{CarpetaMenu}/.arte_{VersionArte}.txt";
        forzarRecorte = !File.Exists(marca);

        fuente = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

        Sprite fondo = SpriteDe("arc_bg_oficina", Modo.Fondo);
        Sprite orbJugar = SpriteDe("arc_orbe_jugar", Modo.Croma);
        Sprite orbMisiones = SpriteDe("arc_orbe_misiones", Modo.Croma);
        Sprite orbAjustes = SpriteDe("arc_orbe_ajustes", Modo.Croma);
        Sprite icoMusica = SpriteDe("arc_ico_musica", Modo.Croma);
        Sprite icoSonido = SpriteDe("arc_ico_sonido", Modo.Croma);
        Sprite icoPerfil = SpriteDe("arc_ico_perfil", Modo.Croma);
        Sprite icoConfig = SpriteDe("arc_ico_config", Modo.Croma);
        Sprite icoSalir = SpriteDe("arc_ico_salir", Modo.Croma);

        if (forzarRecorte)
        {
            File.WriteAllText($"{CarpetaMenu}/.arte_{VersionArte}.txt", VersionArte);
            forzarRecorte = false;
        }

        // La escena actual puede estar sucia: se guarda sin preguntar para que
        // NewScene no abra un diálogo modal que bloquee la ejecución.
        Scene actual = SceneManager.GetActiveScene();
        if (actual.isDirty && !string.IsNullOrEmpty(actual.path))
            EditorSceneManager.SaveScene(actual);

        Scene escena = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

        // ------------------------------------------------------------------ cámara y eventos
        var camaraGo = new GameObject("Main Camera");
        camaraGo.tag = "MainCamera";
        camaraGo.transform.position = new Vector3(0f, 0f, -10f);
        var camara = camaraGo.AddComponent<Camera>();
        camara.orthographic = true;
        camara.orthographicSize = 5f;
        camara.clearFlags = CameraClearFlags.SolidColor;
        camara.backgroundColor = new Color(0.008f, 0.024f, 0.055f, 1f);
        camaraGo.AddComponent<AudioListener>();

        var eventosGo = new GameObject("EventSystem");
        eventosGo.AddComponent<EventSystem>();
        eventosGo.AddComponent<StandaloneInputModule>();

        // ------------------------------------------------------------------ canvas
        var canvasGo = new GameObject("MenuPrincipal");
        var canvas = canvasGo.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        var escalador = canvasGo.AddComponent<CanvasScaler>();
        escalador.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        escalador.referenceResolution = new Vector2(1920f, 1080f);
        escalador.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        escalador.matchWidthOrHeight = 0.5f;
        canvasGo.AddComponent<GraphicRaycaster>();
        var raiz = (RectTransform)canvasGo.transform;

        // ------------------------------------------------------------------ fondo
        RectTransform rtFondo = Estirar(Nuevo("Fondo", raiz));
        Imagen(rtFondo, fondo, Color.white);

        // Oscurecido suave: el fondo ya es nocturno, basta con asentar la UI.
        RectTransform rtTinte = Estirar(Nuevo("Tinte", raiz));
        Imagen(rtTinte, null, new Color(0.008f, 0.024f, 0.055f, 0.42f));

        // ------------------------------------------------------------------ cabecera
        RectTransform rtCabecera = Nuevo("Cabecera", raiz);
        rtCabecera.anchorMin = new Vector2(0.5f, 1f);
        rtCabecera.anchorMax = new Vector2(0.5f, 1f);
        rtCabecera.pivot = new Vector2(0.5f, 1f);
        rtCabecera.anchoredPosition = new Vector2(0f, -30f);
        rtCabecera.sizeDelta = new Vector2(1320f, 430f);

        // Halos neón: dos capas por fuera del marco, muy transparentes.
        Imagen(Estirar(Nuevo("Halo_Cian_Lejos", rtCabecera), -48f), null,
            new Color(Cian.r, Cian.g, Cian.b, 0.05f));
        Imagen(Estirar(Nuevo("Halo_Cian", rtCabecera), -18f), null,
            new Color(Cian.r, Cian.g, Cian.b, 0.11f));

        RectTransform rtMarco = Estirar(Nuevo("Marco", rtCabecera));
        Imagen(rtMarco, null, Panel);
        Marco(rtMarco, Cian, 4f);
        Marco(Estirar(Nuevo("Marco_Interior", rtMarco), 10f),
            new Color(Cian.r, Cian.g, Cian.b, 0.32f), 2f);
        Esquinas(rtMarco, CianClaro, 58f, 5f);

        // Trazas de circuito en las esquinas: verdes arriba (como la referencia),
        // cian abajo; cada traza termina en un nodo cuadrado.
        Circuito(rtMarco, new Vector2(-1f, 1f), Verde);
        Circuito(rtMarco, new Vector2(1f, 1f), Verde);
        Circuito(rtMarco, new Vector2(-1f, -1f), Cian);
        Circuito(rtMarco, new Vector2(1f, -1f), Cian);

        // Título en dos líneas con contorno grueso oscuro y halo de color.
        RectTransform rtLinea1 = Nuevo("Titulo", rtMarco);
        rtLinea1.anchorMin = new Vector2(0f, 1f);
        rtLinea1.anchorMax = new Vector2(1f, 1f);
        rtLinea1.pivot = new Vector2(0.5f, 1f);
        rtLinea1.offsetMin = new Vector2(40f, -206f);
        rtLinea1.offsetMax = new Vector2(-40f, -30f);
        Text titulo = Texto(rtLinea1, "<color=#8AF4FF>TI:</color> SOPORTE", 132, Blanco,
            TextAnchor.MiddleCenter);
        Contorno(titulo.gameObject, TintaContorno, 5f, 1f);
        Contorno(titulo.gameObject, TintaContorno, 10f, 0.9f);
        Contorno(titulo.gameObject, new Color(Cian.r, Cian.g, Cian.b, 0.22f), 18f, 1f);

        RectTransform rtLinea2 = Nuevo("Subtitulo", rtMarco);
        rtLinea2.anchorMin = new Vector2(0f, 1f);
        rtLinea2.anchorMax = new Vector2(1f, 1f);
        rtLinea2.pivot = new Vector2(0.5f, 1f);
        rtLinea2.offsetMin = new Vector2(40f, -322f);
        rtLinea2.offsetMax = new Vector2(-40f, -206f);
        Text subtitulo = Texto(rtLinea2, "EN ACCIÓN", 120, Naranja, TextAnchor.MiddleCenter);
        Contorno(subtitulo.gameObject, TintaContorno, 5f, 1f);
        Contorno(subtitulo.gameObject, TintaContorno, 10f, 0.9f);
        Contorno(subtitulo.gameObject, new Color(Naranja.r, Naranja.g, Naranja.b, 0.28f), 18f, 1f);

        RectTransform rtLema = Nuevo("Lema", rtMarco);
        rtLema.anchorMin = new Vector2(0f, 0f);
        rtLema.anchorMax = new Vector2(1f, 0f);
        rtLema.pivot = new Vector2(0.5f, 0f);
        rtLema.offsetMin = new Vector2(40f, 22f);
        rtLema.offsetMax = new Vector2(-40f, 76f);
        Texto(rtLema, "N O V A T E C H   S O L U T I O N S   ·   S O P O R T E   T É C N I C O",
            26, CianClaro, TextAnchor.MiddleCenter);

        // ------------------------------------------------------------------ botones principales
        RectTransform rtBotones = Nuevo("Botones", raiz);
        rtBotones.anchorMin = new Vector2(0.5f, 0f);
        rtBotones.anchorMax = new Vector2(0.5f, 0f);
        rtBotones.pivot = new Vector2(0.5f, 0f);
        rtBotones.anchoredPosition = new Vector2(0f, 46f);
        rtBotones.sizeDelta = new Vector2(1660f, 460f);

        Button btnJugar = BotonOrbe(rtBotones, "Boton_Jugar", orbJugar, Naranja,
            new Vector2(0f, 0f), 320f, "JUGAR", 48, true, out RectTransform rtOrbeJugar);
        Button btnMisiones = BotonOrbe(rtBotones, "Boton_Misiones", orbMisiones, Verde,
            new Vector2(-505f, 0f), 250f, "MISIONES", 36, false, out _);
        Button btnAjustes = BotonOrbe(rtBotones, "Boton_Ajustes", orbAjustes, Morado,
            new Vector2(505f, 0f), 250f, "AJUSTES", 36, false, out _);

        // ------------------------------------------------------------------ columna izquierda
        RectTransform rtIzquierda = Nuevo("Lateral_Izquierda", raiz);
        rtIzquierda.anchorMin = new Vector2(0f, 1f);
        rtIzquierda.anchorMax = new Vector2(0f, 1f);
        rtIzquierda.pivot = new Vector2(0f, 1f);
        rtIzquierda.anchoredPosition = new Vector2(72f, -44f);
        rtIzquierda.sizeDelta = new Vector2(132f, 330f);

        Button btnMusica = BotonIcono(rtIzquierda, "Boton_Musica", icoMusica,
            new Vector2(0f, 0f), false, out Image imgIconoMusica);
        Button btnSonido = BotonIcono(rtIzquierda, "Boton_Sonido", icoSonido,
            new Vector2(0f, -156f), false, out Image imgIconoSonido);

        // ------------------------------------------------------------------ columna derecha
        RectTransform rtDerecha = Nuevo("Lateral_Derecha", raiz);
        rtDerecha.anchorMin = new Vector2(1f, 1f);
        rtDerecha.anchorMax = new Vector2(1f, 1f);
        rtDerecha.pivot = new Vector2(1f, 1f);
        rtDerecha.anchoredPosition = new Vector2(-72f, -44f);
        rtDerecha.sizeDelta = new Vector2(132f, 490f);

        Button btnPerfil = BotonIcono(rtDerecha, "Boton_Perfil", icoPerfil,
            new Vector2(0f, 0f), true, out _);
        Button btnConfig = BotonIcono(rtDerecha, "Boton_Config", icoConfig,
            new Vector2(0f, -156f), true, out _);
        Button btnSalir = BotonIcono(rtDerecha, "Boton_Salir", icoSalir,
            new Vector2(0f, -312f), true, out _);

        // ------------------------------------------------------------------ pie técnico
        RectTransform rtPieIzq = Nuevo("Pie_Izquierda", raiz);
        rtPieIzq.anchorMin = Vector2.zero;
        rtPieIzq.anchorMax = Vector2.zero;
        rtPieIzq.pivot = Vector2.zero;
        rtPieIzq.anchoredPosition = new Vector2(34f, 14f);
        rtPieIzq.sizeDelta = new Vector2(560f, 26f);
        Texto(rtPieIzq, "TI: SOPORTE EN ACCIÓN   ·   v1.0   ·   NOVATECH SOLUTIONS",
            20, new Color(Cian.r, Cian.g, Cian.b, 0.45f), TextAnchor.MiddleLeft);

        RectTransform rtPieDer = Nuevo("Pie_Derecha", raiz);
        rtPieDer.anchorMin = new Vector2(1f, 0f);
        rtPieDer.anchorMax = new Vector2(1f, 0f);
        rtPieDer.pivot = new Vector2(1f, 0f);
        rtPieDer.anchoredPosition = new Vector2(-34f, 14f);
        rtPieDer.sizeDelta = new Vector2(560f, 26f);
        Texto(rtPieDer, "SOPORTE TÉCNICO NIVEL 1   ·   SESIÓN SEGURA",
            20, new Color(Cian.r, Cian.g, Cian.b, 0.45f), TextAnchor.MiddleRight);

        // ------------------------------------------------------------------ aviso modal (nivel)
        RectTransform rtAviso = Estirar(Nuevo("Aviso", raiz));
        Image imgAviso = Imagen(rtAviso, null, new Color(0.004f, 0.014f, 0.032f, 0.78f));
        imgAviso.raycastTarget = true;

        RectTransform rtVentana = Nuevo("Ventana", rtAviso);
        rtVentana.anchorMin = new Vector2(0.5f, 0.5f);
        rtVentana.anchorMax = new Vector2(0.5f, 0.5f);
        rtVentana.pivot = new Vector2(0.5f, 0.5f);
        rtVentana.anchoredPosition = Vector2.zero;
        rtVentana.sizeDelta = new Vector2(940f, 470f);
        Imagen(Estirar(Nuevo("Halo_Ventana", rtVentana), -22f), null,
            new Color(Cian.r, Cian.g, Cian.b, 0.10f));
        Imagen(rtVentana, null, Panel);
        Marco(rtVentana, Cian, 3f);
        Esquinas(rtVentana, CianClaro, 34f, 4f);

        RectTransform rtAvisoTitulo = Nuevo("Aviso_Titulo", rtVentana);
        rtAvisoTitulo.anchorMin = new Vector2(0f, 1f);
        rtAvisoTitulo.anchorMax = new Vector2(1f, 1f);
        rtAvisoTitulo.pivot = new Vector2(0.5f, 1f);
        rtAvisoTitulo.offsetMin = new Vector2(48f, -110f);
        rtAvisoTitulo.offsetMax = new Vector2(-48f, -36f);
        Text txtAvisoTitulo = Texto(rtAvisoTitulo, "MISIONES", 52, CianClaro, TextAnchor.MiddleLeft);
        Contorno(txtAvisoTitulo.gameObject, TintaContorno, 4f, 0.9f);

        RectTransform rtAvisoTexto = Nuevo("Aviso_Texto", rtVentana);
        rtAvisoTexto.anchorMin = Vector2.zero;
        rtAvisoTexto.anchorMax = Vector2.one;
        rtAvisoTexto.pivot = new Vector2(0.5f, 0.5f);
        rtAvisoTexto.offsetMin = new Vector2(48f, 130f);
        rtAvisoTexto.offsetMax = new Vector2(-48f, -122f);
        Text txtAvisoTexto = Texto(rtAvisoTexto, "—", 30, new Color(0.85f, 0.93f, 0.98f, 1f),
            TextAnchor.UpperLeft, FontStyle.Normal);
        txtAvisoTexto.horizontalOverflow = HorizontalWrapMode.Wrap;
        txtAvisoTexto.verticalOverflow = VerticalWrapMode.Truncate;

        RectTransform rtCerrar = Nuevo("Boton_Cerrar", rtVentana);
        rtCerrar.anchorMin = new Vector2(0.5f, 0f);
        rtCerrar.anchorMax = new Vector2(0.5f, 0f);
        rtCerrar.pivot = new Vector2(0.5f, 0f);
        rtCerrar.anchoredPosition = new Vector2(0f, 38f);
        rtCerrar.sizeDelta = new Vector2(300f, 78f);
        Image imgCerrar = Imagen(rtCerrar, null, PanelClaro, false, true);
        Marco(rtCerrar, Cian, 3f);
        Esquinas(rtCerrar, CianClaro, 22f, 3f);
        Button btnCerrar = Boton(rtCerrar, imgCerrar);
        RectTransform rtCerrarTexto = Estirar(Nuevo("Texto", rtCerrar));
        Text txtCerrar = Texto(rtCerrarTexto, "CERRAR", 32, Blanco, TextAnchor.MiddleCenter);
        Contorno(txtCerrar.gameObject, TintaContorno, 3f, 0.9f);

        // La música vive como AudioSource sin clip: el botón ya deja el sistema
        // cableado y basta con arrastrar un AudioClip para que suene.
        var musicaGo = new GameObject("Musica");
        musicaGo.transform.SetParent(raiz, false);
        var fuenteMusica = musicaGo.AddComponent<AudioSource>();
        fuenteMusica.playOnAwake = false;
        fuenteMusica.loop = true;
        fuenteMusica.spatialBlend = 0f;
        fuenteMusica.volume = 0.6f;

        // ------------------------------------------------------------------ controlador
        var controlador = canvasGo.AddComponent<MainMenuController>();
        var so = new SerializedObject(controlador);
        so.FindProperty("escenaJuego").stringValue = EscenaJuego;
        so.FindProperty("musica").objectReferenceValue = fuenteMusica;
        so.FindProperty("iconoMusica").objectReferenceValue = imgIconoMusica;
        so.FindProperty("iconoSonido").objectReferenceValue = imgIconoSonido;
        so.FindProperty("aviso").objectReferenceValue = rtAviso.gameObject;
        so.FindProperty("avisoTitulo").objectReferenceValue = txtAvisoTitulo;
        so.FindProperty("avisoTexto").objectReferenceValue = txtAvisoTexto;
        so.FindProperty("orbePrincipal").objectReferenceValue = rtOrbeJugar;
        so.ApplyModifiedPropertiesWithoutUndo();

        UnityEventTools.AddPersistentListener(btnJugar.onClick, controlador.Jugar);
        UnityEventTools.AddPersistentListener(btnMisiones.onClick, controlador.Misiones);
        UnityEventTools.AddPersistentListener(btnAjustes.onClick, controlador.Ajustes);
        UnityEventTools.AddPersistentListener(btnMusica.onClick, controlador.AlternarMusica);
        UnityEventTools.AddPersistentListener(btnSonido.onClick, controlador.AlternarSonido);
        UnityEventTools.AddPersistentListener(btnPerfil.onClick, controlador.Perfil);
        UnityEventTools.AddPersistentListener(btnConfig.onClick, controlador.ConfiguracionRapida);
        UnityEventTools.AddPersistentListener(btnSalir.onClick, controlador.Salir);
        UnityEventTools.AddPersistentListener(btnCerrar.onClick, controlador.CerrarAviso);

        rtAviso.gameObject.SetActive(false);

        // ------------------------------------------------------------------ guardado y build settings
        EditorSceneManager.MarkSceneDirty(escena);
        EditorSceneManager.SaveScene(escena, RutaEscena);

        var lista = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);
        lista.RemoveAll(s => s.path == RutaEscena);
        lista.Insert(0, new EditorBuildSettingsScene(RutaEscena, true));
        EditorBuildSettings.scenes = lista.ToArray();

        AssetDatabase.SaveAssets();

        return "MENU OK\n" +
               "escena        = " + RutaEscena + "\n" +
               "build index   = 0\n" +
               "botones       = 9 (3 orbes + 5 laterales + cerrar)\n" +
               "escena juego  = " + EscenaJuego + "\n" +
               "arte recortado= " + CarpetaMenu;
    }

    // ---------------------------------------------------------------------- botones

    /// <summary>
    /// Orbe brillante con placa de nombre debajo. El resplandor se pinta antes
    /// que el orbe para que quede detrás; la placa se pinta después para que el
    /// orbe parezca apoyado sobre ella.
    /// </summary>
    private static Button BotonOrbe(RectTransform padre, string nombre, Sprite orbe, Color color,
        Vector2 posicion, float diametro, string etiqueta, int tamanoTexto, bool principal,
        out RectTransform rtOrbe)
    {
        float anchoPlaca = principal ? 380f : 300f;

        RectTransform contenedor = Nuevo(nombre, padre);
        contenedor.anchorMin = new Vector2(0.5f, 0f);
        contenedor.anchorMax = new Vector2(0.5f, 0f);
        contenedor.pivot = new Vector2(0.5f, 0f);
        contenedor.anchoredPosition = posicion;
        contenedor.sizeDelta = new Vector2(diametro, diametro + 66f);

        // El propio arte del orbe ya trae su resplandor horneado: no se añade una
        // copia ampliada porque el bloom de la imagen llega hasta los bordes del
        // sprite y se vería como un recuadro de color.
        rtOrbe = Nuevo("Orbe", contenedor);
        rtOrbe.anchorMin = new Vector2(0.5f, 1f);
        rtOrbe.anchorMax = new Vector2(0.5f, 1f);
        rtOrbe.pivot = new Vector2(0.5f, 1f);
        rtOrbe.anchoredPosition = Vector2.zero;
        rtOrbe.sizeDelta = new Vector2(diametro, diametro);

        Image imgOrbe = Imagen(rtOrbe, orbe, Color.white, true, true);
        Button boton = Boton(rtOrbe, imgOrbe);

        Placa(contenedor, etiqueta, tamanoTexto, color, anchoPlaca, principal);

        return boton;
    }

    /// <summary>Placa de nombre: navy, filo del color del orbe, escuadras y barra inferior.</summary>
    private static void Placa(RectTransform padre, string etiqueta, int tamano, Color color,
        float ancho, bool principal)
    {
        RectTransform rt = Nuevo("Base_Etiqueta", padre);
        rt.anchorMin = new Vector2(0.5f, 0f);
        rt.anchorMax = new Vector2(0.5f, 0f);
        rt.pivot = new Vector2(0.5f, 0f);
        rt.anchoredPosition = Vector2.zero;
        rt.sizeDelta = new Vector2(ancho, 78f);
        Imagen(rt, null, Panel);
        Marco(rt, color, principal ? 4f : 3f);
        Marco(Estirar(Nuevo("Marco_Interior", rt), 7f), new Color(color.r, color.g, color.b, 0.35f), 1.5f);
        Esquinas(rt, color, 22f, 4f);

        RectTransform rtTexto = Estirar(Nuevo("Etiqueta", rt));
        rtTexto.offsetMin = new Vector2(0f, 8f);
        Text texto = Texto(rtTexto, etiqueta, tamano, Blanco, TextAnchor.MiddleCenter);
        Contorno(texto.gameObject, TintaContorno, 3f, 1f);
        Contorno(texto.gameObject, new Color(color.r, color.g, color.b, 0.55f), 9f, 1f);

        // Barra luminosa bajo el texto.
        Linea(rt, new Vector2(0f, 9f), new Vector2(ancho - 60f, 4f),
            new Color(color.r, color.g, color.b, 0.85f));
    }

    /// <summary>Botón circular de icono (orbe azul): el propio orbe es el gráfico clicable.</summary>
    private static Button BotonIcono(RectTransform padre, string nombre, Sprite icono,
        Vector2 posicion, bool desdeDerecha, out Image imagenIcono)
    {
        RectTransform rt = Nuevo(nombre, padre);
        rt.anchorMin = new Vector2(desdeDerecha ? 1f : 0f, 1f);
        rt.anchorMax = rt.anchorMin;
        rt.pivot = new Vector2(desdeDerecha ? 1f : 0f, 1f);
        rt.anchoredPosition = posicion;
        rt.sizeDelta = new Vector2(132f, 132f);

        RectTransform rtHalo = Nuevo("Halo", rt);
        Estirar(rtHalo, -18f);
        Imagen(rtHalo, icono, new Color(Azul.r, Azul.g, Azul.b, 0.22f), true);

        RectTransform rtIcono = Estirar(Nuevo("Icono", rt));
        imagenIcono = Imagen(rtIcono, icono, Color.white, true, true);

        return Boton(rtIcono, imagenIcono);
    }

    // ---------------------------------------------------------------------- utilidades UI

    private static Button Boton(RectTransform rt, Graphic grafico)
    {
        var boton = rt.gameObject.AddComponent<Button>();
        boton.targetGraphic = grafico;
        boton.transition = Selectable.Transition.ColorTint;

        var colores = boton.colors;
        colores.normalColor = Color.white;
        colores.highlightedColor = new Color(1f, 0.97f, 0.90f, 1f);
        colores.pressedColor = new Color(0.80f, 0.85f, 0.92f, 1f);
        colores.selectedColor = Color.white;
        colores.disabledColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);
        colores.fadeDuration = 0.08f;
        boton.colors = colores;

        return boton;
    }

    private static RectTransform Nuevo(string nombre, Transform padre)
    {
        var go = new GameObject(nombre, typeof(RectTransform));
        go.transform.SetParent(padre, false);
        go.layer = LayerMask.NameToLayer("UI");
        return (RectTransform)go.transform;
    }

    private static RectTransform Estirar(RectTransform rt, float margen = 0f)
    {
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.offsetMin = new Vector2(margen, margen);
        rt.offsetMax = new Vector2(-margen, -margen);
        return rt;
    }

    private static Image Imagen(RectTransform rt, Sprite sprite, Color color,
        bool aspecto = false, bool raycast = false)
    {
        var imagen = rt.gameObject.AddComponent<Image>();
        imagen.sprite = sprite;
        imagen.color = color;
        imagen.preserveAspect = aspecto;
        imagen.raycastTarget = raycast;
        return imagen;
    }

    private static Text Texto(RectTransform rt, string contenido, int tamano, Color color,
        TextAnchor anclaje, FontStyle estilo = FontStyle.Bold)
    {
        var texto = rt.gameObject.AddComponent<Text>();
        texto.font = fuente;
        texto.text = contenido;
        texto.fontSize = tamano;
        texto.color = color;
        texto.alignment = anclaje;
        texto.fontStyle = estilo;
        texto.supportRichText = true;
        texto.horizontalOverflow = HorizontalWrapMode.Overflow;
        texto.verticalOverflow = VerticalWrapMode.Overflow;
        texto.raycastTarget = false;
        return texto;
    }

    /// <summary>Copia desplazada del texto: se apilan varias para lograr contorno grueso.</summary>
    private static void Contorno(GameObject go, Color color, float distancia, float opacidad)
    {
        var contorno = go.AddComponent<Outline>();
        contorno.effectColor = new Color(color.r, color.g, color.b, opacidad);
        contorno.effectDistance = new Vector2(distancia, distancia);
        contorno.useGraphicAlpha = true;
    }

    /// <summary>Marco real de 4 filos (Outline solo desplaza una copia y no se ve).</summary>
    private static void Marco(RectTransform panel, Color color, float grosor)
    {
        var arriba = Nuevo("Filo_Arriba", panel);
        arriba.anchorMin = new Vector2(0f, 1f);
        arriba.anchorMax = new Vector2(1f, 1f);
        arriba.pivot = new Vector2(0.5f, 1f);
        arriba.sizeDelta = new Vector2(0f, grosor);
        arriba.anchoredPosition = Vector2.zero;
        Imagen(arriba, null, color);

        var abajo = Nuevo("Filo_Abajo", panel);
        abajo.anchorMin = new Vector2(0f, 0f);
        abajo.anchorMax = new Vector2(1f, 0f);
        abajo.pivot = new Vector2(0.5f, 0f);
        abajo.sizeDelta = new Vector2(0f, grosor);
        abajo.anchoredPosition = Vector2.zero;
        Imagen(abajo, null, color);

        var izquierda = Nuevo("Filo_Izquierda", panel);
        izquierda.anchorMin = new Vector2(0f, 0f);
        izquierda.anchorMax = new Vector2(0f, 1f);
        izquierda.pivot = new Vector2(0f, 0.5f);
        izquierda.sizeDelta = new Vector2(grosor, 0f);
        izquierda.anchoredPosition = Vector2.zero;
        Imagen(izquierda, null, color);

        var derecha = Nuevo("Filo_Derecha", panel);
        derecha.anchorMin = new Vector2(1f, 0f);
        derecha.anchorMax = new Vector2(1f, 1f);
        derecha.pivot = new Vector2(1f, 0.5f);
        derecha.sizeDelta = new Vector2(grosor, 0f);
        derecha.anchoredPosition = Vector2.zero;
        Imagen(derecha, null, color);
    }

    /// <summary>Cuatro escuadras en las esquinas: el detalle de HUD tecnológico.</summary>
    private static void Esquinas(RectTransform panel, Color color, float largo, float grosor)
    {
        for (int i = 0; i < 4; i++)
        {
            float sx = (i % 2 == 0) ? -1f : 1f;
            float sy = (i < 2) ? 1f : -1f;
            var ancla = new Vector2(sx < 0f ? 0f : 1f, sy > 0f ? 1f : 0f);

            var h = Nuevo("Escuadra_H", panel);
            h.anchorMin = ancla;
            h.anchorMax = ancla;
            h.pivot = ancla;
            h.anchoredPosition = new Vector2(sx * 9f, sy * 9f);
            h.sizeDelta = new Vector2(largo, grosor);
            Imagen(h, null, color);

            var v = Nuevo("Escuadra_V", panel);
            v.anchorMin = ancla;
            v.anchorMax = ancla;
            v.pivot = ancla;
            v.anchoredPosition = new Vector2(sx * 9f, sy * 9f);
            v.sizeDelta = new Vector2(grosor, largo);
            Imagen(v, null, color);
        }
    }

    /// <summary>Traza de circuito en una esquina: línea horizontal y nodo cuadrado al final.</summary>
    private static void Circuito(RectTransform panel, Vector2 ancla, Color color)
    {
        var linea = Nuevo("Circuito", panel);
        linea.anchorMin = new Vector2(ancla.x < 0f ? 0f : 1f, ancla.y > 0f ? 1f : 0f);
        linea.anchorMax = linea.anchorMin;
        linea.pivot = linea.anchorMin;
        linea.anchoredPosition = new Vector2(-ancla.x * 26f, -ancla.y * 26f);
        linea.sizeDelta = new Vector2(130f, 4f);
        Imagen(linea, null, color);

        var nodo = Nuevo("Nodo_Circuito", panel);
        nodo.anchorMin = linea.anchorMin;
        nodo.anchorMax = linea.anchorMin;
        nodo.pivot = linea.anchorMin;
        nodo.anchoredPosition = new Vector2(-ancla.x * 26f - ancla.x * 130f, -ancla.y * 26f);
        nodo.sizeDelta = new Vector2(11f, 11f);
        Imagen(nodo, null, color);
    }

    private static void Linea(RectTransform padre, Vector2 posicion, Vector2 tamano, Color color)
    {
        var rt = Nuevo("Linea", padre);
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition = posicion;
        rt.sizeDelta = tamano;
        Imagen(rt, null, color);
    }

    // ---------------------------------------------------------------------- arte

    private static Sprite SpriteDe(string nombre, Modo modo)
    {
        string origen = $"{CarpetaGeneradas}/{nombre}.png";
        if (!File.Exists(origen))
            throw new FileNotFoundException("Falta la imagen generada: " + origen);

        if (modo == Modo.Fondo)
        {
            Configurar(origen, false);
            return AssetDatabase.LoadAssetAtPath<Sprite>(origen);
        }

        string destino = $"{CarpetaMenu}/{nombre}.png";
        bool caduco = forzarRecorte || !File.Exists(destino) ||
            File.GetLastWriteTimeUtc(origen) > File.GetLastWriteTimeUtc(destino);

        if (caduco)
            Recortar(origen, destino, modo);

        Configurar(destino, true);
        return AssetDatabase.LoadAssetAtPath<Sprite>(destino);
    }

    /// <summary>
    /// Las imágenes generadas salen con fondo negro opaco. Se convierte el negro en
    /// transparencia usando el canal más alto como clave: el brillo neón de los orbes
    /// conserva su alfa y el fondo desaparece sin dejar borde.
    /// </summary>
    private static void Recortar(string origen, string destino, Modo modo)
    {
        var textura = new Texture2D(2, 2, TextureFormat.RGBA32, false);
        textura.LoadImage(File.ReadAllBytes(origen));

        int ancho = textura.width;
        int alto = textura.height;
        Color32[] pixeles = textura.GetPixels32();

        float centroX = (ancho - 1) * 0.5f;
        float centroY = (alto - 1) * 0.5f;
        float radio = Mathf.Min(ancho, alto) * 0.5f;

        for (int y = 0; y < alto; y++)
        {
            for (int x = 0; x < ancho; x++)
            {
                int i = y * ancho + x;
                Color32 c = pixeles[i];
                float alfa = c.a / 255f;

                if (modo == Modo.Croma)
                {
                    // Clave de croma: el negro del fondo desaparece según su brillo.
                    float maximo = Mathf.Max(c.r, Mathf.Max(c.g, c.b)) / 255f;
                    alfa *= Mathf.Clamp01((maximo - 0.10f) / 0.10f);

                    // Caída circular: elimina el residuo cuadrado que deja el fondo.
                    float ex = (x - centroX) / (ancho * 0.5f);
                    float ey = (y - centroY) / (alto * 0.5f);
                    float radial = Mathf.Sqrt(ex * ex + ey * ey);
                    alfa *= Mathf.Clamp01((1f - radial) / 0.10f);
                }
                else if (modo == Modo.Orbe)
                {
                    float dx = (x - centroX) / radio;
                    float dy = (y - centroY) / radio;
                    float distancia = Mathf.Sqrt(dx * dx + dy * dy);
                    alfa *= Mathf.Clamp01((1f - distancia) / 0.06f);
                }
                else
                {
                    float luma = (0.299f * c.r + 0.587f * c.g + 0.114f * c.b) / 255f;
                    alfa *= Mathf.Clamp01((luma - 0.16f) / 0.12f);
                }

                pixeles[i] = new Color32(c.r, c.g, c.b,
                    (byte)Mathf.RoundToInt(Mathf.Clamp01(alfa) * 255f));
            }
        }

        textura.SetPixels32(pixeles);
        textura.Apply();
        File.WriteAllBytes(destino, textura.EncodeToPNG());
        Object.DestroyImmediate(textura);

        AssetDatabase.ImportAsset(destino, ImportAssetOptions.ForceUpdate);
    }

    private static void Configurar(string ruta, bool alfa)
    {
        if (!(AssetImporter.GetAtPath(ruta) is TextureImporter importador))
        {
            AssetDatabase.ImportAsset(ruta, ImportAssetOptions.ForceUpdate);
            importador = AssetImporter.GetAtPath(ruta) as TextureImporter;
            if (importador == null)
                throw new IOException("No se pudo importar " + ruta);
        }

        importador.textureType = TextureImporterType.Sprite;
        importador.spriteImportMode = SpriteImportMode.Single;
        importador.alphaIsTransparency = alfa;
        importador.mipmapEnabled = false;
        importador.filterMode = FilterMode.Bilinear;
        importador.textureCompression = TextureImporterCompression.Uncompressed;
        importador.maxTextureSize = 2048;
        importador.SaveAndReimport();
    }
}

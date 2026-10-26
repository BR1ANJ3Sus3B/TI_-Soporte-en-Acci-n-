using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Construye en tiempo de ejecución toda la interfaz del juego (HUD, objetivo, aviso de
/// interacción, diálogos, ventana de diagnóstico y cartel de misión) y expone una API
/// pequeña para que los sistemas de misión no sepan nada de uGUI.
/// </summary>
public class GameUI : MonoBehaviour
{
    public static GameUI Instancia { get; private set; }

    private static readonly Color PanelFondo = new Color(0.05f, 0.07f, 0.12f, 0.95f);
    private static readonly Color PanelBorde = new Color(0.20f, 0.58f, 0.82f, 1f);
    private static readonly Color TextoClaro = new Color(0.93f, 0.96f, 1f, 1f);
    private static readonly Color TextoSuave = new Color(0.60f, 0.70f, 0.80f, 1f);
    private static readonly Color Acento = new Color(0.36f, 0.86f, 1f, 1f);
    private static readonly Color Bien = new Color(0.42f, 1f, 0.55f, 1f);
    private static readonly Color Mal = new Color(1f, 0.46f, 0.42f, 1f);
    private static readonly Color BotonFondo = new Color(0.11f, 0.17f, 0.27f, 1f);

    private Font fuente;
    private Canvas canvas;

    private Text hudXp;
    private Text hudResumen;

    private GameObject objetivoPanel;
    private Text objetivoTitulo;
    private Text objetivoTexto;

    private GameObject promptPanel;
    private Text promptTexto;

    private GameObject dialogoPanel;
    private Text dialogoNombre;
    private Text dialogoTexto;

    private GameObject diagPanel;
    private Text diagTitulo;
    private Text diagReporte;
    private readonly List<Button> diagBotones = new List<Button>();
    private readonly List<Text> diagEtiquetas = new List<Text>();
    private Action<int> diagCallback;
    private int diagFrame;

    private GameObject avisoPanel;
    private Text avisoTitulo;
    private Text avisoTexto;
    private Button avisoBoton;
    private Action avisoCallback;
    private int avisoFrame;

    private GameObject toastPanel;
    private Text toastTexto;
    private float toastFin;

    void Awake()
    {
        if (Instancia != null && Instancia != this)
        {
            Destroy(this);
            return;
        }
        Instancia = this;
        Construir();
    }

    void OnDestroy()
    {
        if (Instancia == this) Instancia = null;
    }

    // ------------------------------------------------------------------ construcción

    private void Construir()
    {
        fuente = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

        AsegurarEventSystem();

        var canvasGo = new GameObject("Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        canvasGo.transform.SetParent(transform, false);
        canvas = canvasGo.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 100;

        var escalador = canvasGo.GetComponent<CanvasScaler>();
        escalador.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        escalador.referenceResolution = new Vector2(1920f, 1080f);
        escalador.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        escalador.matchWidthOrHeight = 0.5f;

        ConstruirHud(canvasGo.transform);
        ConstruirObjetivo(canvasGo.transform);
        ConstruirToast(canvasGo.transform);
        ConstruirPrompt(canvasGo.transform);
        ConstruirDialogo(canvasGo.transform);
        ConstruirDiagnostico(canvasGo.transform);
        ConstruirAviso(canvasGo.transform);

        ActualizarBloqueo();
    }

    private static void AsegurarEventSystem()
    {
        if (FindFirstObjectByType<EventSystem>() != null) return;
        var go = new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
        go.transform.SetAsFirstSibling();
    }

    private void ConstruirHud(Transform padre)
    {
        var panel = CrearPanel(padre, "HUD", new Vector2(0f, 1f), new Vector2(0f, 1f),
            new Vector2(24f, -24f), new Vector2(380f, 112f), PanelFondo);

        hudXp = CrearTextoEnFranja(panel, "HUD_XP", 26, TextAnchor.UpperLeft, Acento, 10f, 40f);
        hudXp.text = "XP: 0";

        hudResumen = CrearTextoEnFranja(panel, "HUD_Resumen", 20, TextAnchor.UpperLeft, TextoSuave, 54f, 50f);
        hudResumen.text = "Insignias: 0   Misiones: 0";
    }

    private void ConstruirObjetivo(Transform padre)
    {
        objetivoPanel = CrearPanel(padre, "Objetivo", new Vector2(0.5f, 1f), new Vector2(0.5f, 1f),
            new Vector2(0f, -24f), new Vector2(820f, 108f), PanelFondo).gameObject;

        objetivoTitulo = CrearTextoEnFranja(objetivoPanel.transform, "Objetivo_Titulo", 26,
            TextAnchor.UpperCenter, Acento, 10f, 36f);
        objetivoTitulo.text = "MISIÓN 01";

        objetivoTexto = CrearTextoEnFranja(objetivoPanel.transform, "Objetivo_Texto", 21,
            TextAnchor.UpperCenter, TextoClaro, 48f, 54f);

        objetivoPanel.SetActive(false);
    }

    private void ConstruirToast(Transform padre)
    {
        toastPanel = CrearPanel(padre, "Toast", new Vector2(0.5f, 1f), new Vector2(0.5f, 1f),
            new Vector2(0f, -148f), new Vector2(820f, 76f), new Color(0.10f, 0.05f, 0.16f, 0.94f)).gameObject;

        toastTexto = CrearTextoEnFranja(toastPanel.transform, "Toast_Texto", 21,
            TextAnchor.MiddleCenter, new Color(0.94f, 0.86f, 1f, 1f), 6f, 64f);

        toastPanel.SetActive(false);
    }

    private void ConstruirPrompt(Transform padre)
    {
        promptPanel = CrearPanel(padre, "Prompt", new Vector2(0.5f, 0f), new Vector2(0.5f, 0f),
            new Vector2(0f, 244f), new Vector2(700f, 54f), PanelFondo).gameObject;

        promptTexto = CrearTextoEnFranja(promptPanel.transform, "Prompt_Texto", 22,
            TextAnchor.MiddleCenter, TextoClaro, 4f, 46f);

        promptPanel.SetActive(false);
    }

    private void ConstruirDialogo(Transform padre)
    {
        dialogoPanel = CrearPanel(padre, "Dialogo", new Vector2(0.5f, 0f), new Vector2(0.5f, 0f),
            new Vector2(0f, 32f), new Vector2(1240f, 200f), PanelFondo).gameObject;

        dialogoNombre = CrearTextoEnFranja(dialogoPanel.transform, "Dialogo_Nombre", 25,
            TextAnchor.UpperLeft, Acento, 10f, 36f);

        dialogoTexto = CrearTextoEnFranja(dialogoPanel.transform, "Dialogo_Texto", 24,
            TextAnchor.UpperLeft, TextoClaro, 54f, 100f);

        var pista = CrearTextoEnFranja(dialogoPanel.transform, "Dialogo_Pista", 16,
            TextAnchor.LowerRight, TextoSuave, 160f, 30f);
        pista.text = "E / Espacio / clic para continuar";

        dialogoPanel.SetActive(false);
    }

    private void ConstruirDiagnostico(Transform padre)
    {
        diagPanel = CrearPanel(padre, "Diagnostico", new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
            Vector2.zero, new Vector2(1060f, 660f), PanelFondo).gameObject;

        diagTitulo = CrearTextoEnFranja(diagPanel.transform, "Diag_Titulo", 28,
            TextAnchor.UpperCenter, Acento, 16f, 42f);

        diagReporte = CrearTextoEnFranja(diagPanel.transform, "Diag_Reporte", 21,
            TextAnchor.UpperLeft, TextoClaro, 68f, 168f);

        for (int i = 0; i < 4; i++)
        {
            int indice = i;
            float arriba = 244f + i * 74f;
            var boton = CrearBoton(diagPanel.transform, "Diag_Opcion_" + i, "Opción",
                arriba, 66f, 24, TextAnchor.MiddleLeft, () => ElegirOpcion(indice));
            diagBotones.Add(boton);
            diagEtiquetas.Add(boton.GetComponentInChildren<Text>());
        }

        var pie = CrearTextoEnFranja(diagPanel.transform, "Diag_Pie", 17,
            TextAnchor.LowerCenter, TextoSuave, 552f, 32f);
        pie.text = "Pulsa 1-4 o haz clic en una opción";

        diagPanel.SetActive(false);
    }

    private void ConstruirAviso(Transform padre)
    {
        avisoPanel = CrearPanel(padre, "Aviso", new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
            Vector2.zero, new Vector2(960f, 440f), PanelFondo).gameObject;

        avisoTitulo = CrearTextoEnFranja(avisoPanel.transform, "Aviso_Titulo", 40,
            TextAnchor.UpperCenter, Bien, 22f, 64f);

        avisoTexto = CrearTextoEnFranja(avisoPanel.transform, "Aviso_Texto", 24,
            TextAnchor.UpperCenter, TextoClaro, 96f, 210f);

        avisoBoton = CrearBoton(avisoPanel.transform, "Aviso_Boton", "Continuar",
            0f, 60f, 24, TextAnchor.MiddleCenter, ContinuarAviso);
        var rtBoton = avisoBoton.GetComponent<RectTransform>();
        rtBoton.anchorMin = new Vector2(0.5f, 0f);
        rtBoton.anchorMax = new Vector2(0.5f, 0f);
        rtBoton.pivot = new Vector2(0.5f, 0f);
        rtBoton.sizeDelta = new Vector2(280f, 60f);
        rtBoton.anchoredPosition = new Vector2(0f, 26f);

        avisoPanel.SetActive(false);
    }

    // ---------------------------------------------------------------- construccion UI

    private RectTransform CrearPanel(Transform padre, string nombre, Vector2 ancla, Vector2 pivote,
        Vector2 posicion, Vector2 tamano, Color fondo)
    {
        var go = new GameObject(nombre, typeof(RectTransform), typeof(Image), typeof(Outline));
        go.transform.SetParent(padre, false);

        var rt = go.GetComponent<RectTransform>();
        rt.anchorMin = ancla;
        rt.anchorMax = ancla;
        rt.pivot = pivote;
        rt.anchoredPosition = posicion;
        rt.sizeDelta = tamano;

        var imagen = go.GetComponent<Image>();
        imagen.color = fondo;
        imagen.raycastTarget = false;

        var borde = go.GetComponent<Outline>();
        borde.effectColor = PanelBorde;
        borde.effectDistance = new Vector2(2f, 2f);

        return rt;
    }

    private Text CrearTextoEnFranja(Transform padre, string nombre, int tamano, TextAnchor alineacion,
        Color color, float arriba, float altura)
    {
        var go = new GameObject(nombre, typeof(RectTransform), typeof(Text));
        go.transform.SetParent(padre, false);

        var rt = go.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(0f, 1f);
        rt.anchorMax = new Vector2(1f, 1f);
        rt.pivot = new Vector2(0.5f, 1f);
        rt.sizeDelta = new Vector2(-40f, altura);
        rt.anchoredPosition = new Vector2(0f, -arriba);

        var texto = go.GetComponent<Text>();
        texto.font = fuente;
        texto.fontSize = tamano;
        texto.alignment = alineacion;
        texto.color = color;
        texto.horizontalOverflow = HorizontalWrapMode.Wrap;
        texto.verticalOverflow = VerticalWrapMode.Overflow;
        texto.supportRichText = true;
        texto.raycastTarget = false;
        return texto;
    }

    private Button CrearBoton(Transform padre, string nombre, string etiqueta, float arriba,
        float altura, int tamanoFuente, TextAnchor alineacion, Action alPulsar)
    {
        var go = new GameObject(nombre, typeof(RectTransform), typeof(Image), typeof(Button));
        go.transform.SetParent(padre, false);

        var rt = go.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(0f, 1f);
        rt.anchorMax = new Vector2(1f, 1f);
        rt.pivot = new Vector2(0.5f, 1f);
        rt.sizeDelta = new Vector2(-48f, altura);
        rt.anchoredPosition = new Vector2(0f, -arriba);

        var imagen = go.GetComponent<Image>();
        imagen.color = BotonFondo;
        imagen.raycastTarget = true;

        var boton = go.GetComponent<Button>();
        boton.targetGraphic = imagen;
        var colores = boton.colors;
        colores.normalColor = BotonFondo;
        colores.highlightedColor = new Color(0.20f, 0.44f, 0.66f, 1f);
        colores.pressedColor = new Color(0.28f, 0.62f, 0.86f, 1f);
        colores.selectedColor = new Color(0.20f, 0.44f, 0.66f, 1f);
        colores.fadeDuration = 0.08f;
        boton.colors = colores;
        boton.onClick.AddListener(() => alPulsar?.Invoke());

        var texto = new GameObject("Texto", typeof(RectTransform), typeof(Text));
        texto.transform.SetParent(go.transform, false);
        var rtTexto = texto.GetComponent<RectTransform>();
        rtTexto.anchorMin = Vector2.zero;
        rtTexto.anchorMax = Vector2.one;
        rtTexto.offsetMin = new Vector2(18f, 6f);
        rtTexto.offsetMax = new Vector2(-18f, -6f);

        var etiquetaTexto = texto.GetComponent<Text>();
        etiquetaTexto.font = fuente;
        etiquetaTexto.fontSize = tamanoFuente;
        etiquetaTexto.alignment = alineacion;
        etiquetaTexto.color = TextoClaro;
        etiquetaTexto.horizontalOverflow = HorizontalWrapMode.Wrap;
        etiquetaTexto.verticalOverflow = VerticalWrapMode.Overflow;
        etiquetaTexto.raycastTarget = false;
        etiquetaTexto.text = etiqueta;

        return boton;
    }

    // ----------------------------------------------------------------------- update

    void Update()
    {
        if (toastPanel != null && toastPanel.activeSelf && Time.unscaledTime >= toastFin)
            toastPanel.SetActive(false);

        if (diagPanel != null && diagPanel.activeSelf && Time.frameCount != diagFrame)
        {
            for (int i = 0; i < diagBotones.Count; i++)
                if (Input.GetKeyDown(KeyCode.Alpha1 + i) || Input.GetKeyDown(KeyCode.Keypad1 + i))
                {
                    ElegirOpcion(i);
                    break;
                }
        }

        if (avisoPanel != null && avisoPanel.activeSelf && Time.frameCount != avisoFrame &&
            (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter) ||
             Input.GetKeyDown(KeyCode.Space)))
            ContinuarAviso();
    }

    private void ActualizarBloqueo()
    {
        bool modal = (dialogoPanel != null && dialogoPanel.activeSelf)
                  || (diagPanel != null && diagPanel.activeSelf)
                  || (avisoPanel != null && avisoPanel.activeSelf);

        if (modal) GameInput.Bloquear();
        else GameInput.Liberar();
    }

    // -------------------------------------------------------------------------- HUD

    public void ActualizarHud(int xp, int insignias, int misionesCompletadas)
    {
        if (hudXp != null) hudXp.text = "XP: " + xp;
        if (hudResumen != null)
            hudResumen.text = $"Insignias: {insignias}   Misiones: {misionesCompletadas}";
    }

    public void MostrarObjetivo(string titulo, string texto)
    {
        if (objetivoPanel == null) return;
        if (objetivoTitulo != null) objetivoTitulo.text = titulo;
        if (objetivoTexto != null) objetivoTexto.text = texto;
        objetivoPanel.SetActive(true);
    }

    public void OcultarObjetivo()
    {
        if (objetivoPanel != null) objetivoPanel.SetActive(false);
    }

    public void MostrarPrompt(string texto)
    {
        if (promptPanel == null) return;

        if (string.IsNullOrEmpty(texto))
        {
            promptPanel.SetActive(false);
            return;
        }

        promptPanel.SetActive(true);
        if (promptTexto != null) promptTexto.text = "[E] " + texto;
    }

    public void MostrarToast(string mensaje, float segundos = 4.5f)
    {
        if (toastPanel == null || string.IsNullOrEmpty(mensaje)) return;
        if (toastTexto != null) toastTexto.text = mensaje;
        toastPanel.transform.SetAsLastSibling();
        toastPanel.SetActive(true);
        toastFin = Time.unscaledTime + Mathf.Max(1f, segundos);
    }

    // ---------------------------------------------------------------------- diálogo

    public void MostrarDialogo(string hablante, string texto)
    {
        if (dialogoPanel == null) return;
        if (dialogoNombre != null) dialogoNombre.text = hablante;
        if (dialogoTexto != null) dialogoTexto.text = texto;
        dialogoPanel.transform.SetAsLastSibling();
        dialogoPanel.SetActive(true);
        ActualizarBloqueo();
    }

    public void OcultarDialogo()
    {
        if (dialogoPanel != null) dialogoPanel.SetActive(false);
        ActualizarBloqueo();
    }

    // ----------------------------------------------------------------- diagnóstico

    public void MostrarDiagnostico(string titulo, string reporte, string[] opciones, Action<int> alElegir)
    {
        if (diagPanel == null)
        {
            alElegir?.Invoke(0);
            return;
        }

        diagCallback = alElegir;
        if (diagTitulo != null) diagTitulo.text = titulo;
        if (diagReporte != null) diagReporte.text = reporte;

        for (int i = 0; i < diagBotones.Count; i++)
        {
            bool hay = opciones != null && i < opciones.Length;
            diagBotones[i].gameObject.SetActive(hay);
            if (hay && diagEtiquetas[i] != null) diagEtiquetas[i].text = opciones[i];
        }

        diagPanel.transform.SetAsLastSibling();
        diagPanel.SetActive(true);
        diagFrame = Time.frameCount;
        ActualizarBloqueo();
    }

    public void OcultarDiagnostico()
    {
        if (diagPanel != null) diagPanel.SetActive(false);
        diagCallback = null;
        ActualizarBloqueo();
    }

    private void ElegirOpcion(int indice)
    {
        var callback = diagCallback;
        if (diagPanel != null) diagPanel.SetActive(false);
        diagCallback = null;
        ActualizarBloqueo();
        callback?.Invoke(indice);
    }

    // -------------------------------------------------------------------------- aviso

    public void MostrarAviso(string mensaje, Color color, Action alContinuar = null)
    {
        MostrarPanelAviso(string.Empty, mensaje, color, alContinuar);
    }

    public void MostrarBanner(string titulo, string detalle, Action alContinuar = null)
    {
        MostrarPanelAviso(titulo, detalle, Bien, alContinuar);
    }

    private void MostrarPanelAviso(string titulo, string detalle, Color color, Action alContinuar)
    {
        if (avisoPanel == null)
        {
            alContinuar?.Invoke();
            return;
        }

        avisoCallback = alContinuar;

        if (avisoTitulo != null)
        {
            avisoTitulo.gameObject.SetActive(!string.IsNullOrEmpty(titulo));
            avisoTitulo.text = titulo;
            avisoTitulo.color = color;
        }

        if (avisoTexto != null)
        {
            avisoTexto.text = detalle;
            avisoTexto.color = string.IsNullOrEmpty(titulo) ? color : TextoClaro;
        }

        avisoPanel.transform.SetAsLastSibling();
        avisoPanel.SetActive(true);
        avisoFrame = Time.frameCount;
        ActualizarBloqueo();
    }

    private void ContinuarAviso()
    {
        if (avisoPanel == null || !avisoPanel.activeSelf) return;

        var callback = avisoCallback;
        avisoCallback = null;
        avisoPanel.SetActive(false);
        ActualizarBloqueo();
        callback?.Invoke();
    }
}

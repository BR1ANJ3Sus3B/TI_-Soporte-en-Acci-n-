using UnityEngine;

/// <summary>
/// Equipo de trabajo visible para las misiones de los niveles 02 a 04. Se crea al
/// aceptar una misión y requiere una interacción real antes de permitir reportarla.
/// </summary>
public sealed class MissionWorkstation : InteractableBase
{
    [SerializeField] private string misionId;
    [SerializeField] private string nombreEquipo;
    [SerializeField] private SpriteRenderer indicadorInteraccion;
    private Transform jugador;
    private bool resuelto;

    public override string TextoInteraccion => resuelto
        ? nombreEquipo + " revisado"
        : "Revisar " + nombreEquipo;

    public override bool PuedeInteractuar => base.PuedeInteractuar && !resuelto &&
        MissionService.Instancia != null && MissionService.Instancia.Estado(misionId) == MissionStatus.EnProgreso;

    public static void CrearSiNoExiste(string idMision, Vector3 posicion)
    {
        if (string.IsNullOrEmpty(idMision) || Buscar(idMision) != null) return;

        var go = new GameObject("Equipo_" + idMision, typeof(SpriteRenderer), typeof(BoxCollider2D),
            typeof(MissionWorkstation));
        go.transform.position = EncontrarPosicionLibre(posicion);
        var equipo = go.GetComponent<MissionWorkstation>();
        equipo.misionId = idMision;
        equipo.nombreEquipo = NombreEquipo(idMision);
        equipo.textoInteraccion = "Revisar " + equipo.nombreEquipo;
        equipo.radioInteraccion = 2.1f;

        // El objeto azul representa la estación; no sustituye sprites de personajes.
        // El aro ámbar es exclusivamente el indicador de interacción y se oculta
        // cuando Alex se aleja, para que no parezca un objeto duplicado del mapa.
        var visual = go.GetComponent<SpriteRenderer>();
        visual.sprite = Iconos.Marcador;
        visual.color = new Color(0.28f, 0.72f, 0.96f, 1f);
        visual.sortingOrder = 8;
        go.transform.localScale = Vector3.one * 0.72f;
        var collider = go.GetComponent<BoxCollider2D>();
        collider.size = new Vector2(1.05f, 0.82f);
        collider.offset = new Vector2(0f, -0.12f);

        var markerGo = new GameObject("Indicador_En_Rango", typeof(SpriteRenderer));
        markerGo.transform.SetParent(go.transform, false);
        markerGo.transform.localPosition = new Vector3(0f, 1.02f, 0f);
        equipo.indicadorInteraccion = markerGo.GetComponent<SpriteRenderer>();
        equipo.indicadorInteraccion.sprite = Iconos.Marcador;
        equipo.indicadorInteraccion.sortingOrder = 10;
        equipo.indicadorInteraccion.color = new Color(1f, 0.72f, 0.18f, 1f);
        equipo.indicadorInteraccion.enabled = false;
    }

    public static bool TareaCompletada(string idMision)
    {
        return ProgressService.Instancia != null &&
               ProgressService.Instancia.ObjetivoHecho(idMision + "_equipo");
    }

    public override void Interactuar()
    {
        if (resuelto || MissionService.Instancia == null ||
            MissionService.Instancia.Estado(misionId) != MissionStatus.EnProgreso)
            return;

        resuelto = true;
        ProgressService.Instancia?.MarcarObjetivo(misionId + "_equipo");
        if (indicadorInteraccion != null) indicadorInteraccion.enabled = false;
        if (GameUI.Instancia != null)
        {
            GameUI.Instancia.MostrarObjetivo(Titulo(),
                "Equipo revisado. Regresa con " + NombreResponsable(misionId) + " para reportar el resultado.");
            GameUI.Instancia.MostrarToast(nombreEquipo + " revisado correctamente.");
        }
    }

    private static MissionWorkstation Buscar(string id)
    {
        foreach (var station in FindObjectsByType<MissionWorkstation>(FindObjectsSortMode.None))
            if (station.misionId == id) return station;
        return null;
    }

    private void Update()
    {
        if (jugador == null)
        {
            var player = FindFirstObjectByType<PlayerMovement>();
            if (player != null) jugador = player.transform;
        }

        if (indicadorInteraccion == null) return;
        bool enRango = !resuelto && jugador != null &&
                       Vector2.Distance(jugador.position, transform.position) <= RadioInteraccion;
        indicadorInteraccion.enabled = enRango;
    }

    private static Vector3 EncontrarPosicionLibre(Vector3 sugerida)
    {
        // Busca primero junto al puesto de quien asigna la tarea. Solo acepta una
        // celda sin colisiones para que el equipo no aparezca dentro de un escritorio,
        // una pared o una puerta.
        var offsets = new[]
        {
            Vector2.zero, Vector2.right * 1.5f, Vector2.left * 1.5f,
            Vector2.up * 1.5f, Vector2.down * 1.5f,
            new Vector2(2f, -1.5f), new Vector2(-2f, -1.5f)
        };
        foreach (var offset in offsets)
        {
            var candidate = (Vector2)sugerida + offset;
            if (Physics2D.OverlapBox(candidate, new Vector2(0.9f, 0.7f), 0f) == null)
                return candidate;
        }
        return sugerida;
    }

    private static string NombreEquipo(string id) => id switch
    {
        "M02" => "estación de diagnóstico de hardware",
        "M03" => "terminal de red y servidor",
        "M04" => "consola de respuesta crítica",
        _ => "equipo asignado"
    };

    private static string NombreResponsable(string id) => id switch
    {
        "M02" => "Martín",
        "M03" => "Richard",
        "M04" => "Martín",
        _ => "el responsable"
    };

    private static string Titulo() => "MISIÓN EN PROGRESO";
}

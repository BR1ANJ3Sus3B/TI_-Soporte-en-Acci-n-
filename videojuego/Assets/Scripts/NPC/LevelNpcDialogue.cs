using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>Diálogo reutilizable para NPCs de los niveles y su avance de misión.</summary>
[RequireComponent(typeof(NpcInteractable))]
public class LevelNpcDialogue : MonoBehaviour
{
    [SerializeField] private string nombre;
    [SerializeField, TextArea(2, 5)] private string dialogo;
    [SerializeField] private string misionId;
    [SerializeField] private bool iniciaMision;
    [SerializeField] private bool completaMision;
    [SerializeField] private string escenaSiguiente;

    private NpcInteractable npc;

    void Awake()
    {
        npc = GetComponent<NpcInteractable>();
        npc.Interactuado += Hablar;
        // Las escenas existentes se serializaron antes de que se añadiera este
        // campo. El valor por defecto mantiene la progresión sin obligar a
        // reconfigurar manualmente cada NPC en el Inspector.
        if (string.IsNullOrEmpty(escenaSiguiente))
            escenaSiguiente = EscenaSiguientePorDefecto(misionId);
    }

    void OnDestroy()
    {
        if (npc != null) npc.Interactuado -= Hablar;
    }

    public void Configurar(string nombreNpc, string texto, string idMision, bool inicia, bool completa,
        string siguienteEscena = null)
    {
        nombre = nombreNpc;
        dialogo = texto;
        misionId = idMision;
        iniciaMision = inicia;
        completaMision = completa;
        escenaSiguiente = siguienteEscena ?? EscenaSiguientePorDefecto(idMision);
    }

    private void Hablar(NpcInteractable _)
    {
        var misiones = MissionService.Instancia;
        var estadoAnterior = misiones != null ? misiones.Estado(misionId) : MissionStatus.NoIniciada;

        if (!string.IsNullOrEmpty(misionId) && misiones != null && iniciaMision &&
            estadoAnterior == MissionStatus.NoIniciada)
        {
            misiones.EstablecerEstado(misionId, MissionStatus.EnProgreso);
            MissionWorkstation.CrearSiNoExiste(misionId,
                transform.position + new Vector3(3f, -2f, 0f));
            if (GameUI.Instancia != null)
                GameUI.Instancia.MostrarObjetivo(TituloMision(), ObjetivoMision());
        }

        var linea = new DialogueLine(nombre, dialogo);
        if (DialogueRunner.Instancia != null)
            DialogueRunner.Instancia.Reproducir(new[] { linea }, () => AlCerrarDialogo(estadoAnterior));
        else if (GameUI.Instancia != null)
        {
            GameUI.Instancia.MostrarDialogo(nombre, dialogo);
            // Las escenas creadas por el integrador siempre incluyen DialogueRunner,
            // pero conservar este cierre evita que una referencia faltante bloquee
            // permanentemente la misión.
            AlCerrarDialogo(estadoAnterior);
        }
    }

    private void AlCerrarDialogo(MissionStatus estadoAnterior)
    {
        if (!completaMision || string.IsNullOrEmpty(misionId) ||
            MissionService.Instancia == null || estadoAnterior != MissionStatus.EnProgreso)
            return;

        if (!MissionWorkstation.TareaCompletada(misionId))
        {
            if (GameUI.Instancia != null)
                GameUI.Instancia.MostrarAviso("Aún debes revisar el equipo señalado con el marcador ámbar.",
                    new Color(1f, 0.46f, 0.42f, 1f));
            return;
        }

        MissionService.Instancia.EstablecerEstado(misionId, MissionStatus.Completada);
        var progreso = ProgressService.Instancia;
        if (progreso != null)
        {
            progreso.OtorgarXp(125);
            progreso.OtorgarInsignia("mision_" + misionId.Substring(1));
        }

        if (GameUI.Instancia != null)
        {
            GameUI.Instancia.ActualizarHud(
                progreso != null ? progreso.Xp : 0,
                progreso != null ? progreso.InsigniasCount : 0,
                progreso != null ? progreso.MisionesCompletadasCount : 0);
            GameUI.Instancia.OcultarObjetivo();
            GameUI.Instancia.MostrarBanner("¡MISIÓN COMPLETADA!", "+125 XP\n" +
                "Registro de misión guardado", CargarSiguienteEscena);
        }
        else CargarSiguienteEscena();
    }

    private void CargarSiguienteEscena()
    {
        if (!string.IsNullOrEmpty(escenaSiguiente)) SceneManager.LoadScene(escenaSiguiente);
    }

    private string TituloMision() => "MISIÓN " + (misionId ?? string.Empty).Replace("M", "") +
        " — " + NombreMision();

    private string ObjetivoMision() => misionId switch
    {
        "M02" => "Revisa el equipo reportado y aplica un diagnóstico de hardware responsable.",
        "M03" => "Restaura la conectividad siguiendo el procedimiento de red.",
        "M04" => "Atiende el incidente crítico y documenta la respuesta del equipo.",
        _ => "Habla con el equipo de soporte para continuar."
    };

    private string NombreMision() => misionId switch
    {
        "M02" => "PROBLEMAS DE HARDWARE",
        "M03" => "SIN CONEXIÓN",
        "M04" => "CÓDIGO ROJO",
        _ => "SOPORTE"
    };

    private static string EscenaSiguientePorDefecto(string id) => id switch
    {
        "M02" => "03_SinConexion",
        "M03" => "04_CodigoRojo",
        "M04" => "MainMenu",
        _ => string.Empty
    };
}

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Controla el menú principal de *TI: Soporte en Acción*.
///
/// Concentra toda la lógica de la pantalla de inicio para que el Canvas del menú
/// solo contenga presentación: aquí viven la navegación a la escena de juego, el
/// encendido y apagado de la música y el sonido, el aviso modal y la salida.
/// </summary>
public class MainMenuController : MonoBehaviour
{
    [Header("Navegación")]
    [Tooltip("Nombre exacto de la escena que carga el botón JUGAR.")]
    [SerializeField] private string escenaJuego = "nivel1";

    [Header("Audio")]
    [SerializeField] private AudioSource musica;
    [SerializeField] private Image iconoMusica;
    [SerializeField] private Image iconoSonido;

    [Header("Aviso modal")]
    [SerializeField] private GameObject aviso;
    [SerializeField] private Text avisoTitulo;
    [SerializeField] private Text avisoTexto;

    [Header("Animación")]
    [Tooltip("Orbe del botón JUGAR: late suavemente para destacarlo del resto.")]
    [SerializeField] private Transform orbePrincipal;
    [SerializeField] private float amplitudPulso = 0.025f;
    [SerializeField] private float velocidadPulso = 2.2f;

    private const float AlfaIconoActivo = 1f;
    private const float AlfaIconoApagado = 0.32f;

    private bool musicaActiva = true;
    private bool sonidoActivo = true;
    private Vector3 escalaOrbe = Vector3.one;

    private void Awake()
    {
        if (orbePrincipal != null)
            escalaOrbe = orbePrincipal.localScale;
    }

    private void Start()
    {
        CerrarAviso();
        AplicarEstadoAudio();
    }

    private void Update()
    {
        if (orbePrincipal != null)
        {
            float pulso = 1f + Mathf.Sin(Time.unscaledTime * velocidadPulso) * amplitudPulso;
            orbePrincipal.localScale = escalaOrbe * pulso;
        }

        if (aviso != null && aviso.activeSelf && Input.GetKeyDown(KeyCode.Escape))
            CerrarAviso();
    }

    // ------------------------------------------------------------------ navegación

    /// <summary>
    /// Botón principal: reinicia la misión y entra al nivel.
    ///
    /// Borra el progreso guardado antes de cargar la escena: <see cref="ProgressService"/>
    /// lee el archivo en su Awake, así que el nivel arranca siempre desde la etapa
    /// Bienvenida, con 0 XP y la misión 01 sin registrar.
    /// </summary>
    public void Jugar()
    {
        SaveService.Borrar();
        SceneManager.LoadScene(escenaJuego);
    }

    public void Misiones()
    {
        MostrarAviso("MISIONES",
            "Tu registro de misiones, XP e insignias de NovaTech aparece dentro del juego.\n" +
            "Pulsa JUGAR para entrar al nivel 01.");
    }

    public void Ajustes()
    {
        MostrarAviso("AJUSTES",
            "Aquí podrás cambiar el volumen, la resolución y la velocidad del texto.\n" +
            "Mientras tanto usa los botones de música y sonido de los laterales.");
    }

    public void ConfiguracionRapida()
    {
        MostrarAviso("CONFIGURACIÓN RÁPIDA",
            "Música: " + (musicaActiva ? "activada" : "silenciada") + "\n" +
            "Sonido: " + (sonidoActivo ? "activado" : "silenciado") + "\n" +
            "Escena de juego asignada: " + escenaJuego);
    }

    public void Perfil()
    {
        MostrarAviso("PERFIL",
            "Agente: Alex  ·  División de Soporte Técnico\n" +
            "Empresa: NovaTech Solutions  ·  Nivel 01: El Primer Incidente");
    }

    public void Salir()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // ------------------------------------------------------------------ audio

    public void AlternarMusica()
    {
        musicaActiva = !musicaActiva;
        AplicarEstadoAudio();
    }

    public void AlternarSonido()
    {
        sonidoActivo = !sonidoActivo;
        AplicarEstadoAudio();
    }

    private void AplicarEstadoAudio()
    {
        if (musica != null)
            musica.mute = !musicaActiva;

        AudioListener.volume = sonidoActivo ? 1f : 0f;

        Tintar(iconoMusica, musicaActiva);
        Tintar(iconoSonido, sonidoActivo);
    }

    private static void Tintar(Image icono, bool activo)
    {
        if (icono == null)
            return;

        Color color = icono.color;
        color.a = activo ? AlfaIconoActivo : AlfaIconoApagado;
        icono.color = color;
    }

    // ------------------------------------------------------------------ aviso

    private void MostrarAviso(string titulo, string texto)
    {
        if (aviso == null)
            return;

        if (avisoTitulo != null)
            avisoTitulo.text = titulo;

        if (avisoTexto != null)
            avisoTexto.text = texto;

        aviso.SetActive(true);
    }

    public void CerrarAviso()
    {
        if (aviso != null)
            aviso.SetActive(false);
    }
}

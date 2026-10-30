using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// MISIÓN 01 — "La computadora que no responde".
///
/// Dirige el flujo completo del primer incidente: bienvenida en recepción, búsqueda del
/// equipo, ventana de diagnóstico de cuatro opciones, reconexión del cable, vuelta con
/// Martín, recompensas y el primer indicio del Saboteador. Todo el estado se guarda en
/// <see cref="ProgressService"/>, así que la misión no se puede completar dos veces y se
/// recupera al reiniciar el juego.
/// </summary>
public class Mission01ElPrimerIncidente : MonoBehaviour
{
    public enum Etapa
    {
        NoIniciada,
        Bienvenida,
        IrAComputadora,
        ConectarCable,
        VolverConMartin,
        Completada
    }

    private static readonly Color ColorBien = new Color(0.42f, 1f, 0.55f, 1f);
    private static readonly Color ColorMal = new Color(1f, 0.46f, 0.42f, 1f);

    private MissionDefinition definicion;
    private BrokenComputer computadora;
    private PowerCable cable;
    private NpcInteractable martin;
    private SecretRoomDoor paredFalsa;

    private Etapa etapa = Etapa.NoIniciada;

    public Etapa EtapaActual => etapa;

    private string IdMision =>
        definicion != null && !string.IsNullOrEmpty(definicion.id) ? definicion.id : "M01";

    private string TituloMision =>
        definicion != null
            ? $"MISIÓN {definicion.numero} — {definicion.titulo}"
            : "MISIÓN 01 — EL PRIMER INCIDENTE";

    private int RecompensaXp => definicion != null ? definicion.recompensaXp : 100;

    private string InsigniaId =>
        definicion != null && !string.IsNullOrEmpty(definicion.insigniaId)
            ? definicion.insigniaId
            : "primer_diagnostico";

    private string InsigniaNombre =>
        definicion != null && !string.IsNullOrEmpty(definicion.insigniaNombre)
            ? definicion.insigniaNombre
            : "Primer Diagnóstico";

    // ------------------------------------------------------------------- arranque

    /// <summary>Engancha referencias y arranca la misión. Lo llama <see cref="GameBootstrap"/>.</summary>
    public void Configurar(MissionDefinition definicionMision, BrokenComputer equipo,
        PowerCable cableAlimentacion, NpcInteractable mentor, SecretRoomDoor pared)
    {
        definicion = definicionMision;
        computadora = equipo;
        cable = cableAlimentacion;
        martin = mentor;
        paredFalsa = pared;

        if (computadora != null) computadora.AlInteractuar += AlRevisarComputadora;
        if (cable != null) cable.AlConectar += AlConectarCable;
        if (martin != null) martin.Interactuado += AlHablarConMartin;
        if (paredFalsa != null) paredFalsa.AlIntentarEntrar += AlIntentarParedFalsa;

        Iniciar();
    }

    void OnDestroy()
    {
        if (computadora != null) computadora.AlInteractuar -= AlRevisarComputadora;
        if (cable != null) cable.AlConectar -= AlConectarCable;
        if (martin != null) martin.Interactuado -= AlHablarConMartin;
        if (paredFalsa != null) paredFalsa.AlIntentarEntrar -= AlIntentarParedFalsa;
    }

    private void Iniciar()
    {
        var progreso = ProgressService.Instancia;

        if (cable != null) cable.gameObject.SetActive(false);

        if (progreso != null && progreso.MisionCompletada(IdMision))
        {
            etapa = Etapa.Completada;
            if (computadora != null) computadora.Encender();
            if (GameUI.Instancia != null) GameUI.Instancia.OcultarObjetivo();
            ActualizarHud();
            return;
        }

        etapa = Etapa.Bienvenida;
        if (MissionService.Instancia != null)
            MissionService.Instancia.EstablecerEstado(IdMision, MissionStatus.Activa);

        if (computadora != null) computadora.Reiniciar();
        ActualizarHud();

        // La bienvenida NO se reproduce sola: la da Martín cuando el jugador interactúa
        // con él. Hasta entonces la misión se queda en Bienvenida y el HUD solo indica
        // el siguiente paso.
        MostrarObjetivo("Ve a recepción y habla con Martín: acércate y pulsa E.\n" +
                        "Él te asignará el primer reporte de soporte.");
    }

    // ------------------------------------------------------------------ desarrollo

    private void ReproducirBienvenida()
    {
        Reproducir(new[]
        {
            new DialogueLine("Martín", "¡Bienvenido a NovaTech Solutions, Alex! Hoy comienza tu primer día como técnico de soporte."),
            new DialogueLine("Alex", "Gracias, Martín. Estoy listo para comenzar."),
            new DialogueLine("Martín", "Perfecto. Tenemos nuestro primer reporte. Una computadora de la oficina de soporte no responde. Quiero que revises qué sucede."),
            new DialogueLine("Alex", "Entendido. Iré a verificar el equipo.")
        }, EmpezarBusqueda);
    }

    private void EmpezarBusqueda()
    {
        etapa = Etapa.IrAComputadora;

        if (MissionService.Instancia != null)
            MissionService.Instancia.EstablecerEstado(IdMision, MissionStatus.EnProgreso);

        MostrarObjetivo(definicion != null && !string.IsNullOrEmpty(definicion.objetivo)
            ? definicion.objetivo
            : "Dirígete a la oficina de soporte y revisa la computadora reportada.");
    }

    private void AlRevisarComputadora()
    {
        switch (etapa)
        {
            case Etapa.IrAComputadora:
                MostrarVentanaDiagnostico();
                break;

            case Etapa.ConectarCable:
                Aviso("El cable de alimentación sigue desconectado. Reconéctalo antes de encender el equipo.", ColorMal);
                break;

            case Etapa.VolverConMartin:
            case Etapa.Completada:
                Aviso("La computadora ya está encendida y funcionando correctamente.", ColorBien);
                break;

            case Etapa.Bienvenida:
            default:
                Aviso("Todavía no tienes asignado este equipo. Habla antes con Martín.", ColorMal);
                break;
        }
    }

    private void MostrarVentanaDiagnostico()
    {
        if (GameUI.Instancia == null) return;

        GameUI.Instancia.MostrarDiagnostico(
            "MISIÓN 01 — DIAGNÓSTICO DEL EQUIPO",
            "Reporte del incidente:\n«La computadora no enciende cuando se presiona el botón de encendido.»" +
            "\n\nEl equipo está apagado y no emite ninguna señal. ¿Qué revisas primero?",
            new[]
            {
                "1.  A. Revisar la conexión eléctrica y el cable de alimentación.",
                "2.  B. Reinstalar el sistema operativo inmediatamente.",
                "3.  C. Formatear el disco duro.",
                "4.  D. Cambiar el monitor sin comprobar la alimentación."
            },
            AlElegirOpcionDiagnostico);
    }

    private void AlElegirOpcionDiagnostico(int indice)
    {
        if (indice == 0)
        {
            Aviso("¡Correcto! Antes de realizar reparaciones complejas, es importante verificar " +
                  "las conexiones físicas y la alimentación del equipo.", ColorBien, TrasDiagnosticoCorrecto);
        }
        else
        {
            Aviso("Esa acción no es la más adecuada. Primero realiza las comprobaciones básicas " +
                  "para evitar intervenciones innecesarias.", ColorMal, MostrarVentanaDiagnostico);
        }
    }

    private void TrasDiagnosticoCorrecto()
    {
        if (computadora != null) computadora.MarcarDiagnosticada();

        etapa = Etapa.ConectarCable;

        if (cable != null)
        {
            cable.Reiniciar();
            cable.gameObject.SetActive(true);
        }

        MostrarObjetivo("Has encontrado la causa: el cable de alimentación está desconectado.\n" +
                        "Acércate al cable y pulsa E para conectarlo.");

        if (GameUI.Instancia != null)
            GameUI.Instancia.MostrarToast("Cable de alimentación desconectado. Reconéctalo para que el equipo arranque.");
    }

    private void AlConectarCable()
    {
        if (computadora != null) computadora.Encender();

        etapa = Etapa.VolverConMartin;
        MostrarObjetivo("La computadora ya enciende correctamente.\nRegresa con Martín e infórmale del resultado.");

        if (GameUI.Instancia != null)
            GameUI.Instancia.MostrarToast("El cable quedó conectado y la computadora encendió correctamente.", 5f);
    }

    private void AlHablarConMartin(NpcInteractable npc)
    {
        switch (etapa)
        {
            case Etapa.Bienvenida:
                ReproducirBienvenida();
                break;

            case Etapa.IrAComputadora:
                Reproducir(new[]
                {
                    new DialogueLine("Martín", "La oficina de soporte está al sur de la recepción; entra por la puerta del pasillo central."),
                    new DialogueLine("Martín", "Acércate a la computadora reportada y pulsa E para revisarla.")
                });
                break;

            case Etapa.ConectarCable:
                Reproducir(new[]
                {
                    new DialogueLine("Martín", "¿Ya revisaste las conexiones del equipo? Empieza siempre por lo más sencillo.")
                });
                break;

            case Etapa.VolverConMartin:
                Reproducir(new[]
                {
                    new DialogueLine("Alex", "Martín, encontré el problema. La computadora tenía el cable de alimentación desconectado."),
                    new DialogueLine("Martín", "Excelente trabajo, Alex. Recuerda que un buen diagnóstico comienza por revisar las causas más sencillas.")
                }, CompletarMision);
                break;

            default:
                Reproducir(new[]
                {
                    new DialogueLine("Martín", "Buen trabajo con ese primer reporte. Muy pronto tendremos más incidentes.")
                });
                break;
        }
    }

    private void CompletarMision()
    {
        if (etapa == Etapa.Completada) return;
        etapa = Etapa.Completada;

        var progreso = ProgressService.Instancia;
        bool yaRegistrada = progreso != null && progreso.MisionCompletada(IdMision);

        if (progreso != null && !yaRegistrada)
        {
            progreso.OtorgarXp(RecompensaXp);
            progreso.OtorgarInsignia(InsigniaId);
            progreso.MarcarMisionCompletada(IdMision);
            if (definicion != null && !string.IsNullOrEmpty(definicion.registro))
                progreso.MarcarObjetivo(IdMision + "_registro");
        }

        if (MissionService.Instancia != null)
            MissionService.Instancia.EstablecerEstado(IdMision, MissionStatus.Completada);

        ActualizarHud();

        if (GameUI.Instancia != null)
        {
            GameUI.Instancia.OcultarObjetivo();

            string detalle = yaRegistrada
                ? "Esta misión ya estaba registrada.\nNo se otorgan recompensas duplicadas."
                : $"+{RecompensaXp} XP\nInsignia: {InsigniaNombre}\nRegistro de misión guardado";

            GameUI.Instancia.MostrarBanner("¡MISIÓN COMPLETADA!", detalle, TrasBannerMision);
        }
        else
        {
            TrasBannerMision();
        }
    }

    private void TrasBannerMision()
    {
        Reproducir(new[]
        {
            new DialogueLine("Martín", "Qué raro... el sistema registró varias fallas inusuales durante la madrugada."),
            new DialogueLine("Alex", "¿Crees que alguien esté provocándolas?"),
            new DialogueLine("Martín", "Todavía no lo sabemos. Por ahora, mantente atento.")
        }, MostrarMensajeMisterioso);
    }

    private void MostrarMensajeMisterioso()
    {
        if (GameUI.Instancia != null)
            GameUI.Instancia.MostrarToast("Alguien está observando desde las sombras...", 7f);
    }

    // -------------------------------------------------------------- pared secreta

    private void AlIntentarParedFalsa(SecretRoomDoor pared)
    {
        Aviso(pared != null && !string.IsNullOrEmpty(pared.MensajeBloqueado)
            ? pared.MensajeBloqueado
            : "ACCESO BLOQUEADO — se requiere autorización de nivel 2.", ColorMal);
    }

    // -------------------------------------------------------------------- apoyo

    private void Reproducir(IEnumerable<DialogueLine> lineas, Action alTerminar = null)
    {
        if (DialogueRunner.Instancia == null)
        {
            alTerminar?.Invoke();
            return;
        }
        DialogueRunner.Instancia.Reproducir(lineas, alTerminar);
    }

    private void Aviso(string mensaje, Color color, Action alContinuar = null)
    {
        if (GameUI.Instancia == null)
        {
            alContinuar?.Invoke();
            return;
        }
        GameUI.Instancia.MostrarAviso(mensaje, color, alContinuar);
    }

    private void MostrarObjetivo(string texto)
    {
        if (GameUI.Instancia != null) GameUI.Instancia.MostrarObjetivo(TituloMision, texto);
    }

    private void ActualizarHud()
    {
        if (GameUI.Instancia == null) return;

        var progreso = ProgressService.Instancia;
        GameUI.Instancia.ActualizarHud(
            progreso != null ? progreso.Xp : 0,
            progreso != null ? progreso.InsigniasCount : 0,
            progreso != null ? progreso.MisionesCompletadasCount : 0);
    }
}

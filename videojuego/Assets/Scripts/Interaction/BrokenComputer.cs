using System;
using UnityEngine;

/// <summary>
/// La computadora reportada del incidente 001. Empieza apagada, se puede diagnosticar
/// y termina encendida cuando se reconecta su cable de alimentación. El marcador
/// flotante es su indicador de estado: rojo mientras falla, verde cuando funciona.
/// </summary>
public class BrokenComputer : InteractableBase
{
    public enum Estado
    {
        Apagada,
        Diagnosticada,
        Encendida
    }

    [Header("Marcador")]
    [SerializeField] private WorldMarker marcador;

    [Header("Equipo")]
    [SerializeField] private SpriteRenderer visual;

    public Estado EstadoActual { get; private set; } = Estado.Apagada;
    public bool CableConectado { get; private set; }

    /// <summary>Lo dispara el jugador al pulsar E frente al equipo.</summary>
    public event Action AlInteractuar;

    public override string TextoInteraccion =>
        EstadoActual == Estado.Encendida
            ? "Revisar la computadora (en línea)"
            : "Revisar la computadora reportada";

    protected override void OnEnable()
    {
        base.OnEnable();
        AplicarEstado();
    }

    public override void Interactuar()
    {
        AlInteractuar?.Invoke();
    }

    /// <summary>El diagnóstico confirmó la causa, pero el equipo sigue apagado.</summary>
    public void MarcarDiagnosticada()
    {
        if (EstadoActual == Estado.Apagada) EstadoActual = Estado.Diagnosticada;
        AplicarEstado();
    }

    /// <summary>El cable está conectado: el equipo arranca.</summary>
    public void Encender()
    {
        EstadoActual = Estado.Encendida;
        CableConectado = true;
        AplicarEstado();
    }

    private void AplicarEstado()
    {
        if (marcador == null) marcador = GetComponentInChildren<WorldMarker>(true);

        if (marcador != null)
        {
            marcador.Mostrar(true);
            if (EstadoActual == Estado.Encendida) marcador.MarcarResuelto();
            else marcador.MarcarAviso();
        }

        if (visual != null)
            visual.color = EstadoActual == Estado.Encendida ? Color.white : new Color(0.88f, 0.88f, 0.90f, 1f);
    }

    /// <summary>Devuelve el equipo a su estado inicial (usado al reiniciar el progreso).</summary>
    public void Reiniciar()
    {
        EstadoActual = Estado.Apagada;
        CableConectado = false;
        AplicarEstado();
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Reproductor de conversaciones. Encola las líneas y las muestra de una en una; el
/// jugador avanza con E, Espacio o clic. Al cerrar la última línea devuelve el control
/// y ejecuta la acción de cierre, que es la que hace avanzar a la misión.
/// </summary>
public class DialogueRunner : MonoBehaviour
{
    public static DialogueRunner Instancia { get; private set; }

    public KeyCode teclaAvanzar = KeyCode.E;
    public KeyCode teclaAlterna = KeyCode.Space;

    private readonly Queue<DialogueLine> pendientes = new Queue<DialogueLine>();
    private bool activo;
    private int frameApertura;
    private Action alTerminar;

    public bool Activo => activo;

    void Awake()
    {
        if (Instancia != null && Instancia != this)
        {
            Destroy(this);
            return;
        }
        Instancia = this;
    }

    void OnDestroy()
    {
        if (Instancia == this) Instancia = null;
    }

    /// <summary>Empieza una conversación. <paramref name="alTerminar"/> se llama al cerrarla.</summary>
    public void Reproducir(IEnumerable<DialogueLine> lineas, Action alTerminar = null)
    {
        if (lineas == null) return;

        pendientes.Clear();
        foreach (var linea in lineas) pendientes.Enqueue(linea);

        this.alTerminar = alTerminar;

        if (pendientes.Count == 0)
        {
            Finalizar();
            return;
        }

        activo = true;
        frameApertura = Time.frameCount;
        MostrarSiguiente();
    }

    void Update()
    {
        if (!activo) return;
        // La misma pulsación que abre el diálogo no debe saltarse la primera línea.
        if (Time.frameCount == frameApertura) return;

        if (Input.GetKeyDown(teclaAvanzar) || Input.GetKeyDown(teclaAlterna) ||
            Input.GetMouseButtonDown(0))
            Avanzar();
    }

    public void Avanzar()
    {
        if (!activo) return;

        if (pendientes.Count == 0)
        {
            Finalizar();
            return;
        }

        MostrarSiguiente();
    }

    private void MostrarSiguiente()
    {
        var linea = pendientes.Dequeue();
        if (GameUI.Instancia != null)
            GameUI.Instancia.MostrarDialogo(linea.hablante, linea.texto);
    }

    private void Finalizar()
    {
        activo = false;
        if (GameUI.Instancia != null) GameUI.Instancia.OcultarDialogo();

        var callback = alTerminar;
        alTerminar = null;
        callback?.Invoke();
    }
}

using System;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Pared falsa que oculta la sala del Saboteador. Se dibuja con el mismo tile de pared
/// que el resto del muro, así que a la vista es una pared más, y su collider impide el
/// paso hasta que una misión futura llame a <see cref="Desbloquear"/>.
/// </summary>
public class SecretRoomDoor : InteractableBase
{
    [Header("Bloqueo")]
    [Tooltip("Collider que impide atravesar la pared falsa.")]
    [SerializeField] private Collider2D bloqueo;

    [Tooltip("Tilemap de una sola celda que dibuja la pared falsa.")]
    [SerializeField] private Tilemap paredFalsa;

    [Header("Mensaje")]
    [TextArea(2, 4)]
    [SerializeField]
    private string mensajeBloqueado =
        "La pared parece sólida, pero hay una junta metálica.\n\nACCESO BLOQUEADO — se requiere autorización de nivel 2.";

    public bool Desbloqueada { get; private set; }

    /// <summary>La misión lo usa para mostrar el mensaje de acceso denegado.</summary>
    public event Action<SecretRoomDoor> AlIntentarEntrar;

    public string MensajeBloqueado => mensajeBloqueado;

    public override string TextoInteraccion =>
        Desbloqueada ? "La pared está abierta" : "Examinar la pared del fondo";

    void Awake()
    {
        if (bloqueo == null) bloqueo = GetComponent<Collider2D>();
    }

    public override void Interactuar()
    {
        if (Desbloqueada) return;
        AlIntentarEntrar?.Invoke(this);
    }

    /// <summary>Abre el paso: quita el collider y borra el tile que hace de pared.</summary>
    public void Desbloquear()
    {
        if (Desbloqueada) return;

        Desbloqueada = true;
        if (bloqueo != null) bloqueo.enabled = false;
        if (paredFalsa != null) paredFalsa.ClearAllTiles();
    }

    /// <summary>Vuelve a cerrar el acceso, por ejemplo al reiniciar el progreso.</summary>
    public void Bloquear()
    {
        Desbloqueada = false;
        if (bloqueo != null) bloqueo.enabled = true;
    }

    public void AplicarEstado(bool desbloqueada)
    {
        if (desbloqueada) Desbloquear();
        else Bloquear();
    }
}

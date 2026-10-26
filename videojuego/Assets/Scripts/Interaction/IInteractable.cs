using UnityEngine;

/// <summary>
/// Contrato que cumple cualquier objeto del mundo con el que Alex puede interactuar
/// pulsando E. El jugador nunca busca por nombre ni por tag: pregunta a la lista de
/// objetos activos cuál está más cerca.
/// </summary>
public interface IInteractable
{
    /// <summary>Si ahora mismo el objeto acepta interacción.</summary>
    bool PuedeInteractuar { get; }

    /// <summary>Texto que la interfaz muestra en el aviso de interacción.</summary>
    string TextoInteraccion { get; }

    /// <summary>Punto del mundo usado para medir la distancia al jugador.</summary>
    Vector3 PuntoInteraccion { get; }

    /// <summary>Se llama cuando el jugador pulsa la tecla de interacción estando cerca.</summary>
    void Interactuar();
}

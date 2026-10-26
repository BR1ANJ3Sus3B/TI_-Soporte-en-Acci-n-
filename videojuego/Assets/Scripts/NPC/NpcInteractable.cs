using System;
using UnityEngine;

/// <summary>
/// Personaje no jugable con el que se puede hablar. No decide qué dice: la misión se
/// suscribe a <see cref="Interactuado"/> y elige el diálogo según el punto de la trama.
/// </summary>
public class NpcInteractable : InteractableBase
{
    [Header("Personaje")]
    public string nombre = "Martín";

    [Tooltip("Se puede hablar aunque el jugador ya haya hablado antes.")]
    public bool siempreDisponible = true;

    public event Action<NpcInteractable> Interactuado;

    public override bool PuedeInteractuar => base.PuedeInteractuar && siempreDisponible;

    public override string TextoInteraccion => "Hablar con " + nombre;

    void Awake()
    {
        if (string.IsNullOrEmpty(textoInteraccion) || textoInteraccion == "Interactuar")
            textoInteraccion = "Hablar con " + nombre;
    }

    public override void Interactuar()
    {
        Interactuado?.Invoke(this);
    }
}

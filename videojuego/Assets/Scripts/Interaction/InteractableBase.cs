using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base de todos los objetos interactivos. Mantiene un registro de instancias activas
/// para que el jugador pueda localizar el objeto más cercano sin recorrer la escena
/// cada fotograma.
/// </summary>
public abstract class InteractableBase : MonoBehaviour, IInteractable
{
    private static readonly List<InteractableBase> Activos = new List<InteractableBase>();

    [Header("Interacción")]
    [Tooltip("Texto que se muestra en el aviso de la interfaz.")]
    [SerializeField] protected string textoInteraccion = "Interactuar";

    [Tooltip("Distancia máxima, en unidades de mundo, a la que el jugador puede usarlo.")]
    [SerializeField] protected float radioInteraccion = 1.9f;

    public virtual bool PuedeInteractuar => isActiveAndEnabled;
    public virtual string TextoInteraccion => textoInteraccion;
    public virtual Vector3 PuntoInteraccion => transform.position;
    public float RadioInteraccion => radioInteraccion;

    protected virtual void OnEnable()
    {
        if (!Activos.Contains(this)) Activos.Add(this);
    }

    protected virtual void OnDisable()
    {
        Activos.Remove(this);
    }

    public abstract void Interactuar();

    /// <summary>Objeto interactivo habilitado más próximo a <paramref name="desde"/>.</summary>
    public static InteractableBase MasCercano(Vector3 desde, float alcanceJugador)
    {
        InteractableBase mejor = null;
        float mejorDistancia = float.MaxValue;

        for (int i = Activos.Count - 1; i >= 0; i--)
        {
            var candidato = Activos[i];
            if (candidato == null)
            {
                Activos.RemoveAt(i);
                continue;
            }
            if (!candidato.PuedeInteractuar) continue;

            float limite = Mathf.Max(alcanceJugador, candidato.radioInteraccion);
            float distancia = Vector2.Distance(desde, candidato.PuntoInteraccion);
            if (distancia > limite || distancia >= mejorDistancia) continue;

            mejor = candidato;
            mejorDistancia = distancia;
        }

        return mejor;
    }
}

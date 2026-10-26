using UnityEngine;

/// <summary>
/// Detecta el objeto interactivo más cercano y muestra el aviso "[E] ...". Al pulsar E
/// delega en ese objeto. Se detiene por completo mientras hay una ventana modal abierta.
/// </summary>
public class PlayerInteractor : MonoBehaviour
{
    [Header("Detección")]
    [Tooltip("Alcance base en unidades de mundo. Cada objeto puede ampliarlo con su propio radio.")]
    public float alcance = 1.9f;

    public KeyCode teclaInteraccion = KeyCode.E;

    private InteractableBase objetivo;

    public InteractableBase Objetivo => objetivo;

    void Update()
    {
        if (GameInput.Bloqueado)
        {
            EstablecerObjetivo(null);
            return;
        }

        EstablecerObjetivo(InteractableBase.MasCercano(transform.position, alcance));

        if (objetivo != null && Input.GetKeyDown(teclaInteraccion))
            objetivo.Interactuar();
    }

    private void EstablecerObjetivo(InteractableBase nuevo)
    {
        if (objetivo == nuevo) return;
        objetivo = nuevo;
        if (GameUI.Instancia != null)
            GameUI.Instancia.MostrarPrompt(objetivo != null ? objetivo.TextoInteraccion : null);
    }

    void OnDisable()
    {
        objetivo = null;
    }
}

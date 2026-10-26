using UnityEngine;

/// <summary>
/// Indicador flotante que señala un objetivo en el mundo. Late suavemente para llamar
/// la atención sin tapar la escena y cambia de color cuando el objetivo se resuelve.
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class WorldMarker : MonoBehaviour
{
    public float amplitud = 0.09f;
    public float frecuencia = 2.1f;

    public Color colorAviso = new Color(1f, 0.28f, 0.28f, 1f);
    public Color colorResuelto = new Color(0.38f, 1f, 0.52f, 1f);

    private SpriteRenderer render;
    private Vector3 ancla;
    private float fase;
    private bool activo = true;

    void Awake()
    {
        render = GetComponent<SpriteRenderer>();
        if (render.sprite == null) render.sprite = Iconos.Marcador;

        ancla = transform.position;
        fase = Mathf.Repeat(transform.position.x * 1.7f + transform.position.y, Mathf.PI * 2f);
    }

    void Update()
    {
        if (!activo) return;

        float t = Time.time * frecuencia + fase;
        var posicion = ancla;
        posicion.y += Mathf.Sin(t) * amplitud;
        transform.position = posicion;
    }

    public void Mostrar(bool visible)
    {
        activo = visible;
        if (render != null) render.enabled = visible;
    }

    public void MarcarResuelto()
    {
        if (render != null) render.color = colorResuelto;
    }

    public void MarcarAviso()
    {
        if (render != null) render.color = colorAviso;
    }
}

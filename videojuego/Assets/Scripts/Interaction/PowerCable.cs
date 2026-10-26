using System;
using UnityEngine;

/// <summary>
/// Cable de alimentación suelto. Solo aparece cuando el diagnóstico ya confirmó que la
/// causa del fallo es la alimentación, y desaparece al conectarlo.
/// </summary>
public class PowerCable : InteractableBase
{
    [SerializeField] private SpriteRenderer visual;

    public bool Conectado { get; private set; }

    public event Action AlConectar;

    public SpriteRenderer Visual => visual;

    public override bool PuedeInteractuar => base.PuedeInteractuar && !Conectado;

    public override string TextoInteraccion => "Conectar el cable de alimentación";

    void Awake()
    {
        if (visual == null) visual = GetComponent<SpriteRenderer>();
        if (visual != null && visual.sprite == null) visual.sprite = Iconos.Cable;
        textoInteraccion = "Conectar el cable de alimentación";
    }

    public override void Interactuar()
    {
        if (Conectado) return;

        Conectado = true;
        if (visual != null) visual.enabled = false;

        AlConectar?.Invoke();
        gameObject.SetActive(false);
    }

    /// <summary>Vuelve a dejar el cable suelto y visible.</summary>
    public void Reiniciar()
    {
        Conectado = false;
        if (visual != null) visual.enabled = true;
        gameObject.SetActive(true);
    }
}

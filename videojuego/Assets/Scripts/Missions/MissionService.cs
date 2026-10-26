using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Máquina de estados de misiones. Guarda el estado en memoria y lo refleja en el
/// progreso persistente, de forma que una misión completada sigue completada tras
/// reiniciar el juego.
/// </summary>
public class MissionService : MonoBehaviour
{
    public static MissionService Instancia { get; private set; }

    public event Action<string, MissionStatus> AlCambiarEstado;

    private readonly Dictionary<string, MissionStatus> estados = new Dictionary<string, MissionStatus>();

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

    public MissionStatus Estado(string id)
    {
        if (string.IsNullOrEmpty(id)) return MissionStatus.NoIniciada;

        if (ProgressService.Instancia != null && ProgressService.Instancia.MisionCompletada(id))
            return MissionStatus.Completada;

        return estados.TryGetValue(id, out var estado) ? estado : MissionStatus.NoIniciada;
    }

    public void EstablecerEstado(string id, MissionStatus estado)
    {
        if (string.IsNullOrEmpty(id)) return;

        estados[id] = estado;

        var progreso = ProgressService.Instancia;
        if (progreso != null)
        {
            if (estado == MissionStatus.Completada) progreso.MarcarMisionCompletada(id);
            else if (estado == MissionStatus.Activa || estado == MissionStatus.EnProgreso)
                progreso.EstablecerMisionActiva(id);
        }

        AlCambiarEstado?.Invoke(id, estado);
    }
}

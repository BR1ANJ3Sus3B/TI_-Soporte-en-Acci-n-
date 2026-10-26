using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Progreso del jugador: experiencia, insignias, misiones completadas y objetivos.
/// Cada cambio se persiste de inmediato con <see cref="SaveService"/>.
/// </summary>
public class ProgressService : MonoBehaviour
{
    public static ProgressService Instancia { get; private set; }

    public event Action AlCambiar;

    private SaveData datos;

    public SaveData Datos => datos;
    public int Xp => datos != null ? datos.xp : 0;
    public int InsigniasCount => datos?.insignias?.Count ?? 0;
    public int MisionesCompletadasCount => datos?.misionesCompletadas?.Count ?? 0;

    void Awake()
    {
        if (Instancia != null && Instancia != this)
        {
            Destroy(this);
            return;
        }

        Instancia = this;
        datos = SaveService.Cargar();
        Sanear();
    }

    void Start()
    {
        AlCambiar?.Invoke();
    }

    void OnDestroy()
    {
        if (Instancia == this) Instancia = null;
    }

    private void Sanear()
    {
        if (datos == null) datos = new SaveData();
        if (datos.insignias == null) datos.insignias = new List<string>();
        if (datos.misionesCompletadas == null) datos.misionesCompletadas = new List<string>();
        if (datos.objetivos == null) datos.objetivos = new List<string>();
        if (datos.misionActiva == null) datos.misionActiva = "";
    }

    public bool MisionCompletada(string id) =>
        !string.IsNullOrEmpty(id) && datos.misionesCompletadas.Contains(id);

    public bool TieneInsignia(string id) =>
        !string.IsNullOrEmpty(id) && datos.insignias.Contains(id);

    public bool ObjetivoHecho(string id) =>
        !string.IsNullOrEmpty(id) && datos.objetivos.Contains(id);

    public void MarcarObjetivo(string id)
    {
        if (string.IsNullOrEmpty(id) || datos.objetivos.Contains(id)) return;
        datos.objetivos.Add(id);
        Guardar();
    }

    public void MarcarMisionCompletada(string id)
    {
        if (string.IsNullOrEmpty(id) || datos.misionesCompletadas.Contains(id)) return;
        datos.misionesCompletadas.Add(id);
        if (datos.misionActiva == id) datos.misionActiva = "";
        Guardar();
    }

    public void EstablecerMisionActiva(string id)
    {
        datos.misionActiva = id ?? "";
        Guardar();
    }

    public void OtorgarInsignia(string id)
    {
        if (string.IsNullOrEmpty(id) || datos.insignias.Contains(id)) return;
        datos.insignias.Add(id);
        Guardar();
    }

    public void OtorgarXp(int cantidad)
    {
        if (cantidad <= 0) return;
        datos.xp += cantidad;
        Guardar();
    }

    public void Guardar()
    {
        SaveService.Guardar(datos);
        AlCambiar?.Invoke();
    }

    public void ReiniciarProgreso()
    {
        SaveService.Borrar();
        datos = new SaveData();
        Sanear();
        AlCambiar?.Invoke();
    }
}

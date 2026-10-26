using System;
using System.Collections.Generic;

/// <summary>
/// Progreso persistente del jugador. Es deliberadamente plano y serializable con
/// JsonUtility para poder migrarlo después a la API del proyecto sin tocar el gameplay.
/// </summary>
[Serializable]
public class SaveData
{
    public int version = 1;
    public int xp;
    public List<string> insignias = new List<string>();
    public List<string> misionesCompletadas = new List<string>();
    public List<string> objetivos = new List<string>();
    public string misionActiva = "";
    public string ultimoGuardado = "";
}

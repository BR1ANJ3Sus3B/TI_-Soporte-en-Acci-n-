using System;
using System.IO;
using UnityEngine;

/// <summary>
/// Lectura y escritura del progreso en disco. Guarda un JSON legible en
/// Application.persistentDataPath; toda la E/S está protegida para que un archivo
/// corrupto nunca impida arrancar el juego.
/// </summary>
public static class SaveService
{
    public const string NombreArchivo = "novatech_progreso.json";

    public static string Ruta => Path.Combine(Application.persistentDataPath, NombreArchivo);

    public static bool Existe => File.Exists(Ruta);

    public static SaveData Cargar()
    {
        try
        {
            if (!File.Exists(Ruta)) return new SaveData();

            string json = File.ReadAllText(Ruta);
            if (string.IsNullOrWhiteSpace(json)) return new SaveData();

            var datos = JsonUtility.FromJson<SaveData>(json);
            return datos ?? new SaveData();
        }
        catch (Exception excepcion)
        {
            Debug.LogWarning("No se pudo leer el progreso guardado: " + excepcion.Message);
            return new SaveData();
        }
    }

    public static void Guardar(SaveData datos)
    {
        if (datos == null) return;

        try
        {
            string carpeta = Path.GetDirectoryName(Ruta);
            if (!string.IsNullOrEmpty(carpeta)) Directory.CreateDirectory(carpeta);

            datos.ultimoGuardado = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            File.WriteAllText(Ruta, JsonUtility.ToJson(datos, true));
        }
        catch (Exception excepcion)
        {
            Debug.LogWarning("No se pudo guardar el progreso: " + excepcion.Message);
        }
    }

    public static void Borrar()
    {
        try
        {
            if (File.Exists(Ruta)) File.Delete(Ruta);
        }
        catch (Exception excepcion)
        {
            Debug.LogWarning("No se pudo borrar el progreso: " + excepcion.Message);
        }
    }
}

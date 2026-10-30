using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

/// <summary>Informa del estado del menú principal y de las escenas en Build Settings.</summary>
public static class EstadoMenu
{
    public static string Execute()
    {
        var sb = new StringBuilder();

        const string ruta = "Assets/Scenes/MainMenu.unity";
        sb.AppendLine("MainMenu existe = " + File.Exists(ruta));
        if (File.Exists(ruta))
            sb.AppendLine("  tamaño        = " + new FileInfo(ruta).Length + " bytes");

        sb.AppendLine("escenas en build:");
        var escenas = EditorBuildSettings.scenes;
        for (int i = 0; i < escenas.Length; i++)
            sb.AppendLine($"  [{i}] {escenas[i].path}  enabled={escenas[i].enabled}");

        sb.AppendLine("escena activa = " + UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene().path);
        return sb.ToString();
    }
}

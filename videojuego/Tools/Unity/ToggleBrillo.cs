using System.Text;
using UnityEngine;
using UnityEngine.UI;

/// <summary>Activa o desactiva los halos Brillo de los orbes para diagnosticar.</summary>
public static class Tools_ToggleBrillo
{
    public static string Execute(string accion)
    {
        bool activo = accion != "off";
        var sb = new StringBuilder();
        var raiz = GameObject.Find("MenuPrincipal");
        if (raiz == null) return "sin MenuPrincipal";

        foreach (var imagen in raiz.GetComponentsInChildren<Image>(true))
        {
            if (imagen.gameObject.name != "Brillo") continue;
            imagen.enabled = activo;
            sb.AppendLine($"{imagen.transform.parent.name}/Brillo -> enabled={activo}");
        }
        return sb.Length > 0 ? sb.ToString() : "no se encontró ningún Brillo";
    }
}

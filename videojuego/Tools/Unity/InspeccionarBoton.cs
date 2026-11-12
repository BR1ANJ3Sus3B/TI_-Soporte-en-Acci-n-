using System.Text;
using UnityEngine;
using UnityEngine.UI;

/// <summary>Inspecciona los hijos del botón JUGAR para localizar halos rectangulares.</summary>
public static class Tools_InspeccionarBoton
{
    public static string Execute()
    {
        var raiz = GameObject.Find("MenuPrincipal");
        if (raiz == null) return "sin MenuPrincipal";

        var sb = new StringBuilder();
        foreach (var nombre in new[] { "Botones/Boton_Jugar", "Botones/Boton_Misiones", "Botones/Boton_Ajustes" })
        {
            var t = raiz.transform.Find(nombre);
            if (t == null) { sb.AppendLine("FALTA " + nombre); continue; }

            sb.AppendLine("== " + nombre);
            for (int i = 0; i < t.childCount; i++)
            {
                var hijo = t.GetChild(i) as RectTransform;
                var img = hijo.GetComponent<Image>();
                string color = img != null
                    ? $"rgba({img.color.r:0.000},{img.color.g:0.000},{img.color.b:0.000},{img.color.a:0.000})"
                    : "(sin Image)";
                sb.AppendLine($"  {hijo.name}  size={hijo.sizeDelta}  {color}");
            }
        }

        return sb.ToString();
    }
}

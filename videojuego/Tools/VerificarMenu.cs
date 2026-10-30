using System.Text;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>Comprueba que el menú quedó completo y cableado en la escena guardada.</summary>
public static class VerificarMenu
{
    public static string Execute()
    {
        var sb = new StringBuilder();
        var escena = SceneManager.GetActiveScene();
        sb.AppendLine("escena activa = " + escena.name + " (" + escena.path + ")");

        var canvasGo = GameObject.Find("MenuPrincipal");
        if (canvasGo == null) return "ERROR: no hay MenuPrincipal";

        var controlador = canvasGo.GetComponent<MainMenuController>();
        sb.AppendLine("MainMenuController = " + (controlador != null));
        sb.AppendLine("EventSystem        = " + (Object.FindFirstObjectByType<EventSystem>() != null));
        sb.AppendLine("Camara             = " + (Camera.main != null));

        var so = new SerializedObject(controlador);
        foreach (var campo in new[] { "escenaJuego", "musica", "iconoMusica", "iconoSonido",
                                      "aviso", "avisoTitulo", "avisoTexto", "orbePrincipal" })
        {
            var p = so.FindProperty(campo);
            string valor = p.propertyType == SerializedPropertyType.String
                ? p.stringValue
                : (p.objectReferenceValue == null ? "NULL" : p.objectReferenceValue.name);
            sb.AppendLine("  " + campo + " = " + valor);
        }

        sb.AppendLine("--- botones ---");
        var botones = canvasGo.GetComponentsInChildren<Button>(true);
        sb.AppendLine("total botones = " + botones.Length);
        foreach (var b in botones)
        {
            int llamadas = b.onClick.GetPersistentEventCount();
            string destino = llamadas > 0
                ? b.onClick.GetPersistentTarget(0)?.GetType().Name + "." + b.onClick.GetPersistentMethodName(0)
                : "(sin listener)";
            sb.AppendLine("  " + Ruta(b.transform) + " -> " + destino);
        }

        sb.AppendLine("--- escenas en build ---");
        if (EditorBuildSettings.scenes.Length > 0)
        {
            sb.AppendLine("  [0] " + EditorBuildSettings.scenes[0].path);
            sb.AppendLine("  total = " + EditorBuildSettings.scenes.Length);
        }
        else sb.AppendLine("  (vacío)");

        // ¿El arte del menú tiene alfa de verdad en las esquinas?
        foreach (var nombre in new[] { "arc_orbe_jugar", "arc_orbe_misiones", "arc_orbe_ajustes", "arc_ico_musica", "arc_ico_config" })
        {
            var sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Generated_Images/Menu/" + nombre + ".png");
            sb.AppendLine("sprite " + nombre + " = " + (sprite != null ? sprite.rect.size.ToString() : "FALTA"));
        }

        return sb.ToString();
    }

    private static string Ruta(Transform t)
    {
        string ruta = t.name;
        while (t.parent != null && t.parent.name != "MenuPrincipal")
        {
            t = t.parent;
            ruta = t.name + "/" + ruta;
        }
        return ruta;
    }
}

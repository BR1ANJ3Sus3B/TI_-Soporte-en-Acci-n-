using UnityEditor;
using UnityEngine;

/// <summary>Encuadra la vista de escena sobre la entrada sur del edificio.</summary>
public static class Tools_EncuadrarEntrada
{
    public static string Execute()
    {
        var vista = SceneView.lastActiveSceneView;
        if (vista == null) return "sin SceneView";

        vista.in2DMode = true;
        vista.pivot = new Vector3(0.5f, -9.5f, 0f);
        vista.size = 9f;
        vista.Repaint();
        return "encuadrada en (0.5, -9.5) tamaño 9";
    }
}

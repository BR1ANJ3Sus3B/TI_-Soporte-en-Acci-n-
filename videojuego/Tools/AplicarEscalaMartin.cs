using System.Text;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

/// <summary>Iguala la altura de Martín a la de Alex y guarda la escena.</summary>
public static class AplicarEscalaMartin
{
    private const float AlturaPersonaje = 1.28f;   // fotograma de Alex: 128 px @ 100 ppu

    public static string Execute()
    {
        var sb = new StringBuilder();

        var jugador = GameObject.Find("Player");
        var martin = GameObject.Find("Gameplay/Martin");
        if (jugador == null || martin == null)
            return "ERROR: falta Player o Gameplay/Martin";

        var sr = martin.GetComponent<SpriteRenderer>();
        float altoNativo = sr.sprite.bounds.size.y;
        float escala = AlturaPersonaje / altoNativo;
        float alto = altoNativo * escala;

        var t = martin.transform;
        t.localScale = Vector3.one * escala;
        // Pies apoyados en el borde inferior de la celda (2,4) + 0.12, como en el builder.
        t.localPosition = new Vector3(2 + 0.5f, 4 + 0.12f + alto * 0.5f, 0f);

        sb.AppendLine("altoNativo = " + altoNativo.ToString("F5"));
        sb.AppendLine("escala     = " + escala.ToString("F5"));
        sb.AppendLine("altoFinal  = " + alto.ToString("F5"));
        sb.AppendLine("posicion   = " + t.localPosition);
        sb.AppendLine("player.bounds = " + jugador.GetComponent<SpriteRenderer>().bounds.size);
        sb.AppendLine("martin.bounds = " + sr.bounds.size);

        EditorSceneManager.MarkSceneDirty(martin.scene);
        EditorSceneManager.SaveScene(martin.scene);
        sb.AppendLine("scene saved: " + martin.scene.path);
        return sb.ToString();
    }
}

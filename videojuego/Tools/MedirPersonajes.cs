using System.Text;
using UnityEngine;

/// <summary>Mide en mundo los personajes para igualar tamaños sin adivinar.</summary>
public static class MedirPersonajes
{
    public static string Execute()
    {
        var sb = new StringBuilder();

        var jugador = GameObject.Find("Player");
        var martin = GameObject.Find("Gameplay/Martin");

        if (jugador != null)
        {
            var sr = jugador.GetComponent<SpriteRenderer>();
            sb.AppendLine("PLAYER sr.sprite = " + (sr.sprite != null ? sr.sprite.name : "null"));
            if (sr.sprite != null)
            {
                sb.AppendLine("PLAYER sprite.bounds.size = " + sr.sprite.bounds.size);
                sb.AppendLine("PLAYER sprite.ppu        = " + sr.sprite.pixelsPerUnit);
            }
            sb.AppendLine("PLAYER renderer.bounds   = " + sr.bounds.size);
            sb.AppendLine("PLAYER lossyScale        = " + jugador.transform.lossyScale);
        }

        if (martin != null)
        {
            var sr = martin.GetComponent<SpriteRenderer>();
            sb.AppendLine("MARTIN sr.sprite = " + (sr.sprite != null ? sr.sprite.name : "null"));
            if (sr.sprite != null)
            {
                sb.AppendLine("MARTIN sprite.bounds.size = " + sr.sprite.bounds.size);
                sb.AppendLine("MARTIN sprite.ppu        = " + sr.sprite.pixelsPerUnit);
            }
            sb.AppendLine("MARTIN renderer.bounds   = " + sr.bounds.size);
            sb.AppendLine("MARTIN localScale        = " + martin.transform.localScale);
            sb.AppendLine("MARTIN localPosition     = " + martin.transform.localPosition);
        }

        return sb.ToString();
    }
}

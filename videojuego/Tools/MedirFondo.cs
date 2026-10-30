using System.IO;
using System.Text;
using UnityEngine;

/// <summary>Mide los píxeles de esquina y borde del arte generado.</summary>
public static class Tools_MedirFondo
{
    public static string Execute()
    {
        var sb = new StringBuilder();
        foreach (var nombre in new[] { "arc_orbe_jugar", "arc_orbe_misiones", "arc_ico_musica" })
        {
            string ruta = $"Assets/Generated_Images/{nombre}.png";
            if (!File.Exists(ruta)) { sb.AppendLine("FALTA " + ruta); continue; }

            var tex = new Texture2D(2, 2, TextureFormat.RGBA32, false);
            tex.LoadImage(File.ReadAllBytes(ruta));
            int a = tex.width, h = tex.height;

            sb.AppendLine($"== {nombre} {a}x{h}");
            sb.AppendLine($"  esquina(0,0)      = {Px(tex, 0, 0)}");
            sb.AppendLine($"  esquina(a-1,0)    = {Px(tex, a - 1, 0)}");
            sb.AppendLine($"  esquina(0,h-1)    = {Px(tex, 0, h - 1)}");
            sb.AppendLine($"  borde sup centro  = {Px(tex, a / 2, 2)}");
            sb.AppendLine($"  borde izq centro  = {Px(tex, 2, h / 2)}");
            sb.AppendLine($"  fuera del orbe    = {Px(tex, a / 2 + a / 3, h / 2)}");
            sb.AppendLine($"  centro            = {Px(tex, a / 2, h / 2)}");
            Object.DestroyImmediate(tex);
        }
        return sb.ToString();
    }

    private static string Px(Texture2D tex, int x, int y)
    {
        Color c = tex.GetPixel(x, y);
        float max = Mathf.Max(c.r, Mathf.Max(c.g, c.b));
        return $"rgb({c.r:0.000},{c.g:0.000},{c.b:0.000}) max={max:0.000}";
    }
}

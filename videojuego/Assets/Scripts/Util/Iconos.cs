using UnityEngine;

/// <summary>
/// Genera en tiempo de ejecución los sprites auxiliares que el proyecto no tiene como
/// asset: el marcador flotante que señala un objetivo y el cable suelto del incidente.
/// Se generan una sola vez y se comparten.
/// </summary>
public static class Iconos
{
    private static Sprite marcador;
    private static Sprite cable;

    /// <summary>Anillo blanco; el color real lo pone el SpriteRenderer del marcador.</summary>
    public static Sprite Marcador
    {
        get
        {
            if (marcador == null) marcador = GenerarMarcador(64);
            return marcador;
        }
    }

    /// <summary>Cable de alimentación con enchufe, usado por el objeto interactivo.</summary>
    public static Sprite Cable
    {
        get
        {
            if (cable == null) cable = GenerarCable(48);
            return cable;
        }
    }

    private static Sprite GenerarMarcador(int lado)
    {
        var pixeles = new Color[lado * lado];
        float centro = (lado - 1) * 0.5f;
        float exterior = lado * 0.46f;
        float interior = lado * 0.33f;

        for (int y = 0; y < lado; y++)
            for (int x = 0; x < lado; x++)
            {
                float dx = x - centro;
                float dy = y - centro;
                float distancia = Mathf.Sqrt(dx * dx + dy * dy);

                float alfa = 0f;
                if (distancia <= exterior && distancia >= interior) alfa = 1f;

                // Punta hacia abajo, para que el anillo apunte al objeto que señala.
                if (y < centro - lado * 0.02f && y > centro - lado * 0.30f)
                {
                    float ancho = Mathf.Lerp(lado * 0.22f, 0f, (centro - lado * 0.02f - y) / (lado * 0.28f));
                    if (Mathf.Abs(dx) <= ancho) alfa = 1f;
                }

                pixeles[y * lado + x] = new Color(1f, 1f, 1f, alfa);
            }

        return Construir(pixeles, lado, lado, lado, "Icono_Marcador");
    }

    private static Sprite GenerarCable(int lado)
    {
        var pixeles = new Color[lado * lado];
        var vacio = new Color(0f, 0f, 0f, 0f);
        for (int i = 0; i < pixeles.Length; i++) pixeles[i] = vacio;

        var cuerpo = new Color(0.14f, 0.15f, 0.18f, 1f);
        var brillo = new Color(0.34f, 0.36f, 0.40f, 1f);

        // Tramo de cable ondulado.
        for (int x = 4; x < 37; x++)
        {
            float t = (x - 4) / 33f;
            float y = 15f + Mathf.Sin(t * Mathf.PI * 1.7f) * 7.5f;
            for (int d = -1; d <= 1; d++)
            {
                int yy = Mathf.RoundToInt(y) + d;
                if (yy < 0 || yy >= lado) continue;
                pixeles[yy * lado + x] = d == 0 ? cuerpo : brillo;
            }
        }

        // Enchufe en el extremo.
        var carcasa = new Color(0.86f, 0.88f, 0.91f, 1f);
        var clavija = new Color(0.55f, 0.58f, 0.63f, 1f);
        for (int y = 8; y < 24; y++)
            for (int x = 36; x < 45; x++)
            {
                bool dentro = y > 8 && y < 23 && x > 36 && x < 44;
                if (!dentro) continue;
                pixeles[y * lado + x] = y >= 13 && y <= 18 ? clavija : carcasa;
            }

        return Construir(pixeles, lado, lado, lado, "Icono_Cable");
    }

    private static Sprite Construir(Color[] pixeles, int ancho, int alto, float pixelesPorUnidad, string nombre)
    {
        var textura = new Texture2D(ancho, alto, TextureFormat.RGBA32, false);
        textura.name = nombre + "_Tex";
        textura.filterMode = FilterMode.Bilinear;
        textura.wrapMode = TextureWrapMode.Clamp;
        textura.SetPixels(pixeles);
        textura.Apply();

        var sprite = Sprite.Create(textura, new Rect(0, 0, ancho, alto),
            new Vector2(0.5f, 0.5f), pixelesPorUnidad);
        sprite.name = nombre;
        return sprite;
    }
}

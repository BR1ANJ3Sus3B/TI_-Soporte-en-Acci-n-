using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

public static class Sheet4Atlas
{
    private const string SheetPath =
        "Assets/Art/Characters/Scenes/Imagen de ChatGPT 9 oct 2026, 04_16_26 p.m-4.png";

    private static readonly string[][] Font =
    {
        new[] { "111", "101", "101", "101", "111" }, // 0
        new[] { "010", "010", "010", "010", "010" }, // 1
        new[] { "111", "001", "111", "100", "111" }, // 2
        new[] { "111", "001", "111", "001", "111" }, // 3
        new[] { "101", "101", "111", "001", "001" }, // 4
        new[] { "111", "100", "111", "001", "111" }, // 5
        new[] { "111", "100", "111", "101", "111" }, // 6
        new[] { "111", "001", "001", "001", "001" }, // 7
        new[] { "111", "101", "111", "101", "111" }, // 8
        new[] { "111", "101", "111", "001", "111" }, // 9
    };

    public static string Execute()
    {
        var importer = (TextureImporter)AssetImporter.GetAtPath(SheetPath);
        if (importer != null && !importer.isReadable)
        {
            importer.isReadable = true;
            importer.SaveAndReimport();
        }

        var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(SheetPath);
        if (texture == null) return "NO_TEXTURE";
        int W = texture.width, H = texture.height;

        var sprites = new List<(int index, Rect rect)>();
        foreach (var asset in AssetDatabase.LoadAllAssetsAtPath(SheetPath))
        {
            if (!(asset is Sprite sprite)) continue;
            int cut = sprite.name.LastIndexOf('_');
            if (cut < 0 || !int.TryParse(sprite.name.Substring(cut + 1), out int index)) continue;
            sprites.Add((index, sprite.rect));
        }
        sprites.Sort((a, b) => a.index.CompareTo(b.index));

        var buffer = texture.GetPixels(0, 0, W, H);
        var magenta = new Color(1f, 0f, 1f, 1f);

        var sb = new StringBuilder();
        sb.AppendLine($"SHEET {W}x{H} sprites={sprites.Count}");

        // Row/column ordering for the text dump.
        var ordered = sprites
            .Select(s => new
            {
                s.index,
                s.rect,
                top = H - (s.rect.y + s.rect.height)
            })
            .OrderBy(s => s.top)
            .ThenBy(s => s.rect.x)
            .ToList();

        foreach (var s in ordered)
            sb.AppendLine(string.Format("idx={0,3}  x={1,4} top={2,4} w={3,3} h={4,3}",
                s.index, (int)s.rect.x, (int)s.top, (int)s.rect.width, (int)s.rect.height));

        foreach (var s in ordered)
        {
            int x0 = (int)s.rect.x;
            int y0 = (int)s.rect.y;
            int w = (int)s.rect.width;
            int h = (int)s.rect.height;

            Outline(buffer, W, H, x0, y0, w, h, magenta);

            // Label in the top-left corner, inside the sprite box.
            int labelX = x0 + 2;
            int labelY = H - (int)s.top - 24;
            Plate(buffer, W, H, labelX - 1, labelY - 1, 4 * 12 + 6, 26, new Color(0f, 0f, 0.2f, 1f));
            DrawNumber(buffer, W, H, labelX, labelY, s.index);
        }

        var atlas = new Texture2D(W, H, TextureFormat.RGBA32, false);
        atlas.SetPixels(buffer);
        atlas.Apply();
        File.WriteAllBytes("Tools/ref/s4_annot.png", atlas.EncodeToPNG());
        UnityEngine.Object.DestroyImmediate(atlas);

        return sb.ToString();
    }

    private static void Plot(Color[] buffer, int W, int H, int x, int y, Color c)
    {
        if (x < 0 || y < 0 || x >= W || y >= H) return;
        buffer[y * W + x] = c;
    }

    private static void Outline(Color[] buffer, int W, int H, int x0, int y0, int w, int h, Color c)
    {
        for (int x = x0; x < x0 + w; x++)
        {
            Plot(buffer, W, H, x, y0, c);
            Plot(buffer, W, H, x, y0 + h - 1, c);
        }
        for (int y = y0; y < y0 + h; y++)
        {
            Plot(buffer, W, H, x0, y, c);
            Plot(buffer, W, H, x0 + w - 1, y, c);
        }
    }

    private static void Plate(Color[] buffer, int W, int H, int x0, int y0, int w, int h, Color c)
    {
        for (int y = y0; y < y0 + h; y++)
            for (int x = x0; x < x0 + w; x++)
                Plot(buffer, W, H, x, y, c);
    }

    /// <summary>Draws a base-10 index with a 3x5 pixel font scaled 4x, bottom-left origin.</summary>
    private static void DrawNumber(Color[] buffer, int W, int H, int x, int y, int value)
    {
        string digits = value.ToString();
        const int scale = 4;
        const int advance = 3 * scale + 2;
        var white = Color.white;

        for (int d = 0; d < digits.Length; d++)
        {
            var glyph = Font[digits[d] - '0'];
            int gx = x + d * advance;
            for (int row = 0; row < 5; row++)
            {
                string line = glyph[row];
                // row 0 is the top of the glyph.
                int py = y + (4 - row) * scale;
                for (int col = 0; col < 3; col++)
                {
                    if (line[col] != '1') continue;
                    for (int sy = 0; sy < scale; sy++)
                        for (int sx = 0; sx < scale; sx++)
                            Plot(buffer, W, H, gx + col * scale + sx, py + sy, white);
                }
            }
        }
    }
}

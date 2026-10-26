using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

// Renders every slice of the exterior sheet into an index-labelled grid, so a prop can
// be identified by sight and then referenced by its stable sprite index.
public static class Sheet4Grid
{
    private const string SheetPath =
        "Assets/Art/Characters/Scenes/Imagen de ChatGPT 9 oct 2026, 04_16_26 p.m-4.png";

    private const int Cell = 190;
    private const int Cols = 10;
    private const int Pad = 8;

    // 3x5 bitmap digits, one int per digit, rows top to bottom, bits left to right.
    private static readonly int[] Glyph =
    {
        0b111_101_101_101_111, // 0
        0b010_110_010_010_111, // 1
        0b111_001_111_100_111, // 2
        0b111_001_111_001_111, // 3
        0b101_101_111_001_001, // 4
        0b111_100_111_001_111, // 5
        0b111_100_111_101_111, // 6
        0b111_001_001_001_001, // 7
        0b111_101_111_101_111, // 8
        0b111_101_111_001_111, // 9
    };

    public static string Execute()
    {
        var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(SheetPath);
        if (texture == null) return "NO_TEXTURE";

        var sprites = new List<(int index, Sprite sprite)>();
        foreach (var asset in AssetDatabase.LoadAllAssetsAtPath(SheetPath))
        {
            if (!(asset is Sprite sprite)) continue;
            int cut = sprite.name.LastIndexOf('_');
            if (cut < 0 || !int.TryParse(sprite.name.Substring(cut + 1), out int index)) continue;
            sprites.Add((index, sprite));
        }
        sprites.Sort((a, b) => a.index.CompareTo(b.index));

        int rows = (sprites.Count + Cols - 1) / Cols;
        int W = Cols * Cell, H = rows * Cell;
        var buffer = new Color[W * H];
        for (int i = 0; i < buffer.Length; i++) buffer[i] = new Color(0.09f, 0.10f, 0.12f, 1f);

        // Cell separators so the grid is countable.
        var line = new Color(0.30f, 0.32f, 0.36f, 1f);
        for (int c = 0; c <= Cols; c++)
        {
            int x = Mathf.Min(c * Cell, W - 1);
            for (int y = 0; y < H; y++) buffer[y * W + x] = line;
        }
        for (int r = 0; r <= rows; r++)
        {
            int y = Mathf.Min(r * Cell, H - 1);
            for (int x = 0; x < W; x++) buffer[y * W + x] = line;
        }

        var sb = new StringBuilder();
        for (int i = 0; i < sprites.Count; i++)
        {
            var (index, sprite) = sprites[i];
            var r = sprite.rect;
            int px = (int)r.x, py = (int)r.y, pw = (int)r.width, ph = (int)r.height;
            var pixels = texture.GetPixels(px, py, pw, ph);

            int col = i % Cols, row = i / Cols;
            int boxX = col * Cell, boxY = H - (row + 1) * Cell;

            // Fit inside the cell preserving aspect.
            int room = Cell - Pad * 2 - 22;
            float scale = Mathf.Min((float)room / pw, (float)room / ph);
            int dw = Mathf.Max(1, Mathf.RoundToInt(pw * scale));
            int dh = Mathf.Max(1, Mathf.RoundToInt(ph * scale));
            int ox = boxX + (Cell - dw) / 2;
            int oy = boxY + 22 + (room - dh) / 2;

            for (int y = 0; y < dh; y++)
                for (int x = 0; x < dw; x++)
                {
                    int sx = Mathf.Clamp(Mathf.FloorToInt(x / scale), 0, pw - 1);
                    int sy = Mathf.Clamp(Mathf.FloorToInt(y / scale), 0, ph - 1);
                    var c = pixels[sy * pw + sx];
                    if (c.a <= 0.02f) continue;
                    int tx = ox + x, ty = oy + y;
                    if (tx < 0 || ty < 0 || tx >= W || ty >= H) continue;
                    buffer[ty * W + tx] = new Color(c.r, c.g, c.b, 1f);
                }

            DrawNumber(buffer, W, H, boxX + Pad, boxY + 6, index);
            sb.AppendLine($"idx={index,3} rect={pw}x{ph}");
        }

        var image = new Texture2D(W, H, TextureFormat.RGBA32, false);
        image.SetPixels(buffer);
        image.Apply();
        File.WriteAllBytes("Tools/ref/s4_grid.png", image.EncodeToPNG());
        UnityEngine.Object.DestroyImmediate(image);

        sb.AppendLine($"IMAGE Tools/ref/s4_grid.png {W}x{H} cols={Cols} count={sprites.Count}");
        return sb.ToString();
    }

    private static void DrawNumber(Color[] buffer, int W, int H, int x, int y, int value)
    {
        var digits = value.ToString();
        var ink = new Color(1f, 0.35f, 0.95f, 1f);
        const int scale = 3, gx = 3 * scale + 4, gy = 5 * scale;
        for (int d = 0; d < digits.Length; d++)
        {
            int glyph = Glyph[digits[d] - '0'];
            for (int r = 0; r < 5; r++)
                for (int c = 0; c < 3; c++)
                {
                    int bit = 1 << (14 - (r * 3 + c));
                    if ((glyph & bit) == 0) continue;
                    for (int sy = 0; sy < scale; sy++)
                        for (int sx = 0; sx < scale; sx++)
                        {
                            int tx = x + d * gx + c * scale + sx;
                            int ty = y + (4 - r) * scale + sy;
                            if (tx < 0 || ty < 0 || tx >= W || ty >= H) continue;
                            buffer[ty * W + tx] = ink;
                        }
                }
        }
    }
}

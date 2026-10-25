using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

public static class Sheet4Pick
{
    private const string SheetPath =
        "Assets/Art/Characters/Scenes/Imagen de ChatGPT 9 oct 2026, 04_16_26 p.m-4.png";

    // Sprites to inspect.
    private static readonly int[] Wanted = { 68, 69, 70, 71, 72, 73, 74, 89, 90, 91, 98, 99, 100, 107, 108, 109, 110, 111, 112, 113, 114, 75, 45, 41, 40, 12, 4, 16, 51, 20 };

    private const int Cell = 150;
    private const int Cols = 6;

    public static string Execute()
    {
        var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(SheetPath);
        if (texture == null) return "NO_TEXTURE";

        var found = new List<(int index, Sprite sprite)>();
        foreach (var asset in AssetDatabase.LoadAllAssetsAtPath(SheetPath))
        {
            if (!(asset is Sprite sprite)) continue;
            int cut = sprite.name.LastIndexOf('_');
            if (cut < 0 || !int.TryParse(sprite.name.Substring(cut + 1), out int index)) continue;
            if (!Wanted.Contains(index)) continue;
            found.Add((index, sprite));
        }
        found.Sort((a, b) => Array.IndexOf(Wanted, a.index).CompareTo(Array.IndexOf(Wanted, b.index)));

        int rows = (found.Count + Cols - 1) / Cols;
        int W = Cols * Cell, H = rows * Cell;
        var buffer = new Color[W * H];
        for (int i = 0; i < buffer.Length; i++) buffer[i] = new Color(0.1f, 0.1f, 0.12f, 1f);

        var sb = new StringBuilder();
        for (int i = 0; i < found.Count; i++)
        {
            var (index, sprite) = found[i];
            var r = sprite.rect;
            int px = (int)r.x, py = (int)r.y, pw = (int)r.width, ph = (int)r.height;
            var pixels = texture.GetPixels(px, py, pw, ph);

            int minX = pw, minY = ph, maxX = -1, maxY = -1;
            for (int y = 0; y < ph; y++)
                for (int x = 0; x < pw; x++)
                {
                    if (pixels[y * pw + x].a <= 0.02f) continue;
                    if (x < minX) minX = x;
                    if (x > maxX) maxX = x;
                    if (y < minY) minY = y;
                    if (y > maxY) maxY = y;
                }
            float fill = 0f;
            if (maxX >= 0)
            {
                int opaque = 0;
                for (int y = minY; y <= maxY; y++)
                    for (int x = minX; x <= maxX; x++)
                        if (pixels[y * pw + x].a > 0.02f) opaque++;
                fill = (float)opaque / ((maxX - minX + 1) * (maxY - minY + 1));
            }

            sb.AppendLine(string.Format(
                "idx={0,3} rect={1}x{2} opaque=({3},{4},{5},{6}) fill={7:P1}",
                index, pw, ph, minX, minY, maxX - minX + 1, maxY - minY + 1, fill));

            // Blit into the contact cell, centred, 1:1 (cells are bigger than the slices).
            int col = i % Cols, row = i / Cols;
            int cx = col * Cell + (Cell - pw) / 2;
            int cy = H - (row + 1) * Cell + (Cell - ph) / 2;
            for (int y = 0; y < ph; y++)
                for (int x = 0; x < pw; x++)
                {
                    var c = pixels[y * pw + x];
                    if (c.a <= 0.02f) continue;
                    int tx = cx + x, ty = cy + y;
                    if (tx < 0 || ty < 0 || tx >= W || ty >= H) continue;
                    buffer[ty * W + tx] = c;
                }
        }

        var image = new Texture2D(W, H, TextureFormat.RGBA32, false);
        image.SetPixels(buffer);
        image.Apply();
        File.WriteAllBytes("Tools/ref/s4_pick2.png", image.EncodeToPNG());
        UnityEngine.Object.DestroyImmediate(image);

        sb.AppendLine($"IMAGE {W}x{H} cols={Cols} order={string.Join(",", found.Select(f => f.index))}");
        return sb.ToString();
    }
}

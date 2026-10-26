using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class VerifyCoverage
{
    private const int MinX = -22, MaxX = 22, MinY = -11, MaxY = 19;
    private const int PixelsPerUnit = 48;
    private const float Inset = 0.10f;      // ignore the outer 10% of each cell (seams)
    private const float HoleThreshold = 0.02f;

    public static string Execute()
    {
        int width = (MaxX - MinX + 1) * PixelsPerUnit;
        int height = (MaxY - MinY + 1) * PixelsPerUnit;

        var go = new GameObject("__CoverCam__");
        var cam = go.AddComponent<Camera>();
        cam.orthographic = true;
        cam.orthographicSize = height / (2f * PixelsPerUnit);
        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.backgroundColor = new Color(1f, 0f, 1f, 1f);
        cam.nearClipPlane = -100f;
        cam.farClipPlane = 100f;
        go.transform.position = new Vector3(MinX + width / (2f * PixelsPerUnit),
                                            MinY + height / (2f * PixelsPerUnit), -20f);

        var rt = new RenderTexture(width, height, 24);
        cam.targetTexture = rt;
        cam.Render();
        RenderTexture.active = rt;
        var tex = new Texture2D(width, height, TextureFormat.RGBA32, false);
        tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        tex.Apply();
        RenderTexture.active = null;

        var floor = GameObject.Find("Tilemap_Floor")?.GetComponent<Tilemap>();
        var walls = GameObject.Find("Tilemap_Walls")?.GetComponent<Tilemap>();
        var trim = GameObject.Find("Tilemap_WallTrim")?.GetComponent<Tilemap>();
        var win = GameObject.Find("Tilemap_Windows")?.GetComponent<Tilemap>();
        var door = GameObject.Find("Tilemap_Doors")?.GetComponent<Tilemap>();

        var bad = new List<string>();
        long totalHoles = 0;
        int wallCells = 0, trimCells = 0;

        for (int cy = MinY; cy <= MaxY; cy++)
        {
            for (int cx = MinX; cx <= MaxX; cx++)
            {
                var cell = new Vector3Int(cx, cy, 0);
                bool hasWall = walls != null && walls.GetTile(cell) != null;
                bool hasTrim = trim != null && trim.GetTile(cell) != null;
                bool hasFloor = floor != null && floor.GetTile(cell) != null;
                bool hasWin = win != null && win.GetTile(cell) != null;
                bool hasDoor = door != null && door.GetTile(cell) != null;
                if (!hasWall && !hasFloor && !hasWin && !hasDoor) continue;
                if (hasWall) wallCells++;
                if (hasTrim) trimCells++;

                int x0 = (int)((cx - MinX + Inset) * PixelsPerUnit);
                int x1 = (int)((cx - MinX + 1 - Inset) * PixelsPerUnit);
                int y0 = (int)((cy - MinY + Inset) * PixelsPerUnit);
                int y1 = (int)((cy - MinY + 1 - Inset) * PixelsPerUnit);

                int sampled = 0, holes = 0;
                for (int py = y0; py < y1; py++)
                {
                    for (int px = x0; px < x1; px++)
                    {
                        var c = tex.GetPixel(px, py);
                        sampled++;
                        if (c.r > 0.9f && c.g < 0.1f && c.b > 0.9f) holes++;
                    }
                }

                totalHoles += holes;
                float frac = sampled == 0 ? 0f : (float)holes / sampled;
                if (frac > HoleThreshold)
                    bad.Add($"  cell ({cx},{cy}) holes={frac:P1} wall={hasWall} trim={hasTrim} floor={hasFloor} win={hasWin} door={hasDoor}");
            }
        }

        Object.DestroyImmediate(rt);
        Object.DestroyImmediate(tex);
        Object.DestroyImmediate(go);

        var sb = new StringBuilder();
        sb.AppendLine($"wallCells={wallCells} cornerTrimCells={trimCells} totalHolePixels={totalHoles}");
        sb.AppendLine($"cellsWithHoles={bad.Count}");
        foreach (var line in bad) sb.AppendLine(line);
        return sb.ToString();
    }
}

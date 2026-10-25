using System.IO;
using UnityEngine;

public static class RenderSpot
{
    private static readonly (string name, float cx, float cy, float half, int w, int h)[] Spots =
    {
        ("parking",   0f, -21.5f,  5f, 1400, 620),
        ("entrance",  0f, -13f,    6f, 1200, 700),
        ("southwest", -24f, -19f,  6f, 900, 700),
        ("east",      26f,  1f,    7f, 800, 800),
    };

    public static string Execute()
    {
        var report = "";
        foreach (var spot in Spots)
        {
            var go = new GameObject("__SpotCam__");
            var cam = go.AddComponent<Camera>();
            cam.orthographic = true;
            cam.orthographicSize = spot.half;
            cam.clearFlags = CameraClearFlags.SolidColor;
            cam.backgroundColor = new Color(0.02f, 0.02f, 0.04f, 1f);
            cam.nearClipPlane = -100f;
            cam.farClipPlane = 100f;
            go.transform.position = new Vector3(spot.cx, spot.cy, -20f);

            var rt = new RenderTexture(spot.w, spot.h, 24);
            cam.targetTexture = rt;
            cam.Render();
            RenderTexture.active = rt;
            var tex = new Texture2D(spot.w, spot.h, TextureFormat.RGBA32, false);
            tex.ReadPixels(new Rect(0, 0, spot.w, spot.h), 0, 0);
            tex.Apply();
            RenderTexture.active = null;

            string path = "Tools/ref/spot_" + spot.name + ".png";
            File.WriteAllBytes(path, tex.EncodeToPNG());
            report += path + " ";

            cam.targetTexture = null;
            Object.DestroyImmediate(rt);
            Object.DestroyImmediate(tex);
            Object.DestroyImmediate(go);
        }
        return "OK " + report;
    }
}

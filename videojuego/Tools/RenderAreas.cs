using System.IO;
using UnityEngine;

public static class RenderAreas
{
    private static readonly (string name, float cx, float cy, float half, int w, int h)[] Spots =
    {
        ("recepcion", 0f, 4f, 7f, 1200, 1200),
        ("soporte",  -6f, -5f, 4.5f, 1000, 1000),
        ("salaoculta", 26f, 13f, 5f, 1000, 1000),
    };

    public static string Execute()
    {
        var report = "";
        foreach (var spot in Spots)
        {
            var go = new GameObject("__AreaCam__");
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

            string path = "Tools/ref/area_" + spot.name + ".png";
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

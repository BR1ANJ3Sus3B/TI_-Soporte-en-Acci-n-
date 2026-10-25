using System.IO;
using UnityEngine;

public static class RenderMap
{
    public static string Execute()
    {
        // Whole plot, plus a margin.
        int width = 1600;
        int height = 1350;
        float halfHeight = 26.5f;
        var centre = new Vector2(0.5f, -3f);

        var go = new GameObject("__MapCam__");
        var cam = go.AddComponent<Camera>();
        cam.orthographic = true;
        cam.orthographicSize = halfHeight;
        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.backgroundColor = new Color(0.02f, 0.02f, 0.04f, 1f);
        cam.nearClipPlane = -100f;
        cam.farClipPlane = 100f;
        go.transform.position = new Vector3(centre.x, centre.y, -20f);

        var rt = new RenderTexture(width, height, 24);
        cam.targetTexture = rt;
        cam.Render();

        RenderTexture.active = rt;
        var tex = new Texture2D(width, height, TextureFormat.RGBA32, false);
        tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        tex.Apply();
        RenderTexture.active = null;

        File.WriteAllBytes("Tools/ref/map.png", tex.EncodeToPNG());

        cam.targetTexture = null;
        Object.DestroyImmediate(rt);
        Object.DestroyImmediate(tex);
        Object.DestroyImmediate(go);

        return $"OK Tools/ref/map.png {width}x{height} centre={centre} halfHeight={halfHeight}";
    }
}

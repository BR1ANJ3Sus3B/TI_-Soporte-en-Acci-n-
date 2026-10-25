using System;
using System.IO;
using System.Reflection;
using UnityEngine;

public static class RenderFull
{
    public static string Execute()
    {
        int width = 1440;
        int height = 992;
        float halfHeight = 15.5f;
        var centre = new Vector2(0.5f, 4.5f);

        var go = new GameObject("__FullCam__");
        var cam = go.AddComponent<Camera>();
        cam.orthographic = true;
        cam.orthographicSize = halfHeight;
        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.backgroundColor = new Color(0.06f, 0.16f, 0.10f, 1f); // green so empty area is obvious
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

        File.WriteAllBytes("Tools/ref/full.png", tex.EncodeToPNG());

        cam.targetTexture = null;
        UnityEngine.Object.DestroyImmediate(rt);
        UnityEngine.Object.DestroyImmediate(tex);
        UnityEngine.Object.DestroyImmediate(go);

        return $"OK Tools/ref/full.png {width}x{height} centre={centre} halfHeight={halfHeight}";
    }
}

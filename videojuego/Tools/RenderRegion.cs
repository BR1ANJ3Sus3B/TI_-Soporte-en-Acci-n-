using System.IO;
using UnityEngine;

public static class RenderRegion
{
    // Centre and half-height in world units.
    private static readonly Vector2 Centre = new Vector2(0.5f, 4f);
    private const float HalfHeight = 12f;
    private const int Resolution = 1024;
    private const string OutPath = "Tools/ref/region.png";

    public static string Execute()
    {
        var go = new GameObject("__RegionCam__");
        var cam = go.AddComponent<Camera>();
        cam.orthographic = true;
        cam.orthographicSize = HalfHeight;
        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.backgroundColor = new Color(0.015f, 0.03f, 0.06f, 1f); // the game camera's own clear colour
        cam.nearClipPlane = -100f;
        cam.farClipPlane = 100f;
        go.transform.position = new Vector3(Centre.x, Centre.y, -20f);

        var rt = new RenderTexture(Resolution, Resolution, 24);
        cam.targetTexture = rt;
        cam.Render();

        RenderTexture.active = rt;
        var tex = new Texture2D(Resolution, Resolution, TextureFormat.RGBA32, false);
        tex.ReadPixels(new Rect(0, 0, Resolution, Resolution), 0, 0);
        tex.Apply();
        RenderTexture.active = null;

        File.WriteAllBytes(OutPath, tex.EncodeToPNG());

        cam.targetTexture = null;
        Object.DestroyImmediate(rt);
        Object.DestroyImmediate(tex);
        Object.DestroyImmediate(go);

        return $"OK {OutPath} centre={Centre} halfHeight={HalfHeight} width={2f * HalfHeight * Resolution / Resolution}";
    }
}

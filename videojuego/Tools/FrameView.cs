using System;
using UnityEditor;
using UnityEngine;

public static class FrameView
{
    public static string Execute(string json)
    {
        float x = 0f, y = 0f, size = 20f;
        if (!string.IsNullOrEmpty(json))
        {
            var parts = json.Trim('{', '}').Split(',');
            foreach (var part in parts)
            {
                var kv = part.Split(':');
                if (kv.Length != 2) continue;
                var key = kv[0].Trim().Trim('"');
                float value;
                if (!float.TryParse(kv[1].Trim(), System.Globalization.NumberStyles.Float,
                        System.Globalization.CultureInfo.InvariantCulture, out value)) continue;
                if (key == "x") x = value;
                else if (key == "y") y = value;
                else if (key == "size") size = value;
            }
        }

        var view = SceneView.lastActiveSceneView;
        if (view == null) return "NO_SCENE_VIEW";
        view.in2DMode = true;
        view.LookAt(new Vector3(x, y, 0f), Quaternion.identity, size, true, false);
        view.Repaint();
        return string.Format("FRAMED x={0} y={1} size={2}", x, y, size);
    }
}

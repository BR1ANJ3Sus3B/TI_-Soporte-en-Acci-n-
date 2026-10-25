using UnityEditor;
using UnityEngine;

public static class SceneViewAt
{
    public static string Execute()
    {
        var view = SceneView.lastActiveSceneView;
        if (view == null) return "NO_SCENEVIEW";

        var pivot = new Vector3(-11f, 2f, 0f);
        view.orthographic = true;
        view.LookAt(pivot, Quaternion.identity, 6f, true);
        view.pivot = pivot;
        view.rotation = Quaternion.identity;
        view.orthographic = true;
        view.size = 5f;
        view.Repaint();
        return "OK pivot=" + view.pivot + " size=" + view.size + " ortho=" + view.orthographic;
    }
}

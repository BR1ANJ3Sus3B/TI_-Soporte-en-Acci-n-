using System;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

// An explicit request file lets this one-off operation run inside the open editor.
// It never changes scenes on an ordinary reload or startup without that request.
public static class OpenRepairedMap
{
    private const string Request = "Logs/open-repaired-map.request";
    private const string ScenePath = "Assets/Art/Characters/Scenes/Nivel01_Empresa/nivel1.unity";

    [InitializeOnLoadMethod]
    private static void CheckRequest()
    {
        if (File.Exists(Request)) EditorApplication.delayCall += ProcessRequest;
    }

    [MenuItem("Tools/TI Soporte/Abrir mapa reparado")]
    public static void Open()
    {
        Directory.CreateDirectory("Logs");
        File.WriteAllText(Request, "Open saved level, preserving current scenes first.");
        ProcessRequest();
    }

    private static void ProcessRequest()
    {
        if (!File.Exists(Request)) return;
        if (EditorApplication.isPlayingOrWillChangePlaymode)
        {
            EditorApplication.playModeStateChanged -= OnPlayModeChanged;
            EditorApplication.playModeStateChanged += OnPlayModeChanged;
            EditorApplication.isPlaying = false;
            return;
        }
        if (EditorApplication.isCompiling || EditorApplication.isUpdating)
        {
            EditorApplication.delayCall += ProcessRequest;
            return;
        }
        // Consume the request once; a failure must not create an automatic retry loop.
        File.Delete(Request);
        try
        {
            var destination = AssetDatabase.LoadAssetAtPath<SceneAsset>(ScenePath);
            if (destination == null) throw new InvalidOperationException("No se encontró nivel1.");
            string backup = "Logs/MapRepairBackup/Editor-" + DateTime.Now.ToString("yyyyMMdd-HHmmss-fff");
            Directory.CreateDirectory(backup);
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                if (scene.isLoaded && !EditorSceneManager.SaveScene(scene, backup + "/Scene-" + i + ".unity", true))
                    throw new IOException("No se pudo conservar la escena abierta. No se reemplazó.");
            }
            var level = EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);
            foreach (var root in level.GetRootGameObjects())
                SceneVisibilityManager.instance.Show(root, true);
            var view = SceneView.lastActiveSceneView;
            if (view != null)
            {
                view.in2DMode = true;
                view.Frame(new Bounds(Vector3.zero, new Vector3(28, 12, 1)), false);
            }
            File.WriteAllText("Logs/open-repaired-map.result", "OPENED " + ScenePath + "\nBACKUP " + backup);
            Debug.Log("Mapa reparado cargado. Copia de la escena anterior: " + backup);
        }
        catch (Exception exception)
        {
            File.WriteAllText("Logs/open-repaired-map.result", "ERROR " + exception.Message);
            Debug.LogException(exception);
        }
    }

    private static void OnPlayModeChanged(PlayModeStateChange state)
    {
        if (state != PlayModeStateChange.EnteredEditMode) return;
        EditorApplication.playModeStateChanged -= OnPlayModeChanged;
        EditorApplication.delayCall += ProcessRequest;
    }
}

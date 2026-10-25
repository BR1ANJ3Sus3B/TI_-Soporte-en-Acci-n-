using System;
using UnityEditor;
using UnityEditor.Animations;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class MovementSetup
{
    private const string ControllerPath = "Assets/Art/Characters/Player.controller";
    private const string PrefabPath = "Assets/Scripts/Player.prefab";

    [MenuItem("Tools/TI Soporte/Asignar animaciones al Player seleccionado")]
    private static void AssignToSelectedPlayer()
    {
        var player = Selection.activeGameObject;
        if (player == null || EditorUtility.IsPersistent(player))
        {
            EditorUtility.DisplayDialog("Selecciona Player",
                "Selecciona el objeto Player en Hierarchy o abre su prefab para editarlo.", "Aceptar");
            return;
        }

        var controller = AssetDatabase.LoadAssetAtPath<AnimatorController>(ControllerPath);
        if (controller == null)
        {
            Debug.LogError("No se encontró Player.controller en " + ControllerPath);
            return;
        }
        if (player.GetComponent<SpriteRenderer>() == null)
        {
            EditorUtility.DisplayDialog("Selecciona el personaje",
                "Selecciona el objeto que contiene el SpriteRenderer del personaje.", "Aceptar");
            return;
        }

        Undo.IncrementCurrentGroup();
        int group = Undo.GetCurrentGroup();
        Undo.SetCurrentGroupName("Asignar animaciones a Player");
        var animator = player.GetComponent<Animator>();
        if (animator == null) animator = Undo.AddComponent<Animator>(player);
        Undo.RecordObject(animator, "Asignar controlador Player");
        animator.runtimeAnimatorController = controller;
        animator.enabled = true;
        animator.applyRootMotion = false;

        var body = player.GetComponent<Rigidbody2D>();
        if (body == null) body = Undo.AddComponent<Rigidbody2D>(player);
        Undo.RecordObject(body, "Configurar movimiento 2D");
        body.gravityScale = 0;
        body.freezeRotation = true;
        body.interpolation = RigidbodyInterpolation2D.Interpolate;

        var movement = player.GetComponent<PlayerMovement>();
        if (movement == null) movement = Undo.AddComponent<PlayerMovement>(player);
        Undo.RecordObject(movement, "Activar movimiento");
        movement.enabled = true;

        foreach (var component in new Component[] { animator, body, movement })
        {
            EditorUtility.SetDirty(component);
            if (PrefabUtility.IsPartOfPrefabInstance(component))
                PrefabUtility.RecordPrefabInstancePropertyModifications(component);
        }
        EditorSceneManager.MarkSceneDirty(player.scene);
        Undo.CollapseUndoOperations(group);
        Debug.Log("Animaciones de reposo, caminar y correr asignadas a " + player.name +
            ". Guarda la escena para conservar el cambio.", player);
    }

    [MenuItem("Tools/TI Soporte/Asignar animaciones al Player seleccionado", true)]
    private static bool CanAssignToSelectedPlayer()
    {
        return !EditorApplication.isPlayingOrWillChangePlaymode &&
            Selection.activeGameObject != null;
    }

    // Run explicitly; never modifies assets automatically on import.
    public static void RepairAndValidate()
    {
        var controller = AssetDatabase.LoadAssetAtPath<AnimatorController>(ControllerPath);
        var machine = controller.layers[0].stateMachine;
        string[] directions = { "Down", "Up", "Left", "Right" };
        foreach (string direction in directions)
        {
            var run = AssetDatabase.LoadAssetAtPath<AnimationClip>("Assets/Animacion/" +
                (direction == "Down" ? "AlexRunn" : "Alex_Run_" + direction) + ".anim");
            var walk = direction == "Down"
                ? AssetDatabase.LoadAssetAtPath<AnimationClip>("Assets/Animacion/Alex_Walk_Down.anim")
                : run;
            SetState(machine, "Alex_Run_" + direction, run, 1f);
            SetState(machine, "Alex_Walk_" + direction, walk, direction == "Down" ? 1f : 0.6f);
        }
        foreach (var child in machine.states)
            if (child.state.name == "Alex_Idle_Down") machine.defaultState = child.state;
        EditorUtility.SetDirty(controller);

        var prefab = PrefabUtility.LoadPrefabContents(PrefabPath);
        try
        {
            var animator = prefab.GetComponent<Animator>();
            animator.runtimeAnimatorController = controller;
            if (!prefab.GetComponent<PlayerMovement>()) prefab.AddComponent<PlayerMovement>();
            var body = prefab.GetComponent<Rigidbody2D>();
            body.gravityScale = 0;
            body.freezeRotation = true;
            body.interpolation = RigidbodyInterpolation2D.Interpolate;
            body.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            var sprite = prefab.GetComponent<SpriteRenderer>().sprite;
            var collider = prefab.GetComponent<CapsuleCollider2D>();
            collider.size = new Vector2(sprite.bounds.size.x * 0.45f, sprite.bounds.size.y * 0.25f);
            collider.offset = new Vector2(sprite.bounds.center.x, sprite.bounds.min.y + collider.size.y / 2f);
            PrefabUtility.SaveAsPrefabAsset(prefab, PrefabPath);
        }
        finally { PrefabUtility.UnloadPrefabContents(prefab); }

        var scene = EditorSceneManager.OpenScene("Assets/Art/Characters/Scenes/SampleScene.unity");
        if (UnityEngine.Object.FindFirstObjectByType<PlayerMovement>() == null)
        {
            var player = (GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>(PrefabPath));
            player.transform.position = Vector3.zero;
        }
        EditorSceneManager.SaveScene(scene);
        AssetDatabase.SaveAssets();

        var instance = UnityEngine.Object.FindFirstObjectByType<PlayerMovement>();
        var testAnimator = instance.GetComponent<Animator>();
        testAnimator.Rebind();
        foreach (string direction in directions)
            foreach (string action in new[] { "Idle", "Walk", "Run" })
                if (!testAnimator.HasState(0, Animator.StringToHash("Base Layer.Alex_" + action + "_" + direction)))
                    throw new Exception("Missing animation: " + action + " " + direction);
        if (instance.GetComponent<CapsuleCollider2D>().size.x >= instance.GetComponent<SpriteRenderer>().sprite.bounds.size.x)
            throw new Exception("Player collider is too wide.");
        Debug.Log("MOVEMENT_VALIDATION_PASSED: prefab, scene and all 12 directional states.");
    }

    private static void SetState(AnimatorStateMachine machine, string name, AnimationClip clip, float speed)
    {
        if (clip == null) throw new Exception("Missing clip for " + name);
        AnimatorState state = null;
        foreach (var child in machine.states)
            if (child.state.name == name) state = child.state;
        if (state == null) state = machine.AddState(name);
        state.motion = clip;
        state.speed = speed;
    }
}

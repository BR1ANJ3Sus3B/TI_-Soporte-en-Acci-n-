using UnityEngine;

// Keeps the entire repaired level visible in landscape and portrait windows.
[RequireComponent(typeof(Camera))]
public sealed class MapCameraFrame : MonoBehaviour
{
    [SerializeField] private Vector2 mapCenter = Vector2.zero;
    [SerializeField] private Vector2 mapSize = new Vector2(26f, 10f);
    [SerializeField, Min(0f)] private float padding = 3f;

    private Camera mapCamera;

    private void Awake()
    {
        mapCamera = GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        float aspect = Mathf.Max(mapCamera.aspect, 0.01f);
        mapCamera.orthographic = true;
        mapCamera.orthographicSize = Mathf.Max(
            Mathf.Abs(mapSize.y) * 0.5f + padding,
            (Mathf.Abs(mapSize.x) * 0.5f + padding) / aspect);
        transform.position = new Vector3(mapCenter.x, mapCenter.y, transform.position.z);
    }
}

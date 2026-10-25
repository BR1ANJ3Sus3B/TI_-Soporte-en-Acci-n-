using UnityEngine;

// Follows the player and keeps the viewport inside the level bounds.
[RequireComponent(typeof(Camera))]
public sealed class MapCameraFrame : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector2 mapCenter = Vector2.zero;
    [SerializeField] private Vector2 mapSize = new Vector2(45f, 31f);
    [SerializeField, Min(0.1f)] private float zoomSize = 6.5f;
    [SerializeField, Min(0f)] private float followSmoothing = 0.18f;

    private Camera mapCamera;
    private Vector3 velocity;

    private void Awake()
    {
        mapCamera = GetComponent<Camera>();
        mapCamera.orthographic = true;
        mapCamera.orthographicSize = zoomSize;
        transform.position = FocusPoint();
    }

    private void LateUpdate()
    {
        mapCamera.orthographic = true;
        mapCamera.orthographicSize = zoomSize;

        var desired = FocusPoint();
        transform.position = followSmoothing <= 0f
            ? desired
            : Vector3.SmoothDamp(transform.position, desired, ref velocity, followSmoothing);
    }

    private Vector3 FocusPoint()
    {
        Vector2 focus = target != null ? (Vector2)target.position : mapCenter;

        float halfHeight = zoomSize;
        float halfWidth = halfHeight * Mathf.Max(mapCamera.aspect, 0.01f);

        focus.x = ClampInside(focus.x,
            mapCenter.x - mapSize.x * 0.5f, mapCenter.x + mapSize.x * 0.5f, halfWidth);
        focus.y = ClampInside(focus.y,
            mapCenter.y - mapSize.y * 0.5f, mapCenter.y + mapSize.y * 0.5f, halfHeight);

        return new Vector3(focus.x, focus.y, transform.position.z);
    }

    // Keeps the viewport inside [min, max]; when the level is smaller than the
    // view on that axis the level is centred instead.
    private static float ClampInside(float value, float min, float max, float halfExtent)
    {
        float low = min + halfExtent;
        float high = max - halfExtent;
        return low > high ? (min + max) * 0.5f : Mathf.Clamp(value, low, high);
    }
}

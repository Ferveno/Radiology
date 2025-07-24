using UnityEngine;

public class Minimap2D : MonoBehaviour
{
    [Header("References")]
    public RectTransform minimapPoint_1;
    public RectTransform minimapPoint_2;
    public Transform worldPoint_1;
    public Transform worldPoint_2;

    [Header("Player")]
    public RectTransform playerMinimap;
    public Transform playerWorld;

    [Header("Minimap UI Bounds")]
    public RectTransform minimapContainer;

    [Header("Offset Tweak (UI units)")]
    // Adjust these in the Inspector to push the icon 
    // up/down or left/right away from walls.
    public Vector2 uiOffset = Vector2.zero;

    private float minimapRatio;

    private void Awake()
    {
        CalculateMapRatio();
    }

    private void Update()
    {
        // 1) Compute raw minimap position in UI-space
        float offsetX = playerWorld.position.x - worldPoint_1.position.x;
        float offsetY = playerWorld.position.y - worldPoint_1.position.y;
        Vector2 rawPos = minimapPoint_1.anchoredPosition
                       + new Vector2(offsetX * minimapRatio,
                                     offsetY * minimapRatio);

        // 2) Apply your custom UI offset
        rawPos += uiOffset;

        // 3) Clamp it to the minimapContainer’s rect
        Rect r = minimapContainer.rect;
        float minX = r.xMin, maxX = r.xMax;
        float minY = r.yMin, maxY = r.yMax;

        Vector2 clampedPos = new Vector2(
            Mathf.Clamp(rawPos.x, minX, maxX),
            Mathf.Clamp(rawPos.y, minY, maxY)
        );

        playerMinimap.anchoredPosition = clampedPos;
    }

    public void CalculateMapRatio()
    {
        // world‐space distance in XY (ignore Z)
        Vector3 worldDelta = worldPoint_1.position - worldPoint_2.position;
        worldDelta.z = 0f;
        float worldDistance = worldDelta.magnitude;

        // UI‐space distance
        Vector2 mapDelta = minimapPoint_1.anchoredPosition - minimapPoint_2.anchoredPosition;
        float mapDistance = mapDelta.magnitude;

        minimapRatio = mapDistance / worldDistance;
    }
}

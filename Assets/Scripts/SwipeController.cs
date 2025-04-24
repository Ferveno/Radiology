using UnityEngine;
using UnityEngine.EventSystems;

public class SwipeController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    // Minimum horizontal distance (in pixels) to consider as a valid swipe.
    public float minSwipeDistance = 50f;

    // The starting position of the drag (in screen coordinates).
    private Vector2 startTouchPosition;

    // The original anchored position of the card (so we can snap back if needed).
    private Vector2 originalAnchoredPosition;

    // Reference to the RectTransform component.
    private RectTransform rectTransform;

    // Delegate and event to broadcast swipe direction.
    public delegate void SwipeAction(bool isRightSwipe);
    public event SwipeAction OnSwipeDetected;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originalAnchoredPosition = rectTransform.anchoredPosition;
    }

    // Called when a drag begins.
    public void OnBeginDrag(PointerEventData eventData)
    {
        startTouchPosition = eventData.position;
        // Store the original anchored position in case we need to snap back.
        originalAnchoredPosition = rectTransform.anchoredPosition;
    }

    // Called while dragging.
    public void OnDrag(PointerEventData eventData)
    {
        // Calculate the horizontal delta from the start of the drag.
        float deltaX = eventData.position.x - startTouchPosition.x;

        // Update the card's anchoredPosition only on the x-axis.
        rectTransform.anchoredPosition = new Vector2(originalAnchoredPosition.x + deltaX, originalAnchoredPosition.y);
    }

    // Called when the drag ends.
    public void OnEndDrag(PointerEventData eventData)
    {
        // Determine the total horizontal swipe distance.
        float deltaX = eventData.position.x - startTouchPosition.x;

        // Check if the horizontal movement meets the minimum swipe distance.
        if (Mathf.Abs(deltaX) >= minSwipeDistance)
        {
            // Determine if the swipe is to the right (deltaX > 0) or left.
            bool isRightSwipe = deltaX > 0;

            // Optionally, animate the card off-screen here.
            // For now, we simply invoke the event.
            OnSwipeDetected?.Invoke(isRightSwipe);
        }
        else
        {
            // If the swipe wasn't far enough, snap the card back to its original position.
            rectTransform.anchoredPosition = originalAnchoredPosition;
        }
    }
}

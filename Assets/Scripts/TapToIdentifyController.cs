using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System.Collections.Generic;

public class TapToIdentifyController : MonoBehaviour, IPointerClickHandler
{
    // The target tap position (local coordinates)
    public Vector2 targetPosition;
    // Allowed tolerance (in local units)
    public float tolerance = 50f;
    // Maximum allowed tap attempts (set to 3)
    public int maxAttempts = 3;

    // Internal attempt counter
    private int attempts = 0;

    // UI element to display the current number of attempts.
    // This UI is enabled when the tap phase begins.
    public TextMeshProUGUI attemptsText;

    // Flag to control whether tap input is processed.
    public bool inputEnabled = true;

    public delegate void IdentificationResult(bool correct, int attemptsUsed);
    public event IdentificationResult OnIdentificationComplete;

    private RectTransform panelRect;

    // Store all tap positions (in local coordinates)
    public List<Vector2> tappedPositions = new List<Vector2>();

    // Store the instantiated orange boxes so they can be cleared later.
    private List<GameObject> orangeBoxes = new List<GameObject>();

    // Prefabs for the red box (latest tap marker) and orange box (final markers)
    public GameObject redBoxPrefab;
    public GameObject orangeBoxPrefab;

    private void Awake()
    {
        panelRect = GetComponent<RectTransform>();
        attempts = 0;
        if (attemptsText != null)
        {
            attemptsText.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Resets the tap controller for a new image.
    /// Enables the attempt counter UI, resets its value, clears previous tap records,
    /// and destroys any previously instantiated orange boxes.
    /// </summary>
    public void ResetController()
    {
        // Clear previously created orange boxes.
        foreach (GameObject box in orangeBoxes)
        {
            if (box != null)
                Destroy(box);
        }
        orangeBoxes.Clear();

        tappedPositions.Clear();
        attempts = 0;
        inputEnabled = true;
        if (attemptsText != null)
        {
            attemptsText.text = "Attempts: 0/" + maxAttempts;
            attemptsText.gameObject.SetActive(true);
        }
        this.enabled = true; // Ensure component is enabled.
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!inputEnabled)
            return; // Do not process taps if input is disabled.

        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(panelRect, eventData.position, eventData.pressEventCamera, out localPoint);

        attempts++;
        if (attemptsText != null)
        {
            attemptsText.text = "Attempts: " + attempts + "/" + maxAttempts;
        }

        // Record the tap position.
        tappedPositions.Add(localPoint);

        // Instantiate a red box at the tapped position.
        if (redBoxPrefab != null)
        {
            GameObject redBox = Instantiate(redBoxPrefab, panelRect);
            RectTransform rbRect = redBox.GetComponent<RectTransform>();
            if (rbRect != null)
            {
                rbRect.anchoredPosition = localPoint;
            }
            // Destroy the red box after 2 seconds.
            Destroy(redBox, 2f);
        }

        bool correct = Vector2.Distance(localPoint, targetPosition) <= tolerance;
        OnIdentificationComplete?.Invoke(correct, attempts);

        // Disable further tap input if the tap is correct or maximum attempts are reached.
        if (correct || attempts >= maxAttempts)
        {
            inputEnabled = false;
            Debug.Log("Tap input disabled after " + attempts + " attempt(s).");
        }
    }

    /// <summary>
    /// Instantiates orange boxes for all recorded tap positions.
    /// These boxes remain visible for the player to compare their taps to the correct location.
    /// </summary>
    public void ShowAllTapBoxes()
    {
        if (orangeBoxPrefab != null)
        {
            foreach (Vector2 pos in tappedPositions)
            {
                GameObject orangeBox = Instantiate(orangeBoxPrefab, panelRect);
                Debug.Log("Orange Box created");
                RectTransform obRect = orangeBox.GetComponent<RectTransform>();
                if (obRect != null)
                {
                    obRect.anchoredPosition = pos;
                }
                orangeBoxes.Add(orangeBox);
                // Optionally, you can destroy them after a set time or leave them active until the next Reset.
            }
        }
    }

    /// <summary>
    /// When the tap controller is disabled, hide the attempt counter UI.
    /// This ensures the attemptsText is only visible during the tap phase.
    /// </summary>
    private void OnDisable()
    {
        if (attemptsText != null)
        {
            attemptsText.gameObject.SetActive(false);
        }
    }
}

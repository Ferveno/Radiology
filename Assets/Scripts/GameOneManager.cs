using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameOneManager : MonoBehaviour
{
    public static GameOneManager instance;

    // --- Draggable & Case Management ---
    public int CorrectCount = 0;
    public int maxDraggables = 5; // Number of correct placements needed per case

    public GameObject Case1;
    public GameObject Case2;
    public GameObject Case3;

    public TextMeshProUGUI CaseCompleteText;
    public TextMeshProUGUI TimerText; // UI text for countdown

    public float caseDuration = 10f; // Initial countdown time for each case
    private float timeRemaining;
    private Coroutine timerCoroutine;

    // --- Clock Powerup Variables ---
    public Button clockPowerupButton;   // Button to activate Clock powerup
    public int clockPowerupCost = 50;     // Score cost for using the Clock powerup
    public float extraTime = 5f;          // Extra time (in seconds) granted by the Clock powerup
    private bool clockPowerupUsed = false; // Allow only one use per case

    // --- Glasses Powerup Variables ---
    public Button glassesPowerupButton;   // Button to activate Glasses powerup
    public int glassesPowerupCost = 30;     // Score cost for using the Glasses powerup
    public float glassesHelpTextDuration = 3f; // Duration to display help texts (in seconds)
    private bool glassesPowerupUsed = false;   // Allow only one use per case

    // --- Eraser Powerup Variables ---
    public Button eraserPowerupButton;   // Button to activate Eraser powerup
    public int eraserPowerupCost = 100;     // Score cost for using the Eraser powerup
    private bool eraserPowerupUsed = false;   // Can be used only once per game session

    // --- NEW: Assist Powerup Variables ---
    public Button assistPowerupButton;    // Button to activate Assist powerup
    public int assistPowerupCost = 40;      // Score cost for using the Assist powerup
    private bool assistPowerupUsed = false; // Allow only one use per case

    public float assistAnimationDuration = 1f; // Duration in seconds for the assist animation


    public GameObject GameMain1;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void OnEnable()
    {
        CaseCompleteText.gameObject.SetActive(false);
        StartCase1();
    }

    private void Update()
    {
        // Update Clock powerup button interactivity.
        if (clockPowerupButton != null)
        {
            clockPowerupButton.interactable = (!clockPowerupUsed && GameManager.instance.Score >= clockPowerupCost);
        }

        // Update Glasses powerup button interactivity.
        if (glassesPowerupButton != null)
        {
            glassesPowerupButton.interactable = (!glassesPowerupUsed && GameManager.instance.Score >= glassesPowerupCost);
        }

        // Update Eraser powerup button interactivity.
        if (eraserPowerupButton != null)
        {
            eraserPowerupButton.interactable = (!eraserPowerupUsed && GameManager.instance.Score >= eraserPowerupCost);
        }

        // --- NEW: Update Assist powerup button interactivity.
        if (assistPowerupButton != null)
        {
            assistPowerupButton.interactable = (!assistPowerupUsed && GameManager.instance.Score >= assistPowerupCost);
        }
    }

    // --- Case & Timer Management ---
    private void StartCase1()
    {
        SetActiveCase(Case1);
        StartTimer();
    }

    private void StartTimer()
    {
        if (timerCoroutine != null)
            StopCoroutine(timerCoroutine);

        timeRemaining = caseDuration;
        timerCoroutine = StartCoroutine(TimerCountdown());
    }

    private IEnumerator TimerCountdown()
    {
        while (timeRemaining > 0)
        {
            TimerText.text = $"Time: {timeRemaining}";
            yield return new WaitForSeconds(1f);
            timeRemaining--;
        }

        TimerText.text = "Time: 0";
        CheckIfComplete(); // Auto-progress when time runs out
    }

    // Call this method when a draggable is placed correctly.
    public void DraggablePlacedCorrectly()
    {
        CorrectCount++;
        if (CorrectCount >= maxDraggables)
        {
            CheckIfComplete();
        }
    }

    // Check for case completion and proceed accordingly.
    public void CheckIfComplete()
    {
        if (Case1.activeInHierarchy)
        {
            CaseCompleteText.gameObject.SetActive(true);
            CaseCompleteText.text = "Case 1 completed!";
            Invoke(nameof(ProceedToCase2), 0.5f);
        }
        else if (Case2.activeInHierarchy)
        {
            CaseCompleteText.gameObject.SetActive(true);
            CaseCompleteText.text = "Case 2 completed!";
            Invoke(nameof(ProceedToCase3), 0.5f);
        }
        else if (Case3.activeInHierarchy)
        {
            CaseCompleteText.gameObject.SetActive(true);
            CaseCompleteText.text = "All cases completed!";
            Invoke(nameof(AllCasesCompleted), 0.5f);
        }
    }

    private void ProceedToCase2()
    {
        CorrectCount = 0;
        SetActiveCase(Case2);
        StartTimer();
    }

    private void ProceedToCase3()
    {
        CorrectCount = 0;
        SetActiveCase(Case3);
        StartTimer();
    }

    private void AllCasesCompleted()
    {
        Case1.SetActive(false);
        Case2.SetActive(false);
        Case3.SetActive(false);
        CaseCompleteText.gameObject.SetActive(false);
        TimerText.text = "All cases completed!";

        GameMain1.SetActive(false);
        GameManager.instance.OnGame1Complete();
    }

    // Sets the active case and resets per-case variables.
    // Note: We reset clock, glasses, and assist powerup flags here, but do NOT reset eraserPowerupUsed.
    private void SetActiveCase(GameObject activeCase)
    {
        Case1.SetActive(false);
        Case2.SetActive(false);
        Case3.SetActive(false);
        activeCase.SetActive(true);
        CaseCompleteText.gameObject.SetActive(false);

        CorrectCount = 0;
        clockPowerupUsed = false;
        glassesPowerupUsed = false;
        assistPowerupUsed = false;  // --- NEW: Reset Assist powerup flag.
        // eraserPowerupUsed is not reset, so Eraser can only be used once per session.
    }

    // --- Clock Powerup Activation ---
    public void ActivateClockPowerup()
    {
        if (!clockPowerupUsed && GameManager.instance.Score >= clockPowerupCost)
        {
            GameManager.instance.Score -= clockPowerupCost;
            GameManager.instance.ScoreUpdater();

            timeRemaining += extraTime;
            clockPowerupUsed = true;
        }
    }

    // --- Glasses Powerup Activation ---
    public void ActivateGlassesPowerup()
    {
        if (!glassesPowerupUsed && GameManager.instance.Score >= glassesPowerupCost)
        {
            GameManager.instance.Score -= glassesPowerupCost;
            GameManager.instance.ScoreUpdater();

            glassesPowerupUsed = true;

            if (Case1.activeInHierarchy && Case1HelpTexts != null)
            {
                ShowHelpTexts(Case1HelpTexts);
            }
            else if (Case2.activeInHierarchy && Case2HelpTexts != null)
            {
                ShowHelpTexts(Case2HelpTexts);
            }
            else if (Case3.activeInHierarchy && Case3HelpTexts != null)
            {
                ShowHelpTexts(Case3HelpTexts);
            }
        }
    }

    private void ShowHelpTexts(GameObject[] helpTexts)
    {
        foreach (GameObject helpText in helpTexts)
        {
            if (helpText != null)
                helpText.SetActive(true);
        }
        StartCoroutine(DisableHelpTextsAfterDelay(helpTexts, glassesHelpTextDuration));
    }

    private IEnumerator DisableHelpTextsAfterDelay(GameObject[] helpTexts, float delay)
    {
        yield return new WaitForSeconds(delay);
        foreach (GameObject helpText in helpTexts)
        {
            if (helpText != null)
                helpText.SetActive(false);
        }
    }

    // --- Eraser Powerup Activation ---
    public void ActivateEraserPowerup()
    {
        if (!eraserPowerupUsed && GameManager.instance.Score >= eraserPowerupCost)
        {
            GameManager.instance.Score -= eraserPowerupCost;
            GameManager.instance.ScoreUpdater();

            eraserPowerupUsed = true;
            ResetGameState();
        }
    }

    private void ResetGameState()
    {
        if (timerCoroutine != null)
            StopCoroutine(timerCoroutine);

        ResetAllDraggables();
        SetActiveCase(Case1);
        StartTimer();
    }

    private void ResetAllDraggables()
    {
        ResetDraggablesInCase(Case1);
        ResetDraggablesInCase(Case2);
        ResetDraggablesInCase(Case3);
    }

    private void ResetDraggablesInCase(GameObject caseObj)
    {
        DragDrop[] draggables = caseObj.GetComponentsInChildren<DragDrop>();
        foreach (DragDrop draggable in draggables)
        {
            draggable.ResetPosition();
        }
    }

    //// --- NEW: Assist Powerup Activation ---
    //// This powerup automatically places one unplaced draggable in the correct position.
    //public void ActivateAssistPowerup()
    //{
    //    if (!assistPowerupUsed && GameManager.instance.Score >= assistPowerupCost)
    //    {
    //        GameManager.instance.Score -= assistPowerupCost;
    //        assistPowerupUsed = true;

    //        // Determine which case is active.
    //        GameObject activeCase = null;
    //        if (Case1.activeInHierarchy)
    //            activeCase = Case1;
    //        else if (Case2.activeInHierarchy)
    //            activeCase = Case2;
    //        else if (Case3.activeInHierarchy)
    //            activeCase = Case3;

    //        if (activeCase != null)
    //        {
    //            // Find an unplaced draggable in the active case.
    //            DragDrop[] draggables = activeCase.GetComponentsInChildren<DragDrop>();
    //            foreach (DragDrop draggable in draggables)
    //            {
    //                if (!draggable.isLocked && draggable.ObjectDragToPos != null)
    //                {
    //                    // Automatically place this draggable correctly.
    //                    draggable.ObjectToDrag.transform.position = draggable.ObjectDragToPos.transform.position;
    //                    draggable.isLocked = true;
    //                    GameOneManager.instance.DraggablePlacedCorrectly();
    //                    GameManager.instance.Score++; // Simulate score increment as in a correct drop.
    //                    break; // Only assist with one draggable.
    //                }
    //            }
    //        }
    //    }
    //}

    // --- NEW: Assist Powerup Activation ---
    // This powerup automatically moves one unplaced draggable (with a valid target) smoothly to its correct position.
    public void ActivateAssistPowerup()
    {
        if (!assistPowerupUsed && GameManager.instance.Score >= assistPowerupCost)
        {
            GameManager.instance.Score -= assistPowerupCost;
            GameManager.instance.ScoreUpdater();
            assistPowerupUsed = true;

            // Determine which case is active.
            GameObject activeCase = null;
            if (Case1.activeInHierarchy)
                activeCase = Case1;
            else if (Case2.activeInHierarchy)
                activeCase = Case2;
            else if (Case3.activeInHierarchy)
                activeCase = Case3;

            if (activeCase != null)
            {
                // Find an unplaced draggable in the active case with a valid target.
                DragDrop[] draggables = activeCase.GetComponentsInChildren<DragDrop>();
                foreach (DragDrop draggable in draggables)
                {
                    if (!draggable.isLocked && draggable.ObjectDragToPos != null)
                    {
                        // Start smooth animation to move the draggable.
                        StartCoroutine(AnimateAssist(draggable, assistAnimationDuration));
                        break; // Only assist with one draggable.
                    }
                }
            }
        }
    }

    // Coroutine to smoothly animate a draggable to its target position.
    private IEnumerator AnimateAssist(DragDrop draggable, float duration)
    {
        Vector3 startPos = draggable.ObjectToDrag.transform.position;
        Vector3 endPos = draggable.ObjectDragToPos.transform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            draggable.ObjectToDrag.transform.position = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }

        // Ensure final position is set.
        draggable.ObjectToDrag.transform.position = endPos;
        draggable.isLocked = true;
        GameOneManager.instance.DraggablePlacedCorrectly();
        GameManager.instance.Score++; // Simulate score increment as in a correct drop.
        GameManager.instance.ScoreUpdater();

    }

    // --- Help Text Arrays for Each Case (assumed to be declared as before) ---
    public GameObject[] Case1HelpTexts;
    public GameObject[] Case2HelpTexts;
    public GameObject[] Case3HelpTexts;
}

using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameThreeGamePlay_PanelUI : MonoBehaviour
{
    public static GameThreeGamePlay_PanelUI instance;

    public Image questionImage;
    public TextMeshProUGUI option_1;
    public TextMeshProUGUI option_2;
    public TextMeshProUGUI option_3;
    public TextMeshProUGUI option_4;
    public TextMeshProUGUI question_Text;
    public TextMeshProUGUI correctAnswerDescription_Text;
    public Image correctAnswerDescription_Image;
    public Button[] optionButtons_Ref;
    public GameObject optionButtons;
    public GameObject correctAnswerPanel;

    // Timer fields
    public TextMeshProUGUI timerText;
    public float timePerQuestion = 10f;
    private float currentRemainingTime;
    private Coroutine timerCoroutine;

    // Clock powerup fields
    public Button clockButton;
    public int clockCost = 10;          // Score cost to use the Clock powerup
    public int clockExtraSeconds = 5;   // Seconds added when powerup is used

    // Glasses powerup fields
    public Button glassesButton;
    public int glassesCost = 5;         // Score cost to use the Glasses powerup

    // Eraser powerup fields
    public Button eraserButton;
    public TextMeshProUGUI eraserCountdownText; // Dedicated UI element for the grace period countdown
    public int eraserCost = 5;         // Score cost to use the Eraser
    public float eraserGracePeriod = 3f; // Duration (in seconds) of the grace period
    private bool isAwaitingEraser = false;
    private Coroutine eraserCountdownCoroutine = null;

    // AI Assist powerup fields
    public Button aiAssistButton;
    public int aiAssistCost = 15;       // Score cost to use AI Assist

    [SerializeField] public int playerLevel = 1;

    // Colors for enabled/disabled states
    private Color enabledColor = Color.white;
    private Color disabledColor = Color.gray;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        NextLevel();
    }

    public void UpdatePlayerLevel()
    {
        playerLevel++;
        PlayerPrefs.SetInt("PlayerLevel", playerLevel);
    }

    public void NextLevel()
    {
        // Stop any existing timer and any active Eraser countdown
        if (timerCoroutine != null)
            StopCoroutine(timerCoroutine);
        if (eraserCountdownCoroutine != null)
        {
            StopCoroutine(eraserCountdownCoroutine);
            eraserCountdownCoroutine = null;
        }
        isAwaitingEraser = false;
        eraserCountdownText.gameObject.SetActive(false);
        eraserButton.interactable = false;
        eraserButton.image.color = disabledColor;

        // Reset timer values
        currentRemainingTime = timePerQuestion;
        timerText.text = Mathf.CeilToInt(currentRemainingTime).ToString();

        // Reset answer buttons to default state
        ResetAnswerButtons();

        // Load the next question
        GameThreeManager.instance.FindNextLevel();

        // Start the timer for the new question
        if (this.gameObject.activeInHierarchy)
        {
            timerCoroutine = StartCoroutine(QuestionTimer());
        }
        // Update all powerup button states initially
        UpdatePowerupButtonStates();
    }

    private IEnumerator QuestionTimer()
    {
        while (currentRemainingTime > 0)
        {
            timerText.text = Mathf.CeilToInt(currentRemainingTime).ToString();
            currentRemainingTime -= Time.deltaTime;
            yield return null;
        }
        timerText.text = "0";
        Debug.Log("Time expired! Skipping question.");
        yield return new WaitForSeconds(1f);
        NextLevel();
    }

    // Called when an answer is selected to cancel the timer.
    public void CancelTimer()
    {
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
            timerCoroutine = null;
        }
    }

    // CLOCK powerup: Adds extra seconds to the current timer.
    public void UseClockPowerup()
    {
        if (correctAnswerPanel != null && correctAnswerPanel.activeInHierarchy)
        {
            Debug.Log("Cannot use powerup when the answer panel is active.");
            return;
        }
        if (GameManager.instance.Score >= clockCost)
        {
            GameManager.instance.Score -= clockCost;
            currentRemainingTime += clockExtraSeconds;
            timerText.text = Mathf.CeilToInt(currentRemainingTime).ToString();
            UpdatePowerupButtonStates();
        }
        else
        {
            Debug.Log("Not enough score to use the Clock powerup!");
        }
    }

    // GLASSES powerup: Provides a hint by highlighting the button with the correct answer.
    public void UseGlassesPowerup()
    {
        if (correctAnswerPanel != null && correctAnswerPanel.activeInHierarchy)
        {
            Debug.Log("Cannot use powerup when the answer panel is active.");
            return;
        }
        if (GameManager.instance.Score >= glassesCost)
        {
            GameManager.instance.Score -= glassesCost;
            foreach (Button btn in optionButtons_Ref)
            {
                TextMeshProUGUI txt = btn.GetComponentInChildren<TextMeshProUGUI>();
                if (txt != null && txt.text == GameThreeManager.instance.currentLevel.correctAnswer)
                {
                    btn.image.color = Color.yellow; // Visual hint
                    break;
                }
            }
            UpdatePowerupButtonStates();
        }
        else
        {
            Debug.Log("Not enough score to use the Glasses powerup!");
        }
    }

    // ERASER powerup handling: Called when a wrong answer is selected.
    // This stops the timer, highlights the correct answer, and if the player has enough score,
    // starts a grace period countdown during which the player can undo the mistake.
    public void HandleWrongAnswer()
    {
        // Cancel the timer
        CancelTimer();
        // Highlight the correct answer
        HighlightCorrectAnswer();

        // Check if the player has enough score for the Eraser powerup.
        if (GameManager.instance.Score >= eraserCost)
        {
            isAwaitingEraser = true;
            eraserButton.interactable = true;
            eraserButton.image.color = enabledColor;
            eraserCountdownText.gameObject.SetActive(true);
            eraserCountdownCoroutine = StartCoroutine(DoEraserCountdown());
        }
        else
        {
            // If not enough score, wait a moment then proceed as normal.
            StartCoroutine(WaitForWrongAnswerAndAdvance());
        }
    }

    // Coroutine for the Eraser grace period countdown.
    private IEnumerator DoEraserCountdown()
    {
        float countdown = eraserGracePeriod;
        while (countdown > 0 && isAwaitingEraser)
        {
            eraserCountdownText.text = countdown.ToString("F0");
            countdown -= Time.deltaTime;
            yield return null;
        }
        eraserCountdownText.text = "0";
        // If the countdown expires without using the Eraser, disable and proceed.
        isAwaitingEraser = false;
        eraserCountdownText.gameObject.SetActive(false);
        eraserButton.interactable = false;
        StartCoroutine(WaitForWrongAnswerAndAdvance());
    }

    // Wait a short period then move to the next question.
    private IEnumerator WaitForWrongAnswerAndAdvance()
    {
        yield return new WaitForSeconds(0.5f);
        NextLevel();
    }

    // ERASER powerup: Called when the player taps the Eraser button during the grace period.
    public void UseEraserPowerup()
    {
        if (!isAwaitingEraser)
        {
            Debug.Log("No mistake to undo.");
            return;
        }
        if (GameManager.instance.Score < eraserCost)
        {
            Debug.Log("Not enough score to use the Eraser!");
            return;
        }

        // Deduct the Eraser cost and cancel the grace period.
        GameManager.instance.Score -= eraserCost;
        //UseClockPowerup();
        currentRemainingTime += clockExtraSeconds;
        timerText.text = Mathf.CeilToInt(currentRemainingTime).ToString();

        if (eraserCountdownCoroutine != null)
        {
            StopCoroutine(eraserCountdownCoroutine);
            eraserCountdownCoroutine = null;
        }
        isAwaitingEraser = false;
        eraserCountdownText.gameObject.SetActive(false);
        eraserButton.interactable = false;
        eraserButton.image.color = disabledColor;

        // Reset the answer buttons so the player can try again.
        ResetAnswerButtons();
        // Resume the timer for the current question.
        ResumeTimer();
    }

    // AI ASSIST powerup: Automatically answers the current question for the player.
    public void UseAIAssistPowerup()
    {
        if (correctAnswerPanel != null && correctAnswerPanel.activeInHierarchy)
        {
            Debug.Log("Cannot use powerup when the answer panel is active.");
            return;
        }
        if (GameManager.instance.Score < aiAssistCost)
        {
            Debug.Log("Not enough score to use AI Assist!");
            return;
        }

        // Deduct the cost.
        GameManager.instance.Score -= aiAssistCost;

        // Mark the current level as answered.
        if (GameThreeManager.instance.currentLevel != null)
        {
            GameThreeManager.instance.currentLevel.isAnswered = true;
            // Optionally persist this status.
        }

        CancelTimer();
        // Optionally increase the player's score as if they answered correctly.
        GameManager.instance.Score++;

        // Simulate the correct answer button feedback.
        foreach (Button btn in optionButtons_Ref)
        {
            TextMeshProUGUI txt = btn.GetComponentInChildren<TextMeshProUGUI>();
            if (txt != null && txt.text == GameThreeManager.instance.currentLevel.correctAnswer)
            {
                btn.image.color = Color.green;
                txt.color = Color.white;
                break;
            }
        }

        // Start the coroutine that waits briefly (for feedback) and then shows the correctAnswerPanel.
        StartCoroutine(ApplyAIAssistAnswer());
    }

    // Coroutine to simulate feedback before showing the correctAnswerPanel.
    private IEnumerator ApplyAIAssistAnswer()
    {
        // Wait for a short delay to mimic the feedback animation.
        yield return new WaitForSeconds(0.5f);

        // Now show the correct answer feedback.
        questionImage.sprite = GameThreeManager.instance.currentLevel.correctAnswerSprite;
        correctAnswerPanel.SetActive(true);
        yield return new WaitForSeconds(2f);
        // Keep the panel visible (if desired) then hide and proceed.
        yield return new WaitForSeconds(7f);
        correctAnswerPanel.SetActive(false);

        // Proceed to the next question.
        NextLevel();
    }


    // Helper: Reset answer buttons to their default appearance.
    private void ResetAnswerButtons()
    {
        foreach (Button button in optionButtons_Ref)
        {
            button.interactable = true;
            button.image.color = new Color32(68, 114, 196, 255);
            button.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
        }
    }

    // Helper: Resume the question timer if it's not already running.
    private void ResumeTimer()
    {
        if (timerCoroutine == null)
        {
            timerCoroutine = StartCoroutine(QuestionTimer());
        }
    }

    // Consolidated method to update all powerup buttons based on score and current UI state.
    public void UpdatePowerupButtonStates()
    {
        bool disableDueToPanel = (correctAnswerPanel != null && correctAnswerPanel.activeInHierarchy);

        // Update Clock button.
        if (disableDueToPanel)
        {
            clockButton.interactable = false;
            clockButton.image.color = disabledColor;
        }
        else
        {
            if (GameManager.instance.Score >= clockCost)
            {
                clockButton.interactable = true;
                clockButton.image.color = enabledColor;
            }
            else
            {
                clockButton.interactable = false;
                clockButton.image.color = disabledColor;
            }
        }

        // Update Glasses button.
        if (disableDueToPanel)
        {
            glassesButton.interactable = false;
            glassesButton.image.color = disabledColor;
        }
        else
        {
            if (GameManager.instance.Score >= glassesCost)
            {
                glassesButton.interactable = true;
                glassesButton.image.color = enabledColor;
            }
            else
            {
                glassesButton.interactable = false;
                glassesButton.image.color = disabledColor;
            }
        }

        // Update Eraser button.
        if (!isAwaitingEraser || GameManager.instance.Score < eraserCost)
        {
            eraserButton.interactable = false;
            eraserButton.image.color = disabledColor;
            eraserCountdownText.gameObject.SetActive(false);
        }
        else
        {
            eraserButton.interactable = true;
            eraserButton.image.color = enabledColor;
        }

        // Update AI Assist button.
        if (disableDueToPanel)
        {
            aiAssistButton.interactable = false;
            aiAssistButton.image.color = disabledColor;
        }
        else
        {
            if (GameManager.instance.Score >= aiAssistCost)
            {
                aiAssistButton.interactable = true;
                aiAssistButton.image.color = enabledColor;
            }
            else
            {
                aiAssistButton.interactable = false;
                aiAssistButton.image.color = disabledColor;
            }
        }
    }

    private void Update()
    {
        // Update powerup states every frame.
        UpdatePowerupButtonStates();
    }

    // Dummy method to highlight the correct answer.
    // Replace this with your actual highlighting logic.
    private void HighlightCorrectAnswer()
    {
        foreach (Button btn in optionButtons_Ref)
        {
            TextMeshProUGUI txt = btn.GetComponentInChildren<TextMeshProUGUI>();
            if (txt != null && txt.text == GameThreeManager.instance.currentLevel.correctAnswer)
            {
                btn.image.color = Color.green;
                break;
            }
        }
    }
}

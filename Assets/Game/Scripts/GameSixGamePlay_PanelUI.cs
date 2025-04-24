using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameSixGamePlay_PanelUI : MonoBehaviour
{
    public static GameSixGamePlay_PanelUI instance;

    public Image questionImage;
    public TextMeshProUGUI option_1, option_2, option_3, option_4, question_Text;
    public Button[] optionButtons_Ref;
    public GameObject optionButtons;

    [SerializeField] public int playerLevel = 1;

    // Timer Variables
    public TextMeshProUGUI timerText;
    private float timeRemaining = 20f;
    private Coroutine timerCoroutine;

    // Glasses powerup state and coroutine
    public bool glassesActive = false;
    private Coroutine glassesCoroutine;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        NextLevel();
    }

    public void NextLevel()
    {
        if (playerLevel == GameSixManager.instance.allLevels.Count - 1)
        {
            // Optionally show a completion panel here.
        }

        ResetButtonStates();

        // Remove any active glasses highlight when loading a new question.
        DeactivateGlasses();

        GameSixManager.instance.FindNextLevel();
        StartTimer(); // Start the timer when a new question appears
    }

    void ResetButtonStates()
    {
        foreach (Button button in optionButtons_Ref)
        {
            button.interactable = true;
            button.image.color = new Color32(68, 114, 196, 255); // Default color
            button.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
        }
    }

    // Timer Logic
    private void StartTimer()
    {
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
        }
        timeRemaining = 20f;
        timerCoroutine = StartCoroutine(TimerCountdown());
    }

    public void StopTimer()
    {
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
            timerCoroutine = null;
        }
    }

    private IEnumerator TimerCountdown()
    {
        while (timeRemaining > 0)
        {
            timerText.text = "Time: " + Mathf.Ceil(timeRemaining) + "s";
            yield return null;  // update every frame
            timeRemaining -= Time.deltaTime;
        }
        timerText.text = "Time: 0s";
        SkipQuestion(); // Automatically move to the next question when time runs out
    }

    void SkipQuestion()
    {
        Debug.Log("Time Up! Skipping to next question.");
        StopTimer();
        StartCoroutine(WaitForNextQuestion());
    }

    public IEnumerator WaitForNextQuestion()
    {
        yield return new WaitForSeconds(1f);  // Reduced delay for a snappier transition
        GameSixManager.instance.currentLevel = null;
        NextLevel();
    }

    // ----- Glasses Powerup Methods -----

    /// <summary>
    /// Activates the glasses powerup that highlights the correct answer in orange.
    /// </summary>
    public void ActivateGlasses()
    {
        // Cancel any existing glasses routine.
        if (glassesCoroutine != null)
        {
            StopCoroutine(glassesCoroutine);
        }
        glassesActive = true;
        HighlightCorrectAnswer(Color.Lerp(Color.red, Color.yellow, 0.5f)); // An orange-like color

        // Deactivate after 3 seconds.
        glassesCoroutine = StartCoroutine(DeactivateGlassesAfterDelay(3f));
    }

    /// <summary>
    /// Highlights the correct answer button with the specified color.
    /// </summary>
    /// <param name="highlightColor">The color to highlight.</param>
    private void HighlightCorrectAnswer(Color highlightColor)
    {
        if (GameSixManager.instance.currentLevel == null)
            return;

        foreach (Button btn in optionButtons_Ref)
        {
            TextMeshProUGUI btnText = btn.GetComponentInChildren<TextMeshProUGUI>();
            if (btnText != null && btnText.text == GameSixManager.instance.currentLevel.correctAnswer)
            {
                btn.image.color = highlightColor;
            }
        }
    }

    /// <summary>
    /// Waits for a delay and then deactivates the glasses highlight.
    /// </summary>
    private IEnumerator DeactivateGlassesAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        DeactivateGlasses();
    }

    /// <summary>
    /// Deactivates the glasses powerup, removing the highlight.
    /// </summary>
    public void DeactivateGlasses()
    {
        if (!glassesActive)
            return;

        glassesActive = false;
        if (glassesCoroutine != null)
        {
            StopCoroutine(glassesCoroutine);
            glassesCoroutine = null;
        }

        // Revert the correct answer button back to its default color.
        foreach (Button btn in optionButtons_Ref)
        {
            TextMeshProUGUI btnText = btn.GetComponentInChildren<TextMeshProUGUI>();
            if (btnText != null && GameSixManager.instance.currentLevel != null &&
                btnText.text == GameSixManager.instance.currentLevel.correctAnswer)
            {
                btn.image.color = new Color32(68, 114, 196, 255);
            }
        }
    }
}

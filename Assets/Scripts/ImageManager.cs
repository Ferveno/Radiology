using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class ImageManager : MonoBehaviour
{
    [Header("Zone References")]
    public RectTransform leftZone;
    public RectTransform rightZone;

    [Header("Score UI")]
    //public Text scoreText;

    [Header("Message Box UI - Swipe Phase")]
    public GameObject messageBoxPanel;       // For swipe feedback
    public TextMeshProUGUI messageText;        // Swipe feedback text
    public Image messageBoxImage;              // Swipe feedback background

    [Header("Message Box UI - Tap Phase")]
    public GameObject tapMessageBoxPanel;      // For tap feedback
    public TextMeshProUGUI tapMessageText;       // Tap feedback text
    public Image tapMessageBoxImage;           // Tap feedback background

    [Header("Tap Phase UI")]
    public GameObject tapPanel;                // Panel that shows the zoomed view for tapping
    public TapToIdentifyController tapController; // Tap controller script (attached to the tapPanel)
    public Image tapZoomedImage;               // An Image inside tapPanel to show the zoomed image
    public Image tapStar;                      // A star marker to denote the correct tip (disabled by default)
    public GameObject AbnormalityDescriptionPanel;      // For Abnormality Description
    public TextMeshProUGUI AbnormalityDescription;        // Abnormality Description Text

    [Header("Tap Instruction")]
    public TextMeshProUGUI tapInstructionText;

    [Header("Image Cards (Stacked in Scene)")]
    public List<GameObject> imageCards;        // Pre-placed image GameObjects (each must have a SwipeController and ImageData)

    [Header("End-of-Game UI")]
    public GameObject gameCompletePanel; // The panel to show when the game is complete.
    public GameObject mainCanvas;        // The main canvas containing your game UI.

    [Header("Timer Settings")]
    public float timePerImage = 20f; // Seconds allowed per image.
    public TextMeshProUGUI timerText; // UI element to display the countdown.

    [Header("Clock Powerup")]
    public Button clockPowerupButton;         // Button for the Clock powerup.
    public int clockPowerupCost = 10;           // Score cost for using the Clock powerup.
    public int clockPowerupTimeIncrease = 5;    // Additional seconds to add to the timer.

    [Header("Glasses Powerup")]
    public Button glassesPowerupButton;         // Button for the Glasses powerup.
    public int glassesPowerupCost = 15;           // Score cost for using the Glasses powerup.
    public TextMeshProUGUI glassesHintText;       // Optional UI element to display a hint in swipe phase.
    private bool glassesUsed = false;             // Ensures Glasses is only used once per image.

    [Header("Eraser Powerup")]
    public Button eraserPowerupButton;          // Button for the Eraser powerup.
    public int eraserPowerupCost = 20;            // Score cost for using the Eraser powerup.
    private bool eraserUsed = false;              // Tracks if Eraser has been used on this image.

    [Header("AI Assist Powerup")]
    public Button aiAssistPowerupButton;        // Button for the AI Assist powerup.
    public int aiAssistPowerupCost = 25;          // Score cost for using the AI Assist powerup.

    [Header("Swipe Arrows")]
    public GameObject swipeArrowLeft;
    public GameObject swipeArrowRight;

    // Variables to track if a tap-phase mistake occurred (for Eraser availability).
    private bool mistakeOccurred = false;
    private enum MistakePhase { None, Swipe, Tap }
    private MistakePhase mistakePhase = MistakePhase.None;

    // Internal tracking
    private int currentImageIndex = 0;

    // Timer tracking variables.
    private float timeRemaining;
    private bool timerRunning = false;

    // References to the current image and its components
    private GameObject currentImage;
    private SwipeController currentSwipeController;
    private ImageData currentCardData;

    // Record whether the swipe was correct (only valid when a swipe has occurred)
    private bool swipeCorrect = false;

    // Indicates whether the tap phase is complete (i.e. no further tapping is expected)
    private bool tapPhaseComplete = false;

    private bool isAIAssistActive = false;

    private bool isLoadingNextImage = false;

    private void Start()
    {
        // Hide UI panels at start.
        messageBoxPanel.SetActive(false);
        tapMessageBoxPanel.SetActive(false);
        tapPanel.SetActive(false);
        if (tapStar != null)
            tapStar.gameObject.SetActive(false);
        if (glassesHintText != null)
            glassesHintText.gameObject.SetActive(false);

        // Initialize powerup usage flags.
        glassesUsed = false;
        eraserUsed = false;
        mistakeOccurred = false;
        mistakePhase = MistakePhase.None;

        // Activate the first image (if any) and subscribe to its swipe events.
        if (imageCards != null && imageCards.Count > 0)
        {
            for (int i = 0; i < imageCards.Count; i++)
            {
                imageCards[i].SetActive(i == currentImageIndex);
            }
            SubscribeToCurrentImage();
            // Initialize timer for the first image.
            timeRemaining = timePerImage;
            timerRunning = true;
        }
        else
        {
            Debug.LogError("No image cards assigned to the ImageManager!");
        }
    }

    private void Update()
    {
        // Timer update logic.
        if (timerRunning)
        {
            timeRemaining -= Time.deltaTime;
            if (timerText != null)
                timerText.text = "Time: " + Mathf.CeilToInt(timeRemaining);
            if (timeRemaining <= 0)
            {
                timerRunning = false;
                HandleTimeOut();
            }
        }

        // Clock powerup button enabled if enough score.
        if (clockPowerupButton != null && GameManager.instance != null)
        {
            clockPowerupButton.interactable = (GameManager.instance.Score >= clockPowerupCost);
        }

        // Glasses powerup button enabled if not already used and enough score.
        if (glassesPowerupButton != null && GameManager.instance != null)
        {
            glassesPowerupButton.interactable = (!glassesUsed && GameManager.instance.Score >= glassesPowerupCost);
        }

        // Eraser powerup button is enabled only if a tap-phase mistake occurred, Eraser hasn’t been used,
        // and if the player has enough score.
        if (eraserPowerupButton != null && GameManager.instance != null)
        {
            eraserPowerupButton.interactable = (mistakeOccurred && !eraserUsed && GameManager.instance.Score >= eraserPowerupCost);
        }

        // AI Assist is available if the player has enough score.
        if (aiAssistPowerupButton != null && GameManager.instance != null)
        {
            aiAssistPowerupButton.interactable = (GameManager.instance.Score >= aiAssistPowerupCost);
        }
    }

    /// <summary>
    /// Subscribe to the swipe event on the current image.
    /// </summary>
    void SubscribeToCurrentImage()
    {
        currentImage = imageCards[currentImageIndex];
        currentSwipeController = currentImage.GetComponent<SwipeController>();
        currentCardData = currentImage.GetComponent<ImageData>();

        if (currentSwipeController != null)
        {
            currentSwipeController.OnSwipeDetected += HandleSwipe;
        }
        else
        {
            Debug.Log("SwipeController component missing on current image!");
        }
    }

    /// <summary>
    /// Unsubscribe from the current image’s swipe event.
    /// </summary>
    void UnsubscribeFromCurrentImage()
    {
        if (currentSwipeController != null)
        {
            currentSwipeController.OnSwipeDetected -= HandleSwipe;
        }
    }

    // --- Step 1: Swipe Phase ---
    /// <summary>
    /// Called when the player swipes the current image.
    /// Checks the swipe against the zones, updates score, and shows the swipe feedback message box.
    /// </summary>
    /// <param name="isRightSwipe">True if the player swiped right.</param>
    void HandleSwipe(bool isRightSwipe)
    {
        // Get the current image's screen position.
        RectTransform imageRect = currentImage.GetComponent<RectTransform>();
        Vector2 imageScreenPos = imageRect.position;

        // Check whether the image lands in the correct zone.
        bool inLeftZone = RectTransformUtility.RectangleContainsScreenPoint(leftZone, imageScreenPos);
        bool inRightZone = RectTransformUtility.RectangleContainsScreenPoint(rightZone, imageScreenPos);

        // For our game:
        // - Normal images should be swiped left.
        // - Abnormal images should be swiped right.
        if (isRightSwipe && currentCardData.isAbnormal && inRightZone)
        {
            swipeCorrect = true;
        }
        else if (!isRightSwipe && !currentCardData.isAbnormal && inLeftZone)
        {
            swipeCorrect = true;
        }
        else
        {
            swipeCorrect = false;
            // For swipe phase, we no longer offer Eraser—mistakes simply update the score.
        }

        // Update score based on the swipe result.
        if (swipeCorrect)
        {
            GameManager.instance.Score++;  // Increase score for a correct swipe.
            Debug.Log("Correct swipe! Score: " + GameManager.instance.Score);
        }
        else
        {
            GameManager.instance.Score--;  // Deduct score for an incorrect swipe.
            Debug.Log("Incorrect swipe. Score: " + GameManager.instance.Score);
        }
        // Unsubscribe and hide the current image.
        UnsubscribeFromCurrentImage();
        currentImage.SetActive(false);

        // Show the swipe feedback message box.
        string prefix = swipeCorrect ? "Correct!\n" : "Wrong!\n";
        string feedback = currentCardData.isAbnormal ? currentCardData.abnormalFeedback : currentCardData.normalFeedback;
        messageText.text = prefix + feedback;
        messageBoxImage.color = swipeCorrect ? Color.green : Color.red;
        messageBoxPanel.SetActive(true);
    }

    /// <summary>
    /// Called by the OK button on the swipe feedback message box.
    /// If the image is abnormal and the swipe was correct, starts the tap phase.
    /// Otherwise, proceeds to the next image.
    /// </summary>
    public void OnSwipeMessageBoxClosed()
    {
        if (isAIAssistActive)
            return; // Ignore button click if AI Assist is active.

        messageBoxPanel.SetActive(false);

        if (currentCardData.isAbnormal /* && swipeCorrect*/)
        {
            StartTapPhase();
        }
        else
        {
            LoadNextImage();
        }
    }

    // --- Step 2: Tap Phase ---
    /// <summary>
    /// Initiates the tap phase.
    /// Displays a zoomed copy of the image so the player can tap on the abnormal region.
    /// </summary>
    void StartTapPhase()
    {
        tapPhaseComplete = false; // Reset flag.
        tapPanel.SetActive(true);

        // Enable tap instruction text.
        if (tapInstructionText != null)
        {
            tapInstructionText.text = "Click on the tube tip";
            tapInstructionText.gameObject.SetActive(true);
        }

        // Disable the swipe arrows since we're now in tap phase.
        if (swipeArrowLeft != null)
            swipeArrowLeft.SetActive(false);
        if (swipeArrowRight != null)
            swipeArrowRight.SetActive(false);

        // Copy the sprite from the original image to the zoomed image.
        Image origImage = currentImage.GetComponent<Image>();
        if (origImage != null && tapZoomedImage != null)
        {
            tapZoomedImage.sprite = origImage.sprite;
        }
        if (tapStar != null)
            tapStar.gameObject.SetActive(false);

        // Reset the tap controller.
        tapController.ResetController();
        tapController.targetPosition = currentCardData.tapTargetPosition;
        tapController.tolerance = currentCardData.tapTolerance;

        // Subscribe to tap events.
        tapController.OnIdentificationComplete += HandleTapResult;
    }


    /// <summary>
    /// Handles tap attempts from the tap controller.
    /// Depending on the attempt, shows appropriate tap feedback.
    /// </summary>
    /// <param name="correctTap">True if the tap is within tolerance.</param>
    /// <param name="attemptsUsed">Number of attempts used (1 or 2).</param>
    void HandleTapResult(bool correctTap, int attemptsUsed)
    {
        // --- First or Second Attempt ---
        if (attemptsUsed == 1 || attemptsUsed == 2)
        {
            if (correctTap)
            {
                timerRunning = false;
                GameManager.instance.Score++;
                tapMessageText.gameObject.SetActive(true);
                tapMessageText.text = currentCardData.tapSuccessFeedback;
                tapMessageBoxImage.color = Color.green;
                tapMessageBoxPanel.SetActive(true);
                tapController.inputEnabled = false; // Disable input without disabling the component.
                tapPhaseComplete = true;
                tapController.OnIdentificationComplete -= HandleTapResult;
            }
            else
            {
                tapMessageText.gameObject.SetActive(true);
                tapMessageText.text = "Incorrect. Please try again.";
                tapMessageBoxImage.color = Color.red;
                tapMessageBoxPanel.SetActive(true);
                tapController.inputEnabled = false; // Disable input while feedback is active.
                                                    // Optionally, re-enable input after a delay if you want to allow retries.
            }
        }
        // --- Third (Last) Attempt ---
        else if (attemptsUsed >= 3)
        {
            if (correctTap)
            {
                timerRunning = false;
                GameManager.instance.Score++;
                tapMessageText.gameObject.SetActive(true);
                tapMessageText.text = currentCardData.tapSuccessFeedback;
                tapMessageBoxImage.color = Color.green;
                tapMessageBoxPanel.SetActive(true);
                tapController.inputEnabled = false;
                tapPhaseComplete = true;
                tapController.OnIdentificationComplete -= HandleTapResult;
            }
            else
            {
                if (tapStar != null)
                {
                    tapStar.rectTransform.anchoredPosition = currentCardData.tapTargetPosition;
                    tapStar.gameObject.SetActive(true);
                    tapZoomedImage.sprite = currentCardData.DescriptionImage;
                }
                if (AbnormalityDescriptionPanel != null)
                {
                    AbnormalityDescription.text = currentCardData.abnormalImageDescription;
                    AbnormalityDescriptionPanel.SetActive(true);
                }
                // Pause the timer so it doesn't expire during feedback.
                timerRunning = false;
                mistakeOccurred = true;
                mistakePhase = MistakePhase.Tap;
                tapController.OnIdentificationComplete -= HandleTapResult;
                tapController.inputEnabled = false;
                StartCoroutine(WaitForEraserTimeout(3f));
            }
        }
    }


    /// <summary>
    /// Waits for a specified time for the player to use the Eraser.
    /// If not used, closes the tap panel and proceeds to the next image.
    /// </summary>
    /// <param name="waitTime">Time to wait in seconds.</param>
    //IEnumerator WaitForEraserTimeout(float waitTime)
    //{
    //    yield return new WaitForSeconds(waitTime);
    //    if (mistakeOccurred) // Still waiting for Eraser use.
    //    {
    //        tapPanel.SetActive(false);
    //        if (tapStar != null)
    //            tapStar.gameObject.SetActive(false);
    //        if (AbnormalityDescriptionPanel != null)
    //            AbnormalityDescriptionPanel.SetActive(false);
    //        mistakeOccurred = false;
    //        mistakePhase = MistakePhase.None;
    //        LoadNextImage();
    //    }
    //}

    IEnumerator WaitForEraserTimeout(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        if (mistakeOccurred) // Still waiting for Eraser use.
        {
            // Show all the tap boxes for the player's taps.
            tapController.ShowAllTapBoxes();
            // Wait 2 seconds for the player to review before proceeding.
            yield return new WaitForSeconds(2f);
            tapPanel.SetActive(false);
            if (tapStar != null)
                tapStar.gameObject.SetActive(false);
            if (AbnormalityDescriptionPanel != null)
                AbnormalityDescriptionPanel.SetActive(false);
            mistakeOccurred = false;
            mistakePhase = MistakePhase.None;
            LoadNextImage();
        }
    }

    /// <summary>
    /// Called by the OK button on the tap feedback message box.
    /// If the tap phase is complete, hides the tap panel and loads the next image.
    /// </summary>
    public void OnTapMessageBoxClosed()
    {
        if (isAIAssistActive)
            return; // Ignore input if AI Assist is active.

        tapMessageBoxPanel.SetActive(false);
        tapController.inputEnabled = true;

        if (tapPhaseComplete)
        {
            // Disable tap instruction text since tap phase feedback is closing.
            if (tapInstructionText != null)
                tapInstructionText.gameObject.SetActive(false);

            // Show all tap boxes (orange markers) before proceeding.
            tapController.ShowAllTapBoxes();
            // Wait for 2 seconds so the player can review their taps,
            // then close the tap panel and load the next image.
            StartCoroutine(WaitThenProceedFromTapPhase(2f));
        }
    }

    // New coroutine to wait a few seconds before closing the tap phase.
    IEnumerator WaitThenProceedFromTapPhase(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        tapPanel.SetActive(false);
        LoadNextImage();
    }

    /// <summary>
    /// Handles when the timer runs out for the current image.
    /// Disables the image, clears active UI, unsubscribes from events, and proceeds to the next image.
    /// </summary>
    void HandleTimeOut()
    {
        Debug.Log("Time is up for this image!");

        if (currentImage != null)
            currentImage.SetActive(false);
        if (messageBoxPanel.activeSelf)
            messageBoxPanel.SetActive(false);
        if (tapMessageBoxPanel.activeSelf)
            tapMessageBoxPanel.SetActive(false);
        if (tapPanel.activeSelf)
            tapPanel.SetActive(false);

        // Disable the Tap Star if it's active.
        if (tapStar != null && tapStar.gameObject.activeSelf)
            tapStar.gameObject.SetActive(false);

        // Disable the Abnormality Description Panel if it's active.
        if (AbnormalityDescriptionPanel != null && AbnormalityDescriptionPanel.activeSelf)
            AbnormalityDescriptionPanel.SetActive(false);

        // Disable the tap instruction text.
        if (tapInstructionText != null && tapInstructionText.gameObject.activeSelf)
            tapInstructionText.gameObject.SetActive(false);

        if (currentSwipeController != null)
            currentSwipeController.OnSwipeDetected -= HandleSwipe;
        if (tapController != null)
            tapController.OnIdentificationComplete -= HandleTapResult;

        LoadNextImage();
    }


    /// <summary>
    /// Loads the next image from the deck and resets per-image powerup flags.
    /// </summary>
    void LoadNextImage()
    {
        // Prevent duplicate loading.
        if (isLoadingNextImage)
            return;
        isLoadingNextImage = true;

        currentImageIndex++;
        // Reset per-image powerup flags.
        glassesUsed = false;
        eraserUsed = false;
        mistakeOccurred = false;
        mistakePhase = MistakePhase.None;

        if (imageCards != null && currentImageIndex < imageCards.Count)
        {
            imageCards[currentImageIndex].SetActive(true);
            SubscribeToCurrentImage();
            // Reset timer for the next image.
            timeRemaining = timePerImage;
            timerRunning = true;

            // Enable the swipe arrows in the swipe phase.
            if (swipeArrowLeft != null)
                swipeArrowLeft.SetActive(true);
            if (swipeArrowRight != null)
                swipeArrowRight.SetActive(true);

            // Reset the guard flag at the end of this frame.
            StartCoroutine(ResetLoadingFlag());
        }
        else
        {
            Debug.Log("All images have been processed. Final score: " + GameManager.instance.Score);
            if (gameCompletePanel != null)
                gameCompletePanel.SetActive(true);
            if (mainCanvas != null)
                mainCanvas.SetActive(false);
            // No need to reset the flag if the game is over.
        }
    }

    IEnumerator ResetLoadingFlag()
    {
        // Wait until the end of the frame before resetting the flag.
        yield return new WaitForEndOfFrame();
        isLoadingNextImage = false;
    }



    /// <summary>
    /// Called by the Clock powerup button.
    /// Deducts score and adds extra time to the current timer.
    /// </summary>
    public void UseClockPowerup()
    {
        if (GameManager.instance.Score >= clockPowerupCost)
        {
            GameManager.instance.Score -= clockPowerupCost;
            timeRemaining += clockPowerupTimeIncrease;
            Debug.Log("Clock powerup used! Time increased by " + clockPowerupTimeIncrease + " seconds.");
        }
    }

    /// <summary>
    /// Called by the Glasses powerup button.
    /// Deducts score and provides a hint:
    /// In swipe phase, shows a text hint.
    /// In tap phase, visually marks the abnormal region.
    /// </summary>
    public void UseGlassesPowerup()
    {
        if (GameManager.instance.Score >= glassesPowerupCost && !glassesUsed)
        {
            GameManager.instance.Score -= glassesPowerupCost;
            glassesUsed = true;
            if (tapPanel.activeSelf)
            {
                StartCoroutine(ShowTapHint());
            }
            else
            {
                StartCoroutine(ShowSwipeHint());
            }
        }
    }

    /// <summary>
    /// Displays a visual hint on the tap image by repositioning and showing tapStar for 2 seconds.
    /// </summary>
    IEnumerator ShowTapHint()
    {
        if (tapStar != null)
        {
            tapStar.rectTransform.anchoredPosition = currentCardData.tapTargetPosition;
            tapStar.gameObject.SetActive(true);
        }
        yield return new WaitForSeconds(2f);
        if (tapStar != null)
            tapStar.gameObject.SetActive(false);
    }

    /// <summary>
    /// Displays a text hint in swipe phase indicating whether the image is normal or abnormal.
    /// </summary>
    IEnumerator ShowSwipeHint()
    {
        if (glassesHintText != null)
        {
            string hint = currentCardData.isAbnormal ? "Hint: This image is abnormal." : "Hint: This image is normal.";
            glassesHintText.text = hint;
            glassesHintText.gameObject.SetActive(true);
            yield return new WaitForSeconds(2f);
            glassesHintText.gameObject.SetActive(false);
            glassesHintText.text = "";
        }
        else
        {
            Debug.Log("Glasses Hint: " + (currentCardData.isAbnormal ? "This image is abnormal." : "This image is normal."));
            yield return null;
        }
    }

    /// <summary>
    /// Called by the Eraser powerup button.
    /// Deducts score and resets the tap phase to allow the player to retry.
    /// (Eraser is available only in the tap phase.)
    /// </summary>
    public void UseEraserPowerup()
    {
        if (GameManager.instance.Score >= eraserPowerupCost && mistakeOccurred && !eraserUsed)
        {
            GameManager.instance.Score -= eraserPowerupCost;
            ResetCurrentImageState();
            Debug.Log("Eraser powerup used to undo tap mistake.");
        }
    }

    /// <summary>
    /// Resets the current tap phase state.
    /// Hides tap feedback, resets the tap controller, and re-allows tap input.
    /// </summary>
    private void ResetCurrentImageState()
    {
        tapMessageBoxPanel.SetActive(false);
        if (tapStar != null)
            tapStar.gameObject.SetActive(false);
        if (AbnormalityDescriptionPanel != null)
            AbnormalityDescriptionPanel.SetActive(false);
        tapController.ResetController();
        tapController.OnIdentificationComplete += HandleTapResult;
        tapPanel.SetActive(true);

        mistakeOccurred = false;
        mistakePhase = MistakePhase.None;
        eraserUsed = true;
    }

    /// <summary>
    /// Called by the AI Assist powerup button.
    /// Deducts score and auto-answers the current question:
    /// - For normal images: auto-answers the swipe.
    /// - For abnormal images: if in swipe phase, auto-answers both swipe and tap phases; if in tap phase, auto-answers the tap.
    /// </summary>
    public void UseAIAssistPowerup()
    {
        if (GameManager.instance.Score >= aiAssistPowerupCost)
        {
            GameManager.instance.Score -= aiAssistPowerupCost;
            isAIAssistActive = true;

            if (!currentCardData.isAbnormal)
            {
                // Normal image: auto-answer swipe.
                StartCoroutine(AutoAnswerNormal());
            }
            else
            {
                // Abnormal image.
                if (!tapPanel.activeSelf)
                {
                    // In swipe phase: auto-answer both swipe and tap phases.
                    StartCoroutine(AutoAnswerAbnormalSwipe());
                }
                else
                {
                    // In tap phase: auto-answer tap.
                    StartCoroutine(AutoAnswerAbnormalTap());
                }
            }
        }
    }

    /// <summary>
    /// Auto-answers a normal image by simulating a correct swipe.
    /// </summary>
    IEnumerator AutoAnswerNormal()
    {
        if (currentImage != null)
        {
            // Unsubscribe from events and disable the image.
            UnsubscribeFromCurrentImage();
            currentImage.SetActive(false);
            currentImage = null; // Clear reference to avoid race conditions.
        }
        swipeCorrect = true;
        GameManager.instance.Score++; // Award correct swipe point.
        timerRunning = false;

        messageText.text = "Correct!\n" + currentCardData.normalFeedback;
        messageBoxImage.color = Color.green;
        messageBoxPanel.SetActive(true);
        yield return new WaitForSeconds(1f);
        messageBoxPanel.SetActive(false);
        isAIAssistActive = false;
        LoadNextImage();
    }

    /// <summary>
    /// Auto-answers an abnormal image during the swipe phase by simulating a correct swipe then auto-answering the tap phase.
    /// </summary>
    IEnumerator AutoAnswerAbnormalSwipe()
    {
        if (currentImage != null)
        {
            UnsubscribeFromCurrentImage();
            currentImage.SetActive(false);
            currentImage = null;
        }
        // Auto-answer swipe phase.
        swipeCorrect = true;
        GameManager.instance.Score++; // Award swipe point.
        timerRunning = false;

        messageText.text = "Correct!\n" + currentCardData.abnormalFeedback;
        messageBoxImage.color = Color.green;
        messageBoxPanel.SetActive(true);
        yield return new WaitForSeconds(1f);
        messageBoxPanel.SetActive(false);
        // Now auto-answer tap phase.
        GameManager.instance.Score++; // Award tap bonus.
        tapMessageText.text = currentCardData.tapSuccessFeedback;
        tapMessageBoxImage.color = Color.green;
        tapMessageBoxPanel.SetActive(true);
        yield return new WaitForSeconds(1f);
        tapMessageBoxPanel.SetActive(false);
        tapPanel.SetActive(false);
        isAIAssistActive = false;
        LoadNextImage();
    }

    /// <summary>
    /// Auto-answers an abnormal image during the tap phase by simulating a correct tap.
    /// </summary>
    IEnumerator AutoAnswerAbnormalTap()
    {
        if (currentImage != null)
        {
            UnsubscribeFromCurrentImage();
            currentImage.SetActive(false);
            currentImage = null;
        }
        GameManager.instance.Score++; // Award tap bonus.
        tapMessageText.text = currentCardData.tapSuccessFeedback;
        tapMessageBoxImage.color = Color.green;
        tapMessageBoxPanel.SetActive(true);
        yield return new WaitForSeconds(1f);
        tapMessageBoxPanel.SetActive(false);
        tapPanel.SetActive(false);
        isAIAssistActive = false;
        LoadNextImage();
    }


}

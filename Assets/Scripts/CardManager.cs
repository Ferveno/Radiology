using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class CardManager : MonoBehaviour
{
    [SerializeField] List<Card> Cards; // 16 total card objects in the scene
    private List<Card> CardsWithSprite;   // Holds shuffled cards with sprite references
    [SerializeField] List<Sprite> LineSprites;    // 6 unique images (lines/tubes)
    [SerializeField] List<Sprite> NameSprites;      // 6 corresponding name images
    [SerializeField] List<Sprite> DistractorSprites;  // 4 distractor cards
    [SerializeField] Sprite FaceDownCard;
    [SerializeField] GameObject WinText;
    [SerializeField] GameObject LoseText;  // Lose message display

    private int Matches = 0;
    private Card FirstCard;
    private Card SecondCard;
    private bool noMatch = false;

    public GameObject MainGame2;
    public GameObject TimerTextObj;
    public GameObject FlipsTextObj;

    public GameObject PowerupPanel;

    // --- Timer Variables ---
    [SerializeField] float timeLimit = 60f;      // Total time allowed (60 seconds)
    private float remainingTime;                   // Countdown timer
    [SerializeField] TextMeshProUGUI TimerText;    // UI text to display remaining time

    // --- Flip Limit Variables ---
    [SerializeField] int totalFlipsAllowed = 30;   // Total number of flips allowed
    private int remainingFlips;                     // Flips remaining
    [SerializeField] TextMeshProUGUI flipsText;     // UI text to display remaining flips

    // --- Clock Powerup Variables ---
    [SerializeField] Button clockPowerupButton;    // Button for Clock powerup
    [SerializeField] int clockPowerupCost = 50;      // Score cost for Clock powerup
    [SerializeField] float clockExtraTime = 5f;      // Extra time added when powerup is used
    private bool clockPowerupUsed = false;           // Allowed only once per game

    // --- Glasses Powerup Variables ---
    [SerializeField] Button glassesPowerupButton;    // Button for Glasses powerup
    [SerializeField] int glassesPowerupCost = 30;      // Score cost for Glasses powerup
    [SerializeField] float glassesDuration = 3f;       // Duration (in seconds) for which all cards are shown
    private bool glassesPowerupUsed = false;           // Allowed only once per game
    private bool glassesActive = false;                // Flag to disable card selection during the effect

    // --- Eraser Powerup Variables ---
    [SerializeField] Button eraserPowerupButton;     // Button for Eraser powerup
    [SerializeField] int eraserPowerupCost = 40;       // Score cost for Eraser powerup
    private bool eraserPowerupUsed = false;             // Allowed only once per game

    // --- Assist Powerup Variables ---
    [SerializeField] Button assistPowerupButton;     // Button for Assist powerup
    [SerializeField] int assistPowerupCost = 60;       // Score cost for Assist powerup
    private bool assistPowerupUsed = false;             // Allowed only once per game

    void Awake()
    {
        CardsWithSprite = new List<Card>();
        SetAllCards();
        FirstCard = null;
        SecondCard = null;
        Matches = 0;

        TimerTextObj.SetActive(true);
        FlipsTextObj.SetActive(true);

        PowerupPanel.SetActive(true);

        remainingTime = timeLimit; // Initialize timer
        remainingFlips = totalFlipsAllowed; // Initialize flip count
        UpdateFlipsUI();
    }

    void Update()
    {
        // Timer countdown logic.
        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            TimerText.text = "Time: " + Mathf.Ceil(remainingTime).ToString();
            if (remainingTime <= 0)
            {
                TimerText.text = "Time: 0";
                LoseGame(); // Time ran out: trigger lose condition.
            }
        }

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

        // Update Assist powerup button interactivity.
        if (assistPowerupButton != null)
        {
            assistPowerupButton.interactable = (!assistPowerupUsed && GameManager.instance.Score >= assistPowerupCost);
        }

        // Update Eraser powerup button interactivity.
        if (eraserPowerupButton != null)
        {
            eraserPowerupButton.interactable = (!eraserPowerupUsed && GameManager.instance.Score >= eraserPowerupCost);
        }
    }

    // Updates the flips UI text.
    void UpdateFlipsUI()
    {
        if (flipsText != null)
        {
            flipsText.text = "Flips left: " + remainingFlips.ToString();
        }
    }

    void SetAllCards()
    {
        int index = 0;
        // Adding the 6 pairs: one for the tube image, one for the text image.
        for (int i = 0; i < LineSprites.Count; i++)
        {
            Card TubeCard = new Card { cardArtWork = LineSprites[i], nameArtWork = null, index = index };
            Card NameCard = new Card { cardArtWork = NameSprites[i], nameArtWork = null, index = index };
            index++;

            CardsWithSprite.Add(TubeCard);
            CardsWithSprite.Add(NameCard);
        }

        // Adding 4 distractor cards.
        foreach (Sprite s in DistractorSprites)
        {
            Card distractor = new Card { cardArtWork = s, nameArtWork = null, index = -1, isDistractor = true };
            CardsWithSprite.Add(distractor);
        }

        reshuffle(CardsWithSprite);
        AssignCards();
    }

    void AssignCards()
    {
        for (int i = 0; i < Cards.Count; i++)
        {
            Cards[i].cardArtWork = CardsWithSprite[i].cardArtWork;
            Cards[i].index = CardsWithSprite[i].index;
            Cards[i].isDistractor = CardsWithSprite[i].isDistractor;
        }
    }

    public void CardSelected(Card C)
    {
        // If the Glasses powerup effect is active, ignore input.
        if (glassesActive)
            return;

        // Check if there are any flips left.
        if (remainingFlips <= 0)
        {
            LoseGame();
            return;
        }

        // Deduct a flip for every card selection and update UI.
        remainingFlips--;
        UpdateFlipsUI();

        if (noMatch)
        {
            noMatch = false;
            if (FirstCard != null)
                FirstCard.NotChosen(FaceDownCard);
            if (SecondCard != null)
                SecondCard.NotChosen(FaceDownCard);
            FirstCard = null;
            SecondCard = null;
        }

        if (FirstCard == null)
        {
            FirstCard = C;
            FirstCard.OpenCard();
            return;
        }

        if (SecondCard == null)
        {
            SecondCard = C;
            SecondCard.OpenCard();

            // If either card is a distractor, flag mismatch.
            if (FirstCard.isDistractor || SecondCard.isDistractor)
            {
                noMatch = true;
                return;
            }

            // Check for a matching pair.
            if (FirstCard.index == SecondCard.index)
            {
                // Start a coroutine to delay the hiding of matched cards.
                StartCoroutine(DelayMatch());
            }
            else
            {
                noMatch = true;
                FirstCard.NoMatch();
                SecondCard.NoMatch();
            }
        }
    }

    IEnumerator DelayMatch()
    {
        // Wait for a short delay (e.g., 0.5 seconds) so the player can see the matched pair.
        yield return new WaitForSeconds(0.5f);
        // Hide the matched cards.
        if (FirstCard != null)
            FirstCard.Chosen();
        if (SecondCard != null)
            SecondCard.Chosen();
        FirstCard = null;
        SecondCard = null;
        Matches++;
        if (Matches == 6)  // All 6 pairs found.
            GameOver();
    }

    void GameOver()
    {
        WinText.SetActive(true);
        StartCoroutine(EndGame());
    }

    void LoseGame()
    {
        LoseText.SetActive(true);
        StartCoroutine(EndGame());
    }

    IEnumerator EndGame()
    {
        yield return new WaitForSeconds(3f);
        MainGame2.SetActive(false);
        TimerTextObj.SetActive(false);
        FlipsTextObj.SetActive(false);
        PowerupPanel.SetActive(false);
        LoseText.gameObject.SetActive(false);

        GameManager.instance.OnGame2Complete();
    }

    void reshuffle(List<Card> list)
    {
        for (int t = 0; t < list.Count; t++)
        {
            Card temp = list[t];
            int r = Random.Range(t, list.Count);
            list[t] = list[r];
            list[r] = temp;
        }
    }

    // --- Clock Powerup Activation ---
    // This method should be linked to the Clock powerup button's OnClick event.
    public void ActivateClockPowerup()
    {
        if (!clockPowerupUsed && GameManager.instance.Score >= clockPowerupCost)
        {
            GameManager.instance.Score -= clockPowerupCost;
            GameManager.instance.ScoreUpdater();

            remainingTime += clockExtraTime;
            clockPowerupUsed = true;
        }
    }

    // --- Glasses Powerup Activation ---
    // This method should be linked to the Glasses powerup button's OnClick event.
    public void ActivateGlassesPowerup()
    {
        if (!glassesPowerupUsed && GameManager.instance.Score >= glassesPowerupCost)
        {
            GameManager.instance.Score -= glassesPowerupCost;
            GameManager.instance.ScoreUpdater();

            glassesPowerupUsed = true;
            StartCoroutine(GlassesPowerupCoroutine());
        }
    }

    // Coroutine to flip all cards face-up temporarily, then flip back any unmatched ones.
    IEnumerator GlassesPowerupCoroutine()
    {
        // Disable card selection during the effect.
        glassesActive = true;

        // Flip all cards face-up.
        foreach (Card card in Cards)
        {
            card.OpenCard();
        }

        // Wait for the specified duration.
        yield return new WaitForSeconds(glassesDuration);

        // Flip back cards that are not matched.
        foreach (Card card in Cards)
        {
            if (card.gameObject.activeSelf) // Only process if card is still active (unmatched).
            {
                card.NotChosen(FaceDownCard);
            }
        }

        // Re-enable card selection.
        glassesActive = false;
    }

    // --- Eraser Powerup Activation ---
    // This powerup undoes the current selection and refunds the corresponding flip(s).
    public void ActivateEraserPowerup()
    {
        if (!eraserPowerupUsed && GameManager.instance.Score >= eraserPowerupCost)
        {
            GameManager.instance.Score -= eraserPowerupCost;
            GameManager.instance.ScoreUpdater();

            eraserPowerupUsed = true;
            int refunds = 0;

            // If a card is currently selected, flip it back and refund a flip.
            if (FirstCard != null)
            {
                FirstCard.NotChosen(FaceDownCard);
                FirstCard = null;
                refunds++;
            }
            if (SecondCard != null)
            {
                SecondCard.NotChosen(FaceDownCard);
                SecondCard = null;
                refunds++;
            }
            // Refund the flips for the undone selections.
            remainingFlips += refunds;
            UpdateFlipsUI();
        }
    }

    // --- Assist Powerup Activation ---
    // This powerup automatically reveals one matching pair and awards the corresponding score.
    public void ActivateAssistPowerup()
    {
        if (!assistPowerupUsed && GameManager.instance.Score >= assistPowerupCost)
        {
            GameManager.instance.Score -= assistPowerupCost;
            GameManager.instance.ScoreUpdater();

            assistPowerupUsed = true;

            // Clear any current selection.
            if (FirstCard != null)
            {
                FirstCard.NotChosen(FaceDownCard);
                FirstCard = null;
            }
            if (SecondCard != null)
            {
                SecondCard.NotChosen(FaceDownCard);
                SecondCard = null;
            }

            // Search for an unmatched, non-distractor pair.
            bool pairFound = false;
            for (int i = 0; i < Cards.Count; i++)
            {
                Card card1 = Cards[i];
                // Skip if already matched (card hidden) or if it's a distractor.
                if (!card1.gameObject.activeSelf || card1.isDistractor)
                    continue;
                for (int j = i + 1; j < Cards.Count; j++)
                {
                    Card card2 = Cards[j];
                    if (!card2.gameObject.activeSelf || card2.isDistractor)
                        continue;
                    if (card1.index == card2.index)
                    {
                        // Found a matching pair.
                        card1.OpenCard();
                        card2.OpenCard();
                        card1.Chosen();
                        card2.Chosen();
                        Matches++;
                        pairFound = true;
                        break;
                    }
                }
                if (pairFound)
                    break;
            }

            // Optionally check if the game is won after auto-matching.
            if (Matches == 6)
            {
                GameOver();
            }
        }
    }
}

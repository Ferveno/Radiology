using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using DG.Tweening;    // Make sure you have DOTween imported

public class DialogueManager : MonoBehaviour
{
    [Header("UI Refs")]
    public TextMeshProUGUI dialogueText;    // Your TMP text box
    public Button continueButton;           // Your Continue button
    public Image blackoutImage;             // Full‑screen black Image (alpha=0 at start)
    public GameObject[] dialogueImages;     // Portraits or images, one per line

    [Header("Dialogue Data")]
    public string[] dialogues;              // Your 4 dialogue strings
    public float typingSpeed = 0.05f;       // Typewriter speed

    [Header("Blackout Settings")]
    public float fadeDuration = 0.5f;       // Fade‑in/out duration
    public float holdDuration = 1f;         // How long black stays at 100% alpha

    private int currentIndex = 0;
    private bool isTyping = false;

    void Start()
    {
        if (dialogues.Length == 0)
        {
            Debug.LogWarning("No dialogues assigned!");
            return;
        }

        // Init blackout overlay
        blackoutImage.color = new Color(0, 0, 0, 0);

        // Wire the button
        continueButton.onClick.AddListener(OnContinuePressed);

        // Hide all portraits, then show the first
        ToggleAllImages(false);
        dialogueImages[0].SetActive(true);

        // Start first line
        StartCoroutine(TypeDialogue(dialogues[0]));
    }

    void OnContinuePressed()
    {
        if (isTyping) return;               // Don't advance mid‑type

        currentIndex++;

        // If still in range of dialogues
        if (currentIndex < dialogues.Length)
        {
            

            // after first line, do blackout
            if (currentIndex == 1)
                StartCoroutine(BlackoutThenNext());
            else
                StartCoroutine(TypeDialogue(dialogues[currentIndex]));

            // swap portraits
            ToggleAllImages(false);
            dialogueImages[currentIndex].SetActive(true);
        }
        else
        {
            EndDialogue();
        }
    }

    IEnumerator BlackoutThenNext()
    {
        // Fade to black
        yield return blackoutImage
            .DOFade(1f, fadeDuration)
            .SetUpdate(true)    // unaffected by timeScale if you pause elsewhere
            .WaitForCompletion();

        // Hold
        yield return new WaitForSeconds(holdDuration);

        // Fade back to transparent
        yield return blackoutImage
            .DOFade(0f, fadeDuration)
            .SetUpdate(true)
            .WaitForCompletion();

        // Finally type next
        StartCoroutine(TypeDialogue(dialogues[currentIndex]));
    }

    IEnumerator TypeDialogue(string dialogue)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char c in dialogue)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    void EndDialogue()
    {
        // disable the entire panel (and its script)
        gameObject.SetActive(false);
    }

    void ToggleAllImages(bool on)
    {
        foreach (var img in dialogueImages)
            img.SetActive(on);
    }
}

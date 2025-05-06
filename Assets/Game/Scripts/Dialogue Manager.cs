using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;



public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI dialogueText; // Reference to UI Text
    public Button continueButton; // Reference to the Continue button
    public string[] dialogues; // Array of dialogue strings

    // private int currentIndex = 0;

    // void Start()
    // {
    //     if (dialogues.Length > 0)
    //     {
    //         dialogueText.text = dialogues[currentIndex]; // Display first dialogue
    //         continueButton.onClick.AddListener(ShowNextDialogue); // Add listener to button
    //     }
    //     else
    //     {
    //         Debug.LogWarning("Dialogue list is empty!");
    //     }
    // }

    // void ShowNextDialogue()
    // {
    //     currentIndex++;

    //     if (currentIndex < dialogues.Length)
    //     {
    //         dialogueText.text = dialogues[currentIndex]; // Update dialogue text
    //     }
    //     else
    //     {
    //         Debug.Log("All dialogues finished.");
    //         continueButton.interactable = false; // Disable button after last dialogue
    //     }
    // }

     public float typingSpeed = 0.05f; // Speed of typewriter effect

    private int currentIndex = 0;
    private bool isTyping = false;

    void Start()
    {
        if (dialogues.Length > 0)
        {
            continueButton.onClick.AddListener(ShowNextDialogue);
            StartCoroutine(TypeDialogue(dialogues[currentIndex])); // Start first dialogue
        }
        else
        {
            Debug.LogWarning("Dialogue list is empty!");
        }
    }

    void ShowNextDialogue()
    {
        if (isTyping) return; // Prevent skipping while typing

        currentIndex++;

        if (currentIndex < dialogues.Length)
        {
            StartCoroutine(TypeDialogue(dialogues[currentIndex])); // Start typing next dialogue
        }
        else
        {
            Debug.Log("All dialogues finished.");
            continueButton.interactable = false; // Disable button after last dialogue
        }
    }

    IEnumerator TypeDialogue(string dialogue)
    {
        isTyping = true;
        dialogueText.text = ""; // Clear text before typing

        foreach (char letter in dialogue.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed); // Delay between letters
        }

        isTyping = false;
    }
}

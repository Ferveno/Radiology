using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AnswerButton : MonoBehaviour
{
    UnityEngine.UI.Button button;

    private void Start()
    {
        button = GetComponent<UnityEngine.UI.Button>();
        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        // Disable all answer buttons so that only one click is registered per question.
        foreach (Button btn in GameThreeGamePlay_PanelUI.instance.optionButtons_Ref)
        {
            btn.interactable = false;
        }

        // Cancel the timer since the player has made a choice.
        GameThreeGamePlay_PanelUI.instance.CancelTimer();

        TextMeshProUGUI answerText = GetComponentInChildren<TextMeshProUGUI>();
        if (answerText != null)
        {
            string buttonTextString = answerText.text;
            if (buttonTextString == GameThreeManager.instance.currentLevel.correctAnswer)
            {
                answerText.color = Color.white;
                button.image.color = Color.green;

                for (int i = 0; i < GameThreeManager.instance.allLevels.Count; i++)
                {
                    if (GameThreeManager.instance.allLevels[i] == GameThreeManager.instance.currentLevel)
                    {
                        GameThreeManager.instance.allLevels[i].isAnswered = true;
                        // Optionally, save status here.

                        GameManager.instance.Score++;
                    }
                }
                StartCoroutine(WaitForAnswerDescription());
            }
            else
            {
                OnClick_WrongButton();
                answerText.color = Color.white;
                button.image.color = Color.red;
            }
        }
    }

    void OnClick_WrongButton()
    {
        for (int i = 0; i < GameThreeGamePlay_PanelUI.instance.optionButtons_Ref.Length; i++)
        {
            if (GameThreeGamePlay_PanelUI.instance.optionButtons_Ref[i].GetComponentInChildren<TextMeshProUGUI>().text == GameThreeManager.instance.currentLevel.correctAnswer)
            {
                GameThreeGamePlay_PanelUI.instance.optionButtons_Ref[i].image.color = Color.green;
            }
        }

        // Call the new wrong-answer handling routine.
        GameThreeGamePlay_PanelUI.instance.HandleWrongAnswer();
    }

    public IEnumerator WaitForAnswerDescription()
    {
        yield return new WaitForSeconds(0.5f);
        GameThreeGamePlay_PanelUI.instance.questionImage.sprite = GameThreeManager.instance.currentLevel.correctAnswerSprite;
        GameThreeGamePlay_PanelUI.instance.correctAnswerPanel.gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        GameThreeGamePlay_PanelUI.instance.correctAnswerPanel.gameObject.SetActive(true);
        yield return new WaitForSeconds(7);
        GameThreeGamePlay_PanelUI.instance.correctAnswerPanel.gameObject.SetActive(false);
        GameThreeGamePlay_PanelUI.instance.NextLevel();
    }

    // Optionally, you can remove the old WaitForNextQuestion coroutine if it is no longer needed.
}

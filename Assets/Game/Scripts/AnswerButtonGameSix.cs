using System.Collections;
using UnityEngine;
using TMPro;

public class AnswerButtonGameSix : MonoBehaviour
{
    UnityEngine.UI.Button button;

    private void Start()
    {
        button = GetComponent<UnityEngine.UI.Button>();
        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        // Stop the timer and remove any glasses highlight when the player clicks.
        GameSixGamePlay_PanelUI.instance.StopTimer();
        GameSixGamePlay_PanelUI.instance.DeactivateGlasses();

        TextMeshProUGUI answerText = GetComponentInChildren<TextMeshProUGUI>();
        if (answerText != null)
        {
            string buttonTextString = answerText.text;
            if (buttonTextString == GameSixManager.instance.currentLevel.correctAnswer)
            {
                answerText.color = Color.white;
                button.image.color = Color.green;

                for (int i = 0; i < GameSixManager.instance.allLevels.Count; i++)
                {
                    if (GameSixManager.instance.allLevels[i] == GameSixManager.instance.currentLevel)
                    {
                        GameSixManager.instance.allLevels[i].isAnswered = true;
                        GameManager.instance.Score++;
                        GameManager.instance.ScoreUpdater();
                    }
                }
                StartCoroutine(WaitForNextQuestion());
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
        // Highlight the correct answer in green if answered wrong.
        for (int i = 0; i < GameSixGamePlay_PanelUI.instance.optionButtons_Ref.Length; i++)
        {
            if (GameSixGamePlay_PanelUI.instance.optionButtons_Ref[i]
                .GetComponentInChildren<TextMeshProUGUI>().text == GameSixManager.instance.currentLevel.correctAnswer)
            {
                GameSixGamePlay_PanelUI.instance.optionButtons_Ref[i].image.color = Color.green;
            }
        }
        StartCoroutine(WaitForNextQuestion());
    }

    public IEnumerator WaitForNextQuestion()
    {
        yield return new WaitForSeconds(1f);  // Reduced delay for smoother transition
        GameSixManager.instance.currentLevel = null;
        GameSixGamePlay_PanelUI.instance.NextLevel();
    }
}

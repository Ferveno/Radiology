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
        // 1) Stop the timer and glasses
        GameSixGamePlay_PanelUI.instance.StopTimer();
        GameSixGamePlay_PanelUI.instance.DeactivateGlasses();

        // 2) **Disable all option buttons** immediately
        foreach (var btn in GameSixGamePlay_PanelUI.instance.optionButtons_Ref)
            btn.interactable = false;

        // 3) Check this button’s text
        TextMeshProUGUI answerText = GetComponentInChildren<TextMeshProUGUI>();
        if (answerText == null) return;

        bool correct = answerText.text == GameSixManager.instance.currentLevel.correctAnswer;
        // 4) In both cases, mark this question answered
        GameSixManager.instance.currentLevel.isAnswered = true;

        if (correct)
        {
            // — correct answer flow —
            answerText.color = Color.white;
            button.image.color = Color.green;

            GameManager.instance.Score++;
            GameManager.instance.ScoreUpdater();
        }
        else
        {
            // — wrong answer flow —
            answerText.color = Color.white;
            button.image.color = Color.red;

            // highlight the right one in green
            for (int i = 0; i < GameSixGamePlay_PanelUI.instance.optionButtons_Ref.Length; i++)
            {
                var btnRef = GameSixGamePlay_PanelUI.instance.optionButtons_Ref[i];
                if (btnRef.GetComponentInChildren<TextMeshProUGUI>().text
                    == GameSixManager.instance.currentLevel.correctAnswer)
                {
                    btnRef.image.color = Color.green;
                }
            }
        }

        // 5) Wait and move on
        StartCoroutine(WaitForNextQuestion());
    }

    public IEnumerator WaitForNextQuestion()
    {
        yield return new WaitForSeconds(1f);
        GameSixManager.instance.currentLevel = null;
        GameSixGamePlay_PanelUI.instance.NextLevel();
    }
}

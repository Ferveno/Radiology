using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AnswerButton : MonoBehaviour
{
    UnityEngine.UI.Button button;
    private TextMeshProUGUI answerText;

    private void Start()
    {
        button = GetComponent<Button>();
        answerText = GetComponentInChildren<TextMeshProUGUI>();
        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        StartCoroutine(ProcessAnswer());
    }

    private IEnumerator ProcessAnswer()
    {
        var ui = GameThreeGamePlay_PanelUI.instance;
        var mgr = GameThreeManager.instance;

        // 1. Disable all buttons and cancel timer
        foreach (var btn in ui.optionButtons_Ref)
            btn.interactable = false;
        ui.CancelTimer();

        // 2. Determine correctness & mark as answered
        bool isCorrect = answerText.text == mgr.currentLevel.correctAnswer;
        mgr.currentLevel.isAnswered = true;
        int idx = mgr.allLevels.IndexOf(mgr.currentLevel);
        //PlayerPrefs.SetInt("CheckStatus" + idx, 1);
        //PlayerPrefs.Save();

        // 3. Color the chosen button
        answerText.color = Color.white;
        button.image.color = isCorrect ? Color.green : Color.red;

        // 4. If wrong, also highlight the correct button in green
        if (!isCorrect)
        {
            foreach (var btn in ui.optionButtons_Ref)
            {
                var txt = btn.GetComponentInChildren<TextMeshProUGUI>();
                if (txt != null && txt.text == mgr.currentLevel.correctAnswer)
                {
                    txt.color = Color.white;
                    btn.image.color = Color.green;
                    break;
                }
            }
        }
        else
        {
            // award points only on correct
            GameManager.instance.Score++;
            GameManager.instance.ScoreUpdater();
        }

        // 5. Short delay so players see the color feedback
        yield return new WaitForSeconds(0.5f);

        // 6. Show explanation panel
        ui.questionImage.sprite = mgr.currentLevel.correctAnswerSprite;
        ui.correctAnswerPanel.SetActive(true);

        // 7. Keep panel up for your existing delays
        yield return new WaitForSeconds(2f);
        yield return new WaitForSeconds(7f);

        // 8. Hide panel and advance
        ui.correctAnswerPanel.SetActive(false);
        ui.NextLevel();
    }
}

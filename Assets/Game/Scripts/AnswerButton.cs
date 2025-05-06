using TMPro;
using UnityEngine;
using System.Collections;

public class AnswerButtonGameThree : MonoBehaviour
{
    UnityEngine.UI.Button button;

    private void Start()
    {
        button = GetComponent<UnityEngine.UI.Button>();
        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
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

                        //PlayerPrefs.SetInt("CheckStatus" + i, 1);
                        //GameThreeGamePlay_PanelUI.instance.UpdatePlayerLevel();


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
        for(int i = 0; i < GameThreeGamePlay_PanelUI.instance.optionButtons_Ref.Length; i++)
        {
            if (GameThreeGamePlay_PanelUI.instance.optionButtons_Ref[i].GetComponentInChildren<TextMeshProUGUI>().text == GameThreeManager.instance.currentLevel.correctAnswer)
            {
                GameThreeGamePlay_PanelUI.instance.optionButtons_Ref[i].image.color = Color.green;
            }
        }

        StartCoroutine(WaitForNextQuestion());
    }

     public IEnumerator WaitForAnswerDescription()
    {
        GameThreeGamePlay_PanelUI.instance.questionImage.sprite = GameThreeManager.instance.currentLevel.correctAnswerSprite;
        GameThreeGamePlay_PanelUI.instance.correctAnswerPanel.gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        GameThreeGamePlay_PanelUI.instance.correctAnswerPanel.gameObject.SetActive(true);
        yield return new WaitForSeconds(10);
        GameThreeGamePlay_PanelUI.instance.correctAnswerPanel.gameObject.SetActive(false);
        GameThreeGamePlay_PanelUI.instance.NextLevel();
    }

      public IEnumerator WaitForNextQuestion()
    {
        GameThreeGamePlay_PanelUI.instance.optionButtons.gameObject.SetActive(true);
        GameThreeGamePlay_PanelUI.instance.correctAnswerPanel.gameObject.SetActive(false);
        yield return new WaitForSeconds(3);
        GameThreeManager.instance.currentLevel=null;
        yield return new WaitForSeconds(1);
        GameThreeGamePlay_PanelUI.instance.NextLevel();
    }
    
}
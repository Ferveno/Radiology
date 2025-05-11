using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameSixManager : MonoBehaviour
{
    public static GameSixManager instance;
    public List<GameSixLevel> allLevels;
    public GameSixLevel currentLevel;

    private int currentIndex = 0; // Track the current question index

    public GameObject MainGameCanvas;

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
        //CheckStatusOfIsAnswered();
    }

    public void FindNextLevel()
    {
        currentLevel = GetNextUnansweredLevel();
        if (currentLevel != null)
        {
            Debug.Log("Displaying question: " + currentIndex);
            GameSixGamePlay_PanelUI.instance.question_Text.text = currentLevel.question;
            GameSixGamePlay_PanelUI.instance.option_1.text = currentLevel.option1;
            GameSixGamePlay_PanelUI.instance.option_2.text = currentLevel.option2;
            GameSixGamePlay_PanelUI.instance.option_3.text = currentLevel.option3;
            GameSixGamePlay_PanelUI.instance.option_4.text = currentLevel.option4;

            currentIndex++; // Move to the next question for the next call
        }
        else
        {
            Debug.Log("No more unanswered questions.");
            MainGameCanvas.SetActive(false);
            GameManager.instance.OnGame6Complete();
        }
    }

    public GameSixLevel GetNextUnansweredLevel()
    {
        List<GameSixLevel> unansweredLevels = allLevels.Where(level => !level.isAnswered).ToList();
        if (unansweredLevels.Count > 0)
        {
            return unansweredLevels[Random.Range(0, unansweredLevels.Count)];
        }
        return null; // No unanswered questions left
    }

    public void CheckStatusOfIsAnswered()
    {
        for (int i = 0; i < allLevels.Count; i++)
        {
            if (PlayerPrefs.GetInt("CheckStatus" + i) == 1)
            {
                allLevels[i].isAnswered = true;
            }
        }
    }
}

[System.Serializable]
public class GameSixLevel
{
    public string question;
    public string option1;
    public string option2;
    public string option3;
    public string option4;
    public string correctAnswer;
    public bool isAnswered;
}

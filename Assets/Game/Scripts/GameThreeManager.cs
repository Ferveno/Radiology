using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;

public class GameThreeManager : MonoBehaviour
{
    public static GameThreeManager instance;
    public List<Level> allLevels;
    public Level currentLevel;
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

        CheckStatusOfIsAnswered();
    }

    public void FindNextLevel()
    {
        currentLevel = GetNextUnansweredLevel();
        if (currentLevel != null)
        {
            DisplayLevel(currentLevel);
        }
        else
        {
            Debug.Log("No more unanswered questions.");
            MainGameCanvas.SetActive(false);
            GameManager.instance.OnGame3Complete();
        }
    }

    private void DisplayLevel(Level level)
    {
        Debug.Log("Displaying question: " + level.question);
        GameThreeGamePlay_PanelUI.instance.question_Text.text = level.question;
        GameThreeGamePlay_PanelUI.instance.questionImage.sprite = level.modeSprite;
        GameThreeGamePlay_PanelUI.instance.option_1.text = level.option1;
        GameThreeGamePlay_PanelUI.instance.option_2.text = level.option2;
        GameThreeGamePlay_PanelUI.instance.option_3.text = level.option3;
        GameThreeGamePlay_PanelUI.instance.option_4.text = level.option4;
        GameThreeGamePlay_PanelUI.instance.correctAnswerDescription_Text.text = level.correctAnswerDescription;
    }

    public Level GetNextUnansweredLevel()
    {
        List<Level> unansweredLevels = allLevels.Where(level => !level.isAnswered).ToList();
        if (unansweredLevels.Count > 0)
        {
            return unansweredLevels[Random.Range(0, unansweredLevels.Count)];
        }
        return null; // No unanswered questions left
    }

    public void CheckAnswer(string selectedOption)
    {
        if (currentLevel != null)
        {
            if (selectedOption == currentLevel.correctAnswer)
            {
                currentLevel.isAnswered = true;
                PlayerPrefs.SetInt("CheckStatus" + allLevels.IndexOf(currentLevel), 1);
                PlayerPrefs.Save();
            }

            FindNextLevel();
        }
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
public class Level
{
    public Sprite modeSprite;
    public string question;
    public string option1;
    public string option2;
    public string option3;
    public string option4;
    public string correctAnswer;
    public bool isAnswered;
    public Sprite correctAnswerSprite;
    public string correctAnswerDescription;
}
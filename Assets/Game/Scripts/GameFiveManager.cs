using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFiveManager : MonoBehaviour
{
   public static GameFiveManager instance;
    public List<Level> allLevels;
    public Level currentLevel;
    
    private int currentIndex = 0; // Track the current question index

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
        
        currentLevel = GetNextQuestion();
        if (currentLevel != null)
        {
            Debug.Log("Displaying question: " + currentIndex);
            GameFiveGamePlay_PanelUI.instance.question_Text.text = currentLevel.question;
            GameFiveGamePlay_PanelUI.instance.questionImage.sprite = currentLevel.modeSprite;
            GameFiveGamePlay_PanelUI.instance.option_1.text = currentLevel.option1;
            GameFiveGamePlay_PanelUI.instance.option_2.text = currentLevel.option2;
            GameFiveGamePlay_PanelUI.instance.option_3.text = currentLevel.option3;
            GameFiveGamePlay_PanelUI.instance.option_4.text = currentLevel.option4;
            
            currentIndex++; // Move to the next question for the next call
        }
        else
        {
            Debug.Log("No more unanswered questions.");
        }
    }

    public Level GetNextQuestion()
    {
        while (currentIndex < allLevels.Count)
        {
            if (!allLevels[currentIndex].isAnswered)
            {
                return allLevels[currentIndex];
            }
            currentIndex++;
        }
        return null; // No unanswered questions left
    }

     public void CheckStatusOfIsAnswered()
    {
        for(int i = 0; i < allLevels.Count; i++)
        {
            if (PlayerPrefs.GetInt("CheckStatus"+i) == 1)
            {
                allLevels[i].isAnswered = true;
            }
        }
    }
   
}

[System.Serializable]
public class GameFiveLevel
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


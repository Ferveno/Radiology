using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameFiveGamePlay_PanelUI : MonoBehaviour
{
    public static GameFiveGamePlay_PanelUI instance;

    public Image questionImage;
    public TextMeshProUGUI option_1;
    public TextMeshProUGUI option_2;
    public TextMeshProUGUI option_3;
    public TextMeshProUGUI option_4;

    public TextMeshProUGUI question_Text;   


    public Button[] optionButtons_Ref;
    public GameObject optionButtons;

    [SerializeField] public int playerLevel=1;

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
       
    }

    void Start()
    {
        //playerLevel = PlayerPrefs.GetInt("PlayerLevel", 1);
        NextLevel();
    }


    public void UpdatePlayerLevel()
    {
        playerLevel += 1;
        PlayerPrefs.SetInt("PlayerLevel", playerLevel);
    }

    public void NextLevel()
    {
        if (playerLevel == GameThreeManager.instance.allLevels.Count - 1)
        {
            // UIManager.instance.allLevelsCompeletePanel_Ref.SetActive(true);
        }
 
        foreach (Button button in optionButtons_Ref)
        {
            button.interactable = false;
        }

        foreach (Button button in optionButtons_Ref)
        {
            button.interactable = true;
            button.image.color = Color.white;
            button.GetComponentInChildren<TextMeshProUGUI>().color = Color.black;
        }
        
        GameThreeManager.instance.FindNextLevel();
    }
}

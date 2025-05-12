using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int Score = 0;

    public GameObject JoyStickCanvas;

    public GameObject Game1Key;
    public GameObject Game2Key;
    public GameObject Game3Key;
    public GameObject Game4Key;
    public GameObject Game5Key;
    public GameObject Game6Key;


    public GameObject Game1;
    public GameObject Game2;
    public GameObject Game3;
    public GameObject Game4;
    public GameObject Game5;
    public GameObject Game6;


    public GameObject Room2Hurldes;
    public GameObject Room3Hurldes;
    public GameObject Room4Hurldes;
    public GameObject Room5Hurldes;
    public GameObject Room6Hurldes;

    public GameObject Game2Camera;
    public GameObject PlayerCamera;


    public TextMeshProUGUI ScoreText;
    public GameObject MainMenuPanel;
    public GameObject HelpPanel;
    public GameObject HelpSubpanel1;
    public GameObject HelpSubpanel2;

    public GameObject GameOverPanel;
    public Slider ResultScoreBar;
    public TextMeshProUGUI finalScoreText;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        JoyStickCanvas.SetActive(false);
    }

    public void ScoreUpdater() {
        ScoreText.text = "Score:" + Score;
    }

    public void OnGame1Start()
    {
        JoyStickCanvas.SetActive(false);
        Game1.SetActive(true);
        Game1Key.SetActive(false);
        ScoreText.rectTransform.anchoredPosition = new Vector3(735f, 0f, 0f);
    }


    public void OnGame1Complete() {
        Game1.SetActive(false);
        Room2Hurldes.SetActive(false);
        JoyStickCanvas.SetActive(true);
        ScoreText.rectTransform.anchoredPosition = new Vector3(0f, 0f, 0f);
    }


    public void OnGame2Start()
    {
        JoyStickCanvas.SetActive(false);
        PlayerCamera.SetActive(false);
        Game2Camera.SetActive(true);
        Game2.SetActive(true);
        Game2Key.SetActive(false);
        ScoreText.rectTransform.anchoredPosition = new Vector3(735f, 0f, 0f);
    }
    public void OnGame2Complete()
    {
        Room3Hurldes.SetActive(false);
        JoyStickCanvas.SetActive(true);
        PlayerCamera.SetActive(true);
        Game2Camera.SetActive(false);
        Game2.SetActive(false);
        ScoreText.rectTransform.anchoredPosition = new Vector3(0f, 0f, 0f);
    }

    public void OnGame3Start()
    {
        JoyStickCanvas.SetActive(false);
        Game3.SetActive(true);
        Game3Key.SetActive(false);
        ScoreText.rectTransform.anchoredPosition = new Vector3(180f, -960f, 0f);
    }


    public void OnGame3Complete()
    {
        Game3.SetActive(false);
        Room4Hurldes.SetActive(false);
        JoyStickCanvas.SetActive(true);
        ScoreText.rectTransform.anchoredPosition = new Vector3(0f, 0f, 0f);
    }

    public void OnGame4Start()
    {
        JoyStickCanvas.SetActive(false);

        PlayerCamera.SetActive(false);
        Game2Camera.SetActive(true);

        Game4.SetActive(true);
        Game4Key.SetActive(false);
        ScoreText.rectTransform.anchoredPosition = new Vector3(730f, -980f, 0f);
    }


    public void OnGame4Complete()
    {
        Game4.SetActive(false);

        PlayerCamera.SetActive(true);
        Game2Camera.SetActive(false);

        Room5Hurldes.SetActive(false);
        JoyStickCanvas.SetActive(true);
        ScoreText.rectTransform.anchoredPosition = new Vector3(0f, 0f, 0f);
    }

    public void OnGame5Start()
    {
        JoyStickCanvas.SetActive(false);

        PlayerCamera.SetActive(false);
        Game2Camera.SetActive(true);

        Game5.SetActive(true);
        Game5Key.SetActive(false);
        ScoreText.rectTransform.anchoredPosition = new Vector3(730f, -980f, 0f);
    }


    public void OnGame5Complete()
    {
        Game5.SetActive(false);

        PlayerCamera.SetActive(true);
        Game2Camera.SetActive(false);

        Room6Hurldes.SetActive(false);
        JoyStickCanvas.SetActive(true);
        ScoreText.rectTransform.anchoredPosition = new Vector3(0f, 0f, 0f);
    }

    public void OnGame6Start()
    {
        JoyStickCanvas.SetActive(false);

        PlayerCamera.SetActive(false);
        Game2Camera.SetActive(true);

        Game6.SetActive(true);
        Game6Key.SetActive(false);
        ScoreText.rectTransform.anchoredPosition = new Vector3(730f, -980f, 0f);

    }


    public void OnGame6Complete()
    {
        Game6.SetActive(false);

        PlayerCamera.SetActive(true);
        Game2Camera.SetActive(false);

        JoyStickCanvas.SetActive(true);
        ScoreText.rectTransform.anchoredPosition = new Vector3(0f, 0f, 0f);
    }

    /// <summary>
    /// Main Menu and Help Panel Helper FUnctions
    /// </summary>

    public void OnClickStartButton() {
        HelpPanel.SetActive(false);
        MainMenuPanel.SetActive(false);
        JoyStickCanvas.SetActive(true);
    }

    public void OnClickHelpButton() { 
        JoyStickCanvas.SetActive(false);
        HelpPanel.SetActive(true);
        HelpSubpanel1.SetActive(true);
        JoyStickCanvas.SetActive(false);
    }

    public void OnClickHelpPanelNextButton() { 
        HelpSubpanel1.SetActive(false);
        HelpSubpanel2.SetActive(true);
    }

    public void OnClickHelpPanelGotitButton()
    {
        HelpSubpanel1.SetActive(false);
        HelpSubpanel2.SetActive(false);
        HelpPanel.SetActive(false);
    }

    public void OnClickQuitButton() {
        Application.Quit();
    }

    public void OnGameOver()
    {
        GameOverPanel.SetActive(true);

        float duration = 1.5f;
        int finalScore = Score;    // your score variable
        Ease easeType = Ease.OutCubic;

        // grab the Image component on the fill
        Image fillImage = ResultScoreBar.fillRect.GetComponent<Image>();

        // reset slider & text
        ResultScoreBar.value = 0;
        finalScoreText.text = "0";
        fillImage.color = Color.red; // start color

        // 1) Slider tween with color thresholds and end punch
        ResultScoreBar
            .DOValue(finalScore, duration)
            .SetEase(easeType)
            .OnUpdate(() =>
            {
                float v = ResultScoreBar.value;
                if (v < 30f) fillImage.color = Color.red;
                else if (v < 60f) fillImage.color = Color.yellow;
                else fillImage.color = Color.green;
            })
            .OnComplete(() =>
            {
                // punch when bar finishes
                ResultScoreBar.fillRect
                    .DOPunchScale(Vector3.one * 0.05f, 0.3f, 3, 1f);
            });

        // 2) Text count-up (unchanged)
        int current = 0;
        DOTween
            .To(() => current, x =>
            {
                current = x;
                finalScoreText.text = "Your Score: " + x.ToString() + "/98";
            }, finalScore, duration)
            .SetEase(easeType);
    }


}

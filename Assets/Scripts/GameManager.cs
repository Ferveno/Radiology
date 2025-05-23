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
    public GameObject HelpSubpanel3;

    int MaxPossibleScore = 98;
    public GameObject GameOverPanel;
    public Slider ResultScoreBar;
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI feedbackText;

    public GameObject PlayerCam;
    public GameObject PlayerVirtualCam;
    public GameObject Game2Cam;

    public GameObject ScoreHolder;

    public GameObject Player;

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
        ScoreHolder.SetActive(false);
        Player.gameObject.tag = "Untagged";
    }


    public void OnGame1Complete() {
        Game1.SetActive(false);
        Room2Hurldes.SetActive(false);
        JoyStickCanvas.SetActive(true);
        ScoreText.rectTransform.anchoredPosition = new Vector3(0f, 0f, 0f);
        ScoreHolder.SetActive(true);
        Player.gameObject.tag = "Player";

    }


    public void OnGame2Start()
    {
        JoyStickCanvas.SetActive(false);
        PlayerCamera.SetActive(false);
        Game2Camera.SetActive(true);
        Game2.SetActive(true);
        Game2Key.SetActive(false);
        ScoreText.rectTransform.anchoredPosition = new Vector3(735f, 0f, 0f);
        ScoreHolder.SetActive(false);
        Player.gameObject.tag = "Untagged";
    }
    public void OnGame2Complete()
    {
        Room3Hurldes.SetActive(false);
        JoyStickCanvas.SetActive(true);
        PlayerCamera.SetActive(true);
        Game2Camera.SetActive(false);
        Game2.SetActive(false);
        ScoreText.rectTransform.anchoredPosition = new Vector3(0f, 0f, 0f);
        ScoreHolder.SetActive(true);
        Player.gameObject.tag = "Player";
    }

    public void OnGame3Start()
    {
        JoyStickCanvas.SetActive(false);
        Game3.SetActive(true);
        Game3Key.SetActive(false);
        ScoreText.rectTransform.anchoredPosition = new Vector3(180f, -960f, 0f);
        ScoreHolder.SetActive(false);
        Player.gameObject.tag = "Untagged";
    }


    public void OnGame3Complete()
    {
        Game3.SetActive(false);
        Room4Hurldes.SetActive(false);
        JoyStickCanvas.SetActive(true);
        ScoreText.rectTransform.anchoredPosition = new Vector3(0f, 0f, 0f);
        ScoreHolder.SetActive(true);
        Player.gameObject.tag = "Player";
    }

    public void OnGame4Start()
    {
        JoyStickCanvas.SetActive(false);

        PlayerCamera.SetActive(false);
        Game2Camera.SetActive(true);

        Game4.SetActive(true);
        Game4Key.SetActive(false);
        ScoreText.rectTransform.anchoredPosition = new Vector3(730f, -980f, 0f);
        ScoreHolder.SetActive(false);
        Player.gameObject.tag = "Untagged";
    }


    public void OnGame4Complete()
    {
        Game4.SetActive(false);

        PlayerCamera.SetActive(true);
        Game2Camera.SetActive(false);

        Room5Hurldes.SetActive(false);
        JoyStickCanvas.SetActive(true);
        ScoreText.rectTransform.anchoredPosition = new Vector3(0f, 0f, 0f);
        ScoreHolder.SetActive(true);
        Player.gameObject.tag = "Player";
    }

    public void OnGame5Start()
    {
        JoyStickCanvas.SetActive(false);

        PlayerCamera.SetActive(false);
        Game2Camera.SetActive(true);

        Game5.SetActive(true);
        Game5Key.SetActive(false);
        ScoreText.rectTransform.anchoredPosition = new Vector3(730f, -980f, 0f);
        ScoreHolder.SetActive(false);
        Player.gameObject.tag = "Untagged";
    }


    public void OnGame5Complete()
    {
        Game5.SetActive(false);

        PlayerCamera.SetActive(true);
        Game2Camera.SetActive(false);

        Room6Hurldes.SetActive(false);
        JoyStickCanvas.SetActive(true);
        ScoreText.rectTransform.anchoredPosition = new Vector3(0f, 0f, 0f);
        ScoreHolder.SetActive(true);
        Player.gameObject.tag = "Player";
    }

    public void OnGame6Start()
    {
        JoyStickCanvas.SetActive(false);

        PlayerCamera.SetActive(false);
        Game2Camera.SetActive(true);

        Game6.SetActive(true);
        Game6Key.SetActive(false);
        ScoreText.rectTransform.anchoredPosition = new Vector3(730f, -980f, 0f);
        ScoreHolder.SetActive(false);
        Player.gameObject.tag = "Untagged";
    }


    public void OnGame6Complete()
    {
        Game6.SetActive(false);

       // PlayerCamera.SetActive(true);
        Game2Camera.SetActive(true);

        //JoyStickCanvas.SetActive(true);
        ScoreText.rectTransform.anchoredPosition = new Vector3(0f, 0f, 0f);
        ScoreHolder.SetActive(true);
        Player.gameObject.tag = "Player";

        OnGameOver();
    }

    /// <summary>
    /// Main Menu and Help Panel Helper FUnctions
    /// </summary>

    public void OnClickStartButton() {

        PlayerCam.SetActive(true);
        PlayerVirtualCam.SetActive(true);

        Game2Cam.SetActive(false);

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

    public void OnClickHelpPanel1NextButton() { 
        HelpSubpanel1.SetActive(false);
        HelpSubpanel2.SetActive(true);
        HelpSubpanel3.SetActive(false);
    }

    public void OnClickHelpPanel2NextButton()
    {
        HelpSubpanel1.SetActive(false);
        HelpSubpanel2.SetActive(false);
        HelpSubpanel3.SetActive(true);
    }

    public void OnClickHelpPanelGotitButton()
    {
        HelpSubpanel1.SetActive(false);
        HelpSubpanel3.SetActive(false);
        HelpPanel.SetActive(false);
    }

    public void OnClickQuitButton() {
        Application.Quit();
    }

    public void OnGameOver()
    {
        GameOverPanel.SetActive(true);
        feedbackText.gameObject.SetActive(false);      // hide feedback until ready

        float duration = 1.5f;
        int finalScore = Score;                      // your runtime score
        Ease easeType = Ease.OutCubic;

        // grab the Image component on the fill
        Image fillImage = ResultScoreBar.fillRect.GetComponent<Image>();

        // initial states
        ResultScoreBar.value = 0;
        finalScoreText.text = "Your Score: 0/" + MaxPossibleScore;
        fillImage.color = Color.red;

        // build a single Sequence
        var seq = DOTween.Sequence();

        // 1) Animate the bar fill
        seq.Append(
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
        );

        // 2) In parallel, animate the score text count-up
        seq.Join(
            DOTween.To(() => 0, x =>
            {
                finalScoreText.text = $"Your Score: {x}/{MaxPossibleScore}";
            }, finalScore, duration)
            .SetEase(easeType)
        );

        // 3) When *both* tweens finish�
        seq.OnComplete(() =>
        {
            // a) punch the bar for a little bounce
            ResultScoreBar.fillRect
                .DOPunchScale(Vector3.one * 0.05f, 0.3f, 3, 1f);

            // b) decide feedback text and color
            string msg;
            Color col;
            if (finalScore < 30) { msg = "You can do a lot better"; col = Color.red; }
            else if (finalScore < 60) { msg = "Satisfactory"; col = Color.yellow; }
            else { msg = "You did great!"; col = Color.green; }

            feedbackText.text = msg;
            feedbackText.color = col;
            feedbackText.transform.localScale = Vector3.zero;
            feedbackText.gameObject.SetActive(true);

            // c) animate feedback popping in
            feedbackText.transform
                .DOScale(1f, 0.5f)
                .SetEase(Ease.OutBack);
        });

        // 4) start it
        seq.Play();
    }



}

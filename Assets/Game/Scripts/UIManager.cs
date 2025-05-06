using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public GameObject mainMenuPanel;
    public GameObject settingsPanel;
    public GameObject inventoryPanel;
    public GameObject inAppPurchasePanel;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(Instance);
        }
        
        if (PlayerPrefs.GetInt("GamePlayed") >= 2)
        {
            PlayerPrefs.SetInt("GamePlayed", 0);
           // AdManager_Admob.instance.ShowInterstitialAd();
        }
    }

    private void Start()
    {
        // GameManager.Instance.UpdateCoins();
        StartCoroutine(SoundManager.instance.ChangeBGMusicByFade(SoundManager.instance.menu_Music));
    }

    public void OnClick_StartButton()
    {
        Time.timeScale = 1f;

            GameSceneManager.Instance.StartLoadingScene("World 1", "LoadingIn");
        //if (PlayerPrefs.GetInt("PlayerLevel", 1) == 1)
        //{
        //    GameSceneManager.Instance.StartLoadingScene("World 1", "LoadingIn");
        //}
        //else if (PlayerPrefs.GetInt("PlayerLevel", 1) == 2)
        //{
        //    GameSceneManager.Instance.StartLoadingScene("World 2","LoadingIn");
        //}
        //else if (PlayerPrefs.GetInt("PlayerLevel", 1) == 3)
        //{
        //    GameSceneManager.Instance.StartLoadingScene("World 2","LoadingIn");
        //}
    }


    public void OnClick_QuitButton()
    {
        Application.Quit();
    }

    public void OnClick_SettingsPanel()
    {
        settingsPanel.SetActive(true);
    }

    public void OnClick_InventoryPanel()
    {
        //InventoryPanel_UI.ShowUI();
        inventoryPanel.SetActive(true);
    }

    public void OnClick_InAppPurchasePanel()
    {
        inAppPurchasePanel.SetActive(true);
    }
}

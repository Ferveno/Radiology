using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverPanel_UI : MonoBehaviour
{
    public static GameOverPanel_UI instance;
    public Button restartButton;

    public GameObject player;
    public FlameThrower flameThrower;
    public PlayerHealth playerHealth;
    int addWatchedCount = 0;
    private void Start()
    {
        //StartCoroutine(SoundManager.instance.ChangeBGMusicByFade(SoundManager.instance.gameOver_Music));
        

        restartButton.onClick.AddListener(OnClick_RestartGameButton);
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
        flameThrower = FindObjectOfType<FlameThrower>();


        if (PlayerPrefs.GetInt("RewardedAdWatched") == 2)
        {
            restartButton.gameObject.SetActive(false);
            PlayerPrefs.SetInt("RewardedAdWatched", 0);
        }
    }

    public static GameOverPanel_UI ShowUI()
    {

        if (instance == null)
        {
            GameObject obj = Instantiate(Resources.Load("Prefabs/GameOverPanel_UI")) as GameObject;
            Canvas[] cans = GameObject.FindObjectsOfType<Canvas>() as Canvas[];
            for (int i = 0; i < cans.Length; i++)
            {
                if (cans[i].gameObject.activeInHierarchy && cans[i].gameObject.tag.Equals("mainCanvas"))
                {
                    obj.transform.SetParent(cans[i].transform, false);
                    break;
                }
            }
            instance = obj.GetComponent<GameOverPanel_UI>();
        }

        return instance;
    }

    public void OnBackPressed()
    {
        Destroy(gameObject);
    }

    public void OnClick_MainMenuButton()
    {
        Time.timeScale = 1f;
        GameSceneManager.Instance.StartLoadingScene("MainMenu","LoadingOut");
        PlayerPrefs.SetInt("GamePlayed", PlayerPrefs.GetInt("GamePlayed") + 1);
    }

    public void OnClick_RestartGameButton()
    {
        PlayerPrefs.SetInt("RewardedAdWatched", PlayerPrefs.GetInt("RewardedAdWatched")+1);

        //if (AdManager_Admob.instance != null && AdManager_Admob.instance.IsAdsEnabled)
        //{
        //    AdManager_Admob.instance.ShowRewardedAd(OnRewardedAdCompleted);
        //}
        //else
        //{
        //}
            OnRewardedAdCompleted();
    }

    private void OnRewardedAdCompleted()
    {

        Debug.Log("Rewarded ad watched. Continue the game.");
        //SceneManager.LoadScene("MainMenu");
        playerHealth.currentHealth = playerHealth.maxHealth;
        playerHealth.UpdateHealthUI();
        flameThrower.gameObject.SetActive(true);
        player.gameObject.SetActive(true);



        Time.timeScale = 1f;
        Destroy(gameObject);
    }
}

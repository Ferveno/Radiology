using UnityEngine;
using UnityEngine.SceneManagement;

public class GameWinPanel_UI : MonoBehaviour
{
    public static GameWinPanel_UI instance;
    public GameObject nextLevelButton_ref;
    private void Start()
    {
        //StartCoroutine(SoundManager.instance.ChangeBGMusicByFade(SoundManager.instance.gameWin_Music));
        Time.timeScale = 0f;

        if (SceneManager.GetActiveScene().name.Equals("World 3"))
        {
            nextLevelButton_ref.SetActive(false);
        }
    }

    public static GameWinPanel_UI ShowUI()
    {

        if (instance == null)
        {
            GameObject obj = Instantiate(Resources.Load("Prefabs/GameWinPanel_UI")) as GameObject;
            Canvas[] cans = GameObject.FindObjectsOfType<Canvas>() as Canvas[];
            for (int i = 0; i < cans.Length; i++)
            {
                if (cans[i].gameObject.activeInHierarchy && cans[i].gameObject.tag.Equals("mainCanvas"))
                {
                    obj.transform.SetParent(cans[i].transform, false);
                    break;
                }
            }
            instance = obj.GetComponent<GameWinPanel_UI>();
        }

        return instance;
    }

    public void OnBackPressed()
    {
        Destroy(gameObject);
    }

    public void OnClick_HomeButton()
    {
        Time.timeScale = 1f;
        GameSceneManager.Instance.StartLoadingScene("MainMenu", "LoadingOut");
    }

    public void OnClick_NextLevel()
    {
        Time.timeScale = 1f;

        if (SceneManager.GetActiveScene().name.Equals("World 1"))
        {
            PlayerPrefs.SetInt("PlayerLevel", 2);
            GameSceneManager.Instance.StartLoadingScene("World 2", "LoadingIn");
        }
        else if (SceneManager.GetActiveScene().name.Equals("World 2"))
        {
            PlayerPrefs.SetInt("PlayerLevel", 3);
            GameSceneManager.Instance.StartLoadingScene("World 3", "LoadingIn");
        }
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    public static GameSceneManager Instance; // Singleton instance
    public Slider loadingBar; // Assign the slider in the Inspector
    public Text loadingText; // Assign the text in the Inspector
    public float minimumLoadingTime = 2f; // Minimum loading time in seconds

    private string nextScene; // Next scene to load
    private string loadingScene; // Current loading scene

    private void Awake()
    {
        // Implement Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        StartLoadingScene("MainMenu", "LoadingIn");
    }

    // Method to start loading a new scene with a specified loading scene
    public void StartLoadingScene(string sceneName, string loadingSceneName)
    {
        nextScene = sceneName;
        loadingScene = loadingSceneName;
        StartCoroutine(LoadSceneAsync());
    }

    // Coroutine to load a new scene asynchronously
    private IEnumerator LoadSceneAsync()
    {
        // Load the specified loading scene
        AsyncOperation loadingSceneOperation = SceneManager.LoadSceneAsync(loadingScene);

        // Wait until the loading scene is fully loaded
        while (!loadingSceneOperation.isDone)
        {
            yield return null;
        }

        // Find the loading bar and text in the loaded scene
        AssignLoadingScreenElements();

        // Start loading the actual target scene
        AsyncOperation actualSceneOperation = SceneManager.LoadSceneAsync(nextScene);

        // Disable the automatic scene activation
        actualSceneOperation.allowSceneActivation = false;

        // Timer for minimum loading time
        float elapsedLoadingTime = 0f;

        // Update the loading bar and text
        while (!actualSceneOperation.isDone)
        {
            float progress = Mathf.Clamp01(actualSceneOperation.progress / 0.9f);
            if (loadingBar != null) loadingBar.value = progress;
            //if (loadingText != null) loadingText.text = $"Loading... {progress * 100}%";

            // Increase elapsed time
            elapsedLoadingTime += Time.deltaTime;

            // Activate the scene when fully loaded and minimum loading time has passed
            if (actualSceneOperation.progress >= 0.9f && elapsedLoadingTime >= minimumLoadingTime)
            {
                actualSceneOperation.allowSceneActivation = true;
            }

            yield return null;
        }
    }

    // Method to find and assign the loading bar and text references
    private void AssignLoadingScreenElements()
    {
        loadingBar = FindObjectOfType<Slider>();
        //loadingText = FindObjectOfType<Text>();
    }
}

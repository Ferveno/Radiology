using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadingScreenController : MonoBehaviour
{
    public Slider loadingBar; // Assign the slider in the Inspector
    public Text loadingText; // Assign the text in the Inspector

    // Method to start loading the actual scene
    public void StartLoadingScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    // Coroutine to load the actual scene asynchronously
    private IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        // Disable the automatic scene activation
        operation.allowSceneActivation = false;

        // Update the loading bar and text
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            //if (loadingBar != null) loadingBar.value = progress;
            //if (loadingText != null) loadingText.text = $"Loading... {progress * 100}%";

            // Activate the scene when fully loaded
            if (operation.progress >= 0.9f)
            {
                operation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}

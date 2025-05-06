using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private Slider loadingBar;
    [SerializeField] private float timeForLoadingGame;
    [SerializeField] private float timerForLoading;


    void Start()
    {
        timerForLoading = timeForLoadingGame;
    }

    void Update()
    {
        if (timerForLoading > 0 )
        {
            timerForLoading -= Time.deltaTime;
            float fillAmount = 1 - (timerForLoading / timeForLoadingGame);
            loadingBar.value = fillAmount;
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }
}

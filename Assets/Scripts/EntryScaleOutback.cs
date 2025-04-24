using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class EntryScaleOutback : MonoBehaviour
{
    public Vector3 StartScale = new Vector3(0f,0f,0f);
    public float Delay = .1f;
    public float Duration = .4f;
    public bool PlayOnEnable = false;
    public bool IgnoreTimeScale = true;
    private Sequence sequence;
    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Scale()
    {
        rectTransform = GetComponent<RectTransform>();
        rectTransform.localScale = StartScale;
        sequence = DOTween.Sequence();
        sequence.Append(rectTransform.DOScale(1, Duration).SetDelay(Delay).SetEase(Ease.OutBack)).SetUpdate(IgnoreTimeScale);
    }

    private void OnEnable()
    {
        if (PlayOnEnable)
        {
            sequence?.Kill();
            Scale();
        }
    }

    public void Hide(Action onComplete = null)
    {
        sequence?.Kill();
        sequence?.Append(rectTransform.DOScale(0, .3f).SetEase(Ease.InBack))
            .OnComplete(() =>
            {
                onComplete?.Invoke();
            });
        
    }

    public void CompleteSequence()
    {
        sequence?.Complete();
    }
}

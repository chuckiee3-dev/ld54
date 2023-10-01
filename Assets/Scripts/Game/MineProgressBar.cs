using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MineProgressBar : MonoBehaviour
{
    public bool isInUse;
    [SerializeField] private Image fillImage;
    [SerializeField] private CanvasGroup canvasGroup;

    private void Start()
    {
        SetInvisible();
    }

    public void FillInSeconds(float time)
    {
        if (isInUse)
        {
            return;
        }
        SetVisible();
        isInUse = true;
        float progress = 0;
        DOTween.To(() => progress, x => progress = x, 1, time)
            .OnUpdate(() => { fillImage.fillAmount = progress;}).OnComplete((() =>
            {
                isInUse = false;
                SetInvisible();
            }));
    }

    public void SetVisible()
    {
        canvasGroup.alpha = 1;
    }

    public void SetInvisible()
    {
        canvasGroup.alpha = 0;
    }
}
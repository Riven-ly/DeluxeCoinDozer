using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NetworkCheckerPanel : UIBase
{
    public Button retryBtn;
    public CanvasGroup canvasGroup;

    void Start()
    {
        retryBtn.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayBtnMusic();
            retryBtn.interactable = false;
            canvasGroup.alpha = 0.5f;
            NetworkChecker.Instance.CheckNetworkManually();
            DOTween.Sequence().AppendInterval(2f).AppendCallback(() =>
            {
                retryBtn.interactable = true;
                canvasGroup.alpha = 1f;
            });
        });
    }

    public override void Refresh(object data = null)
    {
        base.Refresh(data);
        retryBtn.interactable = true;
        canvasGroup.alpha = 1f;
    }

    public override void Hide()
    {
        base.Hide();
    }

}

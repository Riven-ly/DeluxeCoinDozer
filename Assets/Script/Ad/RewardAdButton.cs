using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardAdButton : MonoBehaviour
{
    public Button adsBtn;
    public CanvasGroup adsCanvasGroup;

    private bool isGetAdsReward;
    private Action adsCallback;

    private string page_id;
    private bool isContainAdmob;
    public void Init(Action _adsCallback,string _page_id ,bool _isContainAdmob = true)
    {
        adsCallback = _adsCallback;
        page_id = _page_id;
        isContainAdmob = _isContainAdmob;
        adsBtn.onClick.RemoveAllListeners();
        adsBtn.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayBtnMusic();
            AdsOnClick();
        });

        UpdateAdsBtnState(true);
        isGetAdsReward = false;
    }

    private void AdsOnClick()
    {
        UpdateAdsBtnState(false);
        Debug.Log("播放激励广告 page_id :" + page_id);
        //播放广告
        if(isContainAdmob)
        {
            AdManager.Instance.ShowRewardedAd(
                 page_id,
                 AdsCallback,
                 AdsPlayError
           );
        }
        else
        {
            AdManager.Instance.ShowRewardedAd(
                 page_id,
                 AdsCallback,
                 AdsPlayError
               );
        }
    }
    private void AdsCallback()
    {
        UpdateAdsBtnState(false);
        //获得奖励
        isGetAdsReward = true;
        adsCallback?.Invoke();
        adsCallback = null;
        adsBtn.onClick.RemoveAllListeners();
    }
    private void AdsPlayError()
    {
        if (!isGetAdsReward)
        {
            UpdateAdsBtnState(true);
        }
    }
    private void UpdateAdsBtnState(bool _bool)
    {
        adsBtn.interactable = _bool;     
        if(adsCanvasGroup != null)
        {
            adsCanvasGroup.alpha = _bool ? 1 : 0.5f;
        }
    }
}

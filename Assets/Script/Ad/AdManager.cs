using DG.Tweening;
using System;
using System.Text;
using UnityEngine;


public class AdManager : MonoBehaviour
{
    public static AdManager Instance;
    //-----------------------------------------------------------
    public ApplovinMaxRewardOperator applovinMaxRewardOperator;
    public ApplovinMaxInterstitialOperator applovinMaxInterstitialOperator;
    //private string SDK_key = "PbbJng_h8aD16wZWrSaHN5gtVDExorX-b1ywfx8Gal1WlU7kvbWVDpzsPARTTLwex_cbeU8SGZanUXSoA1WDMx";
    private string SDK_key = "FvJJxbZn23JxApkGSJLXJVce+fSh1+/94j9P7LzNTYgtV0ukP77sxULxX42BJ1uYMzy3E8fzhS4/+JFwbui3IaHkoZGo6I8k6/Al0ZHXXSIAzcowAgrQV+5MBAqe4wEf2FXpDxMR64Y=";
    private void Awake()
    {
        Instance = this;
    }

    public void Init()
    {
        Debug.Log("TopOn SDK初始化");

        MaxSdkCallbacks.OnSdkInitializedEvent += (MaxSdk.SdkConfiguration sdkConfiguration) =>
        {
            applovinMaxRewardOperator.Init();
            applovinMaxInterstitialOperator.Init();
        };

        string decryptedSdkKey = EncryptSDKKey.DecryptWithRandomSalt(SDK_key);
        //Debug.Log("解密结果（还原原值）：" + decryptedSdkKey);
        MaxSdk.SetSdkKey(decryptedSdkKey);
        MaxSdk.SetUserId(GameApiConfig.ClientUUID);
        MaxSdk.InitializeSdk();
    }

    /// <summary>
    /// 激励广告(有)
    /// </summary>
    public void ShowRewardedAd(string _page_id, Action _rewardCallback, Action _displayErrorCallback)
    {
        applovinMaxRewardOperator.RewardReceivedCallback = _rewardCallback;
        applovinMaxRewardOperator.RewardDisplayErrorCallback = _displayErrorCallback;
        applovinMaxRewardOperator.ShowRewardedAd();
    }

    /// <summary>
    /// 激励广告(无)
    /// </summary>
    public void ShowRewardedAd2(string _page_id, Action _rewardCallback, Action _displayErrorCallback)
    {
        DOTween.Sequence().AppendInterval(1f).AppendCallback(() =>
        {
            _rewardCallback?.Invoke();
            EventManager.Instance.TriggerEvent(GameEvent.PlayAds);
        });
    }

    /// <summary>
    /// 插屏广告
    /// </summary>
    public void OnClickInterstitialAd(string _page_id, bool isClick = true)
    {
        applovinMaxInterstitialOperator.OnClickInterstitialAd(isClick);
    }
}
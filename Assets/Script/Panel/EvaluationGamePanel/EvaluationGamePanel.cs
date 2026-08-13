using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EvaluationGamePanel : UIBase
{
    public Transform bg1;
    public Transform bg2;

    public Button btn1;
    public Button btn2;
    public Button btn3;
    public Button btn4;

    // Start is called before the first frame update
    void Start()
    {
        btn1.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayBtnMusic();
            Hide();

            OtherSdkManager.Instance.CustomEvent("Event_Rate_Not");
        });
        btn2.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayBtnMusic();
            bg1.gameObject.SetActive(false);
            bg2.gameObject.SetActive(true);

            OtherSdkManager.Instance.CustomEvent("Event_Rate_Love");
        });
        btn3.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayBtnMusic();
            Hide();
            OtherSdkManager.Instance.CustomEvent("Event_Rate_Not");
        });
        btn4.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayBtnMusic();
            PlayerPrefs.SetInt("EvaluationGameStar", 5);
            callback = () =>
            {
                OtherSdkManager.Instance.CustomEvent("Event_Rate_5Stars");
                PingJiaTiaoZhuan();
            };
            Hide();
        });
    }

    public override void Refresh(object data = null)
    {
        base.Refresh(data);
        bg1.gameObject.SetActive(true);
        bg2.gameObject.SetActive(false);

        OtherSdkManager.Instance.CustomEvent("Event_RatePopup");
    }
    public override void Hide()
    {
        base.Hide();
        PlayerPrefs.SetString("EvaluationGameDialog", "yes");
    }

    private void PingJiaTiaoZhuan()
    {
#if UNITY_ANDROID
        Application.OpenURL("https://play.google.com/store/apps/details?id=" + Application.identifier);
#elif UNITY_IOS
        return;
        string reviewUrl = $"itms-apps://itunes.apple.com/app/id{"IOSAppId"}?action=write-review";
        Application.OpenURL(reviewUrl);
#endif
    }
}

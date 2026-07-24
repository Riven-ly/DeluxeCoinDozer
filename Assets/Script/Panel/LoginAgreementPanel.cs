using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginAgreementPanel : UIBase
{
    public Transform explain;
    public Button TermsofService;
    public Button PrivacyNotice;

    public Transform explain_p;
    public Button TermsofService_p;
    public Button PrivacyNotice_p;

    public Transform explain_in;
    public Button TermsofService_in;
    public Button PrivacyNotice_in;

    public Button acceptBtn;
    // Start is called before the first frame update
    void Start()
    {
        acceptBtn.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayBtnMusic();
            PlayerPrefs.SetString("LoginAgreementPanel", "YES");
            Hide();

            CustomApiManager.Instance.RequestCustomEventV2(new CustomEventData()
            {
                page_id = "LoginAgreement",
                name = "Event_AgreementAccept",
                value = "",
            });
        });
        TermsofService.onClick.AddListener(() =>
        {
            SettingPanel.OpenPrivacyPolicy();
        });
        PrivacyNotice.onClick.AddListener(() =>
        {
            SettingPanel.OpenTermsOfServic();
        });
        TermsofService_p.onClick.AddListener(() =>
        {
            SettingPanel.OpenPrivacyPolicy();
        });
        PrivacyNotice_p.onClick.AddListener(() =>
        {
            SettingPanel.OpenTermsOfServic();
        });
        TermsofService_in.onClick.AddListener(() =>
        {
            SettingPanel.OpenPrivacyPolicy();
        });
        PrivacyNotice_in.onClick.AddListener(() =>
        {
            SettingPanel.OpenTermsOfServic();
        });
    }


    public override void Refresh(object data = null)
    {
        base.Refresh(data);
        explain.gameObject.SetActive(LanguageManager.Instance.type == MultilingualType.English);
        explain_p.gameObject.SetActive(LanguageManager.Instance.type == MultilingualType.Portuguese);
        explain_in.gameObject.SetActive(LanguageManager.Instance.type == MultilingualType.Indonesian);
        CustomApiManager.Instance.RequestCustomEventV2(new CustomEventData()
        {
            page_id = "LoginAgreement",
            name = "Event_AgreementShow",
            value = "",
        });
    }
    public override void Hide()
    {
        base.Hide();
    }
}

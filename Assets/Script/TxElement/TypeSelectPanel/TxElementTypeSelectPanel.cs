using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TxElementTypeSelectPanel : UIBase
{
    public Text title;
    public Text explain;
    public InputField inputField1;
    public InputField inputField2;

    public List<Button> buttons;
    public Transform selectBtnIcon;
    public CanvasGroup errorTrans;
    public Text errorTransStr;
    public Button hideBtn;
    public Button submitBtn;
    private int index;
    private void Start()
    {
        Refresh();

        for (int i = 0; i < buttons.Count; i++)
        {
            int _index = i;
            buttons[_index].onClick.AddListener(() =>
            {
                AudioManager.Instance.PlayBtnMusic();
                index = _index;
                UpdateselectBtnIconPos();
            });
        }

        hideBtn.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayBtnMusic();
            Hide();
        });

        submitBtn.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayBtnMusic();
            SubmitOnClick();
        });
    }
    public override void Refresh(object data = null)
    {
        base.Refresh(data);

        string s1 = LanguageManager.Instance.GetText_Encrypt("WH");
        title.text = string.Format(LanguageManager.Instance.GetText("TxElementTypeSelectPanel_title"), s1);
        explain.text = LanguageManager.Instance.GetText("TxElementTypeSelectPanel_explain");

        string s2 = LanguageManager.Instance.GetText_Encrypt("wH");
        inputField1.placeholder.transform.GetComponent<Text>().text = string.Format(LanguageManager.Instance.GetText("TxElementTypeSelectPanel_input1"), s2);
        inputField1.text = "";

        inputField2.placeholder.transform.GetComponent<Text>().text = string.Format(LanguageManager.Instance.GetText("TxElementTypeSelectPanel_input2"), s2);
        inputField2.text = "";

        index = 0;
        UpdateselectBtnIconPos();
        errorTrans.alpha = 0f;
    }

    public override void Hide()
    {
        base.Hide();
    }

    private void UpdateselectBtnIconPos()
    {
        Vector3 vec = buttons[index].transform.localPosition;
        vec.y += 5f;
        selectBtnIcon.localPosition = vec;
    }

    public void SubmitOnClick()
    {
        if(string.IsNullOrEmpty(inputField1.text) || inputField1.text != inputField2.text)
        {
            errorTransStr.text = LanguageManager.Instance.GetText("TxElementTypeSelectPanel_Error");
            ErrorAnim();
        }
        else
        {
            if (GameManager.CheckSimpleEmail(inputField1.text))
            {
                TxElementMananger.Instance.info.accountInfo.type = (TxElementAccountType)index;
                TxElementMananger.Instance.info.accountInfo.email = inputField1.text;
                TxElementMananger.Instance.SaveElementManangerInfo();
                UIManager.Instance.GetUI<TxElementPanel>().UpdateAccountTypeIcon();
                Hide();

                CustomApiManager.Instance.RequestCustomEventV2(new CustomEventData()
                {
                    page_id = "TXPanel",
                    name = "Event_TXPPAdd",
                    value = "",
                });
            }
            else
            {
                errorTransStr.text = LanguageManager.Instance.GetText("TxElementTypeSelectPanel_Error2");
                ErrorAnim();
            }
        }
    }

    public void ErrorAnim()
    {
        errorTrans.alpha = 1;
        errorTrans.transform.localScale = Vector3.one;

        DOTween.Kill(this);
        DOTween.Sequence()
            .Append(errorTrans.transform.DOScale(1.1f, 0.1f))
            .Append(errorTrans.transform.DOScale(1f, 0.1f))
            .AppendInterval(1.1f)
            .Append(errorTrans.DOFade(0f, 0.2f))
            .SetTarget(this);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TxElementPanel_InitCell : MonoBehaviour
{
    public Text title;
    public Text count;
    public Text explain;
    public Button btn;

    private void Start()
    {
        btn.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayBtnMusic();

            if (string.IsNullOrEmpty(TxElementMananger.Instance.info.accountInfo.email))
            {
                UIManager.Instance.OpenUI<TxElementTypeSelectPanel>();
            }
            else
            {
                btn.interactable = false;
                UIManager.Instance.OpenUI<TxElementJinDuPanel>(null, () =>
                {
                    RreatNewOrder();
                });

                CustomApiManager.Instance.RequestCustomEventV2(new CustomEventData()
                {
                    page_id = "TXPanel",
                    name = "Event_TXClick",
                    value = TxElementMananger.Instance.info.initInfo.diamond.ToString(),
                });
            }

        });
    }
    public void Init(TxElementInitInfo _info)
    {
        int curLv = GameManager.Instance.playerInfo.playerData.level;

        string str1 = LanguageManager.Instance.GetText_Encrypt("blc");
        title.text = string.Format(LanguageManager.Instance.GetText("InitCell_title"), str1);

        string str2 = LanguageManager.Instance.GetText_Encrypt("wH");
        string str3 = $"{curLv}/{_info.targetLv}";
        explain.text = string.Format(LanguageManager.Instance.GetText("InitCell_explain"), str2, str3);

        string str4 = LanguageManager.Instance.GetText_Encrypt("Special_Diamond__unit");
        count.text = $"{str4}{_info.diamond}";

        btn.interactable = false;
        if (_info.diamond > 0f && curLv >= _info.targetLv && TxElementMananger.Instance.info.orderStatus == TxElementType.Init)
        {
            btn.interactable = true;
        }
    }

    public void RreatNewOrder()
    {
        TxElementManangerInfo _info = TxElementMananger.Instance.info;
        _info.queueUpInfo.Init();
        _info.queueUpInfo.diamond = _info.initInfo.diamond;
        _info.initInfo.Init();

        DateTime curTime = GameManager.Instance.GetNowTime();
        DateTime targetTime = curTime.AddSeconds(TxElementPanel_QueueUpCell.Phase1Time + TxElementPanel_QueueUpCell.Phase2Time + TxElementPanel_QueueUpCell.Phase3Time);

        _info.queueUpInfo.startTime = GameManager.DateTimeToTimeStamp(curTime);
        _info.queueUpInfo.targetTime = GameManager.DateTimeToTimeStamp(targetTime);

        _info.orderStatus = TxElementType.QueueUp;
        TxElementMananger.Instance.SaveElementManangerInfo();
        UIManager.Instance.GetUI<TxElementPanel>().RefreshPanel();

        CustomApiManager.Instance.RequestCustomEventV2(new CustomEventData()
        {
            page_id = "TXPanel",
            name = "Event_TXStatus",
            value = " reviewing",
        });
    }
}

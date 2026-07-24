using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TxElementPanel : UIBase
{
    public Text title;
    public Button hideBtn;
    public Button historyBtn;
    public Button selectTypeEnterBtn;
    public Image typeIcon;
    public Text accountText;

    public TxElementPanel_InitCell txElementPanel_InitCell;
    public TxElementPanel_QueueUpCell txElementPanel_QueueUpCell;
    public TxElementPanel_TaskCell txElementPanel_TaskCell;
    private void Start()
    {
        hideBtn.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayBtnMusic();
            GameManager.Instance.TryEvaluationGame();
            Hide();
        });
        historyBtn.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayBtnMusic();
            UIManager.Instance.OpenUI<TxElementHistoryPanel>();
        });
        selectTypeEnterBtn.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayBtnMusic();
            UIManager.Instance.OpenUI<TxElementTypeSelectPanel>();
        });


        title.text = LanguageManager.Instance.GetText_Encrypt("WH");
    }

    public override void Refresh(object data = null)
    {
        base.Refresh(data);

        string str = PlayerPrefs.GetString("TxElementYindaoPanel");
        if (string.IsNullOrEmpty(str))
        {
            DOTween.Sequence().AppendInterval(0.5f).AppendCallback(() =>
            {
                UIManager.Instance.OpenUI<TxElementYindaoPanel>();
            });
        }
   
        UpdateAccountTypeIcon();
        RefreshPanel();

        CustomApiManager.Instance.RequestCustomEventV2(new CustomEventData()
        {
            page_id = "TXPanel",
            name = "Event_TXOpen",
            value = "",
        });
    }

    public void RefreshPanel()
    {
        txElementPanel_QueueUpCell.gameObject.SetActive(false);
        txElementPanel_TaskCell.gameObject.SetActive(false);

        txElementPanel_InitCell.Init(TxElementMananger.Instance.info.initInfo);
        if(TxElementMananger.Instance.info.orderStatus == TxElementType.QueueUp)
        {
            txElementPanel_QueueUpCell.gameObject.SetActive(true);
            txElementPanel_QueueUpCell.Init(TxElementMananger.Instance.info.queueUpInfo);     
        }
        else if (TxElementMananger.Instance.info.orderStatus == TxElementType.Task)
        {
            txElementPanel_TaskCell.gameObject.SetActive(true);
            txElementPanel_TaskCell.Init(TxElementMananger.Instance.info.taskInfo);
        }
    }

    public override void Hide()
    {
        base.Hide();
    }

    public void UpdateAccountTypeIcon()
    {
        typeIcon.sprite = TxElementMananger.Instance.accountTypeSprites[(int)TxElementMananger.Instance.info.accountInfo.type];
        typeIcon.SetNativeSize();

        if(string.IsNullOrEmpty(TxElementMananger.Instance.info.accountInfo.email))
        {
            accountText.text = LanguageManager.Instance.GetText("selectType_explain");
        }
        else
        {
            accountText.text = TxElementMananger.Instance.info.accountInfo.email;
        }
    }


}

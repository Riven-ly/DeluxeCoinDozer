using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public enum SceneItemType
{
    Null,
    //巨大金币
    Big_Gold,
    //护墙
    City_Wall,
    //金币爆炸
    Gold_Explode,
    //震动
    Machine_Vibration,
}

public class SceneItemPanelInfo
{
    public SceneItemType type;
    public string title;
    public string explain;
    public Sprite icon;
    public Action clickCallback;
}
public class SceneItemBase : MonoBehaviour
{
    public SceneItemType type;
    public Button btn;
    public Image icon;
    public GameObject itemCntBg;
    public Text itemCntText;
    public GameObject addTips;
    public CanvasGroup canvasGroup;
    public Text str;

    private int curItemCnt;
    private SceneItemPanelInfo sceneItemPanelInfo = new SceneItemPanelInfo();

    private void Start()
    {
        btn.onClick.AddListener(() =>
        {
            BtnClick();
        });
    }
    public virtual void Init(int _cnt)
    {
        SetBtnAction(true);

        RefreshUI(_cnt);
        SetItemPanelInfo();
        str.gameObject.SetActive(false);
    }

    public void RefreshUI(int _cnt)
    {
        curItemCnt = _cnt;
        itemCntBg.SetActive(curItemCnt > 0);
        addTips.SetActive(curItemCnt <= 0);

        itemCntText.text = curItemCnt.ToString();

    }

    private void SetItemPanelInfo()
    {
        switch (type)
        {
            case SceneItemType.Big_Gold:
                sceneItemPanelInfo.title = LanguageManager.Instance.GetText("SceneItem_Big_Gold_Title");
                sceneItemPanelInfo.explain = LanguageManager.Instance.GetText("SceneItem_Big_Gold_Explain");
                break;
            case SceneItemType.City_Wall:
                sceneItemPanelInfo.title = LanguageManager.Instance.GetText("SceneItem_City_Wall_Title");
                sceneItemPanelInfo.explain = LanguageManager.Instance.GetText("SceneItem_City_Wall_Explain");
                break;
            case SceneItemType.Gold_Explode:
                sceneItemPanelInfo.title = LanguageManager.Instance.GetText("SceneItem_Gold_Explode_Title");
                sceneItemPanelInfo.explain = LanguageManager.Instance.GetText("SceneItem_Gold_Explode_Explain");
                break;
            case SceneItemType.Machine_Vibration:
                sceneItemPanelInfo.title = LanguageManager.Instance.GetText("SceneItem_Machine_Vibration_Title");
                sceneItemPanelInfo.explain = LanguageManager.Instance.GetText("SceneItem_Machine_Vibration_Explain");
                break;
        }
        sceneItemPanelInfo.type = type;
        sceneItemPanelInfo.icon = icon.sprite;
        sceneItemPanelInfo.clickCallback = UseItem;
    }

    public void BtnClick()
    {
        if(curItemCnt > 0)
        {
            curItemCnt--;
            UseItem();
        }
        else
        {
            AudioManager.Instance.PlayBtnMusic();
            UIManager.Instance.OpenUI<GetSceneItemPanel>(sceneItemPanelInfo);
        }
    }
    private void UseItem()
    {
        AudioManager.Instance.PlaySceneSingleMusic("UseItem");
        EventManager.Instance.TriggerEvent(GameEvent.UseSceneItem, type);

        CustomApiManager.Instance.RequestCustomEventV2(new CustomEventData()
        {
            page_id = "UseItem",
            name = "Event_UseItem",
            value = type.ToString(),
        });
    }

    public void SetBtnAction(bool _bool)
    {
        btn.interactable = _bool;
        canvasGroup.alpha = _bool ? 1f : 0.7f;
       
    }
    public void SetTimeStrState(bool isbool)
    {
        str.gameObject.SetActive(isbool);
        str.text = "";
    }
    public void StartTimeText(int _time)
    {
        StartCoroutine(timeIE(_time));
    }
    IEnumerator timeIE(int _time)
    {
        str.gameObject.SetActive(true);
        int temTime = _time;
        str.text = temTime.ToString();
        while (temTime > 0)
        {
            yield return new WaitForSeconds(1f);
            temTime--;
            str.text = temTime.ToString();
        }
    }
}

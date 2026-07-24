using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DailySignInCell : MonoBehaviour
{
    public Button clickBtn;
    public GameObject baseBgObj;
    public Text baseTitle;
    public GameObject todayBgObj;
    public Text todayTitle;
    public GameObject signInObj;
    public Transform cells;

    public Transform effectRoot;

    private List<ItemBase> items;
    private List<ItemData> itemDatas;
    private int day;
    // Start is called before the first frame update
    void Start()
    {
        clickBtn.onClick.AddListener(() =>
        {
            SignIn();

            CustomApiManager.Instance.RequestCustomEventV2(new CustomEventData()
            {
                page_id = "DailySignInPanel",
                name = "Event_SignOpen",
                value = $"day :{day}",
            });

            if(day == 7)
            {
                CustomApiManager.Instance.RequestCustomEventV2(new CustomEventData()
                {
                    page_id = "DailySignInPanel",
                    name = "Event_SignComplete",
                    value = $"day :{day}",
                });
            }
        });
    }

    public void Init(int index, List<ItemData> _itemDatas)
    {
        day = index;
        itemDatas = _itemDatas;
        if (index == 0)
            day = 7;
        baseTitle.text = $"{LanguageManager.Instance.GetText("DAY")} {day}";
        todayTitle.text = $"{LanguageManager.Instance.GetText("DAY")} {day}";
        items = GameManager.Instance.CreatItems(itemDatas, cells);
    }

    public void IsToday(bool istoday)
    {
        baseBgObj.SetActive(!istoday);
        todayBgObj.SetActive(istoday);
    }
    public void SignInState(bool isSignIn)
    {
        signInObj.gameObject.SetActive(isSignIn);
    }
    private void SignIn()
    {
        UIManager.Instance.OpenUI<GeneralRewardsPanel>(itemDatas, OpenRewardPanelCallback);
    }

    private void OpenRewardPanelCallback()
    {
        effectRoot.gameObject.SetActive(false);
        clickBtn.interactable = false;
        SignInState(true);
        DailySignIn.SignIn(day);
        bool isDoublereward = GeneralRewardsPanel.GetIsDoubleReward();
        ItemsGetRewardAndAnim(items, isDoublereward);
    }

    /// <summary>
    /// 获得道具奖励并且走动画
    /// </summary>
    private void ItemsGetRewardAndAnim(List<ItemBase> _items, bool _isDoubleReward = false)
    {
        bool isAwaitAnim = false;
        foreach (var item in _items)
        {
            if (item.itemType == ItemType.Gold)
            {
                GoldCollectEffect.Instance.StartEffect(ItemType.Gold, item.transform.position, UIManager.Instance.playInfoUI.goldUI.icon.transform.position);
                isAwaitAnim = true;
            }
            else if (item.itemType == ItemType.Diamond)
            {
                GoldCollectEffect.Instance.StartEffect(ItemType.Diamond, item.transform.position, UIManager.Instance.playInfoUI.diamondUI.icon.transform.position);
                isAwaitAnim = true;
            }
        }
        if(isAwaitAnim)
        {
            DOTween.Sequence().AppendInterval(0.7f).AppendCallback(() =>
            {
                foreach (var item in _items)
                {
                    item.GetItemReward();
                    if (_isDoubleReward)
                    {
                        item.GetItemReward();
                    }
                }
            });
        }
        else
        {
            foreach (var item in _items)
            {
                item.GetItemReward();
                if (_isDoubleReward)
                {
                    item.GetItemReward();
                }
            }
        }


        CustomApiManager.Instance.RequestCustomEventV2(new CustomEventData()
        {
            page_id = "GoldGain",
            name = "Event_GoldGain",
            value = "sign_in",
        });
    }
}

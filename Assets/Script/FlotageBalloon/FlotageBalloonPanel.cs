using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlotageBalloonPanel : UIBase
{
    public Image icon;
    public Button hideBtn;
    public RewardAdButton rewardAdButton;
    public Transform itmeRoot;


    private List<ItemBase> itemBases;
    private List<ItemData> itemDatas;
    private string page_id = "FlotageBalloonPanel";
    // Start is called before the first frame update
    void Start()
    {
        hideBtn.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayBtnMusic();
            Hide();
            AdManager.Instance.OnClickInterstitialAd(page_id, true);
        });
    }

    public override void Refresh(object data = null)
    {
        base.Refresh(data);
        AudioManager.Instance.PlaySceneSingleMusic("GetItemPanel");
        if (itemBases == null)
        {
            itemDatas = new List<ItemData>();
            itemDatas.Add(new ItemData(ItemType.Gold, 1));
            itemBases = GameManager.Instance.CreatItems(itemDatas, itmeRoot.transform);
            foreach (ItemBase item in itemBases)
            {
                item.cntText.transform.localPosition = new Vector3(0f, -70f, 0f);
                //item.gameObject.SetActive(false);
            }
        }
        itemDatas[0].count = FlotageBalloon.rewardCnt;
        itemBases[0].count = itemDatas[0].count;
        itemBases[0].cntText.text = itemBases[0].count.ToString();

        rewardAdButton.Init(AdRewardCallback, page_id);

        hideBtn.transform.localScale = Vector3.zero;
        DOTween.Sequence()
                     .AppendInterval(1.5f)
                     .Append(hideBtn.transform.DOScale(1.1f, 0.2f))
                     .Append(hideBtn.transform.DOScale(0.9f, 0.1f))
                     .Append(hideBtn.transform.DOScale(1f, 0.1f));
    }
    public override void Hide()
    {
        base.Hide();
    }

    private void AdRewardCallback()
    {
        //UIManager.Instance.OpenUI<GeneralRewardsPanel>(itemDatas, () =>
        //{
        //    bool isDoublereward = GeneralRewardsPanel.GetIsDoubleReward();
        //    foreach (var item in itemBases)
        //    {
        //        item.GetItemReward();
        //        if (isDoublereward)
        //        {
        //            item.GetItemReward();
        //        }
        //    }

        //    callback = () =>
        //    {
        //        UIManager.Instance.mainBtnUI.flotageBalloon.FlotageBalloonLeave();
        //    };
        //    Hide();
        //});

        foreach (var item in itemBases)
        {
            item.GetItemReward();
        }
        callback = () =>
        {
            UIManager.Instance.mainBtnUI.flotageBalloon.FlotageBalloonLeave();
        };
        Hide();

        CustomApiManager.Instance.RequestCustomEventV2(new CustomEventData()
        {
            page_id = "GoldGain",
            name = "Event_GoldGain",
            value = "bubble",
        });

        CustomApiManager.Instance.RequestCustomEventV2(new CustomEventData()
        {
            page_id = "FlotageBalloon",
            name = "Event_BubbleOpenAD",
            value = "",
        });
    }

    //private void AdRewardCallback()
    //{
    //    UIManager.Instance.OpenUI<GeneralRewardsPanel>(itemDatas, () =>
    //    {
    //        GoldCollectEffect.Instance.StartEffect(ItemType.Gold, icon.transform.position, UIManager.Instance.playInfoUI.goldUI.icon.transform.position);

    //        DOTween.Sequence().AppendInterval(0.7f).AppendCallback(() =>
    //        {
    //            foreach (var item in itemBases)
    //            {
    //                item.GetItemReward();
    //                if (GeneralRewardsPanel.GetIsDoubleReward())
    //                {
    //                    item.GetItemReward();
    //                }
    //            }
    //        });

    //        UIManager.Instance.OpenUIMask();
    //        DOTween.Sequence().AppendInterval(2f).AppendCallback(() =>
    //        {
    //            callback = () =>
    //            {
    //                UIManager.Instance.mainBtnUI.flotageBalloon.FlotageBalloonLeave();
    //            };
    //            UIManager.Instance.HideUIMask();
    //            Hide();
    //        });
    //    });

    //}
}

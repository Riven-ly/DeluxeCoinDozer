using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardGamePanel : UIBase
{
    public Button claimBtn;
    public RewardAdButton rewardAdButton;
    public Transform clickMask;
    public List<CardGameCell> cells;

    [HideInInspector]public List<ItemBase> itemBases;
    [HideInInspector] public List<ItemData> itemDatas;

    private List<ItemData> itemDatasConfig;
    private List<int> weightList;
    private string page_id = "CardGamePanel";

    private void Start()
    {
        claimBtn.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayBtnMusic();
            GetReward();
            Hide();

            AdManager.Instance.OnClickInterstitialAd(page_id, true);
        });
    }
    private void InitConfig()
    {
        itemDatasConfig = new List<ItemData>();
        itemDatasConfig.Add(new ItemData(ItemType.Gold, 10));
        itemDatasConfig.Add(new ItemData(ItemType.Diamond, 5));
        itemDatasConfig.Add(new ItemData(ItemType.Gold, 5));
        itemDatasConfig.Add(new ItemData(ItemType.Machine_Vibration, 1));
        itemDatasConfig.Add(new ItemData(ItemType.City_Wall, 1));
        itemDatasConfig.Add(new ItemData(ItemType.Gold_Explode, 1));
        weightList = new List<int>() { 15, 10, 35, 15, 5, 5 };
    }
    public override void Refresh(object data = null)
    {
        base.Refresh(data);
        AudioManager.Instance.PlaySceneSingleMusic("SpecialGame");
        if(itemDatasConfig == null)
        {
            InitConfig();
        }
        itemBases = new List<ItemBase>();
        itemDatas = new List<ItemData>();
        clickMask.gameObject.SetActive(true);
        rewardAdButton.transform.localScale = Vector3.zero;
        claimBtn.transform.localScale = Vector3.zero;

        foreach (var cell in cells)
        {
            int targetIndex = GetRandomItemByWeight();
            cell.Init(itemDatasConfig[targetIndex], this);
        }

        rewardAdButton.Init(AdRewardCallback, page_id, UnityEngine.Random.Range(0, 2) == 1);

        CustomApiManager.Instance.RequestCustomEventV2(new CustomEventData()
        {
            page_id = "Card_Game",
            name = "Event_Card",
            value = "",
        });
    }
    public override void Hide()
    {
        callback = () =>
        {
            foreach (var cell in cells)
            {
                cell.ClearItem();
            }
        };
        base.Hide();
    }
    public void AddReward(ItemBase _itemBase, ItemData _itemData)
    {
        itemBases.Add(_itemBase);
        itemDatas.Add(_itemData);
    }

    public void FanpaiAll()
    {
        float allTime = 1f;
        foreach (var cell in cells)
        {
            if (cell.isClick)
                continue;

            cell.FanpaiAnim(allTime);
            allTime += 0.3f;
        }

        DOTween.Sequence().AppendInterval(allTime + 0.2f)
                 .AppendCallback(() =>
                 {
                     clickMask.gameObject.SetActive(false);
                     DOTween.Sequence().Append(rewardAdButton.transform.DOScale(1.1f, 0.2f))
                                       .Append(rewardAdButton.transform.DOScale(0.9f, 0.1f))
                                       .Append(rewardAdButton.transform.DOScale(1f, 0.1f));

                     DOTween.Sequence()
                           .AppendInterval(1.5f)
                           .Append(claimBtn.transform.DOScale(1.1f, 0.2f))
                           .Append(claimBtn.transform.DOScale(0.9f, 0.1f))
                           .Append(claimBtn.transform.DOScale(1f, 0.1f));
                 });
    }

    private void GetReward()
    {
        foreach (var item in itemBases)
        {
            item.GetItemReward();
        }

        CustomApiManager.Instance.RequestCustomEventV2(new CustomEventData()
        {
            page_id = "GoldGain",
            name = "Event_GoldGain",
            value = "minigame_card",
        });

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
        //    Hide();
        //});

    }

    private void AdRewardCallback()
    {
        foreach (var cell in cells)
        {
            cell.AddReward();
        }
        GetReward();
        Hide();

        CustomApiManager.Instance.RequestCustomEventV2(new CustomEventData()
        {
            page_id = "Card_Game",
            name = "Event_CardAD",
            value = "",
        });
    }

    public int GetRandomItemByWeight()
    {
        // 1. 计算总权重
        int totalWeight = 0;
        foreach (int value in weightList)
        {
            // 过滤无效权重（避免负数或0）
            if (value > 0)
            {
                totalWeight += value;
            }
        }

        // 2. 生成0到总权重之间的随机数
        int randomValue = UnityEngine.Random.Range(0, totalWeight);

        // 3. 遍历权重，找到随机数所在的区间
        int targetIndex = 0;
        int currentWeight = 0;
        foreach (int value in weightList)
        {
            if (value <= 0)
            {
                targetIndex++;
                continue;
            }

            currentWeight += value;
            // 当累加权重超过随机数时，返回当前物品
            if (randomValue < currentWeight)
            {
                return targetIndex;
            }
            targetIndex++;
        }

        return targetIndex;
    }
}

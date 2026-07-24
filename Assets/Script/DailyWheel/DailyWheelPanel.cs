using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DailyWheelPanel : UIBase
{
    public Button hideBtn;
    public Button dailyBtn;
    public CanvasGroup dailyBtncanvasGroup;
    public RewardAdButton rewardAdButton;
    public Text explain;
    public Transform wheel;

    public List<ItemBase> itemBases;
    List<ItemData> itemDatas;
    List<int> itemWeights;
    private string page_id = "DailyWheelPanel";
    void Start()
    {
        hideBtn.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayBtnMusic();
            Hide();
        });
        dailyBtn.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayBtnMusic();
            dailyBtn.interactable = false;
            dailyBtncanvasGroup.alpha =  0.5f;
            DailyWheel.DailyWheelRecord();

            CustomApiManager.Instance.RequestCustomEventV2(new CustomEventData()
            {
                page_id = page_id,
                name = "Event_WheelStart",
                value = "",
            });

            SpinWheel();
        });

        string str1 = LanguageManager.Instance.GetText("DailyWheel_explain");
        string jiami1 = LanguageManager.Instance.GetText_Encrypt("CHT");
        string jiami2 = LanguageManager.Instance.GetText_Encrypt("Special_Diamond__unit");
        explain.text = string.Format(str1, jiami1, jiami2);


        itemDatas = new List<ItemData>();
        itemDatas.Add(new ItemData(ItemType.Gold, 25));
        itemDatas.Add(new ItemData(ItemType.City_Wall, 1));
        itemDatas.Add(new ItemData(ItemType.Gold_Explode, 1));
        itemDatas.Add(new ItemData(ItemType.Diamond, 50f));
        itemDatas.Add(new ItemData(ItemType.Gold, 25));
        itemDatas.Add(new ItemData(ItemType.Machine_Vibration, 1));
        itemDatas.Add(new ItemData(ItemType.Big_Gold, 1));
        itemDatas.Add(new ItemData(ItemType.Gold_Explode, 1));
        itemDatas.Add(new ItemData(ItemType.Machine_Vibration, 1));
        itemDatas.Add(new ItemData(ItemType.Big_Gold, 1));
        itemWeights = new List<int>() { 3,6,19,0,3,19,6,19,19,6};
        
        int index = 0;
        foreach (var itemData in itemDatas)
        {
            itemBases[index].Init(itemData.count);
            index++;
        }
    }
    public override void Refresh(object data = null)
    {
        base.Refresh(data);

        explain.gameObject.SetActive(GameManager.appATTtype == 1);
        rewardAdButton.Init(AdRewardCallback, page_id, UnityEngine.Random.Range(0, 2) == 1);
        CheckBtnState();

        CustomApiManager.Instance.RequestCustomEventV2(new CustomEventData()
        {
            page_id = page_id,
            name = "Event_WheelOpen",
            value = "",
        });
    }
    public override void Hide()
    {
        base.Hide();
    }

    private void CheckBtnState()
    {
        bool isDailyWheel = DailyWheel.CheckDailyWheel();
        rewardAdButton.gameObject.SetActive(isDailyWheel);

        dailyBtn.gameObject.SetActive(!isDailyWheel);
        dailyBtn.interactable = !isDailyWheel;
        dailyBtncanvasGroup.alpha = !isDailyWheel ? 1f : 0.5f;
    }
    private void SpinWheel()
    {
        AudioManager.Instance.PlaySceneLoopMusic("wheelPanel");
        DOTween.Sequence()
            .AppendInterval(0.4f)
            .AppendCallback(() =>
            {
                AudioManager.Instance.StopSceneLoopMusic();
                AudioManager.Instance.PlaySceneLoopMusic("wheelPanel");
            })
             .AppendInterval(0.3f)
            .AppendCallback(() =>
            {
                AudioManager.Instance.StopSceneLoopMusic();
                AudioManager.Instance.PlaySceneLoopMusic("wheelPanel");
            })
             .AppendInterval(0.2f)
            .AppendCallback(() =>
            {
                AudioManager.Instance.StopSceneLoopMusic();
                AudioManager.Instance.PlaySceneLoopMusic("wheelPanel");
            })        
            ;


        int targetIndex = GetRandomItemByWeight();
        float targetaugle = targetIndex * 36f;
        DOTween.Sequence()
            .Append(wheel.transform.DORotate(new Vector3(0, 0, 360 * 1f), 1f, RotateMode.FastBeyond360).SetEase(Ease.InCubic))
            .Append(wheel.transform.DORotate(new Vector3(0, 0, 360 * 3f + targetaugle), 2f, RotateMode.FastBeyond360).SetEase(Ease.OutCubic))
            .AppendCallback(() =>
            {
                AudioManager.Instance.StopSceneLoopMusic();
            })
            .AppendInterval(0.5f)
            .AppendCallback(() =>
            {
                List<ItemData> rewardList = new List<ItemData>();
                rewardList.Add(itemDatas[targetIndex]);

                UIManager.Instance.OpenUI<GeneralRewardsPanel>(rewardList, () =>
                {
                    bool isDoublereward = GeneralRewardsPanel.GetIsDoubleReward();

                    itemBases[targetIndex].GetItemReward();
                    if(isDoublereward)
                    {
                        itemBases[targetIndex].GetItemReward();
                    }
                    CheckBtnState();
                    rewardAdButton.Init(AdRewardCallback, page_id, UnityEngine.Random.Range(0, 2) == 1);
                });

                CustomApiManager.Instance.RequestCustomEventV2(new CustomEventData()
                {
                    page_id = "GoldGain",
                    name = "Event_GoldGain",
                    value = "wheel",
                });

                CustomApiManager.Instance.RequestCustomEventV2(new CustomEventData()
                {
                    page_id = page_id,
                    name = "Event_WheelReward",
                    value = $"type :{itemDatas[targetIndex].itemType.ToString()}, count :{itemDatas[targetIndex].count}",
                });
            });
    }
    private void AdRewardCallback()
    {
        CustomApiManager.Instance.RequestCustomEventV2(new CustomEventData()
        {
            page_id = page_id,
            name = "Event_WheelAD",
            value = "",
        });

        SpinWheel();
    }

    /// <summary>
    /// 根据权重随机抽取物品
    /// </summary>
    public int GetRandomItemByWeight()
    {
        // 1. 计算总权重
        int totalWeight = 0;
        foreach (int value in itemWeights)
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
        foreach (int value in itemWeights)
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

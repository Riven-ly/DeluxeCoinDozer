using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropGamePanel : UIBase
{
    public static bool isGameOver;
    public Transform points;
    public Text timeText;
    public Transform items;
    public Transform itembaseRoot;
    public GameObject itemPrefab;
    public DropGameCar dropGameCar;

    private List<ItemData> itemDatas;
    private List<ItemData> itemDatasConfig;
    private List<int> weightList;
    private float time = 12f;
    private float timer;
    private void InitCpnfig()
    {
        itemDatasConfig = new List<ItemData>();
        itemDatasConfig.Add(new ItemData(ItemType.Gold, 1));
        itemDatasConfig.Add(new ItemData(ItemType.Gold, 3));
        itemDatasConfig.Add(new ItemData(ItemType.Diamond, 2));
        itemDatasConfig.Add(new ItemData(ItemType.Gold_Explode, 1));
        itemDatasConfig.Add(new ItemData(ItemType.City_Wall, 1));
        itemDatasConfig.Add(new ItemData(ItemType.Machine_Vibration, 1));
        weightList = new List<int>() { 50, 20, 10, 5, 3, 5 };
    }  
    public override void Refresh(object data = null)
    {
        base.Refresh(data);
        if(itemDatasConfig == null)
        {
            InitCpnfig();
        }
        itemDatas = new List<ItemData>();
        dropGameCar.Init(this);
        isGameOver = false;
        //10秒结束
        timer = time;
        timeText.text = time.ToString();
        DOTween.Sequence()
            .AppendInterval(1f)
            .AppendCallback(() =>
            {
                timer--;
                timeText.text = timer.ToString();
                if(timer <= 0)
                {
                    isGameOver = true;
                    Invoke("GameOver", 0.5f);
                    //GameOver();
                }
            })
            .SetLoops((int)time);
        //10个道具
        DOTween.Sequence()
            .AppendCallback(() =>
            {
                CreatItem();
            })
            .AppendInterval(1f)
            .SetLoops(10);


        CustomApiManager.Instance.RequestCustomEventV2(new CustomEventData()
        {
            page_id = "Drop_Game",
            name = "Event_Drop",
            value = "",
        });
    }
    public override void Hide()
    {
        callback = () =>
        {
            ClearItemAll();
        };
        base.Hide();
    }
    private void ClearItemAll()
    {
        foreach (Transform item in itembaseRoot)
        {
            Destroy(item.gameObject);
        }
        foreach (Transform item in items)
        {
            DropGamePanelItem dropGamePanelItem = item.transform.GetComponent<DropGamePanelItem>();
            dropGamePanelItem.Clear();
        }
    }
    private void GameOver()
    {
        isGameOver = true;
        if(itemDatas.Count == 0)
        {
            Hide();
            return;
        }

        var rewarditemList = GameManager.Instance.CreatItems(itemDatas, itembaseRoot);
        //给奖
        UIManager.Instance.OpenUI<GeneralRewardsPanel>(itemDatas, () =>
        {
            bool isDoublereward = GeneralRewardsPanel.GetIsDoubleReward();
            foreach (var item in rewarditemList)
            {
                item.GetItemReward();
                if (isDoublereward)
                {
                    item.GetItemReward();

                    CustomApiManager.Instance.RequestCustomEventV2(new CustomEventData()
                    {
                        page_id = "Drop_Game",
                        name = "Event_DropAD",
                        value = "",
                    });
                }
                else
                {
                    AdManager.Instance.OnClickInterstitialAd("Drop_Game", true);
                }
            }
            Hide();
        });


        CustomApiManager.Instance.RequestCustomEventV2(new CustomEventData()
        {
            page_id = "GoldGain",
            name = "Event_GoldGain",
            value = "minigame_catch",
        });
    }
    private void CreatItem()
    {
        if (isGameOver)
            return;

        GameObject obj = Instantiate(itemPrefab, items);
        DropGamePanelItem dropGamePanelItem = obj.transform.GetComponent<DropGamePanelItem>();
        int ramdomPosIndex = Random.Range(0, points.childCount);
        dropGamePanelItem.transform.position = points.GetChild(ramdomPosIndex).transform.position;

        int ramdomDataIndex = GetRandomItemByWeight();
        dropGamePanelItem.Init(itemDatasConfig[ramdomDataIndex]);
    }

    public void AddDropReward(ItemBase itemBase)
    {
        bool isHave = false;
        foreach (var item in itemDatas)
        {
            if(item.itemType == itemBase.itemType)
            {
                isHave = true;
                item.count += itemBase.count;
                break;
            }
        }
        if(!isHave)
        {
            itemDatas.Add(new ItemData(itemBase.itemType, itemBase.count));
        }

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

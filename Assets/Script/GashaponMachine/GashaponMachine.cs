using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum GashaponItemType
{
    //空奖
    NULL,
    //巨大金币
    Big_Gold,
    //护墙
    City_Wall,
    //钻石(超大金币，金币的另一种形式)
    Diamond,
    //金币雨
    Gold_Rain,
    //金币塔
    Gold_Tower,
    //震动
    Machine_Vibration,
    //巨大金币雨
    Big_Gold_Rain,
    //
    Special_Diamond,
    //碎片
    Special_Fragment,
    Letter_A,
    Letter_C,
    Letter_E,
    Letter_L,
    Test_debug,
}


public class GashaponMachine : MonoBehaviour,IEventListener
{
    public static bool isSpining = false;
    public static bool isAwaitGetReward = false;
    public Transform exitRoot;
    public Animation dengAnim;
    /// <summary>
    /// 机器中的扭蛋
    /// </summary>
    public List<GashaponMachineCell> machineGashaponcells;
    /// <summary>
    /// 扭蛋出来的道具
    /// </summary>
    public List<GashaponItemBase> gashaponItems;
    public AudioSource audioSource;
    public Dictionary<GashaponItemType, int> curRewards;
    private Dictionary<GashaponItemType, int> base_Rewards;
    private Dictionary<GashaponItemType, int> special_Rewards;

    private void OnEnable()
    {
        EventManager.Instance.RegisterListener(GameEvent.SpinGachapon,this);
        //dengAnim.Play("GashaponMachineDengAnim");
    }

    private void OnDisable()
    {
        EventManager.Instance.UnregisterListener(GameEvent.SpinGachapon,this);
    }

    private void Awake()
    {
        base_Rewards = new Dictionary<GashaponItemType, int>();
        base_Rewards.Add(GashaponItemType.NULL, 30);
        base_Rewards.Add(GashaponItemType.Big_Gold, 25);
        base_Rewards.Add(GashaponItemType.City_Wall, 10);
        base_Rewards.Add(GashaponItemType.Diamond, 8);
        base_Rewards.Add(GashaponItemType.Gold_Rain, 7);
        base_Rewards.Add(GashaponItemType.Gold_Tower, 3);
        base_Rewards.Add(GashaponItemType.Machine_Vibration, 10);
        base_Rewards.Add(GashaponItemType.Big_Gold_Rain, 5);

        special_Rewards = base_Rewards.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        special_Rewards.Add(GashaponItemType.Special_Diamond, 15);
        special_Rewards.Add(GashaponItemType.Special_Fragment, 5);
        special_Rewards.Add(GashaponItemType.Letter_A, 5);
        special_Rewards.Add(GashaponItemType.Letter_C, 5);
        special_Rewards.Add(GashaponItemType.Letter_E, 5);
        special_Rewards.Add(GashaponItemType.Letter_L, 5);

        curRewards = base_Rewards;
    }
 
    public void UpdateAppATT(GameObject _GashaponItem_Special_Diamond_Prefab, GameObject _gashaponItem_Special_Fragment_Prefab)
    {
        curRewards = special_Rewards;

        GameObject obj1 = Instantiate(_GashaponItem_Special_Diamond_Prefab, exitRoot);
        obj1.SetActive(false);
        GashaponItemBase item1 = obj1.GetComponent<GashaponItemBase>();
        SpriteRenderer icon1 = machineGashaponcells[15].transform.GetChild(0).transform.Find("icon").GetComponent<SpriteRenderer>();
        icon1.sprite = item1.icon.sprite;
        icon1.transform.localScale = Vector3.one * 0.5f;
        gashaponItems.Add(item1);

        GameObject obj2 = Instantiate(_gashaponItem_Special_Fragment_Prefab, exitRoot);
        obj2.SetActive(false);
        GashaponItemBase item2 = obj2.GetComponent<GashaponItemBase>();
        SpriteRenderer icon2 = machineGashaponcells[16].transform.GetChild(0).transform.Find("icon").GetComponent<SpriteRenderer>();
        icon2.sprite = item2.icon.sprite;
        icon2.transform.localScale = Vector3.one * 1.7f;
        gashaponItems.Add(item2);
    }
    public void GetRandomReward(GashaponItemType _customType)
    {
        GashaponItemBase targetItem = null;
        GashaponItemType targetType = _customType;
        if (targetType == GashaponItemType.Test_debug)
        {
            targetType = GetRandomItemByWeight();
        }
        //碎片检查
        targetType = CheckFragment(targetType);
        foreach (var item in gashaponItems)
        {
            if (targetType == item.type)
            {
                targetItem = item;
                break;
            }
        }
        //--------------------
        if (targetItem == null)
        {
            Debug.LogError($"未配置类型{targetType.ToString()}的扭蛋机道具");
            isAwaitGetReward = false;
            return;
        }

        CustomApiManager.Instance.RequestCustomEventV2(new CustomEventData()
        {
            page_id = "GashaponMachine",
            name = "Event_SlotResult",
            value = targetType.ToString(),
        });

        targetItem.transform.localPosition = new Vector3(0f, 0.048f, 0f);
        targetItem.transform.localScale = Vector3.zero;
        targetItem.transform.eulerAngles = Vector3.zero;
        targetItem.gameObject.SetActive(true);

        DOTween.Sequence().Append(targetItem.transform.DOLocalMoveY(0, 1f));
        DOTween.Sequence().Append(targetItem.transform.DORotate(new Vector3(0, 0, 720f), 1f, RotateMode.FastBeyond360));
        DOTween.Sequence().Append(targetItem.transform.DOScale(1, 1f)).AppendCallback(() =>
        {
            targetItem.gameObject.SetActive(false);
            //弹窗打开时间
            DOTween.Sequence().AppendInterval(95f / 60f).AppendCallback(() =>
            {
                //给奖
                targetItem.GetGashaponItemReward();
                isAwaitGetReward = false;
            });
            //弹窗
            UIManager.Instance.OpenUI<GashaponItemPanel>(targetItem, () =>
            {
                //targetItem.gameObject.SetActive(false);
            });
        });
    }
    public GashaponItemType CheckFragment(GashaponItemType _type)
    {
        if(_type != GashaponItemType.Special_Fragment)
        {
            return _type;
        }
        ItemFragment itemFragment = UIManager.Instance.mainBtnUI.itemFragment;
        if(itemFragment == null || !itemFragment.gameObject.activeSelf)
        {
            return GashaponItemType.Big_Gold;
        }
        List<MachineItemType> fragmentList = itemFragment.GetSpecialFragment();
        if(fragmentList.Count == 0)
        {
            return GashaponItemType.Big_Gold;
        }
        
        return GashaponItemType.Special_Fragment;
    }
    /// <summary>
    /// 根据权重随机抽取物品
    /// </summary>
    /// <returns>随机抽取的物品类型</returns>
    public GashaponItemType GetRandomItemByWeight()
    {
        // 1. 计算总权重
        int totalWeight = 0;
        foreach (var pair in curRewards)
        {
            // 过滤无效权重（避免负数或0）
            if (pair.Value > 0)
            {
                totalWeight += pair.Value;
            }
        }

        // 处理空字典或全无效权重的情况
        if (totalWeight <= 0)
        {
            return GashaponItemType.NULL;
        }

        // 2. 生成0到总权重之间的随机数
        int randomValue = UnityEngine.Random.Range(0,totalWeight);

        // 3. 遍历权重，找到随机数所在的区间
        int currentWeight = 0;
        foreach (var pair in curRewards)
        {
            if (pair.Value <= 0) continue; // 跳过无效权重

            if(pair.Key == GashaponItemType.Letter_A 
                || pair.Key == GashaponItemType.Letter_E
                || pair.Key == GashaponItemType.Letter_C
                || pair.Key == GashaponItemType.Letter_L
                )
            {
                if (TxElementMananger.Instance != null && !TxElementMananger.Instance.CheckIsCanDropLetter())
                {
                    continue;
                }
            }

            currentWeight += pair.Value;
            // 当累加权重超过随机数时，返回当前物品
            if (randomValue < currentWeight)
            {
                return pair.Key;
            }
        }

        return GashaponItemType.NULL;
    }


    public void OnEventTriggered(GameEvent eventType, object data = null)
    {
        StartSpin();
    }
    public void StartSpin(GashaponItemType _customType = GashaponItemType.Test_debug)
    {
        if (isAwaitGetReward)
            return;

        AudioManager.Instance.SetAudioSource(audioSource,"GashaponSpin");
        DOTween.Sequence().AppendInterval(0.8f).AppendCallback(() =>
        {
            AudioManager.Instance.SetAudioSource(audioSource, "GashaponSpin");
        })
        .AppendInterval(0.8f).AppendCallback(() =>
        {
            AudioManager.Instance.SetAudioSource(audioSource, "GashaponSpin");
        });
        isAwaitGetReward = true;
        StartCoroutine(GashaponMachineIE(_customType));
    }
    private IEnumerator GashaponMachineIE(GashaponItemType _customType)
    {
        isSpining = true;

        //dengAnim.Play("GashaponMachineDengAnim2");
        ApplyRandomAngleForceAllCells();
        yield return new WaitForSeconds(3f);
        //dengAnim.Play("GashaponMachineDengAnim");
        CancelForceAllCells();
        isSpining = false;
        GetRandomReward(_customType);
    }

    private void ApplyRandomAngleForceAllCells()
    {
        foreach (var cell in machineGashaponcells)
        {
            cell.ApplyRandomAngleForce();
        }
    }
    private void CancelForceAllCells()
    {
        foreach (var cell in machineGashaponcells)
        {
            cell.rig2D.velocity = Vector3.zero;
            cell.rig2D.angularVelocity = 0f;
        }
    }

    public void SetGashaponItemRatio(GashaponItemType targetType, float targetRatio)
    {
        // 校验占比合法性（必须0<ratio<1，避免无意义计算）
        if (targetRatio <= 0 || targetRatio >= 1)
        {
            return;
        }

        //计算其他物品的固定总权重
        int fixedTotal = 0;
        foreach (var pair in curRewards)
        {
            if (pair.Key != targetType)
            {
                fixedTotal += pair.Value;
            }
        }

        //按核心公式计算目标权重
        int targetWeight = Mathf.RoundToInt((fixedTotal * targetRatio) / (1 - targetRatio));

        // 赋值新权重
        curRewards[targetType] = targetWeight;
    }
}

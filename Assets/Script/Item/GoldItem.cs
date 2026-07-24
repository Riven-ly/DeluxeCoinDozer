using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldItem : ItemBase
{
    public override void Init(float _itemCnt)
    {
        base.Init(_itemCnt);
    }

    public override void GetItemReward()
    {
        GoldUI.isLongTimerAnim = true;
        EventManager.Instance.TriggerEvent(GameEvent.GetGold, (int)count);
    }
}

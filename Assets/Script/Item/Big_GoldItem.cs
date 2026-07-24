using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Big_GoldItem : ItemBase
{
    public override void Init(float _itemCnt)
    {
        base.Init(_itemCnt);
        cntText.text ="x" + count.ToString();
    }

    public override void GetItemReward()
    {
        List<object> dataList = new List<object>();
        dataList.Add(SceneItemType.Big_Gold);
        dataList.Add(count);
        EventManager.Instance.TriggerEvent(GameEvent.GetSceneItem, dataList);

    }
}

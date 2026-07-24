using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Machine_VibrationItem : ItemBase
{
    public override void Init(float _itemCnt)
    {
        base.Init(_itemCnt);
        cntText.text = "x" + count.ToString();
    }

    public override void GetItemReward()
    {
        List<object> dataList = new List<object>();
        dataList.Add(SceneItemType.Machine_Vibration);
        dataList.Add(count);
        EventManager.Instance.TriggerEvent(GameEvent.GetSceneItem, dataList);

    }
}

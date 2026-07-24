using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamondItem : ItemBase
{
    public List<float> scaleList;
    private void OnEnable()
    {
        GameManager.Instance.UpdateAppATTToDiamondRwardIcon(icon);

        if (scaleList == null || scaleList.Count == 0)
            return;

        float daxiao = (int)GameManager.appATTtype == 0 ? scaleList[0] : scaleList[1];
        icon.transform.localScale = new Vector3(daxiao, daxiao, 1f);
    }
    public override void Init(float _itemCnt)
    {
        base.Init(_itemCnt);
    }

    public override void GetItemReward()
    {
        DiamondUI.isLongTimerAnim = true;
        EventManager.Instance.TriggerEvent(GameEvent.GetDiamond, count);
    }
}

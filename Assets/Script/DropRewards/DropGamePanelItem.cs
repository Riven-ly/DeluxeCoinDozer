using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropGamePanelItem : MonoBehaviour
{
    public Transform item;
    [HideInInspector]public ItemBase itemBase;
    public void Init(ItemData _itemData)
    {
        gameObject.SetActive(true);
        itemBase = GameManager.Instance.CreatItem(_itemData, item, false);
        itemBase.cntText.gameObject.SetActive(false);

        DOTween.Sequence()
            .Append(transform.DOLocalMoveY(-2000f, 7f))
            .AppendCallback(() =>
            {
                Clear();
            })
            .SetTarget(this);
    }

    public void Clear()
    {
        this.DOKill();
        Destroy(gameObject);
    }
}

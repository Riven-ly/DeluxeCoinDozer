using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ItemType
{
    Null,
    Gold,
    Diamond,
    Big_Gold,
    City_Wall,
    Gold_Explode,
    Machine_Vibration,
}

public class ItemData
{
    public ItemType itemType;
    public float count;
    public ItemData(ItemType _itemType, float _count)
    {
        itemType = _itemType;
        count = _count;
    }
}
public class ItemBase : MonoBehaviour
{
    public ItemType itemType;
    public Image icon;
    public Text cntText;
    public Transform effect;
    [HideInInspector] public float count;
    public virtual void Init(float _itemCnt)
    {
        count = _itemCnt;
        cntText.text = count.ToString();  
    }

    public virtual void GetItemReward() { }
}

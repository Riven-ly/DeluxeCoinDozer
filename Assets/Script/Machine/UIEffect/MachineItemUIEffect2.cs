using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

public class MachineItemUIEffect2 : MonoBehaviour
{
    public static MachineItemUIEffect2 Instance;

    //gold
    public MachineItemUIEffect2Cell goldCell;
    private bool isAwaitGoldCount;
    private int goldCount;
    //
    private bool isAwaitOther;
    public MachineItemUIEffect2Cell2 bigGoldCell;
    public MachineItemUIEffect2Cell2 diamondCell;
    public MachineItemUIEffect2Cell2 suipianCell;

    private void Awake()
    {
        Instance = this;
    }
    
    public void PlayEffect(MachineItemType type)
    {
        if(type == MachineItemType.Gold)
        {
            goldCount++;
            if (!isAwaitGoldCount)
            {
                isAwaitGoldCount = true;
                DOTween.Sequence().AppendInterval(0.5f).AppendCallback(() =>
                {
                    goldCell.Init(goldCount, () =>
                    {
                        isAwaitGoldCount = false;
                        goldCount = 0;
                    });
                });
            }
        }
        else if(type == MachineItemType.BigGold)
        {
            if(!isAwaitOther)
            {
                isAwaitOther = true;
                bigGoldCell.Init(() =>
                {
                    isAwaitOther = false;
                });
            }
        }
        else if (type == MachineItemType.Diamond)
        {
            if (!isAwaitOther)
            {
                isAwaitOther = true;
                diamondCell.Init(() =>
                {
                    isAwaitOther = false;
                });
            }
        }
        else if (type == MachineItemType.SpecialFragment_1 
            || type == MachineItemType.SpecialFragment_2
            || type == MachineItemType.SpecialFragment_3
            || type == MachineItemType.SpecialFragment_4
            || type == MachineItemType.SpecialFragment_5
            || type == MachineItemType.SpecialFragment_6
            )
        {
            if (!isAwaitOther)
            {
                isAwaitOther = true;
                suipianCell.Init(() =>
                {
                    isAwaitOther = false;
                });
            }
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GashaponItemPanelInfo
{
    public Sprite icon;
    public string explain;
}

public class GashaponItemBase : MonoBehaviour
{
    public GashaponItemType type;
    public GashaponItemPanelInfo itemPanelInfo;

    public SpriteRenderer icon;
    public SpriteRenderer bg1;
    public SpriteRenderer bg2;
    private void Start()
    {
        itemPanelInfo = new GashaponItemPanelInfo();
        if(icon == null)
        {
            itemPanelInfo.icon = null;
        }
        else
        {
            itemPanelInfo.icon = icon.sprite;
        }
        itemPanelInfo.explain = GetExplain();

    }

    private string GetExplain()
    {
        string str = "";
        switch (type)
        {
            case GashaponItemType.NULL:
                str = LanguageManager.Instance.GetText("GashaponItem_Null");
                break;
            case GashaponItemType.Big_Gold:
                str = LanguageManager.Instance.GetText("GashaponItem_Big_Gold");
                break;
            case GashaponItemType.City_Wall:
                str = LanguageManager.Instance.GetText("GashaponItem_City_Wall");
                break;
            case GashaponItemType.Diamond:
                str = LanguageManager.Instance.GetText("GashaponItem_Diamond");
                break;
            case GashaponItemType.Gold_Rain:
                str = LanguageManager.Instance.GetText("GashaponItem_Gold_Rain");
                break;
            case GashaponItemType.Gold_Tower:
                str = LanguageManager.Instance.GetText("GashaponItem_Gold_Tower");
                break;
            case GashaponItemType.Machine_Vibration:
                str = LanguageManager.Instance.GetText("GashaponItem_Machine_Vibration");
                break;
            case GashaponItemType.Big_Gold_Rain:
                str = LanguageManager.Instance.GetText("GashaponItem_Big_Gold_Rain");
                break;
            case GashaponItemType.Special_Diamond:
                string jiamiStr = LanguageManager.Instance.GetText_Encrypt("Special_Diamond_mymymy");
                str = string.Format(LanguageManager.Instance.GetText("GashaponItem_Special_Diamond"), jiamiStr);
                break;
            case GashaponItemType.Special_Fragment:
                str = LanguageManager.Instance.GetText("GashaponItem_Special_Fragment");
                break;
            case GashaponItemType.Letter_A:
                str = LanguageManager.Instance.GetText("GashaponItem_Letter_A");
                break;
            case GashaponItemType.Letter_E:
                str = LanguageManager.Instance.GetText("GashaponItem_Letter_E");
                break;
            case GashaponItemType.Letter_C:
                str = LanguageManager.Instance.GetText("GashaponItem_Letter_C");
                break;
            case GashaponItemType.Letter_L:
                str = LanguageManager.Instance.GetText("GashaponItem_Letter_L");
                break;
        }
        
        return str;
    }

    public virtual void GetGashaponItemReward()
    {
        switch (type)
        {
            case GashaponItemType.NULL:
                break;
            case GashaponItemType.Big_Gold:
                GameManager.Instance.curMachine.GetBigGold();
                break;
            case GashaponItemType.City_Wall:
                GameManager.Instance.curMachine.Open_City_Wall();
                break;
            case GashaponItemType.Diamond:
                GameManager.Instance.curMachine.GetDiamond();
                break;
            case GashaponItemType.Gold_Rain:
                GameManager.Instance.curMachine.GetGoldRain();
                break;
            case GashaponItemType.Gold_Tower:
                GameManager.Instance.curMachine.GetGoldTower();
                break;
            case GashaponItemType.Machine_Vibration:
                GameManager.Instance.curMachine.ShakeMachine();
                break;
            case GashaponItemType.Big_Gold_Rain:
                GameManager.Instance.curMachine.GetBigGoldRain();
                break;
            case GashaponItemType.Special_Diamond:
                GameManager.Instance.curMachine.GetSpecialDiamond();
                break;
            case GashaponItemType.Special_Fragment:
                GameManager.Instance.curMachine.GetSpecialFragment();
                break;
            case GashaponItemType.Letter_A:
                if(TxElementMananger.Instance != null)
                {
                    TxElementMananger.Instance.GetLetter(GashaponItemType.Letter_A);
                }
                break;
            case GashaponItemType.Letter_E:
                if (TxElementMananger.Instance != null)
                {
                    TxElementMananger.Instance.GetLetter(GashaponItemType.Letter_E);
                }
                break;
            case GashaponItemType.Letter_C:
                if (TxElementMananger.Instance != null)
                {
                    TxElementMananger.Instance.GetLetter(GashaponItemType.Letter_C);
                }
                break;
            case GashaponItemType.Letter_L:
                if (TxElementMananger.Instance != null)
                {
                    TxElementMananger.Instance.GetLetter(GashaponItemType.Letter_L);
                }
                break;
        }
    }

    
}

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class ItemFragmentPanelCell : MonoBehaviour
{
    public Text title;
    public Image icon;
    public Slider slider;
    public Text sliderText;

    public void Init(Sprite _icon, ItemFragmentInfo _info)
    {
        icon.sprite = _icon;
        icon.SetNativeSize();
        title.text = "Fragment";
        sliderText.text = $"{_info.cnt}/{_info.maxCnt}";
        slider.maxValue = _info.maxCnt;
        slider.value = _info.cnt;
        switch (_info.type)
        {
            case MachineItemType.SpecialFragment_1:
                title.text = LanguageManager.Instance.GetText("SpecialFragment_1_Title");
                break;
            case MachineItemType.SpecialFragment_2:
                title.text = LanguageManager.Instance.GetText("SpecialFragment_2_Title");
                break;
            case MachineItemType.SpecialFragment_3:
                title.text = LanguageManager.Instance.GetText("SpecialFragment_3_Title");
                break;
            case MachineItemType.SpecialFragment_4:
                title.text = LanguageManager.Instance.GetText("SpecialFragment_4_Title");
                break;
            case MachineItemType.SpecialFragment_5:
                title.text = LanguageManager.Instance.GetText("SpecialFragment_5_Title");
                break;
            case MachineItemType.SpecialFragment_6:
                title.text = LanguageManager.Instance.GetText("SpecialFragment_6_Title");
                break;
        }
    }
}

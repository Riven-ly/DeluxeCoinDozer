using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TxElementHistoryCell : MonoBehaviour
{
    public Image icon;
    public Text count;
    public Text time;
    public Text state1;
    public Text state2;
    public Text state3;

    public void Init(TxElementHistoryInfo _info)
    {
        icon.sprite = TxElementMananger.Instance.accountTypeSprites[(int)_info.type];
        icon.SetNativeSize();

        string str1 = LanguageManager.Instance.GetText_Encrypt("Special_Diamond__unit");
        count.text = $"{str1}{_info.count}";

        DateTime timeDateTime = GameManager.TimeStampToDateTime(_info.time);
        time.text = timeDateTime.ToString("MM/dd/yyyy   HH:mm:ss");

        state1.text = LanguageManager.Instance.GetText("HistoryCell_state1");
        state2.text = LanguageManager.Instance.GetText_Encrypt("Pyg");
        state3.text = LanguageManager.Instance.GetText("HistoryCell_state3");

        state1.gameObject.SetActive(_info.state == 1);
        state2.gameObject.SetActive(_info.state == 2);
        state3.gameObject.SetActive(_info.state == 3);

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifficultMachineUI : MonoBehaviour
{
    public Text jinduText;
    public Text timeText;
    // Start is called before the first frame update
   

    public void UpdateJinduText(int _cnt, int _targetCnt)
    {
        jinduText.text = $"{_cnt}/{_targetCnt}";
    }
    public void UpdateTimeText(float _second)
    {
        int totalSeconds = Mathf.Max(0, (int)_second);
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;
        timeText.text = $"{minutes:D2}:{seconds:D2}";
    }
}

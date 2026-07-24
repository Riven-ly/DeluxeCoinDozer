using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.UI;

public class TxElementPanel_TaskCell : MonoBehaviour
{
    public Text title;
    public Text count;
    public Text explain;
    public Slider slider;
    public Text sliderText;
    public Text coolingTimeText;
    public Button submitBtn;

    private string coolingTimeText_front;
    private int curSecond;
    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        submitBtn.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayBtnMusic();
            UIManager.Instance.OpenUI<TxElementFinalStepPanel>();
        });
    }

    public void Init(TxElementTaskInfo _info)
    {
        string str1 = LanguageManager.Instance.GetText_Encrypt("Pym");
        title.text = string.Format(LanguageManager.Instance.GetText("TaskCell_title"), str1);
        explain.text = LanguageManager.Instance.GetText("TaskCell_explain");
        string str2 = LanguageManager.Instance.GetText_Encrypt("Special_Diamond__unit");

        float sliderValue = (float)_info.index / (float)_info.TargetIndex;
        sliderValue = Mathf.Clamp(sliderValue, 0.04f, 1f);
        slider.value = sliderValue;
        sliderText.text = $"{_info.index}/{_info.TargetIndex}";

        string str4 = LanguageManager.Instance.GetText_Encrypt("Special_Diamond__unit");
        count.text = $"{str4}{_info.diamond}";

        DateTime targetTime = GameManager.TimeStampToDateTime(_info.targetTime);
        DateTime curTime = GameManager.Instance.GetNowTime();
        TimeSpan targetTimeSpan = targetTime - curTime;
        coolingTimeText_front = LanguageManager.Instance.GetText("TaskCell_coolingTimeText");
        curSecond = (int)targetTimeSpan.TotalSeconds;
        timer = 0;
        submitBtn.interactable = _info.IsComplete;

        UpdateCoolingTimeText();
    }

    public void UpdateCoolingTimeText()
    {
        curSecond = Math.Max(0, curSecond);
        int hours = curSecond / 3600;
        int minutes = (curSecond % 3600) / 60;
        int seconds = curSecond % 60;

        string str3 = $"{hours:D2}:{minutes:D2}:{seconds:D2}";
        coolingTimeText.text = string.Format(coolingTimeText_front, str3);

        if(curSecond <= 0)
        {
            submitBtn.interactable = false;
        }
    }

    private void Update()
    {
        if(curSecond > 0)
        {
            timer += Time.deltaTime;
            if(timer > 1f)
            {
                timer = 0f;
                curSecond--;
                UpdateCoolingTimeText();
            }
        }
    }

}

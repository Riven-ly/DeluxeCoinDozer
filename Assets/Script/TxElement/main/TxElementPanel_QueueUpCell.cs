using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TxElementPanel_QueueUpCell : MonoBehaviour
{
    public const long Phase3Time = 108000;
    public const long Phase2Time = 43200;
    public const long Phase1Time = 21600;

    public Text title;
    public Text count;
    public Text explain;
    public Slider slider;
    public Text sliderText;
    public RewardAdButton rewardAdButton;
    public Button completeBtn;

    private string page_id = "TxElementPanel";
    void Start()
    {
        completeBtn.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayBtnMusic();
            TxElementManangerInfo _info = TxElementMananger.Instance.info;
            _info.taskInfo.Init();
            _info.taskInfo.diamond = _info.queueUpInfo.diamond;
            _info.queueUpInfo.Init();

            DateTime curTime = GameManager.Instance.GetNowTime();
            DateTime targetTime = curTime.AddHours(48);

            _info.taskInfo.targetTime = GameManager.DateTimeToTimeStamp(targetTime);
            _info.taskInfo.historyTime = GameManager.DateTimeToTimeStamp(curTime);

            _info.orderStatus = TxElementType.Task;
            TxElementMananger.Instance.SaveElementManangerInfo();
            UIManager.Instance.GetUI<TxElementPanel>().RefreshPanel();
        });
    }

    public void Init(TxElementQueueUpInfo _info)
    {
        int curLv = GameManager.Instance.playerInfo.playerData.level;

        string str1 = LanguageManager.Instance.GetText_Encrypt("WH");
        title.text = string.Format(LanguageManager.Instance.GetText("QueueUpCell_title"), str1);
       
        //---------------------------------------------------------------------------------------------------------------------
        DateTime targetTime = GameManager.TimeStampToDateTime(_info.targetTime);
        DateTime startTime = GameManager.TimeStampToDateTime(_info.startTime);
        DateTime curTime = GameManager.Instance.GetNowTime().AddSeconds(_info.playAdTime);//当前时间增加看广告的时间
        TimeSpan diffTimeSpan = targetTime - curTime;//剩下多少时间
        int awaitPeople = GetQueueCount((long)diffTimeSpan.TotalSeconds);//剩下的时间转化为多少人
        string str2 = "";
        if(awaitPeople > 999)
        {
            str2 = "999+";
        }
        else
        {
            str2 = awaitPeople.ToString();
        }
        string str3 = LanguageManager.Instance.GetText_Encrypt("wH");
        explain.text = string.Format(LanguageManager.Instance.GetText("QueueUpCell_explain"), str2, str3);
        
        //----------------------------------------------------------------------------------------------------------------------
        TimeSpan needTimeSpan = targetTime - startTime;//一共需要多少时间
        TimeSpan elapsedTimeSpan = curTime - startTime; //过去了多少时间
        float sliderValue = (float)elapsedTimeSpan.TotalSeconds / (float)needTimeSpan.TotalSeconds;
        sliderValue = Mathf.Clamp(sliderValue, 0.04f, 1f);
        slider.value = sliderValue;
        if(elapsedTimeSpan.TotalHours >= needTimeSpan.TotalHours)
        {
            sliderText.text = LanguageManager.Instance.GetText("Complete");
        }
        else
        {
            int _hours = (int)elapsedTimeSpan.TotalHours;
            _hours = Math.Max(0, _hours);
            sliderText.text = $"{_hours}h/{(int)needTimeSpan.TotalHours}h";
        }

        //-----------------------------------------------------------------------------------------------------------------------
        string str4 = LanguageManager.Instance.GetText_Encrypt("Special_Diamond__unit");
        count.text = $"{str4}{_info.diamond}";

        //--------------------------------------------------------------------------------
        bool isLast = curTime > targetTime;
        rewardAdButton.gameObject.SetActive(!isLast);
        completeBtn.gameObject.SetActive(isLast);

        rewardAdButton.Init(RewardAdCallback, page_id, false);
    }

    public void RewardAdCallback()
    {
        TxElementQueueUpInfo _QueueUpInfo = TxElementMananger.Instance.info.queueUpInfo;
        _QueueUpInfo.playAdTime += 3600;
        TxElementMananger.Instance.SaveElementManangerInfo();
        
        DOTween.Sequence()
            .AppendInterval(0.1f)
            .AppendCallback(() =>
        {
            Init(_QueueUpInfo);
        });
    }

    public int GetQueueCount(long remainingSeconds)
    {
        if (remainingSeconds < 1)
            return 0;


        // 阶段3：49~0人，耗时108000秒
        if (remainingSeconds <= Phase3Time)
        {
            // 每减少1人耗时约2204秒
            double interval = Phase3Time / 50.0;
            return Mathf.CeilToInt((float)(remainingSeconds / interval));
        }
        // 阶段2：199~50人，耗时43200秒
        else if (remainingSeconds <= Phase3Time + Phase2Time)
        {
            long phase2Elapsed = remainingSeconds - Phase3Time;
            // 每减少1人耗时约290秒
            double interval = Phase2Time / 150.0;
            return 50 + Mathf.CeilToInt((float)(phase2Elapsed / interval));
        }
        // 阶段1：2000~200人，耗时21600秒
        else if (remainingSeconds <= Phase3Time + Phase2Time + Phase1Time)
        {
            long phase1Elapsed = remainingSeconds - Phase3Time - Phase2Time;
            // 每减少1人耗时12秒
            double interval = Phase1Time / 1800.0;
            return 200 + Mathf.CeilToInt((float)(phase1Elapsed / interval));
        }
        // 超过总时长，人数为2000人
        else
        {
            return 2000;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DailyWheel : MonoBehaviour, IEventListener
{
    public Button clickBtn;
    public GameObject redDot;
    private void OnEnable()
    {
        EventManager.Instance.RegisterListener(GameEvent.DailyWheel, this);
    }

    private void OnDisable()
    {
        EventManager.Instance.UnregisterListener(GameEvent.DailyWheel, this);
    }

    void Start()
    {
        clickBtn.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayBtnMusic();
            UIManager.Instance.OpenUI<DailyWheelPanel>();
        });
    }

    public void OpenState(bool isOpen)
    {
        gameObject.SetActive(isOpen);
        if (!isOpen)
        {
            return;
        }
        UpdateRedDotState(!CheckDailyWheel());
    }
    private void UpdateRedDotState(bool _bool)
    {
        redDot.gameObject.SetActive(_bool);
    }
    public void OnEventTriggered(GameEvent eventType, object data = null)
    {
        UpdateRedDotState(false);
    }

    public static void DailyWheelRecord()
    {
        DateTime currentDate = GameManager.Instance.GetNowTime();
        PlayerPrefs.SetString("DailyWheel_LastDate", GameManager.DateTimeToTimeStamp(currentDate).ToString());
        PlayerPrefs.Save();
        EventManager.Instance.TriggerEvent(GameEvent.DailyWheel);
    }

    public static bool CheckDailyWheel()
    {
        string lastDateStr = PlayerPrefs.GetString("DailyWheel_LastDate", "");
        if (string.IsNullOrEmpty(lastDateStr))
        {
            return false;
        }

        DateTime currentDate = GameManager.Instance.GetNowTime();
        DateTime lastSignDate = GameManager.TimeStampToDateTime(ulong.Parse(lastDateStr));
        //Áè³¿ÅÐ¶Ï
        DateTime todayMidnight = new DateTime(
            currentDate.Year,
            currentDate.Month,
            currentDate.Day,
            0, 0, 0
        );

        DateTime lastSignMidnight = new DateTime(
            lastSignDate.Year,
            lastSignDate.Month,
            lastSignDate.Day,
            0, 0, 0
        );

        TimeSpan timeDiff = todayMidnight - lastSignMidnight;
        if (timeDiff.TotalDays >= 1)
        {
            return false;
        }

        return true;
    }

    
}

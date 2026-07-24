using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DailySignIn : MonoBehaviour, IEventListener
{
    public Button clickBtn;
    public GameObject redDot;

    public static int currentDay;

    private void OnEnable()
    {
        EventManager.Instance.RegisterListener(GameEvent.SignIn, this);
    }

    private void OnDisable()
    {
        EventManager.Instance.UnregisterListener(GameEvent.SignIn, this);
    }

    void Start()
    {
        clickBtn.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayBtnMusic();
            OpenDailySignInPanel();
        });
    }

    public void OpenState(bool isOpen)
    {
        gameObject.SetActive(isOpen);
        if(!isOpen)
        {
            return;
        }
        UpdateRedDotState(!CheckSignIn());
    }

    private void OpenDailySignInPanel()
    {
        UIManager.Instance.OpenUI<DailySignInPanel>();
    }

    private void UpdateRedDotState(bool _bool)
    {
        redDot.gameObject.SetActive(_bool);
    }
    public void OnEventTriggered(GameEvent eventType, object data = null)
    {
        UpdateRedDotState(false);
    }

    public static void SignIn(int _day)
    {
        DateTime currentDate = GameManager.Instance.GetNowTime();
        PlayerPrefs.SetString("SignIn_LastDate", GameManager.DateTimeToTimeStamp(currentDate).ToString());
        PlayerPrefs.SetInt("SignIn_CurrentDay", _day);
        PlayerPrefs.Save();
        EventManager.Instance.TriggerEvent(GameEvent.SignIn);
    }

    public static bool CheckSignIn()
    {
        currentDay = PlayerPrefs.GetInt("SignIn_CurrentDay", 0);

        string lastDateStr = PlayerPrefs.GetString("SignIn_LastDate", "");
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
            currentDay = currentDay >= 7 ? 0 : currentDay;
            return false;
        }

        return true;
    }


}

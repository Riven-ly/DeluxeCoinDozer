using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifficultMachineEnter : MonoBehaviour, IEventListener
{
    public Button clickBtn;
    public GameObject redDot;

    private void OnEnable()
    {
        EventManager.Instance.RegisterListener(GameEvent.Daily_DifficultMachine, this);
    }

    private void OnDisable()
    {
        EventManager.Instance.UnregisterListener(GameEvent.Daily_DifficultMachine, this);
    }
    // Start is called before the first frame update
    void Start()
    {
        clickBtn.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayBtnMusic();
            UIManager.Instance.OpenUI<DifficultMachineEnterPanel>();
            PlayerPrefs.SetString("DifficultMachineEnterYindao", "YES");

            CustomApiManager.Instance.RequestCustomEventV2(new CustomEventData()
            {
                page_id = "Daily Challenge",
                name = "Event_HardEntry",
                value = "",
            });
        });
    }

    public void OpenState(bool isOpen)
    {
        gameObject.SetActive(isOpen);
        if (!isOpen)
        {
            return;
        }
        UpdateRedDotState(!CheckDailyDifficult());

        string s = PlayerPrefs.GetString("DifficultMachineEnterYindao");
        if (string.IsNullOrEmpty(s))
        {
            UIBase.awaitHideAction = () =>
            {
                string s = LanguageManager.Instance.GetText_Encrypt("CH");
                string s2 = string.Format(LanguageManager.Instance.GetText("DifficultMachineEnterYindao"), s);

                List<object> listdata = new List<object>();
                listdata.Add(s2);
                listdata.Add(clickBtn);
                listdata.Add(false);

                UIManager.Instance.OpenUI<GameMainBtnYindaoPanel>(listdata);

                CustomApiManager.Instance.RequestCustomEventV2(new CustomEventData()
                {
                    page_id = "Yindao",
                    name = "Event_GuideStep",
                    value = "step2",
                });
            };
        }
    }

    private void UpdateRedDotState(bool _bool)
    {
        redDot.gameObject.SetActive(_bool);
    }
    public void OnEventTriggered(GameEvent eventType, object data = null)
    {
        DailyDifficultSignIn();
    }
    private  void DailyDifficultSignIn()
    {
        DateTime currentDate = GameManager.Instance.GetNowTime();
        PlayerPrefs.SetString("Daily_DifficultMachine", GameManager.DateTimeToTimeStamp(currentDate).ToString());
        PlayerPrefs.Save();
        UpdateRedDotState(false);
    }

    public static bool CheckDailyDifficult()
    {
        string lastDateStr = PlayerPrefs.GetString("Daily_DifficultMachine", "");
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

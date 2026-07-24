using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

public class TxElementMananger : MonoBehaviour,IEventListener
{
    public static TxElementMananger Instance;

    public List<Sprite> accountTypeSprites;

    [HideInInspector] public TxElementManangerInfo info;

    private float timer;
    private void Awake()
    {
        Instance = this;
    }
    private void OnEnable()
    {
        EventManager.Instance.RegisterListener(GameEvent.LevelUp, this);
        EventManager.Instance.RegisterListener(GameEvent.PlayAds, this);
        EventManager.Instance.RegisterListener(GameEvent.GetDiamond, this);
    }
    private void OnDisable()
    {
        EventManager.Instance.UnregisterListener(GameEvent.LevelUp, this);
        EventManager.Instance.UnregisterListener(GameEvent.PlayAds, this);
        EventManager.Instance.UnregisterListener(GameEvent.GetDiamond, this);
    }
    private void Start()
    {
        Init();
    }
    private void Update()
    {
        if(info.orderStatus == TxElementType.Task)
        {
            timer -= Time.deltaTime;
            if(timer < 0)
            {
                timer = 1f;
                DateTime targetTime = GameManager.TimeStampToDateTime(info.taskInfo.targetTime);
                DateTime curTime = GameManager.Instance.GetNowTime();
                if (curTime > targetTime)
                {
                    TaskPast();
                }
            }
        }
    }
    public void Init()
    {
        InitElementManangerInfo();
    }
    public void TaskPast()
    {
        //历史
        TxElementHistoryInfo historyInfo = new TxElementHistoryInfo();
        historyInfo.type = info.accountInfo.type;
        historyInfo.count = info.taskInfo.diamond;
        historyInfo.time = GameManager.DateTimeToTimeStamp(GameManager.Instance.GetNowTime());
        historyInfo.state = 3;
        info.historyInfo.Add(historyInfo);

        info.initInfo.diamond += info.taskInfo.diamond;
        info.taskInfo.Init();
        info.orderStatus = TxElementType.Init;

        SaveElementManangerInfo();

        CustomApiManager.Instance.RequestCustomEventV2(new CustomEventData()
        {
            page_id = "TXPanel",
            name = "Event_TXStatus",
            value = " failed",
        });
    }
    //检查是否可以掉落字母
    public bool CheckIsCanDropLetter()
    {
        try
        {
            if (info == null || info.taskInfo == null)
            {
                return false;
            }
            if (info.orderStatus == TxElementType.Task && info.taskInfo.IsComplete)
            {
                return true;
            }
            return false;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public void GetLetter(GashaponItemType type)
    {
        switch (type)
        {
            case GashaponItemType.Letter_A:
                if(info.taskInfo.isHave_A)
                {
                    EventManager.Instance.TriggerEvent(GameEvent.GetGold, 50);
                }
                info.taskInfo.isHave_A = true;
                break;
            case GashaponItemType.Letter_E:
                if (info.taskInfo.isHave_E)
                {
                    EventManager.Instance.TriggerEvent(GameEvent.GetGold, 50);
                }
                info.taskInfo.isHave_E = true;
                break;
            case GashaponItemType.Letter_C:
                if (info.taskInfo.isHave_C)
                {
                    EventManager.Instance.TriggerEvent(GameEvent.GetGold, 50);
                }
                info.taskInfo.isHave_C = true;
                break;
            case GashaponItemType.Letter_L:
                if (info.taskInfo.isHave_L)
                {
                    EventManager.Instance.TriggerEvent(GameEvent.GetGold, 50);
                }
                info.taskInfo.isHave_L = true;
                break;
        }
        SaveElementManangerInfo();
    }

    public void OnEventTriggered(GameEvent eventType, object data = null)
    {
        if(eventType == GameEvent.LevelUp)
        {
            if(info != null && info.orderStatus == TxElementType.Task)
            {
                info.taskInfo.index += 1;
                info.taskInfo.index = Mathf.Clamp(info.taskInfo.index, 0, info.taskInfo.TargetIndex);
                SaveElementManangerInfo();
            }
        }
        else if (eventType == GameEvent.PlayAds)
        {
            if (info != null && info.orderStatus == TxElementType.Task)
            {
                int count = 1;
                if(data != null)
                {
                    count = (int)data;
                }
                info.taskInfo.index += 2 * count;
                info.taskInfo.index = Mathf.Clamp(info.taskInfo.index, 0, info.taskInfo.TargetIndex);
                SaveElementManangerInfo();
            }
        }
        else if (eventType == GameEvent.GetDiamond)
        {
            if (info != null)
            {
                info.initInfo.diamond += (float)data;
                SaveElementManangerInfo();
            }
        }
    }

    public void OpenState(bool isOpen)
    {
        gameObject.SetActive(isOpen);
        if (!isOpen)
        {
            return;
        }

    }

    private void InitElementManangerInfo()
    {
        string jsonStr = PlayerPrefs.GetString("TxElementManangerInfo", "");
        if(string.IsNullOrEmpty(jsonStr))
        {
            info = new TxElementManangerInfo();
            info.accountInfo = new TxElementAccountInfo();
            info.initInfo = new TxElementInitInfo();
            info.queueUpInfo = new TxElementQueueUpInfo();
            info.taskInfo = new TxElementTaskInfo();
            info.historyInfo = new List<TxElementHistoryInfo>();
        }
        else
        {
            info = JsonConvert.DeserializeObject<TxElementManangerInfo>(jsonStr);
        }
    }

    public void SaveElementManangerInfo()
    {
        string jsonStr = JsonConvert.SerializeObject(info, Formatting.Indented);
        PlayerPrefs.SetString("TxElementManangerInfo", jsonStr);
        PlayerPrefs.Save();
        Debug.Log("TxElementManangerInfo :" + jsonStr);
    }
}

public enum TxElementType
{
    Init,
    QueueUp,
    Task
}

public enum TxElementAccountType
{
    type1,
    type2,
    type3,
    type4,
    type5,
    type6,
    type7,
    type8
}
public class TxElementManangerInfo
{
    public TxElementType orderStatus;
    public TxElementAccountInfo accountInfo;
    public TxElementInitInfo initInfo;
    public TxElementQueueUpInfo queueUpInfo;
    public TxElementTaskInfo taskInfo;

    public List<TxElementHistoryInfo> historyInfo;
}
public class TxElementAccountInfo
{
    public TxElementAccountType type;
    public string email;
    public TxElementAccountInfo()
    {
        type = TxElementAccountType.type1;
        email = "";
    }
}

public class TxElementInitInfo
{
    public float diamond;
    public int targetLv;
    public TxElementInitInfo()
    {
        Init();
    }

    public void Init()
    {
        diamond = 0f;
        targetLv = 20;
    }
}

public class TxElementQueueUpInfo
{
    public float diamond;
    public ulong startTime;//秒时间戳
    public ulong targetTime;//秒时间戳
    public ulong playAdTime; //秒
    public TxElementQueueUpInfo()
    {
        Init();
    }
    public void Init()
    {
        diamond = 0f;
        startTime = 0;
        targetTime = 0;
        playAdTime = 0;
    }
}

public class TxElementTaskInfo
{
    public float diamond;
    public ulong targetTime;//秒时间戳
    public int index;
    public int TargetIndex;

    public ulong historyTime;//历史记录时间
    public bool IsComplete
    {
        get
        {
            return index >= TargetIndex;
        }
    }
    public bool isHave_C;
    public bool isHave_L;
    public bool isHave_E;
    public bool isHave_A;
    public bool isHave_R;

    public TxElementTaskInfo()
    {
        Init();
    }
    public void Init()
    {
        diamond = 0f;
        targetTime = 0;
        index = 0;
        TargetIndex = 100;
        historyTime = 0;
        isHave_C = false;
        isHave_L = false;
        isHave_E = false;
        isHave_A = false;
        isHave_R = false;
    }
}

public class TxElementHistoryInfo
{
    public TxElementAccountType type;
    public ulong time;//秒时间戳
    public float count;
    public int state; 
}


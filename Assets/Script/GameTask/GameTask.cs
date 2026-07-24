using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.UI;
public class GameTask : MonoBehaviour,IEventListener
{
    public Button clickBtn;
    public GameObject redDot;


    public DailyGameTask dailyGameTask;
    public OtherGameTask otherGameTask;
    private void OnEnable()
    {
        EventManager.Instance.RegisterListener(GameEvent.SaveGameTask, this);
        EventManager.Instance.RegisterListener(GameEvent.GetMachineItemReward, this);
        EventManager.Instance.RegisterListener(GameEvent.PlayAds, this);
        EventManager.Instance.RegisterListener(GameEvent.SpinWheel, this);
        EventManager.Instance.RegisterListener(GameEvent.SpinGachapon, this);
        EventManager.Instance.RegisterListener(GameEvent.UpdateTaskRedDot, this);

    }
    private void OnDisable()
    {
        EventManager.Instance.UnregisterListener(GameEvent.SaveGameTask, this);
        EventManager.Instance.UnregisterListener(GameEvent.GetMachineItemReward, this);
        EventManager.Instance.UnregisterListener(GameEvent.PlayAds, this);
        EventManager.Instance.UnregisterListener(GameEvent.SpinWheel, this);
        EventManager.Instance.UnregisterListener(GameEvent.SpinGachapon, this);
        EventManager.Instance.UnregisterListener(GameEvent.UpdateTaskRedDot, this);
    }
    // Start is called before the first frame update
    void Start()
    {
        clickBtn.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayBtnMusic();
            UIManager.Instance.OpenUI<GameTaskPanel>(this);
        });
    }

    public void InitGameTask()
    {
        InitDailyGameTask();
        InitOtherGameTask();
    }

    private void InitDailyGameTask()
    {
        string data = PlayerPrefs.GetString("DailyGameTask", "");
        if (string.IsNullOrEmpty(data))
        {
            ResetDailyGameTask();
            SaveDailyGameTask();
        }
        else
        {
            dailyGameTask = JsonConvert.DeserializeObject<DailyGameTask>(data);
            string curTime = GameManager.Instance.GetNowTime().ToString("yyyy-MM-dd");
            if (curTime != dailyGameTask.curTime)
            {
                ResetDailyGameTask();
            }
        }
    }

    private void InitOtherGameTask()
    {
        string data = PlayerPrefs.GetString("OtherGameTask", "");
        if (string.IsNullOrEmpty(data))
        {
            otherGameTask = new OtherGameTask();
            List<GameTaskInfo> _lvtasks = new List<GameTaskInfo>();
            _lvtasks.Add(new GameTaskInfo(GameTaskType.LevelUp, 2, 5));
            _lvtasks.Add(new GameTaskInfo(GameTaskType.LevelUp, 5, 10));
            _lvtasks.Add(new GameTaskInfo(GameTaskType.LevelUp, 10, 20));
            _lvtasks.Add(new GameTaskInfo(GameTaskType.LevelUp, 20, 30));
            _lvtasks.Add(new GameTaskInfo(GameTaskType.LevelUp, 30, 40));
            _lvtasks.Add(new GameTaskInfo(GameTaskType.LevelUp, 40, 50));
            _lvtasks.Add(new GameTaskInfo(GameTaskType.LevelUp, 50, 70));
            _lvtasks.Add(new GameTaskInfo(GameTaskType.LevelUp, 75, 100));
            _lvtasks.Add(new GameTaskInfo(GameTaskType.LevelUp, 100, 150));
            _lvtasks.Add(new GameTaskInfo(GameTaskType.LevelUp, 150, 200));
            otherGameTask.lvUpGameTask = _lvtasks;

            List<GameTaskInfo> _Gap_tasks = new List<GameTaskInfo>();
            _Gap_tasks.Add(new GameTaskInfo(GameTaskType.SpinGashaponMachine, 50, 10));
            _Gap_tasks.Add(new GameTaskInfo(GameTaskType.SpinGashaponMachine, 100, 20));
            _Gap_tasks.Add(new GameTaskInfo(GameTaskType.SpinGashaponMachine, 150, 30));
            _Gap_tasks.Add(new GameTaskInfo(GameTaskType.SpinGashaponMachine, 200, 40));
            _Gap_tasks.Add(new GameTaskInfo(GameTaskType.SpinGashaponMachine, 300, 50));
            _Gap_tasks.Add(new GameTaskInfo(GameTaskType.SpinGashaponMachine, 400, 70));
            _Gap_tasks.Add(new GameTaskInfo(GameTaskType.SpinGashaponMachine, 500, 100));
            _Gap_tasks.Add(new GameTaskInfo(GameTaskType.SpinGashaponMachine, 600, 125));
            _Gap_tasks.Add(new GameTaskInfo(GameTaskType.SpinGashaponMachine, 700, 150));
            _Gap_tasks.Add(new GameTaskInfo(GameTaskType.SpinGashaponMachine, 800, 200));
            _Gap_tasks.Add(new GameTaskInfo(GameTaskType.SpinGashaponMachine, 900, 250));
            _Gap_tasks.Add(new GameTaskInfo(GameTaskType.SpinGashaponMachine, 1000, 300));
            otherGameTask.gachaponGameTask = _Gap_tasks;
            SaveOtherGameTask();
        }
        else
        {
            otherGameTask = JsonConvert.DeserializeObject<OtherGameTask>(data);
        }
    }

    public void OnEventTriggered(GameEvent eventType, object data = null)
    {
        switch (eventType)
        {
            case GameEvent.SaveGameTask:
                SaveDailyGameTask();
                SaveOtherGameTask();
                break;
            case GameEvent.GetMachineItemReward: 
                GetMachineGameTaskRecord(data);
                break;
            case GameEvent.PlayAds:
                PlayAdsGameTaskRecord();
                break;
            case GameEvent.SpinWheel:
                SpinWheelGameTaskRecord();
                break;
            case GameEvent.SpinGachapon:
                GachaponyGameTaskRecord();
                break;
            case GameEvent.UpdateTaskRedDot:
                UpdateRedDotState();
                break;
        }
    }

    private void GetMachineGameTaskRecord(object data = null)
    {
        foreach (var task in dailyGameTask.dailyTasks)
        {
            if (task.gameTaskType == GameTaskType.GetGold && !task.IsComplete)
            {
                task.cnt += (int)data;
                task.cnt = Mathf.Clamp(task.cnt, 0, task.maxCnt);
            }
        }
        UpdateRedDotState();
    }

    private void PlayAdsGameTaskRecord()
    {
        foreach (var task in dailyGameTask.dailyTasks)
        {
            if (task.gameTaskType == GameTaskType.PlayAds && !task.IsComplete)
            {
                task.cnt++;
                task.cnt = Mathf.Clamp(task.cnt, 0, task.maxCnt);
            }
        }
        UpdateRedDotState();
    }

    private void SpinWheelGameTaskRecord()
    {
        foreach (var task in dailyGameTask.dailyTasks)
        {
            if (task.gameTaskType == GameTaskType.SpinWheel && !task.IsComplete)
            {
                task.cnt++;
                task.cnt = Mathf.Clamp(task.cnt, 0, task.maxCnt);
            }
        }
        UpdateRedDotState();
    }

    private void GachaponyGameTaskRecord()
    {
        foreach (var task in otherGameTask.gachaponGameTask)
        {
            if (task.gameTaskType == GameTaskType.SpinGashaponMachine && !task.IsComplete)
            {
                task.cnt ++;
                task.cnt = Mathf.Clamp(task.cnt, 0, task.maxCnt);
            }
        }
        UpdateRedDotState();
    }

    private void ResetDailyGameTask()
    {
        dailyGameTask = new DailyGameTask();
        dailyGameTask.curTime = GameManager.Instance.GetNowTime().ToString("yyyy-MM-dd");
        dailyGameTask.dailyTasks = new List<GameTaskInfo>();
        GameTaskInfo task1 = new GameTaskInfo(GameTaskType.GetGold, 25, 5);
        GameTaskInfo task2 = new GameTaskInfo(GameTaskType.PlayAds, 5, 5);
        GameTaskInfo task3 = new GameTaskInfo(GameTaskType.SpinWheel, 3, 15);

        dailyGameTask.dailyTasks.Add(task1);
        dailyGameTask.dailyTasks.Add(task2);
        dailyGameTask.dailyTasks.Add(task3);
    }


    private void SaveDailyGameTask()
    {
        if (dailyGameTask == null)
            return;

        string jsonStr = JsonConvert.SerializeObject(dailyGameTask, Formatting.Indented);
        if(jsonStr == null)
        {
            return;
        }
        PlayerPrefs.SetString("DailyGameTask", jsonStr);
        //PlayerPrefs.Save();
        Debug.Log("每日任务数据保存成功：" + jsonStr);
    }

    private void SaveOtherGameTask()
    {
        if (otherGameTask == null)
            return;

        string jsonStr = JsonConvert.SerializeObject(otherGameTask, Formatting.Indented);
        if (jsonStr == null)
        {
            return;
        }
        PlayerPrefs.SetString("OtherGameTask", jsonStr);
        PlayerPrefs.Save();
        Debug.Log("其他任务数据保存成功：" + jsonStr);
    }

    public static string GetTaskTypeExplain(GameTaskType type)
    {
        string explain = "";
        switch (type)
        {
            case GameTaskType.Null:
                break;
            case GameTaskType.GetGold:
                explain = LanguageManager.Instance.GetText("TaskType_GetGold");
                break;
            case GameTaskType.PlayAds:
                explain = LanguageManager.Instance.GetText("TaskType_PlayAds");
                break;
            case GameTaskType.SpinWheel:
                explain = LanguageManager.Instance.GetText("TaskType_SpinWheel");
                break;
            case GameTaskType.SpinGashaponMachine:
                explain = LanguageManager.Instance.GetText("TaskType_SpinGashaponMachine");
                break;
            case GameTaskType.LevelUp:
                explain = LanguageManager.Instance.GetText("TaskType_LevelUp");
                break;
        }
        return explain;
    }
    public void OpenState(bool isOpen)
    {
        gameObject.SetActive(isOpen);
        if (!isOpen)
        {
            return;
        }
        UpdateRedDotState();
    }

    private void UpdateRedDotState()
    {
        if (dailyGameTask == null || otherGameTask == null)
            return;

        //有没有可以领取的任务奖励
        bool isHaveTack = false;
        foreach (var task in dailyGameTask.dailyTasks)
        {
            if(task.IsComplete && !task.isCollect)
            {
                isHaveTack = true;
            }
        }
        foreach (var task in otherGameTask.lvUpGameTask)
        {
            task.cnt = GameManager.Instance.playerInfo.playerData.level;
            if (task.IsComplete && !task.isCollect)
            {
                isHaveTack = true;
            }
        }
        foreach (var task in otherGameTask.gachaponGameTask)
        {
            if (task.IsComplete && !task.isCollect)
            {
                isHaveTack = true;
            }
        }
        redDot.gameObject.SetActive(isHaveTack);
    }


}

public class DailyGameTask
{
    public List<GameTaskInfo> dailyTasks;
    public string curTime;
}

public class OtherGameTask
{
    public List<GameTaskInfo> lvUpGameTask;
    public List<GameTaskInfo> gachaponGameTask;
}

public enum GameTaskType
{
    Null,
    GetGold,
    PlayAds,
    SpinWheel,
    SpinGashaponMachine,
    LevelUp,
}

public class GameTaskInfo
{
    public GameTaskType gameTaskType;
    public string explain;
    public int cnt;
    public int maxCnt;
    public float reward;

    public bool isCollect;
    public bool IsComplete
    {
        get
        {
            return cnt >= maxCnt;
        }
    }

    public GameTaskInfo(GameTaskType _type, int _maxCnt, float _reward)
    {
        gameTaskType = _type;
        explain = GameTask.GetTaskTypeExplain(_type);
        cnt = 0;
        maxCnt = _maxCnt;
        reward = _reward;
        isCollect = false;
    }
}
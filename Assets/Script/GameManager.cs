using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static bool isPause = false;
    //appATTtype
    public static int appATTtype = 0;
    public static bool LoadABAsyncOK = false;
    public static bool TrySceneClick
    {
        get
        {
            return !UIManager.Instance.CheckIstheUIopen();
        }
    }

    public PlayerInfo playerInfo;
    public OrdinaryMachine ordinaryMachine;
    public DifficultMachine difficultMachine;
    //道具预制体
    public List<GameObject> itemPrefabs;
    //奖励道具预制体
    public List<GameObject> itemRewardPrefabs;

    [HideInInspector]public MachineBase curMachine;
    private float autoSaveGameDataTime = 31f;
    private float autoSaveGameDataTimer = 31f;

    public List<Sprite> Diamonds;
    public List<Sprite> DiamondRerardIcons;
    //
    private float EvaluationGameColing = 0f;
    //----debug
    public int addDebug_hours = 0;
    private void Awake()
    {
        Application.runInBackground = false;
        Application.targetFrameRate = 120;

        Instance = this;
        playerInfo = new PlayerInfo();
        playerInfo.Init();
    }

    void Start()
    {
        difficultMachine.gameObject.SetActive(false);
        curMachine = ordinaryMachine;
        //预创建
        for (int i = 0; i < 300; i++)
        {
            curMachine.GameBeforeCreatMachineItem(MachineItemType.Gold);
        }
        curMachine.ClearMachine();
    }

    // Update is called once per frame
    void Update()
    {
        if(EvaluationGameColing > 0)
        {
            EvaluationGameColing -= Time.deltaTime;
        }
        autoSaveGameDataTimer -= Time.deltaTime;
        if(autoSaveGameDataTimer < 0f)
        {
            autoSaveGameDataTimer = autoSaveGameDataTime;
            SaveGameData();
        }
    }

    public void EnterOrdinaryMachine(bool isAnim =true)
    {
        curMachine = ordinaryMachine;
        ordinaryMachine.Init();
        UIManager.Instance.Init();
        ordinaryMachine.gameObject.SetActive(true);

        if (!isAnim)
            return;

        UIManager.Instance.mainBtnUI.EnterBaseMachineUIAnim2();
        UIManager.Instance.playInfoUI.EnterBaseMachineUIAnim2();
    }
    public void SwitchDifficultMachine()
    {
        SaveGameData();
        ordinaryMachine.ClearMachine();
        ordinaryMachine.gameObject.SetActive(false);

        curMachine = difficultMachine;
        difficultMachine.Init();
        UIManager.Instance.Init();
        difficultMachine.gameObject.SetActive(true);
        UIManager.Instance.mainBtnUI.EnterDifficultMachineUIAnim();
        UIManager.Instance.playInfoUI.EnterDifficultMachineUIAnim();
    }
    public void UpdateAppATT()
    {
        int sAtt = PlayerPrefs.GetInt("UpdateAppATT", 0);
        if (sAtt == 1)
        {
            appATTtype = 1;
        }

        //LoadOtherPrefab();
        LoadABAsyncOK = true;
        if (appATTtype == 1)
        {
            if (sAtt != 1)
            {
                PlayerPrefs.SetInt("UpdateAppATT", appATTtype);
            }
            LoadAssetBundleAsync();
        }
    }

    private async void LoadAssetBundleAsync()
    {
        Debug.Log("开始异步加载");
        LoadABAsyncOK = false;
        // 异步加载 AB 包
        AssetBundle ab = await ABbuildManager.LoadABAsync();
        if (ab == null)
        {
            Debug.LogError("AB 包加载失败");
            LoadABAsyncOK = true;
            return;
        }

        if (appATTtype == 1)
        {
            AssetBundleRequest request2 = ab.LoadAssetAsync<GameObject>("assets/abres/abresmanager/abresmanager.prefab");
            await request2;

            GameObject prefab = request2.asset as GameObject;
            if (prefab != null)
            {
                Instantiate(prefab);
                Debug.Log("加载并生成：ABResManager");
            }
        }
        Debug.Log("异步加载完成");
        LoadABAsyncOK = true;
    }


    public ItemBase CreatItem(ItemData data, Transform _root, bool isReward = false)
    {
        if (data == null || _root == null) return null;

        var obj = Instantiate(GetItemPrefab(data.itemType, isReward), _root);
        obj.transform.localPosition = Vector3.zero;
        ItemBase itemObjs = obj.GetComponent<ItemBase>();
        itemObjs.Init(data.count);
        return itemObjs;
    }
    /// <summary>
    /// 创建道具
    /// </summary>
    /// <param name="itemDatas"></param>
    /// <param name="_root"></param>
    /// <returns>道具对象ItemBase</returns>
    public List<ItemBase> CreatItems(List<ItemData> itemDatas, Transform _root, bool isReward = false)
    {
        if (itemDatas == null || _root == null) return null;
        List<ItemBase> itemObjs = new List<ItemBase>();
        foreach (var item in itemDatas)
        {
            var obj = Instantiate(GetItemPrefab(item.itemType, isReward), _root);
            obj.transform.localPosition = Vector3.zero;
            obj.GetComponent<ItemBase>().Init(item.count);
            itemObjs.Add(obj.GetComponent<ItemBase>());
        }
        return itemObjs;
    }

    private GameObject GetItemPrefab(ItemType itemType, bool isReward = false)
    {
        GameObject itemObj = null;
        List<GameObject> prefabs = isReward == false ? itemPrefabs : itemRewardPrefabs;
        foreach (var item in prefabs)
        {
            ItemBase itemBase = item.GetComponent<ItemBase>();
            if (itemBase != null && itemBase.itemType == itemType)
            {
                return itemBase.gameObject;
            }
        }
        return itemObj;
    }


    public void UpdateAppATTToDiamond(Image image)
    {
        image.sprite = Diamonds[(int)appATTtype];
        image.SetNativeSize();
    }
    public void UpdateAppATTToDiamondRwardIcon(Image image)
    {
        image.sprite = DiamondRerardIcons[(int)appATTtype];
        image.SetNativeSize();
    }

    /// <summary>
    /// 获取当前时间
    /// </summary>
    /// <returns></returns>
    public DateTime GetNowTime()
    {
        DateTime newNow = DateTime.UtcNow;
        //newNow = newNow.AddHours(addDebug_hours);
        return newNow;
    }
    public static DateTime TimeStampToDateTime(ulong timestamp)
    {
        // Unix时间戳的基准时间（UTC）
        DateTime utcEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        return utcEpoch.AddSeconds(timestamp);
    }
    /// <summary>
    /// 获取秒级时间戳
    /// </summary>
    public static ulong DateTimeToTimeStamp(DateTime nowUtc)
    {
        // 基准时间：1970-01-01 00:00:00 UTC
        DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        TimeSpan timeSpan = nowUtc - epoch;
        return (ulong)timeSpan.TotalSeconds;
    }
    /// <summary>
    /// 获取微秒级时间戳
    /// </summary>
    public static ulong GetMicrosecondTimestamp(DateTime nowUtc)
    {
        // 基准时间：1970-01-01 00:00:00 UTC
        DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        TimeSpan timeSpan = nowUtc - epoch;
        ulong microseconds = (ulong)(timeSpan.TotalMilliseconds * 1000);
        return microseconds;
    }

    public static bool CheckSimpleEmail(string email)
    {
        // 空值/空字符串直接返回false
        if (string.IsNullOrEmpty(email)) return false;

        // 1. 必须包含且仅包含一个@
        int atIndex = email.IndexOf('@');
        if (atIndex == -1 || atIndex != email.LastIndexOf('@')) return false;

        // 2. @前后必须有内容
        string front = email.Substring(0, atIndex);
        string back = email.Substring(atIndex + 1);
        if (string.IsNullOrEmpty(front) || string.IsNullOrEmpty(back)) return false;

        // 3. @后面必须包含至少一个.（且.不能在开头/结尾）
        int dotIndex = back.IndexOf('.');
        if (dotIndex == -1 || dotIndex == 0 || dotIndex == back.Length - 1) return false;

        // 满足以上条件则认为格式有效
        return true;
    }
    public static string GetAccountEmail()
    {
        string str = PlayerPrefs.GetString("AccountEmail", "");
        return str;
    }

    public static void SaveAccountEmail(string _str)
    {
        PlayerPrefs.SetString("AccountEmail", _str);
        PlayerPrefs.Save();
    }

    private void SaveGameData()
    {
        Debug.Log("保存游戏数据");
        if(ordinaryMachine.gameObject.activeSelf)
        {
            ordinaryMachine.SaveMachineItems();
        }
        playerInfo.SavePlayerData();
        EventManager.Instance.TriggerEvent(GameEvent.SaveGameTask);
    }

    public void TryEvaluationGame()
    {
        int evaluationGameStar = PlayerPrefs.GetInt("EvaluationGameStar", 0);
        if (evaluationGameStar == 5)//5星评分
        {
            return;
        }
        if (EvaluationGameColing > 0)
        {
            return;
        }
        EvaluationGameColing = 300f;

        DOTween.Sequence().AppendInterval(0.5f).AppendCallback(() =>
        {
            UIManager.Instance.OpenUI<EvaluationGamePanel>();
        });
    }


    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            Debug.Log("【游戏切后台】");
            SaveGameData();
        }
    }

    private void OnApplicationQuit()
    {
        Debug.Log("【游戏退出】");
        SaveGameData();
    }

}

public static class AssetBundleRequestExtensions
{
    public static Awaiter GetAwaiter(this AssetBundleRequest request)
    {
        return new Awaiter(request);
    }

    public struct Awaiter : INotifyCompletion
    {
        private readonly AssetBundleRequest _request;

        public Awaiter(AssetBundleRequest request)
        {
            _request = request;
        }

        public bool IsCompleted => _request.isDone;

        public void OnCompleted(Action continuation)
        {
            _request.completed += _ => continuation();
        }

        public void GetResult() { }
    }
}
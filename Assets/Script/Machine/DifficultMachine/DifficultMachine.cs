using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultMachine : MachineBase,IEventListener
{
    private static List<DifficultMachineInfo> difficultMachineInfos;
    public static bool isPause;
    public DifficultMachineUI difficultMachineUI;

    private int machineGold;
    private int curLv;
    private float gameTime;
    private float timer;
    private int curGetItemCnt;
    private bool isInit = false;

    private void OnEnable()
    {
         EventManager.Instance.RegisterListener(GameEvent.DifficultMachineRecordDiamond, this);
    }
    private void OnDisable()
    {
         EventManager.Instance.UnregisterListener(GameEvent.DifficultMachineRecordDiamond, this);
    }
    public void Init()
    {
        cur_page_id = "DifficultMachine";
        InitDifficultMachineInfos();

        List<MachineItemSaveData> saveInfos = new List<MachineItemSaveData>();
        foreach (Transform item in prefabs)
        {
            MachineItemInfo info = item.GetComponent<MachineItemInfo>();
            saveInfos.Add(MachineItemInfoToSaveData(info));
        }
        foreach (var saveInfo in saveInfos)
        {
            var obj = ObjectPoolManager.Instance.GetObject(saveInfo.machineItemType);
            if (obj == null)
            {
                Debug.LogError($"对象池{saveInfo.machineItemType}为空");
                continue;
            }
            obj.transform.SetParent(goldParent);
            obj.transform.position = new Vector3(saveInfo.x, saveInfo.y, saveInfo.z);
            obj.transform.eulerAngles = new Vector3(saveInfo.r_x, saveInfo.r_y, saveInfo.r_z);
            machineItems.Add(obj.GetComponent<MachineItemInfo>());
        }

        machineGold = 50;
        curLv = GetDifficultMachineLv();
        SaveDifficultMachineLv(curLv);
        InitGameInfo();
        isInit = true;

        AudioManager.Instance.PlayBGM("BGM2");

        CustomApiManager.Instance.RequestCustomEventV2(new CustomEventData()
        {
            page_id = "Daily Challenge",
            name = "Event_HardStart",
            value = $"stage :{curLv}",
        });
    }

    public void InitGameInfo()
    {
        DifficultMachineInfo difficultMachineInfo = difficultMachineInfos[curLv - 1];
        gameTime = difficultMachineInfo.gameTime;
        timer = 0f;
        curGetItemCnt = 0;
        gashaponMachine.SetGashaponItemRatio(difficultMachineInfo.targetMachineItemType, difficultMachineInfo.targetTypeWeight);
        difficultMachineUI.UpdateJinduText(curGetItemCnt, difficultMachineInfo.targetCnt);
        difficultMachineUI.UpdateTimeText(gameTime);
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            MachineClick();
        }
        //时间
        if (!isInit)
        {
            return;
        }
        if (GameManager.isPause)
        {
            return;
        }
        timer += Time.deltaTime;
        if(timer> 1f)
        {
            timer = 0f;
            gameTime--;
            UpdateGameTime();
        }
    }

    private void UpdateGameTime()
    {
        difficultMachineUI.UpdateTimeText(gameTime);
        if(gameTime <= 0)
        {
            EventManager.Instance.TriggerEvent(GameEvent.Daily_DifficultMachine);
            //结束弹窗
            //游戏结束
            UIManager.Instance.OpenUI<DifficultMachineGameLosePanel>(curLv, () =>
            {
                GameExit();
            });

            CustomApiManager.Instance.RequestCustomEventV2(new CustomEventData()
            {
                page_id = "Daily Challenge",
                name = "Event_HardFail",
                value = $"stage :{curLv}",
            });
        }
    }

    public void GameExit()
    {
        ClearMachine();
        gameObject.SetActive(false);
        GameManager.Instance.EnterOrdinaryMachine();
    }

    public void NextLevel()
    {
        List<object> datas = new List<object>();
        datas.Add(curLv);
        datas.Add(difficultMachineInfos[curLv - 1].rewardDatas);

        //打开过关弹窗
        UIManager.Instance.OpenUI<DifficultMachineNextLevelPanel>(datas, () =>
        {
            //给奖
            var rewardItemDatas = difficultMachineInfos[curLv - 1].rewardDatas;
            foreach (var item in rewardItemDatas)
            {
                if (item.itemType == ItemType.Gold)
                {
                    GameManager.Instance.playerInfo.AddGold((int)item.count);
                }
                else if (item.itemType == ItemType.Diamond)
                {
                    EventManager.Instance.TriggerEvent(GameEvent.GetDiamond, item.count);
                }
            }

            CustomApiManager.Instance.RequestCustomEventV2(new CustomEventData()
            {
                page_id = "Daily Challenge",
                name = "Event_HardSuccess",
                value = $"stage :{curLv}",
            });

            CustomApiManager.Instance.RequestCustomEventV2(new CustomEventData()
            {
                page_id = "GoldGain",
                name = "Event_GoldGain",
                value = "hard_mode_reward",
            });

            curLv++;
            if (curLv >= 5)
            {
                EventManager.Instance.TriggerEvent(GameEvent.Daily_DifficultMachine);
            }
            SaveDifficultMachineLv(curLv);
            GameExit();
            UIManager.Instance.OpenUI<DifficultMachineEnterPanel>();
        });   
    }

    public void OnEventTriggered(GameEvent eventType, object data = null)
    {
        if(eventType == GameEvent.DifficultMachineRecordDiamond)
        {
            curGetItemCnt++;
            int targetCnt = difficultMachineInfos[curLv - 1].targetCnt;
            difficultMachineUI.UpdateJinduText(curGetItemCnt, targetCnt);
            if(curGetItemCnt >= targetCnt)
            {
                //下一关
                NextLevel();
            }
        }
    }
    public override int GetGold()
    {
        return machineGold;
    }
    public override void AddGold(int _cnt)
    {
        machineGold += _cnt;
        machineGold = Mathf.Clamp(machineGold, 0, 9999);
    }

    public override void ExpendGold(int _cnt)
    {
        machineGold -= _cnt;
        machineGold = Mathf.Clamp(machineGold, 0, 9999);
    }

    public override void GetMachineItemReward(int _cnt)
    {
        AddGold(_cnt);
    }

    public List<DifficultMachineInfo> GetDifficultMachineInfo()
    {
        if(difficultMachineInfos == null)
        {
            InitDifficultMachineInfos();
        }
        return difficultMachineInfos;
    }

    public static int GetDifficultMachineLv()
    {
        int lv = PlayerPrefs.GetInt("DifficultMachineLv", 1);
        if(!CheckDailyCurLv())
        {
            lv = 1;
        }
        return lv;
    }
    private void SaveDifficultMachineLv(int _lv)
    {
        DailyDifficultCurLv();
        PlayerPrefs.SetInt("DifficultMachineLv", _lv);
        PlayerPrefs.Save();
    }

    private void DailyDifficultCurLv()
    {
        DateTime currentDate = GameManager.Instance.GetNowTime();
        PlayerPrefs.SetString("Daily_CurLv", GameManager.DateTimeToTimeStamp(currentDate).ToString());
        PlayerPrefs.Save();
    }

    private static bool CheckDailyCurLv()
    {
        string lastDateStr = PlayerPrefs.GetString("Daily_CurLv", "");
        if (string.IsNullOrEmpty(lastDateStr))
        {
            return false;
        }

        DateTime currentDate = GameManager.Instance.GetNowTime();
        DateTime lastSignDate = GameManager.TimeStampToDateTime(ulong.Parse(lastDateStr));
        //凌晨判断
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


    private void InitDifficultMachineInfos()
    {
        if (difficultMachineInfos != null)
        {
            return;
        }
        difficultMachineInfos = new List<DifficultMachineInfo>();
        difficultMachineInfos.Add(new DifficultMachineInfo()
        {
            gameTime = 301,
            targetMachineItemType = GashaponItemType.Diamond,
            targetCnt = 5,
            targetTypeWeight = 0.5f,
            rewardDatas = new List<ItemData>()
            {
                new ItemData(ItemType.Gold, 100)
            }
        });
        difficultMachineInfos.Add(new DifficultMachineInfo()
        {
            gameTime = 240,
            targetMachineItemType = GashaponItemType.Diamond,
            targetCnt = 7,
            targetTypeWeight = 0.3f,
            rewardDatas = new List<ItemData>()
            {
                new ItemData(ItemType.Gold, 200)
            }
        });
        difficultMachineInfos.Add(new DifficultMachineInfo()
        {
            gameTime = 180,
            targetMachineItemType = GashaponItemType.Diamond,
            targetCnt = 8,
            targetTypeWeight = 0.2f,
            rewardDatas = new List<ItemData>()
            {
                new ItemData(ItemType.Diamond, 50)
            }
        });
        difficultMachineInfos.Add(new DifficultMachineInfo()
        {
            gameTime = 120,
            targetMachineItemType = GashaponItemType.Diamond,
            targetCnt = 10,
            targetTypeWeight = 0.1f,
            rewardDatas = new List<ItemData>()
            {
                new ItemData(ItemType.Diamond, 100)
            }
        });
    }

}

public class DifficultMachineInfo
{
    public float gameTime;
    //目标道具类型
    public GashaponItemType targetMachineItemType;
    //目标道具数量
    public int targetCnt;
    public float targetTypeWeight;
    //奖励
    public List<ItemData> rewardDatas;
}
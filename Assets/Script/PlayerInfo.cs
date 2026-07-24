using Newtonsoft.Json;
using System;
using UnityEngine;

[Serializable]
public class PlayerInfo
{
    public PlayerData playerData;
    private LevelConfig levelConfig = new LevelConfig();
    public void Init()
    {
        LoadPlayerData();
    }
    public int GetGold()
    {
        return playerData.gold;
    }
    public void AddGold(int cnt)
    {
        playerData.gold += cnt;
        playerData.gold = Mathf.Clamp(playerData.gold, 0, 9999);
    }
    public void ExpendGold(int cnt)
    {
        playerData.gold -= cnt;
        playerData.gold = Mathf.Clamp(playerData.gold, 0, 9999);
    }

    public float GetDiamond()
    {
        return playerData.diamond;
    }
    public void AddDiamond(float cnt)
    {
        playerData.diamond += cnt;
        playerData.diamond = (float)((int)(playerData.diamond * 100)) / 100;
    }

    public void AddExperience(int addExp)
    {
        if (playerData.level < 1)
        {
            return;
        }
        if (playerData.level >= 150)
        {
            return;
        }

        //增加经验
        playerData.levelExperience += addExp;

        while (CanLevelUp())
        {
            //执行单次升级逻辑
            LevelUp();
            Debug.Log("升级:" + playerData.level);
            EventManager.Instance.TriggerEvent(GameEvent.LevelUp);

            CustomApiManager.Instance.RequestCustomEventV2(new CustomEventData()
            {
                page_id = "LevelUp",
                name = "Event_LevelUp",
                value = playerData.level.ToString(),
            });
        }
    }
    public LevelConfigData GetLevelConfigData(int level = -1)
    {
        if(level == -1)
        {
            return levelConfig.GetLevelData(playerData.level);
        }
        return levelConfig.GetLevelData(level);
    }
    private bool CanLevelUp()
    {
        // 满级直接返回false
        if (playerData.level >= 150) return false;
        // 获取当前等级的升级所需经验
        LevelConfigData curLvData = GetLevelConfigData();
        // 当前经验 >= 升级所需经验 → 可以升级
        return playerData.levelExperience >= curLvData.levelExperience;
    }
    private void LevelUp()
    {
        LevelConfigData curLvData = GetLevelConfigData();

        playerData.levelExperience -= curLvData.levelExperience;
        playerData.level++;
        if (playerData.level >= 150)
        {
            playerData.levelExperience = 0;
        }
    }

    public void SavePlayerData()
    {
        if (playerData == null) return;
        string jsonStr = JsonConvert.SerializeObject(playerData, Formatting.Indented);
        PlayerPrefs.SetString("PlayerInfo", jsonStr);
        PlayerPrefs.Save();
        Debug.Log("基本信息数据保存成功：" + jsonStr);
    }

    public void LoadPlayerData()
    {
        string jsonStr = PlayerPrefs.GetString("PlayerInfo","");
        if(string.IsNullOrEmpty(jsonStr))
        {
            playerData = new PlayerData()
            {
                gold = 45,        // 默认金币
                level = 1,         // 默认等级
                levelExperience = 0,// 默认经验
                diamond = 0      // 默认钻石
            };
            SavePlayerData();
        }
        else
        {
            playerData = JsonConvert.DeserializeObject<PlayerData>(jsonStr);
        }
    }

}

[Serializable]
public class PlayerData
{
    public int gold;
    public int level;
    public int levelExperience;
    public float diamond;


}
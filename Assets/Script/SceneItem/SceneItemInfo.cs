using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class SceneItemInfo : MonoBehaviour,IEventListener
{
    SceneItemInfo_Data data;

    public SceneItemBase big_Gold_Item;
    public SceneItemBase city_Wall_Item;
    public SceneItemBase gold_Explode_Item;
    public SceneItemBase machine_Vibration_Item;

    private void OnEnable()
    {
        EventManager.Instance.RegisterListener(GameEvent.UseSceneItem, this);
        EventManager.Instance.RegisterListener(GameEvent.GetSceneItem, this);
        EventManager.Instance.RegisterListener(GameEvent.Open_City_Wall_Btn, this);
        EventManager.Instance.RegisterListener(GameEvent.Hide__City_Wall_Btn, this);
    }
    private void OnDisable()
    {
        EventManager.Instance.UnregisterListener(GameEvent.UseSceneItem, this);
        EventManager.Instance.UnregisterListener(GameEvent.GetSceneItem, this);
        EventManager.Instance.UnregisterListener(GameEvent.Open_City_Wall_Btn, this);
        EventManager.Instance.UnregisterListener(GameEvent.Hide__City_Wall_Btn, this);
    }
    public void Init()
    {
        string dataStr = PlayerPrefs.GetString("SceneItemInfo_Data", "");
        if (string.IsNullOrEmpty(dataStr))
        {
            data = new SceneItemInfo_Data();
            SaveData();
        }
        else
        {
            data = JsonConvert.DeserializeObject<SceneItemInfo_Data>(dataStr);
        }

        big_Gold_Item.Init(data.big_Gold_Cnt);
        city_Wall_Item.Init(data.city_Wall_Cnt);
        gold_Explode_Item.Init(data.gold_Explode_Cnt);
        machine_Vibration_Item.Init(data.machine_Vibration_Cnt);
    }

    private void SaveData()
    {
        string jsonData = JsonConvert.SerializeObject(data,Formatting.Indented);
        PlayerPrefs.SetString("SceneItemInfo_Data", jsonData);
        PlayerPrefs.Save();
    }

    public void OnEventTriggered(GameEvent eventType, object data = null)
    {
        if (eventType == GameEvent.UseSceneItem)
        {
            UseSceneItem((SceneItemType)data);
        }
        else if (eventType == GameEvent.GetSceneItem)
        {
            GetSceneItem(data);
        }
        else if(eventType == GameEvent.Open_City_Wall_Btn)
        {
            city_Wall_Item.SetBtnAction(true);
        }
        else if (eventType == GameEvent.Hide__City_Wall_Btn)
        {
            city_Wall_Item.SetBtnAction(false);
        }
    }

    private void UseSceneItem(SceneItemType type)
    {
        Debug.Log("使用道具 ：" + type.ToString());
        if(type == SceneItemType.Big_Gold)
        {
            data.big_Gold_Cnt--;
            if(data.big_Gold_Cnt < 0)
            {
                data.big_Gold_Cnt = 0;
            }
            big_Gold_Item.RefreshUI(data.big_Gold_Cnt);
            GameManager.Instance.curMachine.GetBigGold();
        }
        else if (type == SceneItemType.City_Wall)
        {
            data.city_Wall_Cnt--;
            if (data.city_Wall_Cnt < 0)
            {
                data.city_Wall_Cnt = 0;
            }
            city_Wall_Item.RefreshUI(data.city_Wall_Cnt);
            GameManager.Instance.curMachine.Open_City_Wall();
        }
        else if (type == SceneItemType.Gold_Explode)
        {
            data.gold_Explode_Cnt--;
            if (data.gold_Explode_Cnt < 0)
            {
                data.gold_Explode_Cnt = 0;
            }
            gold_Explode_Item.RefreshUI(data.gold_Explode_Cnt);
            GameManager.Instance.curMachine.GetGoldExplode();
        }
        else if (type == SceneItemType.Machine_Vibration)
        {
            data.machine_Vibration_Cnt--;
            if (data.machine_Vibration_Cnt < 0)
            {
                data.machine_Vibration_Cnt = 0;
            }
            machine_Vibration_Item.RefreshUI(data.machine_Vibration_Cnt);
            GameManager.Instance.curMachine.ShakeMachine();
        }
        SaveData();
    }
    private void GetSceneItem(object _data)
    {
        List<object> dataList = _data as List<object>;
        var type = (SceneItemType)dataList[0];
        int cnt = (int)(float)dataList[1];

        Debug.Log("获得道具 ：" + type.ToString());
        if (type == SceneItemType.Big_Gold)
        {
            data.big_Gold_Cnt += cnt;
            big_Gold_Item.RefreshUI(data.big_Gold_Cnt);
        }
        else if (type == SceneItemType.City_Wall)
        {
            data.city_Wall_Cnt += cnt;
            city_Wall_Item.RefreshUI(data.city_Wall_Cnt);
        }
        else if (type == SceneItemType.Gold_Explode)
        {
            data.gold_Explode_Cnt += cnt;
            gold_Explode_Item.RefreshUI(data.gold_Explode_Cnt);
        }
        else if (type == SceneItemType.Machine_Vibration)
        {
            data.machine_Vibration_Cnt+= cnt;
            machine_Vibration_Item.RefreshUI(data.machine_Vibration_Cnt);
        }
        SaveData();
    }
}

public class SceneItemInfo_Data
{
    public int big_Gold_Cnt;
    public int city_Wall_Cnt;
    public int gold_Explode_Cnt;
    public int machine_Vibration_Cnt;

    public SceneItemInfo_Data()
    {
        big_Gold_Cnt = 1;
        city_Wall_Cnt = 1;
        gold_Explode_Cnt = 1;
        machine_Vibration_Cnt = 1;
    }
}

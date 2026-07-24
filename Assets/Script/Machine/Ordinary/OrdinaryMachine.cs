using DG.Tweening;
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

public class OrdinaryMachine : MachineBase
{
    private bool isInit = false;
    public void Init()
    {
        cur_page_id = "OrdinaryMachine";
        List<MachineItemSaveData> saveInfos; 
        string str = PlayerPrefs.GetString("OrdinaryMachineItems", "");
        if(string.IsNullOrEmpty(str))
        {
            saveInfos = new List<MachineItemSaveData>();
            foreach (Transform item in prefabs)
            {
                MachineItemInfo info = item.GetComponent<MachineItemInfo>();
                saveInfos.Add(MachineItemInfoToSaveData(info));
            }
        }
        else
        {
            saveInfos = JsonConvert.DeserializeObject<List<MachineItemSaveData>>(str);
        }
           
        //创建
        foreach (var saveInfo in saveInfos)
        {
            var obj = ObjectPoolManager.Instance.GetObject(saveInfo.machineItemType);
            if(obj == null)
            {
                Debug.LogError($"对象池{saveInfo.machineItemType}为空");
                continue;
            }
            obj.transform.SetParent(goldParent);
            obj.transform.position = new Vector3(saveInfo.x, saveInfo.y, saveInfo.z);
            obj.transform.eulerAngles = new Vector3(saveInfo.r_x, saveInfo.r_y, saveInfo.r_z);
            machineItems.Add(obj.GetComponent<MachineItemInfo>());
        }
        isInit = true;

        AudioManager.Instance.PlayBGM("BGM");
    }

    public override int GetGold()
    {
        return GameManager.Instance.playerInfo.GetGold();
    }
    public override void AddGold(int _cnt)
    {
        GameManager.Instance.playerInfo.AddGold(_cnt);
    }

    public override void ExpendGold(int _cnt)
    {
        GameManager.Instance.playerInfo.ExpendGold(_cnt);
    }

    public override void GetMachineItemReward(int _cnt)
    {
        if (!isInit)
        {
            return;
        }
        AddGold(_cnt);
        GameManager.Instance.playerInfo.AddExperience(_cnt);
    }

    public void SaveMachineItems()
    {
        if(!isInit)
        {
            return;
        }
        List<MachineItemSaveData> saveInfos = new List<MachineItemSaveData>();
        foreach (var info in machineItems)
        {
            saveInfos.Add(MachineItemInfoToSaveData(info));
        }
        string jsonStr = JsonConvert.SerializeObject(saveInfos, Formatting.Indented);
        PlayerPrefs.SetString("OrdinaryMachineItems", jsonStr);
        PlayerPrefs.Save();
        Debug.Log("普通机器数据保存成功：" + jsonStr);
    }

}

public class MachineItemSaveData
{
    public MachineItemType machineItemType;
    public float x;
    public float y;
    public float z;
    public float r_x;
    public float r_y;
    public float r_z;
}

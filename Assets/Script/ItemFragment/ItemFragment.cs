using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;

public class ItemFragment : MonoBehaviour,IEventListener
{
    public List<ItemFragmentSpriteInfo> spriteInfos;
    public Button clickBtn;
    public GameObject redDot;

    [HideInInspector]public List<ItemFragmentInfo> itemFragmentInfos;
    private const int limitFragmentMax = 90;

    private void OnEnable()
    {
        EventManager.Instance.RegisterListener(GameEvent.GetMachineItemReward_SpecialFragment, this);
    }
    private void OnDisable()
    {
        EventManager.Instance.UnregisterListener(GameEvent.GetMachineItemReward_SpecialFragment, this);
    }
    void Start()
    {
        clickBtn.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayBtnMusic();
            UIManager.Instance.OpenUI<ItemFragmentPanel>(this);
        });

        string json = PlayerPrefs.GetString("ItemFragment", "");
        if(string.IsNullOrEmpty(json))
        {
            ResetitemFragmentInfos();
        }
        else
        {
            itemFragmentInfos = JsonConvert.DeserializeObject<List<ItemFragmentInfo>>(json);
        }
    }
    public void OpenState(bool isOpen)
    {
        gameObject.SetActive(isOpen);
        if (!isOpen)
        {
            return;
        }
        UpdateRedDotState(false);
    }
    private void UpdateRedDotState(bool _bool)
    {
        redDot.gameObject.SetActive(_bool);
    }
    public void OnEventTriggered(GameEvent eventType, object data = null)
    {
        if(eventType == GameEvent.GetMachineItemReward_SpecialFragment)
        {
            List<object> curdata = data as List<object>;
            MachineItemType type = (MachineItemType)curdata[0];
            int cnt = (int)curdata[1];
            foreach (var info in itemFragmentInfos)
            {
                if(info.type == type)
                {
                    info.cnt += cnt;
                    info.cnt = Mathf.Clamp(info.cnt, 0, limitFragmentMax);
                    SaveitemFragmentInfos();

                    CustomApiManager.Instance.RequestCustomEventV2(new CustomEventData()
                    {
                        page_id = "GameShard",
                        name = "Event_ShardNum",
                        value = $"type:{info.type.ToString()},count :{info.cnt}",
                    });
                    break;
                }
            }
            ItemFragmentEffect.Instance.StartEffect(GetItemFragmentSprite(type));
        }
    }

    public List<MachineItemType> GetSpecialFragment()
    {
        List<MachineItemType> datas = new List<MachineItemType>();
        foreach (var info in itemFragmentInfos)
        {
            if(info.cnt < limitFragmentMax)
            {
                datas.Add(info.type);
            }
        }
        return datas;
    }


    private void ResetitemFragmentInfos()
    {
        itemFragmentInfos = new List<ItemFragmentInfo>();
        itemFragmentInfos.Add(new ItemFragmentInfo(MachineItemType.SpecialFragment_1, 100));
        itemFragmentInfos.Add(new ItemFragmentInfo(MachineItemType.SpecialFragment_2, 100));
        itemFragmentInfos.Add(new ItemFragmentInfo(MachineItemType.SpecialFragment_3, 100));
        itemFragmentInfos.Add(new ItemFragmentInfo(MachineItemType.SpecialFragment_4, 100));
        itemFragmentInfos.Add(new ItemFragmentInfo(MachineItemType.SpecialFragment_5, 100));
        itemFragmentInfos.Add(new ItemFragmentInfo(MachineItemType.SpecialFragment_6, 100));
        SaveitemFragmentInfos();
    }

    private void SaveitemFragmentInfos()
    {
        string json = JsonConvert.SerializeObject(itemFragmentInfos, Formatting.Indented);
        PlayerPrefs.SetString("ItemFragment", json);
        PlayerPrefs.Save();
    }

    public Sprite GetItemFragmentSprite(MachineItemType _type)
    {
        Sprite sp = spriteInfos[0].sprite;
        foreach (var item in spriteInfos)
        {
            if(item.type == _type)
            {
                sp = item.sprite;
                break;
            }
        }
        return sp;
    }

}

[Serializable]
public class ItemFragmentSpriteInfo
{
    public MachineItemType type;
    public Sprite sprite;
}

public class ItemFragmentInfo
{
    public MachineItemType type;
    public int cnt;
    public int maxCnt;

    public ItemFragmentInfo(MachineItemType _type, int _maxCnt)
    {
        type = _type;
        cnt = 0;
        maxCnt = _maxCnt;
    }
}

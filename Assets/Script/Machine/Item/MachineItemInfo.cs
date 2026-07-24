using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Ã®…œµ¿æﬂ¿‡–Õ
/// </summary>
public enum MachineItemType
{
    Gold,
    BigGold,
    Diamond,
    SpecialDiamond,
    SpecialFragment_1,
    SpecialFragment_2,
    SpecialFragment_3,
    SpecialFragment_4,
    SpecialFragment_5,
    SpecialFragment_6,
}
public class MachineItemInfo : MonoBehaviour
{
    public MachineItemType machineItemType;
    public int rewardCnt;
    public AudioSource audioSource;

    [HideInInspector] public Rigidbody rb;
    private Vector3 initScale;
    private Vector3 initEulerAngles;

    private void Awake()
    {
        initScale = transform.localScale;
        initEulerAngles = transform.eulerAngles;
        rb = GetComponent<Rigidbody>();
    }

    public void Init(Vector3 initPos)
    {
        transform.position = initPos;
        transform.eulerAngles = initEulerAngles;
        transform.localScale = initScale;
    }

    public void GetMachineItemReward()
    {
        Vector3 objPos = transform.position;

        if(machineItemType == MachineItemType.Diamond)
        {
            EventManager.Instance.TriggerEvent(GameEvent.DifficultMachineRecordDiamond);
        }
        //ÀÈ∆¨µ¿æﬂΩ±¿¯
        if (machineItemType == MachineItemType.SpecialFragment_1
           || machineItemType == MachineItemType.SpecialFragment_2
           || machineItemType == MachineItemType.SpecialFragment_3
           || machineItemType == MachineItemType.SpecialFragment_4
           || machineItemType == MachineItemType.SpecialFragment_5
           || machineItemType == MachineItemType.SpecialFragment_6)
        {
            //ÀÈ∆¨¬ﬂº≠
            List<object> datas = new List<object>();
            datas.Add(machineItemType);
            datas.Add(1);//–¥À¿Ω±¿¯1
            EventManager.Instance.TriggerEvent(GameEvent.GetMachineItemReward_SpecialFragment, datas);
        }
        else if (machineItemType == MachineItemType.SpecialDiamond)
        {
            float rewardCnt = 10f;//–¥À¿Ω±¿¯10
            EventManager.Instance.TriggerEvent(GameEvent.GetDiamond, rewardCnt);
            UIManager.Instance.CreatSpecialDiamondEffect(objPos, rewardCnt.ToString());
        }
        else
        {
            //∆’Õ®µ¿æﬂΩ±¿¯
            if (machineItemType != MachineItemType.Diamond)
            {
                EventManager.Instance.TriggerEvent(GameEvent.GetMachineItemReward, rewardCnt);
            }
            string str = "+" + rewardCnt;
            UIManager.Instance.CreatMachineItemUIEffect(objPos, str);
            //UIManager.Instance.CreatMachineItemUIEffect2(objPos, str);

            //CustomApiManager.Instance.RequestCustomEventV2(new CustomEventData()
            //{
            //    page_id = "GoldGain",
            //    name = "Event_GoldGain",
            //    value = "pusher_gain",
            //});
        }
        MachineItemUIEffect2.Instance.PlayEffect(machineItemType);
    }
}

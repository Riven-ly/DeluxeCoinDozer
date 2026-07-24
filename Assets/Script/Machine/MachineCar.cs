using DG.Tweening;
using UnityEngine;

public class MachineCar : MonoBehaviour
{
    private Vector3 initPos;
    private float limitX = 0.24f;
    private int moveDir = 1;
    private void Start()
    {
        initPos = transform.position;
        transform.position = new Vector3(0f, initPos.y, initPos.z);
    }
    private void Update()
    {
        float step = 0.15f * Time.deltaTime;
        transform.Translate(new Vector3(step * moveDir, 0, 0));

        if (transform.position.x > limitX)
        {
            moveDir = -1;
        }
        else if (transform.position.x < -limitX)
        {
            moveDir = 1;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.activeSelf || !other.CompareTag("MachineItem"))
        {
            return;
        }

        Vector3 objPos = other.transform.position;
        MachineItemInfo machineItemInfo = other.gameObject.GetComponent<MachineItemInfo>();

        if (GashaponMachine.isAwaitGetReward)
        {
            machineItemInfo.GetMachineItemReward();
        }
        else
        {
            machineItemInfo.GetMachineItemReward();

            //string str = LanguageManager.Instance.GetText("LuckyGashapon");
            string str = $"<color=yellow>{LanguageManager.Instance.GetText("LuckyGashapon")}</color>";
            UIManager.Instance.CreatMachineItemUIEffect(objPos, str);
            Debug.Log("开始Gachapon");
            EventManager.Instance.TriggerEvent(GameEvent.SpinGachapon);

            CustomApiManager.Instance.RequestCustomEventV2(new CustomEventData()
            {
                page_id = "MachineCar",
                name = "Event_SlotTrigger",
                value = "",
            });

        }

        //机器回收
        if (GameManager.Instance.curMachine.machineItems.Contains(machineItemInfo))
        {
            GameManager.Instance.curMachine.machineItems.Remove(machineItemInfo);
        }
        //对象池回收
        ObjectPoolManager.Instance.RecycleObject(machineItemInfo.gameObject, machineItemInfo.machineItemType);

    }
}

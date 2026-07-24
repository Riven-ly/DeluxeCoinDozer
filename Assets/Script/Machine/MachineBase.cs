using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MachineType
{
    Base,
    Difficult,
}
public class MachineBase : MonoBehaviour
{
    public MachineType type;
    public Transform goldParent;
    public Transform prefabs;
    public GashaponMachine gashaponMachine;
    //护墙
    public Transform city_Wall;
    private bool isCity_Wall_Open = false;
    //金币塔
    public GameObject dong;
    public GameObject flat_box;
    public GameObject Dong_clear;
    public GameObject elevator;
    public Transform goldTowerPrefab;
    public Transform GoldTowerRoot;
    //--
    public string cur_page_id = "";
    /// <summary>
    /// 场上道具
    /// </summary>
    [HideInInspector] public List<MachineItemInfo> machineItems = new List<MachineItemInfo>();
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            MachineClick();
        }
    }
    public virtual void ClearMachine()
    {
        Clear_City_Wall();

        machineItems.Clear();
        ObjectPoolManager.Instance.RecycleAllObjects();
    }

    public virtual int GetGold()
    {
        return 0;
    }

    public virtual void AddGold(int _cnt)
    {
    }
    public virtual void ExpendGold(int _cnt)
    {
    }

    public virtual void GetMachineItemReward(int _cnt)
    {
    }

    public void MachineClick()
    {
        if (GameLoadingPanel.isOpenStatic)
        {
            return;
        }
        if (!GameManager.TrySceneClick)
        {
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        LayerMask triggerLayer = LayerMask.GetMask("MachineClick");
        if (Physics.Raycast(ray, out RaycastHit hitInfo, 100, triggerLayer))
        {
            if (hitInfo.collider.CompareTag("MachineClickTrigger"))
            {
                if (GetGold() < 1)
                {
                    CustomApiManager.Instance.RequestCustomEventV2(new CustomEventData()
                    {
                        page_id = this.cur_page_id,
                        name = "Event_OutOfCoins",
                        value = "",
                    });
                    UIManager.Instance.OpenUI<GetGoldPanel>();
                }
                else
                {
                    //Debug.Log("成功点击到触发器区域");
                    //Debug.DrawLine(ray.origin, hitInfo.point, Color.red, 1000);
                    EventManager.Instance.TriggerEvent(GameEvent.CreatMachineItem);
                    Vector3 initPos = new Vector3(hitInfo.point.x, 0.16f, 1.5f);
                    initPos.x = Mathf.Clamp(initPos.x, -0.3f, 0.3f);
                    CreatMachineItem(initPos, MachineItemType.Gold);
                    Machineyindao.Instance.TryMachineyindao();
                }
            }
        }
    }

    /// <summary>
    /// 机器道具信息转保存数据
    /// </summary>
    /// <param name="info"></param>
    /// <returns></returns>
    public MachineItemSaveData MachineItemInfoToSaveData(MachineItemInfo info)
    {
        MachineItemSaveData data = new MachineItemSaveData()
        {
            machineItemType = info.machineItemType,
            x = Mathf.Round(info.transform.position.x * 10000) / 10000,
            y = Mathf.Round(info.transform.position.y * 10000) / 10000,
            z = Mathf.Round(info.transform.position.z * 10000) / 10000,
            r_x = Mathf.Round(info.transform.eulerAngles.x * 10000) / 10000,
            r_y = Mathf.Round(info.transform.eulerAngles.y * 10000) / 10000,
            r_z = Mathf.Round(info.transform.eulerAngles.z * 10000) / 10000,
        };
        return data;
    }

    public MachineItemInfo CreatMachineItem(Vector3 initPos, MachineItemType type)
    {
        GameObject obj = ObjectPoolManager.Instance.GetObject(type);
        obj.transform.SetParent(goldParent);
        MachineItemInfo machineItemInfo = obj.GetComponent<MachineItemInfo>();
        machineItemInfo.rb.velocity = Vector3.down * 0.01f; // 极小速度，不影响正常下落，但能激活物理
        machineItemInfo.Init(initPos);
        machineItems.Add(machineItemInfo);

        DOTween.Sequence().AppendInterval(0.2f).AppendCallback(() =>
        {
            if(machineItemInfo.audioSource != null)
            {
                AudioManager.Instance.SetAudioSource(machineItemInfo.audioSource, "GoldDrop");
            }
        });

        return machineItemInfo;
    }

    public MachineItemInfo GameBeforeCreatMachineItem(MachineItemType type)
    {
        GameObject obj = ObjectPoolManager.Instance.GetObject(type);
        obj.transform.SetParent(goldParent);
        MachineItemInfo machineItemInfo = obj.GetComponent<MachineItemInfo>();
        machineItemInfo.rb.velocity = Vector3.down * 0.01f; // 极小速度，不影响正常下落，但能激活物理
        machineItemInfo.Init(Vector3.one * 1000f);
        machineItems.Add(machineItemInfo);
        return machineItemInfo;
    }

    /// <summary>
    /// 在场中随机位置创建
    /// </summary>
    /// <param name="type"></param>
    public void CreateRanDomPosMachineItem(MachineItemType type, float _y = 0.3f)
    {
        float x = UnityEngine.Random.Range(-0.3f, 0.3f);
        float y = _y;
        float z = UnityEngine.Random.Range(1.28f, 0.45f);
        Vector3 initPos = new Vector3(x, y, z);
        CreatMachineItem(initPos, type);
    }
    //钻石
    public void GetDiamond()
    {
        CreateRanDomPosMachineItem(MachineItemType.Diamond);
    }
    //震动机台
    public void ShakeMachine()
    {
        transform.DOKill(); 
        float originalZ = 0f;
        if (SettingPanel.IsVibrateEnabled)
        {
            Handheld.Vibrate();
        }
        DOTween.Sequence()
            // 第1步：小幅上移（带正弦缓动，有加速度）
            .Append(transform.DOLocalMoveZ(originalZ + 0.09f, 0.07f).SetEase(Ease.InOutSine))
            // 第2步：小幅下移（幅度略小，模拟衰减）
            .Append(transform.DOLocalMoveZ(originalZ - 0.06f, 0.08f).SetEase(Ease.InOutSine))
            .AppendCallback(() =>
            {
                ShakeAllCell();
            })
            // 第3步：再次上移（幅度继续衰减）
            .Append(transform.DOLocalMoveZ(originalZ + 0.06f, 0.07f).SetEase(Ease.InOutSine))
            // 第4步：小幅下移（接近初始位置）
            .Append(transform.DOLocalMoveZ(originalZ - 0.03f, 0.08f).SetEase(Ease.InOutSine))
            // 最后平滑归位（避免突兀停止）
            .Append(transform.DOLocalMoveZ(originalZ, 0.1f)
            .SetTarget(transform)
            .SetEase(Ease.OutCubic));
    }

    private void ShakeAllCell()
    {
        foreach (var item in machineItems)
        {
            Rigidbody rb = item.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 force = new Vector3(0, 0, -2f);
                rb.AddForce(force,ForceMode.Impulse);
            }
        }
    }

    //巨大金币
    public void GetBigGold()
    {
        CreateRanDomPosMachineItem(MachineItemType.BigGold);

        DOTween.Sequence().AppendInterval(0.3f).AppendCallback(() =>
        {
            Transform mainCamera = Camera.main.transform;
            Vector3 originalPos = mainCamera.position;
            mainCamera.DOShakePosition(0.3f, 0.01f, 20, 100f, false, false)
                .OnComplete(() =>
                {
                    // 震动结束后重置相机位置（避免偏移）
                    mainCamera.position = originalPos;
                })
                .SetEase(Ease.OutQuad);

            if (SettingPanel.IsVibrateEnabled)
            {
                Handheld.Vibrate();
            }
        });

    }
    //金币爆炸
    public void GetGoldExplode()
    {
        for (int i = 0; i < 5; i++)
        {
            DOTween.Sequence()
            .AppendInterval(0.1f * i).AppendCallback(() =>
            {
                float x = UnityEngine.Random.Range(-0.3f, 0.3f);
                Vector3 initPos = new Vector3(x, 0.2f, 1.5f);
                CreatMachineItem(initPos, MachineItemType.Gold);

            });
        }
    }
    //打开护墙
    public void Open_City_Wall()
    {
        if (isCity_Wall_Open)
        {
            //道具加一
            List<object> dataList = new List<object>();
            dataList.Add(SceneItemType.City_Wall);
            dataList.Add(1f);
            EventManager.Instance.TriggerEvent(GameEvent.GetSceneItem, dataList);
            return;
        }
        EventManager.Instance.TriggerEvent(GameEvent.Hide__City_Wall_Btn);
        isCity_Wall_Open = true;
        city_Wall.transform.position = new Vector3(0f, -1.05f, 0f);
        city_Wall.gameObject.SetActive(true);
        DOTween.Sequence()
            .Append(city_Wall.transform.DOMoveY(-0.7f, 3f))
            .AppendCallback(() =>
            {
                UIManager.Instance.mainBtnUI.sceneItemInfo.city_Wall_Item.SetTimeStrState(true);
                UIManager.Instance.mainBtnUI.sceneItemInfo.city_Wall_Item.StartTimeText(30);
            })
            .AppendInterval(31f)//护墙持续时间
            .AppendCallback(() =>
            {
                UIManager.Instance.mainBtnUI.sceneItemInfo.city_Wall_Item.SetTimeStrState(false);
            })
            .Append(city_Wall.transform.DOMoveY(-1.05f, 3f))
            .AppendCallback(() =>
            {
                isCity_Wall_Open = false;
                city_Wall.gameObject.SetActive(false);
                EventManager.Instance.TriggerEvent(GameEvent.Open_City_Wall_Btn);
            })
            .SetTarget(city_Wall);
    }

    private void Clear_City_Wall()
    {
        city_Wall.transform.DOKill();

        city_Wall.position = new Vector3(0, -0.7f, 0);
        isCity_Wall_Open = false;
        city_Wall.gameObject.SetActive(false);
    }

    //金币雨
    public void GetGoldRain()
    {
        DOTween.Sequence()
            .AppendCallback(() =>
            {
                CreateRanDomPosMachineItem(MachineItemType.Gold, 0.6f);
            })
            .AppendInterval(0.2f)
            .AppendCallback(() =>
            {
                CreateRanDomPosMachineItem(MachineItemType.Gold, 0.6f);
            })
            .AppendInterval(0.2f)
            .AppendCallback(() =>
            {
                CreateRanDomPosMachineItem(MachineItemType.Gold, 0.6f);
            })
            .AppendInterval(0.6f)
            .SetLoops(5);
    }

    //金币塔
    public void GetGoldTower()
    {
        flat_box.gameObject.SetActive(false);
        dong.gameObject.SetActive(true);
        Dong_clear.gameObject.SetActive(true);

        List<MachineItemInfo> goldObjs = new List<MachineItemInfo>();
        DOTween.Sequence()
            .Append(elevator.transform.DOMoveY(-0.9f, 2f))
            .AppendCallback(() =>
            {
                Dong_clear.gameObject.SetActive(false);
                //创建金币塔
                foreach (Transform trans in goldTowerPrefab)
                {
                    GameObject obj = ObjectPoolManager.Instance.GetObject(MachineItemType.Gold);
                    obj.transform.SetParent(GoldTowerRoot);
                    MachineItemInfo machineItemInfo = obj.GetComponent<MachineItemInfo>();
                    machineItemInfo.Init(Vector3.zero);

                    machineItemInfo.rb.isKinematic = true;
                    machineItemInfo.transform.localPosition = trans.localPosition;
                    goldObjs.Add(machineItemInfo);
                }
          
            })
            .Append(elevator.transform.DOMoveY(0f, 2f))
            .AppendCallback(() =>
            {
                dong.gameObject.SetActive(false);
                flat_box.gameObject.SetActive(true);

                //设置金币刚体
                foreach (var obj in goldObjs)
                {
                    obj.transform.SetParent(goldParent);
                    obj.rb.isKinematic = false;
                    machineItems.Add(obj);
                }
            })
            ;
    }

    //巨大金币雨
    public void GetBigGoldRain()
    {
        DOTween.Sequence()
            .AppendCallback(() =>
            {
                GetBigGold();
            })
            .AppendInterval(1f)
            .AppendCallback(() =>
            {
                GetBigGold();
            })
            .AppendInterval(1f)
            .AppendCallback(() =>
            {
                GetBigGold();
            });
    }
    //
    public void GetSpecialDiamond()
    {
        CreateRanDomPosMachineItem(MachineItemType.SpecialDiamond);
    }
    //碎片
    public void GetSpecialFragment()
    {
        ItemFragment itemFragment = UIManager.Instance.mainBtnUI.itemFragment;
        List<MachineItemType> fragmentList = itemFragment.GetSpecialFragment();
        int index = UnityEngine.Random.Range(0, fragmentList.Count);
        MachineItemType targetType = fragmentList[index];
        CreateRanDomPosMachineItem(targetType);
    }
}

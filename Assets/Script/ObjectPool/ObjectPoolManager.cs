using System;
using System.Collections.Generic;
using UnityEngine;

// 父节点信息类
[Serializable]
public class ObjectParentInfo
{
    public MachineItemType Key;
    public Transform value;
}

// 预制体信息类
[Serializable]
public class ObjectPrefabInfo
{
    public MachineItemType Key;
    public GameObject value;
}

/// <summary>
/// 通用对象池
/// </summary>
public class ObjectPoolManager : MonoBehaviour
{
    // 单例实例
    public static ObjectPoolManager Instance { get; private set; }


    // 未使用的对象池：按MachineItemType分类存储闲置对象
    private Dictionary<MachineItemType, List<GameObject>> _unusedPool = new Dictionary<MachineItemType, List<GameObject>>();
    // 已使用的对象池：按MachineItemType分类存储正在使用的对象
    private Dictionary<MachineItemType, List<GameObject>> _usedPool = new Dictionary<MachineItemType, List<GameObject>>();

    // 预制体列表
    public List<ObjectPrefabInfo> PrefabList = new List<ObjectPrefabInfo>();

    private void Awake()
    {
        Instance = this;
        //DontDestroyOnLoad(gameObject);

        // 初始化对象池容器（为每个预制体MachineItemType创建空列表）
        InitPoolContainers();
    }

    /// <summary>
    /// 初始化对象池容器（为每个预制体MachineItemType创建空列表）
    /// </summary>
    public void InitPoolContainers()
    {
        foreach (var prefabInfo in PrefabList)
        {
            MachineItemType type = prefabInfo.Key;
            // 未使用池初始化
            if (!_unusedPool.ContainsKey(type))
            {
                _unusedPool.Add(type, new List<GameObject>());
            }
            // 已使用池初始化
            if (!_usedPool.ContainsKey(type))
            {
                _usedPool.Add(type, new List<GameObject>());
            }
        }
    }

    /// <summary>
    /// 获取对象
    /// </summary>
    public GameObject GetObject(MachineItemType type)
    {
        MachineItemType targetMachineItemType = type;
        GameObject targetObj = null;

        // 1. 检查未使用池是否有可用对象
        if (_unusedPool.ContainsKey(targetMachineItemType) && _unusedPool[targetMachineItemType].Count > 0)
        {
            // 从未使用池取出第一个对象
            targetObj = _unusedPool[targetMachineItemType][0];
            _unusedPool[targetMachineItemType].RemoveAt(0);

            targetObj.SetActive(true);
        }
        else
        {
            // 2. 未使用池无对象，创建新对象
            targetObj = CreateNewObject(targetMachineItemType);
            if (targetObj == null)
            {
                Debug.LogError($"创建新对象失败，未找到MachineItemType为{targetMachineItemType}的预制体");
                return null;
            }
        }

        // 3. 将对象添加到已使用池
        _usedPool[targetMachineItemType].Add(targetObj);

        return targetObj;
    }

    /// <summary>
    /// 回收对象
    /// </summary>
    public void RecycleObject(GameObject obj, MachineItemType type)
    {
        if (obj == null)
        {
            Debug.LogWarning("回收的对象为空");
            return;
        }
        // 防止重复回收
        if (_unusedPool.ContainsKey(type) && _unusedPool[type].Contains(obj))
        {
            Debug.LogWarning($"对象{obj.name}已在未使用池中，无需重复回收");
            return;
        }
        DoRecycleObject(obj, type);
    }

    /// <summary>
    /// 创建新的对象
    /// </summary>
    /// <returns>新创建的GameObject</returns>
    private GameObject CreateNewObject(MachineItemType type)
    {
        // 查找对应预制体
        var prefabInfo = PrefabList.Find(info => info.Key == type);
        if (prefabInfo == null || prefabInfo.value == null)
        {
            Debug.LogWarning($"无{type}类型预制体");
            return null;
        }

        // 实例化预制体
        GameObject newObj = Instantiate(prefabInfo.value);

        return newObj;
    }

    /// <summary>
    /// 执行回收逻辑
    /// </summary>
    /// <param name="obj">要回收的GameObject</param>
    private void DoRecycleObject(GameObject obj, MachineItemType type)
    {

        MachineItemType targetMachineItemType = type;

        // 1. 从已使用池移除
        if (_usedPool.ContainsKey(targetMachineItemType) && _usedPool[targetMachineItemType].Contains(obj))
        {
            _usedPool[targetMachineItemType].Remove(obj);
        }
        else
        {
            Debug.LogWarning($"对象{obj.name}不在已使用池，无需回收");
            return;
        }

        // 2. 重置对象状态
        ResetObjectTransform(obj, targetMachineItemType);

        // 3. 添加到未使用池
        _unusedPool[targetMachineItemType].Add(obj);
    }

    /// <summary>
    /// 重置对象的Transform（回到指定父节点）
    /// </summary>
    /// <param name="obj">要重置的对象</param>
    /// <param name="type">对象对应的MachineItemType</param>
    private void ResetObjectTransform(GameObject obj, MachineItemType type)
    {
        obj.transform.SetParent(transform);
        // 重置位置旋转缩放
        //obj.transform.localPosition = Vector3.zero;
        //obj.transform.eulerAngles = Vector3.zero;
        //obj.transform.localScale = Vector3.one;
        obj.SetActive(false);
    }

    /// <summary>
    /// 回收指定类型的所有对象
    /// </summary>
    /// <param name="type">对象类型</param>
    public void RecycleAllObjects(MachineItemType type)
    {
        // 先校验类型是否存在
        if (!_usedPool.ContainsKey(type))
        {
            Debug.LogWarning($"回收池没有{type}类型");
            return;
        }

        // 遍历已使用池的所有对象，逐个回收（注意要复制列表，避免遍历中修改原列表）
        List<GameObject> usedObjects = new List<GameObject>(_usedPool[type]);
        foreach (var obj in usedObjects)
        {
            if (obj != null)
            {
                RecycleObject(obj, type);
            }
            else
            {
                Debug.LogWarning($"回收{type}类型对象时发现空对象，已从已使用池移除");
                _usedPool[type].Remove(obj);
            }
        }

        Debug.Log($"已回收{type}类型的所有对象，共回收{usedObjects.Count}个");
    }

    /// <summary>
    /// 回收所有类型的所有对象
    /// </summary>
    public void RecycleAllObjects()
    {
        int totalRecycleCount = 0;

        // 遍历所有类型，逐个回收
        foreach (var type in _usedPool.Keys)
        {
            List<GameObject> usedObjects = new List<GameObject>(_usedPool[type]);
            totalRecycleCount += usedObjects.Count;

            foreach (var obj in usedObjects)
            {
                if (obj != null)
                {
                    RecycleObject(obj, type);
                }
                else
                {
                    Debug.LogWarning($"回收{type}类型对象时发现空对象，已从已使用池移除");
                    _usedPool[type].Remove(obj);
                }
            }
        }

        Debug.Log($"已回收所有类型的对象，总计回收{totalRecycleCount}个");
    }
}


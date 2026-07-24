using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class CoinTowerMulti : MonoBehaviour
{
    [Header("核心资源")]
    [Tooltip("拖入金币预制件")]
    public GameObject coinPrefab;

    [Header("塔身设置")]
    [Tooltip("几堆金币？(图中是5堆)")]
    [Range(1, 10)]
    public int stackCount = 5;

    [Tooltip("每堆叠多高？")]
    [Range(10, 200)] // 上限也调高了，方便生成更高的塔
    public int coinsPerStack = 50;

    [Tooltip("塔的粗细 (半径) - 设为0可以完全聚拢")]
    [Range(0f, 10f)] // ⚠️ 修改：最小值改为 0
    public float radius = 1.2f;

    [Tooltip("金币垂直间距 - 设小一点可以让金币紧贴")]
    [Range(0.01f, 2f)] // ⚠️ 修改：最小值改为 0.01
    public float heightStep = 0.15f;

    [Header("螺旋扭曲")]
    [Tooltip("整体扭曲程度")]
    [Range(-20f, 20f)]
    public float twistAnglePerLayer = 5.0f;

    [Header("朝向微调")]
    [Tooltip("勾选后金币面朝中心")]
    public bool lookAtCenter = true;

    // ----------------------------------------------------
    // 生成逻辑
    // ----------------------------------------------------
    public void Generate()
    {
        if (coinPrefab == null)
        {
            Debug.LogError("❌ 请拖入金币预制件！");
            return;
        }

        Clear();

        float anglePerStack = 360f / stackCount;

        for (int layer = 0; layer < coinsPerStack; layer++)
        {
            float currentHeight = layer * heightStep;
            float currentTwist = layer * twistAnglePerLayer;

            for (int stackIndex = 0; stackIndex < stackCount; stackIndex++)
            {
                float angle = (stackIndex * anglePerStack) + currentTwist;
                float radian = angle * Mathf.Deg2Rad;

                float x = Mathf.Cos(radian) * radius;
                float z = Mathf.Sin(radian) * radius;
                Vector3 pos = new Vector3(x, currentHeight, z);

                CreateCoin(pos);
            }
        }
    }

    void CreateCoin(Vector3 localPos)
    {
        GameObject newCoin = null;

#if UNITY_EDITOR
        if (PrefabUtility.IsPartOfPrefabAsset(coinPrefab))
        {
            newCoin = (GameObject)PrefabUtility.InstantiatePrefab(coinPrefab, transform);
        }
#endif
        if (newCoin == null)
        {
            newCoin = Instantiate(coinPrefab, transform);
        }

        if (newCoin != null)
        {
            newCoin.transform.localPosition = localPos;

            if (lookAtCenter)
            {
                // 让金币看向中心轴 (保持水平)
                Vector3 targetPos = new Vector3(transform.position.x, newCoin.transform.position.y, transform.position.z);
                newCoin.transform.LookAt(targetPos);
            }
            // 为了让金币堆叠更像真的，可以在这里随机一点点偏移(可选)
            // newCoin.transform.localPosition += new Vector3(Random.Range(-0.02f,0.02f), 0, Random.Range(-0.02f,0.02f));

#if UNITY_EDITOR
            Undo.RegisterCreatedObjectUndo(newCoin, "Create Coin Tower");
#endif
        }
    }

    public void Clear()
    {
        while (transform.childCount > 0)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
    }
}

// ----------------------------------------------------
// 编辑器按钮
// ----------------------------------------------------
#if UNITY_EDITOR
[CustomEditor(typeof(CoinTowerMulti))]
public class CoinTowerMultiEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        CoinTowerMulti script = (CoinTowerMulti)target;

        EditorGUILayout.Space(15);
        
        GUIStyle bigButtonStyle = new GUIStyle(GUI.skin.button);
        bigButtonStyle.fontSize = 14;
        bigButtonStyle.fontStyle = FontStyle.Bold;
        bigButtonStyle.fixedHeight = 40;

        GUI.backgroundColor = new Color(0.6f, 1f, 0.6f);
        if (GUILayout.Button("生成螺旋塔 (Generate)", bigButtonStyle))
        {
            script.Generate();
        }

        GUI.backgroundColor = new Color(1f, 0.6f, 0.6f);
        if (GUILayout.Button("清除 (Clear)", bigButtonStyle))
        {
            script.Clear();
        }
    }
}
#endif
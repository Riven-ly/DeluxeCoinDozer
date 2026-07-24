using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABResManager : MonoBehaviour
{
    public GameObject txElementManangerPrefab;
    public GameObject itemFragmentPrefab;
    public GameObject difficultMachineEnterPrefab;
    public GameObject TxElementBtnPrefab;

    public GameObject gashaponItem_Special_Diamond_Prefab;
    public GameObject gashaponItem_Special_Fragment_Prefab;

    public Sprite diamondsSprite;
    public Sprite diamondRerardIconsSprite;
    //新的对象池道具
    public List<ObjectPrefabInfo> PrefabList = new List<ObjectPrefabInfo>();
    //新的UI界面
    public List<GameObject> uiPanel;
    // Start is called before the first frame update
    void Start()
    {
        UpdateDiamondsUI();
        //更新对象池
        UpdateObjectPool();
        //初始化碎片按钮
        InitItemFragment();
        //初始化困难机台进入按钮
        InitDifficultMachineEnter();
        //初始化tx部分
        InitTxElementPanel();
        //更新扭蛋机
        GameManager.Instance.ordinaryMachine.gashaponMachine.UpdateAppATT(gashaponItem_Special_Diamond_Prefab, gashaponItem_Special_Fragment_Prefab);
        GameManager.Instance.difficultMachine.gashaponMachine.UpdateAppATT(gashaponItem_Special_Diamond_Prefab, gashaponItem_Special_Fragment_Prefab);

        //添加新的UI界面
        if (uiPanel != null)
        {
            foreach (var ui in uiPanel)
            {
                UIManager.Instance.AddSpecialUI(ui.gameObject);
            }
        }

        UIManager.Instance.mainBtnUI.CheckButtonOpenState();
    }
    private void UpdateDiamondsUI()
    {
        GameManager.Instance.Diamonds[1] = diamondsSprite;
        GameManager.Instance.DiamondRerardIcons[1] = diamondRerardIconsSprite;
        GameManager.Instance.UpdateAppATTToDiamond(UIManager.Instance.playInfoUI.diamondUI.icon);
    }
    private void UpdateObjectPool()
    {
        foreach (var prefab in PrefabList)
        {
            ObjectPoolManager.Instance.PrefabList.Add(prefab);
        }
        ObjectPoolManager.Instance.InitPoolContainers();
    }
    private void InitItemFragment()
    {
        UIManager.Instance.mainBtnUI.SetItemFragment(itemFragmentPrefab);
    }

    private void InitDifficultMachineEnter()
    {
        UIManager.Instance.mainBtnUI.SetDifficultMachineEnter(difficultMachineEnterPrefab);
    }

    private void InitTxElementPanel()
    {
        Instantiate(txElementManangerPrefab);
        UIManager.Instance.playInfoUI.diamondUI.btn.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayBtnMusic();
            UIManager.Instance.OpenUI<TxElementPanel>();
        });

        UIManager.Instance.mainBtnUI.SetTxElementBtn(TxElementBtnPrefab);
    }
}

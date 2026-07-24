using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBtnUI : MonoBehaviour
{
    public Transform left;
    public Transform right;

    public DailySignIn dailySignIn;
    public GameTask gameTask;
    public DailyWheel dailyWheel;
    public ItemFragment itemFragment;
    public DifficultMachineEnter difficultMachineEnter;
    public TxElementBtn txElementBtn;

    public SceneItemInfo sceneItemInfo;
    public FlotageBalloon flotageBalloon;
    private void Awake()
    {
        RectTransform rect = GetComponent<RectTransform>();
        float topBlockHeight = Screen.height - Screen.safeArea.yMax;
        rect.offsetMax = new Vector2(0, -topBlockHeight);
    }

    public void Init()
    {
        gameTask.InitGameTask();
        CheckButtonOpenState();
        
        sceneItemInfo.Init();
    }
    public void EnterBaseMachineUIAnim()
    {
        left.transform.localPosition = new Vector3(-744f, left.transform.localPosition.y, 0);
        right.transform.localPosition = new Vector3(700f,right.transform.localPosition.y, 0);

        left.transform.DOLocalMoveX(-334f, 1f);
        right.transform.DOLocalMoveX(437f, 1f);
    }
    public void EnterBaseMachineUIAnim2()
    {
        DOTween.Sequence()
                 .AppendInterval(1f)
                 .AppendCallback(() =>
                 {
                     flotageBalloon.FlotageBalloonEnter();
                     left.transform.DOLocalMoveX(-334f, 1f);
                     right.transform.DOLocalMoveX(437f, 1f);
                 });

    }
    public void EnterDifficultMachineUIAnim()
    {
        flotageBalloon.FlotageBalloonLeave();
        left.transform.DOLocalMoveX(-744f, 1f);
        right.transform.DOLocalMoveX(700f, 1f);
    }

    public void CheckButtonOpenState()
    {
        int curLv = GameManager.Instance.playerInfo.playerData.level;
        dailySignIn.OpenState(curLv >= 1);
        gameTask.OpenState(curLv >= 2);
        dailyWheel.OpenState(curLv >= 3);
        flotageBalloon.OpenState(curLv >= 3);
        if(itemFragment != null)
        {
            itemFragment.OpenState(curLv >= 4);
        }
        if(difficultMachineEnter != null)
        {
            difficultMachineEnter.OpenState(curLv >= 8);
        }
        //-------------
        if(TxElementMananger.Instance != null)
        {
            TxElementMananger.Instance.OpenState(curLv >= 2);
        }
        if (txElementBtn != null)
        {
            UIManager.Instance.playInfoUI.diamondUI.btn.gameObject.SetActive(curLv >= 2);
            txElementBtn.gameObject.SetActive(curLv >= 2);
        }
        else
        {
            UIManager.Instance.playInfoUI.diamondUI.btn.gameObject.SetActive(false);
        }
    }
    
    public void SetItemFragment(GameObject ItemFragmentPrefab)
    {
        if (ItemFragmentPrefab == null)
        {
            Debug.LogError("ItemFragmentPrefabÎª¿Õ");
            return;
        }

        GameObject obj = Instantiate(ItemFragmentPrefab, left);
        obj.transform.SetAsLastSibling();
        itemFragment = obj.transform.GetComponent<ItemFragment>();
    }

    public void SetDifficultMachineEnter(GameObject difficultMachineEnterPrefab)
    {
        if (difficultMachineEnterPrefab == null)
        {
            Debug.LogError("difficultMachineEnterPrefabÎª¿Õ");
            return;
        }

        GameObject obj = Instantiate(difficultMachineEnterPrefab, left);
        obj.transform.SetAsLastSibling();
        difficultMachineEnter = obj.transform.GetComponent<DifficultMachineEnter>();
    }

    public void SetTxElementBtn(GameObject txElementManangerPrefab)
    {
        if (txElementManangerPrefab == null)
        {
            Debug.LogError("txElementManangerPrefabÎª¿Õ");
            return;
        }

        GameObject obj = Instantiate(txElementManangerPrefab, left);
        obj.transform.SetAsLastSibling();
        txElementBtn = obj.transform.GetComponent<TxElementBtn>();
    }
}

using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.UI;

public class DifficultMachineQuitPanel : UIBase
{
    public Text explain2;
    public Transform itemRoot;
    public Button continueBtn;
    public Button giveupBtn;

    private void OnEnable()
    {
        isOpen = true;
        GameManager.isPause = true;
    }
    private void OnDisable()
    {
        isOpen = false;
        GameManager.isPause = false;
        foreach (Transform item in itemRoot)
        {
            Destroy(item.gameObject);
        }
    }
    private void Start()
    {
        continueBtn.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayBtnMusic();
            Hide();
        });

        giveupBtn.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayBtnMusic();
            EventManager.Instance.TriggerEvent(GameEvent.Daily_DifficultMachine);
            callback = () =>
            {
                GameManager.Instance.difficultMachine.GameExit();
            };
            Hide();
        });
    }

    public override void Refresh(object data = null)
    {
        base.Refresh(data);

        string str = LanguageManager.Instance.GetText_Encrypt("CH");
        explain2.text = string.Format(LanguageManager.Instance.GetText("DifficultMachineQuitPanel_explain2"), str);

        int lv = DifficultMachine.GetDifficultMachineLv();
        var info = GameManager.Instance.difficultMachine.GetDifficultMachineInfo();
        ItemData itemData = info[lv - 1].rewardDatas[0];
        var obj = GameManager.Instance.CreatItem(itemData, itemRoot);
        obj.transform.localPosition = Vector3.zero;
        obj.icon.color = new Color(142f / 255f, 142f / 255f, 142f / 255f, 255f / 255f);
        obj.cntText.transform.localPosition = new Vector3(0f, -78f, 0f);


        giveupBtn.transform.parent.transform.localScale = Vector3.zero;
        DOTween.Sequence()
                     .AppendInterval(1.5f)
                     .Append(giveupBtn.transform.parent.transform.DOScale(1.1f, 0.2f))
                     .Append(giveupBtn.transform.parent.transform.DOScale(0.9f, 0.1f))
                     .Append(giveupBtn.transform.parent.transform.DOScale(1f, 0.1f));
    }
    public override void Hide()
    {
        base.Hide();
    }
}

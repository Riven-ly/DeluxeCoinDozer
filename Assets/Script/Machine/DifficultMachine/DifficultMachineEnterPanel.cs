using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifficultMachineEnterPanel : UIBase
{
    public Button btn;
    public Button hidebtn;
    public Text btnText;
    public Text explain;

    public List<GameObject> rewardList;
    public List<GameObject> rewardHideList;
    public List<GameObject> zhizhenList;

    private bool isTodayPlaying;
    private void Start()
    {
        hidebtn.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayBtnMusic();
            Hide();
        });
        btn.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayBtnMusic();
            btn.interactable = false;
            if (isTodayPlaying)
            {
                string str = LanguageManager.Instance.GetText("DifficultMachine_todayOver");
                UIManager.Instance.OpenUI<GeneralTipsPanel>(str);
            }
            else
            {
                //Ω¯»Î”Œœ∑
                GameManager.Instance.SwitchDifficultMachine();
                Hide();
            }
        });
    }
    public override void Refresh(object data = null)
    {
        base.Refresh(data);
        string s1 = LanguageManager.Instance.GetText_Encrypt("CH");
        btnText.text = string.Format(LanguageManager.Instance.GetText("DifficultMachineEnterPanel_Btn"), s1);
        explain.text = string.Format(LanguageManager.Instance.GetText("DifficultMachineEnterPanel_explain"), s1);
        

        isTodayPlaying = DifficultMachineEnter.CheckDailyDifficult();
        int curLv = DifficultMachine.GetDifficultMachineLv();

        btn.interactable = curLv < 5;

        rewardList[0].SetActive(curLv <= 1);
        rewardList[1].SetActive(curLv <= 2);
        rewardList[2].SetActive(curLv <= 3);
        rewardList[3].SetActive(curLv <= 4);

        rewardHideList[0].SetActive(curLv > 1);
        rewardHideList[1].SetActive(curLv > 2);
        rewardHideList[2].SetActive(curLv > 3);
        rewardHideList[3].SetActive(curLv > 4);

        zhizhenList[0].SetActive(curLv == 1);
        zhizhenList[1].SetActive(curLv == 2);
        zhizhenList[2].SetActive(curLv == 3);
        zhizhenList[3].SetActive(curLv == 4);

    }
    public override void Hide()
    {
        base.Hide();
    }
}

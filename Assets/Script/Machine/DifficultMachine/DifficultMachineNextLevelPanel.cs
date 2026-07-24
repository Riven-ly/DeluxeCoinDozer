using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifficultMachineNextLevelPanel : UIBase
{
    public Text explain;
    public Button btn;
    public Transform itemRoot;
    public Slider slider;

    public List<Transform> levelList;


    private void OnEnable()
    {
        isOpen = true;
        GameManager.isPause = true;
    }
    private void OnDisable()
    {
        isOpen = false;
        foreach (Transform item in itemRoot)
        {
            Destroy(item.gameObject);
        }
        GameManager.isPause = false;
    }

    private void Start()
    {
        btn.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayBtnMusic();
            Hide();
        });
    }
    public override void Refresh(object data = null)
    {
        base.Refresh(data);
        AudioManager.Instance.PlaySceneSingleMusic("LevelWin");

        List<object> curdata = data as List<object>;
        int lv = (int)curdata[0];
        List<ItemData> itemDatas = curdata[1] as List<ItemData>;

        levelList[0].gameObject.SetActive(lv == 1);
        levelList[1].gameObject.SetActive(lv == 2);
        levelList[2].gameObject.SetActive(lv == 3);
        levelList[3].gameObject.SetActive(lv == 4);
        float sliderV = 0;
        if (lv == 1)
        {
            sliderV = 0.267f;
        }
        else if (lv == 2)
        {
            sliderV = 0.545f;
        }
        else if (lv == 3)
        {
            sliderV = 0.82f;
        }
        else if (lv == 4)
        {
            sliderV = 1f;
        }
        slider.value = sliderV;

        GameManager.Instance.CreatItems(itemDatas, itemRoot, true);
        string str = LanguageManager.Instance.GetText_Encrypt("CH");
        explain.text = string.Format(LanguageManager.Instance.GetText("DifficultMachineNextLevelPanel_explain"), str);
    }
    public override void Hide()
    {
        base.Hide();
    }
}

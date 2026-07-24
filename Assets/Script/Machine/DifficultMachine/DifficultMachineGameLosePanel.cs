using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifficultMachineGameLosePanel : UIBase
{
    public Text explain;
    public Button btn;
    public Slider slider;

    public List<Transform> levelList;
    public List<Transform> levelLoseList;

    private void OnEnable()
    {
        isOpen = true;
        GameManager.isPause = true;
    }
    private void OnDisable()
    {
        isOpen = false;
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
        AudioManager.Instance.PlaySceneSingleMusic("LevelLose");

        int lv = (int)data;
        levelList[0].gameObject.SetActive(lv > 1);
        levelList[1].gameObject.SetActive(lv > 2);
        levelList[2].gameObject.SetActive(lv > 3);
        levelList[3].gameObject.SetActive(lv > 4);

        levelLoseList[0].gameObject.SetActive(lv == 1);
        levelLoseList[1].gameObject.SetActive(lv == 2);
        levelLoseList[2].gameObject.SetActive(lv == 3);
        levelLoseList[3].gameObject.SetActive(lv == 4);
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

        string str = LanguageManager.Instance.GetText_Encrypt("CH");
        explain.text = string.Format(LanguageManager.Instance.GetText("DifficultMachineNextLevelPanel_explain"), str);
    }
    public override void Hide()
    {
        base.Hide();
    }
}

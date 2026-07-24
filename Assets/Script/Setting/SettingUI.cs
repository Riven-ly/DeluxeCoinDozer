using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingUI : MonoBehaviour
{
    public List<Sprite> btnBgSprites;
    public Button btn;
    public Image btnBg;

    private void Start()
    {
        btn.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayBtnMusic();
            UIManager.Instance.OpenUI<SettingPanel>();
        });
    }
    public void Init()
    {
        btnBg.sprite = btnBgSprites[(int)GameManager.Instance.curMachine.type];
    }
}

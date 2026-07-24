using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TxElementYindaoPanel : UIBase
{
    public Text yindao2_explain;
    public Text yindao3_explain;
    public Transform panelAll;
    public Button btn1;
    public Button btn2;
    public Button btn3;

    // Start is called before the first frame update
    void Start()
    {
        btn1.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayBtnMusic();
            UIManager.Instance.OpenUIMask();
            panelAll.DOLocalMoveX(-1200f, 0.5f).OnComplete(() =>
            {
                UIManager.Instance.HideUIMask();
            });
        });
        btn2.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayBtnMusic();
            UIManager.Instance.OpenUIMask();
            panelAll.DOLocalMoveX(-2400f, 0.5f).OnComplete(() =>
            {
                UIManager.Instance.HideUIMask();
            });
        });
        btn3.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayBtnMusic();
            PlayerPrefs.SetString("TxElementYindaoPanel", "yes");
            Hide();
        });
    }
    public override void Refresh(object data = null)
    {
        base.Refresh(data);

        string str1 = LanguageManager.Instance.GetText_Encrypt("CH");
        string str2 = LanguageManager.Instance.GetText_Encrypt("wH");
        yindao2_explain.text = string.Format(LanguageManager.Instance.GetText("TxElement_yindao2_explain"), str1, str2);
        yindao3_explain.text = string.Format(LanguageManager.Instance.GetText("TxElement_yindao3_explain"), str2);

        panelAll.transform.localPosition = Vector3.zero;
    }
    public override void Hide()
    {
        base.Hide();
    }
}

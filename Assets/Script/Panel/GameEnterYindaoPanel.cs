using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameEnterYindaoPanel : UIBase
{
    public Button btn;
    
    private void OnEnable()
    {
        isOpen = false;
    }
    private void Start()
    {
        btn.onClick.AddListener(() =>
        {
            Hide();
            PlayerPrefs.SetString("GameEnterYindaoPanel", "yes");
        });
    }

    public override void Refresh(object data = null)
    {
        base.Refresh(data);
    }
    public override void Hide()
    {
        base.Hide();
    }
}

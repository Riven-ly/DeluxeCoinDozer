using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropRewardsPanel : UIBase
{
    public Button btn;

    private void Start()
    {
        btn.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayBtnMusic();
            callback = () =>
            {
                UIManager.Instance.OpenUI<DropGamePanel>();
            };
            Hide();
        });
    }
    public override void Refresh(object data = null)
    {
        base.Refresh(data);
        AudioManager.Instance.PlaySceneSingleMusic("SpecialGame");
    }
    public override void Hide()
    {
        base.Hide();
    }
}

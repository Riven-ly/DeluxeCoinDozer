using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemFragmentTipsPanel : UIBase
{
    public Button hideBtn;

    public GameObject EnglishTipText;
    public GameObject IndonesianTipText;
    public GameObject PortugueseTipText;

    private void Start()
    {
        hideBtn.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayBtnMusic();
            Hide();
        });
 
    }
    public override void Refresh(object data = null)
    {
        base.Refresh(data);

        EnglishTipText.SetActive(LanguageManager.Instance.type == MultilingualType.English);
        IndonesianTipText.SetActive(LanguageManager.Instance.type == MultilingualType.Indonesian);
        PortugueseTipText.SetActive(LanguageManager.Instance.type == MultilingualType.Portuguese);

        CustomApiManager.Instance.RequestCustomEventV2(new CustomEventData()
        {
            page_id = "ShardInfoPanel",
            name = "Event_ShardInfoOpen",
            value = "",
        });

    }
    public override void Hide()
    {
        base.Hide();
    }
}

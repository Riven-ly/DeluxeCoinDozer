using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetSceneItemPanel : UIBase
{
    public RewardAdButton rewardAdButton;
    public Text title;
    public Image titleImg;
    public Image icon;
    public Text explain;
    public Button hideBtn;

    public List<Sprite> titleSprites;
    public List<Sprite> titleSprites_Portuguese;
    public List<Sprite> titleSprites_Indonesian;
    private string page_id = "GetSceneItem_";

    private void Start()
    {
        hideBtn.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayBtnMusic();
            Hide();

            AdManager.Instance.OnClickInterstitialAd("GetSceneItemPanel", true);
        });
    }
    public override void Refresh(object data = null)
    {
        base.Refresh(data);
        SceneItemPanelInfo info = data as SceneItemPanelInfo;
        title.text = info.title;
        if (LanguageManager.Instance.type == MultilingualType.Portuguese)
        {
            titleImg.sprite = titleSprites_Portuguese[(int)info.type];
        }
        else if(LanguageManager.Instance.type == MultilingualType.Indonesian)
        {
            titleImg.sprite = titleSprites_Indonesian[(int)info.type];
        }
        else
        {
            titleImg.sprite = titleSprites[(int)info.type];
        }

        titleImg.SetNativeSize();
        icon.sprite = info.icon;
        icon.SetNativeSize();
        explain.text = info.explain;

        Action AdRewardCallback = () =>
        {
            CustomApiManager.Instance.RequestCustomEventV2(new CustomEventData()
            {
                page_id = page_id + info.type.ToString(),
                name = "Event_UseItemID",
                value = info.type.ToString(),
            });

            callback = info.clickCallback;
            Hide();
        };
        rewardAdButton.Init(AdRewardCallback, page_id + info.type.ToString(), false);

        if(info.type == SceneItemType.Big_Gold)
        {
            icon.rectTransform.sizeDelta = new Vector2(300f, 300f);
        }
    }
    public override void Hide()
    {
        base.Hide();
    }
}

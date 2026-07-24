using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpPanel : UIBase
{
    public Text curLv;
    public Text nextLv;
    public GoldItem goldItem;
    public DiamondItem diamondItem;

    public RewardAdButton rewardAdButton;
    public Button collectBtn;
    private string page_id = "LevelUpPanel";
    private void Start()
    {
        collectBtn.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayBtnMusic();
            GetReward();
            Hide();
            AdManager.Instance.OnClickInterstitialAd(page_id, true);
            CustomApiManager.Instance.RequestCustomEventV2(new CustomEventData()
            {
                page_id = "GoldGain",
                name = "Event_GoldGain",
                value = "level_up",
            });
        });
    }
    public override void Refresh(object data = null)
    {
        base.Refresh(data);

        AudioManager.Instance.PlaySceneSingleMusic("LevelUp");
        UIManager.Instance.mainBtnUI.CheckButtonOpenState();

        int oldLv = GameManager.Instance.playerInfo.playerData.level - 1;
        var levelConfigData = GameManager.Instance.playerInfo.GetLevelConfigData(oldLv);

        curLv.text = LanguageManager.Instance.GetText("Lv") + oldLv;
        nextLv.text = LanguageManager.Instance.GetText("Lv") + (oldLv + 1);

        goldItem.Init((int)levelConfigData.rewardGold);
        diamondItem.Init(levelConfigData.rewardDiamond);
        rewardAdButton.Init(AdRewardCallback, page_id);

        collectBtn.transform.localScale = Vector3.zero;
        DOTween.Sequence()
                     .AppendInterval(1.5f)
                     .Append(collectBtn.transform.DOScale(1.1f, 0.2f))
                     .Append(collectBtn.transform.DOScale(0.9f, 0.1f))
                     .Append(collectBtn.transform.DOScale(1f, 0.1f));
    }
    public override void Hide()
    {
        base.Hide();

        string s = PlayerPrefs.GetString("DiamondUIBtnYindao");
        if (string.IsNullOrEmpty(s))
        {
            List<object> listdata = new List<object>();
            listdata.Add("");
            listdata.Add(UIManager.Instance.mainBtnUI.txElementBtn.clickBtn);
            listdata.Add(false);

            DOTween.Sequence()
                .AppendInterval(0.5f)
                .AppendCallback(() =>
                {
                    PlayerPrefs.SetString("DiamondUIBtnYindao", "YES");
                    UIManager.Instance.OpenUI<GameMainBtnYindaoPanel>(listdata);
                });

            CustomApiManager.Instance.RequestCustomEventV2(new CustomEventData()
            {
                page_id = "Yindao",
                name = "Event_GuideStep",
                value = "step1",
            });
        }
    }

    private void GetReward()
    {
        goldItem.GetItemReward();
        diamondItem.GetItemReward();
    }
    private void AdRewardCallback()
    {
        //Ë«±¶½±Àø
        GetReward();
        GetReward();
        Hide();
        CustomApiManager.Instance.RequestCustomEventV2(new CustomEventData()
        {
            page_id = "GoldGain",
            name = "Event_GoldGain",
            value = "level_up",
        });
    }
}

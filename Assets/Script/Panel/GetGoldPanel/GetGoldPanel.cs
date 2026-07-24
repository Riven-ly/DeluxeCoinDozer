using DG.Tweening;
using Newtonsoft.Json;
using System;
using UnityEngine;
using UnityEngine.UI;

public class GetGoldPanel : UIBase
{
    public Text explain;
    public Text explain2;

    public Text freeBtnText;
    public Text freeBtnText2;
    public Text adBtnText;
    public Text adBtnText2;

    public Button freeBtn;
    public CanvasGroup freeBrnCanvasGroup;
    public GameObject freeCooling;
    public Text freeCoolingTime;

    public RewardAdButton rewardAdButton;
    public Button hideBtn;

    private GetGoldPanelData getGoldPanelData;
    private int coolingSecond = 300;
    private int freeCnt = 5;
    private int freeRewardGoldNum = 10;
    private int adRewardGoldNum = 20;
    private string page_id = "GetGoldPanel";

    public int coolingTimeSeconds;
    private float timer;

    private string saveDataKey = "";

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
    void Start()
    {
        freeBtn.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayBtnMusic();
            GoldUI.isLongTimerAnim = true;
            GoldCollectEffect.Instance.StartEffect(ItemType.Gold, freeBtn.transform.position, UIManager.Instance.playInfoUI.goldUI.icon.transform.position);
            DOTween.Sequence().AppendInterval(0.7f).AppendCallback(() =>
            {
                EventManager.Instance.TriggerEvent(GameEvent.GetGold, freeRewardGoldNum);              
            });
            getGoldPanelData.cnt++;
            var curTime = GameManager.Instance.GetNowTime();
            double addTime = getGoldPanelData.cnt < freeCnt ? (double)coolingSecond : 0f;
            curTime = curTime.AddSeconds(addTime);
            getGoldPanelData.coolingTimeStamp = GameManager.DateTimeToTimeStamp(curTime).ToString();
            SaveGetGoldPanelData();
            UpdateFreeGoldState();

            AdManager.Instance.OnClickInterstitialAd(page_id, true);
            CustomApiManager.Instance.RequestCustomEventV2(new CustomEventData()
            {
                page_id = "GoldGain",
                name = "Event_GoldGain",
                value = "store_free",
            });
        });

        hideBtn.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayBtnMusic();
            Hide();
            AdManager.Instance.OnClickInterstitialAd(page_id, true);
        });

        explain.text = LanguageManager.Instance.GetText("GetGoldPanel_explain");
        explain2.text = LanguageManager.Instance.GetText("GetGoldPanel_explain2");
        freeBtnText2.text = freeRewardGoldNum + " " + LanguageManager.Instance.GetText("Gold");
        adBtnText.text = LanguageManager.Instance.GetText("Ads");
        adBtnText2.text = adRewardGoldNum +" "+ LanguageManager.Instance.GetText("Gold") ;
    }

    private void Update()
    {
        if(coolingTimeSeconds > 0)
        {
            timer += Time.deltaTime;
            if(timer > 1f)
            {
                timer = 0f;
                coolingTimeSeconds--;
                if(coolingTimeSeconds <= 0)
                {
                    UpdateFreeGoldState();
                }
                else
                {
                    UpdateFreeCooling();
                }
             
            }
        }
    }

    public override void Refresh(object data = null)
    {
        base.Refresh(data);

        saveDataKey = GameManager.Instance.curMachine.type == MachineType.Base ? "GetGoldPanelData" : "GetGoldPanelData_Difficult";

        string savedata = PlayerPrefs.GetString(saveDataKey, "");
        if (string.IsNullOrEmpty(savedata))
        {
            ResetGetGoldPanelData();
        }
        else
        {
            getGoldPanelData = JsonConvert.DeserializeObject<GetGoldPanelData>(savedata);
        }
        string curTime = GameManager.Instance.GetNowTime().ToString("yyyy-MM-dd");
        if (curTime != getGoldPanelData.today)
        {
            ResetGetGoldPanelData();
        }
        UpdateFreeGoldState();
        rewardAdButton.Init(AdRewardCallback, page_id);
    }
    public override void Hide()
    {
        base.Hide();
    }
  
    private void AdRewardCallback()
    {
        GoldUI.isLongTimerAnim = true;
        GoldCollectEffect.Instance.StartEffect(ItemType.Gold, rewardAdButton.transform.position, UIManager.Instance.playInfoUI.goldUI.icon.transform.position);
        DOTween.Sequence().AppendInterval(0.7f).AppendCallback(() =>
        {
            EventManager.Instance.TriggerEvent(GameEvent.GetGold, adRewardGoldNum);
            rewardAdButton.Init(AdRewardCallback, page_id);
        });

        CustomApiManager.Instance.RequestCustomEventV2(new CustomEventData()
        {
            page_id = "GoldGain",
            name = "Event_GoldGain",
            value = "store_ad",
        });
    }

    private void UpdateFreeGoldState()
    {
        string str = LanguageManager.Instance.GetText("GetGoldPanel_freeBtnText");
        freeBtnText.text = $"{str}({getGoldPanelData.cnt}/{freeCnt})";

        var curTime = GameManager.Instance.GetNowTime();
        var coolingTime = GameManager.TimeStampToDateTime(ulong.Parse(getGoldPanelData.coolingTimeStamp));
        // 计算两个时间的间隔
        TimeSpan timeDiff = coolingTime - curTime;
        int diffSeconds = (int)timeDiff.TotalSeconds;
        coolingTimeSeconds = Mathf.Clamp(diffSeconds, 0, diffSeconds);
        timer = 0;

        bool isclick = getGoldPanelData.cnt < freeCnt && coolingTimeSeconds <= 0;
        freeBtn.interactable = isclick;
        freeBrnCanvasGroup.alpha = isclick ? 1f : 0.5f;
        UpdateFreeCooling();
    }

    private void ResetGetGoldPanelData()
    {
        var curTime = GameManager.Instance.GetNowTime();
        getGoldPanelData = new GetGoldPanelData();
        getGoldPanelData.today = curTime.ToString("yyyy-MM-dd");
        getGoldPanelData.cnt = 0;
        getGoldPanelData.coolingTimeStamp = GameManager.DateTimeToTimeStamp(curTime).ToString();
    }

    private void UpdateFreeCooling()
    {
        freeCooling.SetActive(coolingTimeSeconds > 0f);
        int minutes = (int)(coolingTimeSeconds / 60f);
        int seconds = (int)(coolingTimeSeconds % 60f);
        freeCoolingTime.text = $"{minutes:D2}:{seconds:D2}";
    }

    private void SaveGetGoldPanelData()
    {
        if (getGoldPanelData == null) return;
        string jsonStr = JsonConvert.SerializeObject(getGoldPanelData, Formatting.Indented);
        PlayerPrefs.SetString(saveDataKey, jsonStr);
        PlayerPrefs.Save();
        Debug.Log(saveDataKey + " --获取金币界面数据保存成功：" + jsonStr);
    }
}

public class GetGoldPanelData
{
    public string today;
    public int cnt;
    public string coolingTimeStamp;
}

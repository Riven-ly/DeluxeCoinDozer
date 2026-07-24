using DG.Tweening;
using DG.Tweening.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class GoldUI : MonoBehaviour
{
    public Image sliderBg;
    public List<Sprite> sliderBgSprites;
    public Image btnBg;
    public List<Sprite> btnBgSprites;

    public Slider slider;
    public Image icon;
    public Text numText;
    public Text str;
    public Button btn;

    private int strNum;
    private int goldNum;
    private bool isAwaitGoldTween;//金币跳动是否正在动画
    private float strAwaitTimer; 
    private bool alreadyUpdateStr = true;//已经更新str
    private bool isAwaitGoldSideTween; //自动获得金币进度条是否正在动画
    private float autoGetGoldTime = 30f;//自动获得金币时间
    private int autoGetGoldCheckValue = 20; //自动获得金币临界值

    public string saveDataKey = "";
    public static bool isLongTimerAnim = false;
    private void Start()
    {
        btn.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayBtnMusic();
            UIManager.Instance.OpenUI<GetGoldPanel>();
        });
    }

    private void Update()
    {
        if (!isAwaitGoldSideTween)
        {
            //自动获得金币判定
            if (GameManager.Instance.curMachine != null && GameManager.Instance.curMachine.GetGold() < autoGetGoldCheckValue)
            {
                isAwaitGoldSideTween = true;
                SaveAutoGetGoldTime();
                slider.DOValue(1f, autoGetGoldTime).OnComplete(() =>
                {
                    isAwaitGoldSideTween = false;
                    slider.value = 0f;
                    EventManager.Instance.TriggerEvent(GameEvent.GetGold, 1);

                    CustomApiManager.Instance.RequestCustomEventV2(new CustomEventData()
                    {
                        page_id = "GoldGain",
                        name = "Event_GoldGain",
                        value = "auto_recover",
                    });
                });
            }
        }

        //str存在时间
        if (!alreadyUpdateStr)
        {
            strAwaitTimer -= Time.deltaTime;
            if (strAwaitTimer < 0)
            {
                strNum = 0;
                str.text = "";
                alreadyUpdateStr = true;
            }
        }
    }

    public void Init()
    {
        sliderBg.sprite = sliderBgSprites[(int)GameManager.Instance.curMachine.type];
        btnBg.sprite = btnBgSprites[(int)GameManager.Instance.curMachine.type];

        saveDataKey = GameManager.Instance.curMachine.type == MachineType.Base ? "AutoGetGold" : "AutoGetGold_Difficult";
        strNum = 0;
        goldNum = GameManager.Instance.curMachine.GetGold();
        numText.text = goldNum.ToString();
        str.text = "";

        slider.DOKill();
        slider.value = 0f;
        isAwaitGoldSideTween = false;

        CheckAutoGetGold();
    }
    //消耗金币（金币动画跳动不显示，跳动结束会校准）
    public void ExpendGold(int _num)
    {
        if (isAwaitGoldTween)
        {
            return;
        }
        goldNum -= _num;
        numText.text = goldNum.ToString();
    }
    public void AddGold(int _num)
    {
        if (_num == 0)
            return;

        //检查是否正在获得金币
        CheckAutoGetGoldAnim();
        TemporaryStr(_num);

        this.DOKill();
        StartGoldAnim();
    }
    //临时记录并显示获得的金币数量
    private void TemporaryStr(int _num)
    {
        strNum += _num;
        str.text = strNum.ToString();

        alreadyUpdateStr = false;
        strAwaitTimer = 2f;
    }
    /// <summary>
    /// 当金币大于指定值时且自动获得金币在进行就关闭自动获得
    /// </summary>
    private void CheckAutoGetGoldAnim()
    {
        if (GameManager.Instance.curMachine.GetGold() >= autoGetGoldCheckValue)
        {
            slider.DOKill();
            slider.value = 0f;
            isAwaitGoldSideTween = false;
        }
    }
    /// <summary>
    /// 检擦离线时间自动获得金币的数量
    /// </summary>
    private void CheckAutoGetGold()
    {
        if (GameManager.Instance.curMachine.GetGold() >= autoGetGoldCheckValue)
        {
            return;
        }
        string timeStr = PlayerPrefs.GetString(saveDataKey, "");
        if (string.IsNullOrEmpty(timeStr))
        {
            return;
        }
        ulong oldTimeStamp = ulong.Parse(timeStr);
        DateTime curTime = GameManager.Instance.GetNowTime();
        ulong curTimeStamp = GameManager.DateTimeToTimeStamp(curTime);

        int diffTimeStamp = (int)(curTimeStamp - oldTimeStamp);
        int autoGetGoldNum = diffTimeStamp / (int)autoGetGoldTime;
        autoGetGoldNum = Mathf.Clamp(autoGetGoldNum, 0, autoGetGoldCheckValue);//离线最多获得到10
        if (autoGetGoldNum > 0)
        {
            EventManager.Instance.TriggerEvent(GameEvent.GetGold, autoGetGoldNum);
        }
    }
    /// <summary>
    /// 保存开始自动获得金币的时间
    /// </summary>
    private void SaveAutoGetGoldTime()
    {
        DateTime curTime = GameManager.Instance.GetNowTime();
        //时间戳
        ulong timeStamp = GameManager.DateTimeToTimeStamp(curTime);
        PlayerPrefs.SetString(saveDataKey, timeStamp.ToString());
        PlayerPrefs.Save();
    }

    private void StartGoldAnim()
    {
        float targetTime = 0.5f;
        if(isLongTimerAnim)
        {
            isLongTimerAnim = false;
            targetTime = 1.2f;
        }

        isAwaitGoldTween = true;     
        int targetGold = GameManager.Instance.curMachine.GetGold();
        int _currentValue = goldNum;
        DOTween.To(
          () => _currentValue,
          x =>
          {
              goldNum = x;
              numText.text = x.ToString();
          },
          targetGold, // 目标值
          targetTime // 时长
      ).SetTarget(this)
      .OnComplete(() =>
      {
          //校准，动画过程中会有消耗金币的操作
          goldNum = GameManager.Instance.curMachine.GetGold();
          numText.text = goldNum.ToString();
          isAwaitGoldTween = false;
      });
    }

}

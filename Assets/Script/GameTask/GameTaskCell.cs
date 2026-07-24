using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTaskCell : MonoBehaviour
{
    public Text explain;
    public Slider slider;
    public Text sliderStr;
    public GameObject taskinProgress;
    public GameObject Claimed;
    public GameObject ClaimedMask;
    public Button claim;
    public Text reward;

    private GameTaskInfo gameTaskInfo;
    // Start is called before the first frame update
    void Start()
    {
        claim.onClick.AddListener(() =>
        {
            if (gameTaskInfo == null)
                return;

            if (!gameTaskInfo.IsComplete)
                return;

            if (gameTaskInfo.isCollect)
                return;

            AudioManager.Instance.PlayBtnMusic();
            taskinProgress.SetActive(false);
            Claimed.SetActive(true);
            ClaimedMask.SetActive(true);
            claim.gameObject.SetActive(false);
            gameTaskInfo.isCollect = true;

            GoldCollectEffect.Instance.StartEffect(ItemType.Gold, claim.transform.position, UIManager.Instance.playInfoUI.goldUI.icon.transform.position);
            EventManager.Instance.TriggerEvent(GameEvent.UpdateTaskRedDot);
            DOTween.Sequence().AppendInterval(0.7f).AppendCallback(() =>
            {
                GoldUI.isLongTimerAnim = true;
                EventManager.Instance.TriggerEvent(GameEvent.GetGold, (int)gameTaskInfo.reward);
            });

            CustomApiManager.Instance.RequestCustomEventV2(new CustomEventData()
            {
                page_id = "GameTaskPanel",
                name = "Event_TaskClaim",
                value = $"{gameTaskInfo.gameTaskType.ToString()}",
            });

            CustomApiManager.Instance.RequestCustomEventV2(new CustomEventData()
            {
                page_id = "GoldGain",
                name = "Event_GoldGain",
                value = "task_daily",
            });
        });
    }

    public void Init(GameTaskInfo data)
    {
        gameTaskInfo = data;
        explain.text = string.Format(gameTaskInfo.explain, gameTaskInfo.maxCnt);
        slider.value = (float)gameTaskInfo.cnt / (float)gameTaskInfo.maxCnt;
        sliderStr.text = $"{gameTaskInfo.cnt}/{gameTaskInfo.maxCnt}";
        taskinProgress.SetActive(!gameTaskInfo.IsComplete);
        Claimed.SetActive(gameTaskInfo.IsComplete && gameTaskInfo.isCollect);
        ClaimedMask.SetActive(gameTaskInfo.IsComplete && gameTaskInfo.isCollect);
        claim.gameObject.SetActive(gameTaskInfo.IsComplete && !gameTaskInfo.isCollect);
        reward.text = "+" + gameTaskInfo.reward.ToString();

        if(gameTaskInfo.IsComplete && gameTaskInfo.isCollect)
        {
            transform.SetAsLastSibling();
        }
    }

}

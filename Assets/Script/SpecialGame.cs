using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialGame : MonoBehaviour, IEventListener
{
    int dropGoldCnt = 0;
    bool isDropGoldCooling;
    private void OnEnable()
    {
        EventManager.Instance.RegisterListener(GameEvent.GetMachineItemReward, this);
    }
    private void OnDisable()
    {
        EventManager.Instance.UnregisterListener(GameEvent.GetMachineItemReward, this);
    }

    public void OnEventTriggered(GameEvent eventType, object data = null)
    {

        if(eventType == GameEvent.GetMachineItemReward)
        {
            if (isDropGoldCooling)
                return;

            if (!GameManager.TrySceneClick)
                return;

            dropGoldCnt++;
            CheckTriggerSpecialGame();
            isDropGoldCooling = true;
            DOTween.Sequence().AppendInterval(1f).AppendCallback(() =>
            {
                isDropGoldCooling = false;
            });
        }
    }

    private void CheckTriggerSpecialGame()
    {
        float prob = 0f;
        int curLv = GameManager.Instance.playerInfo.playerData.level;
        if (curLv >= 3 && curLv < 10)
        {
            prob = (0.1f + (dropGoldCnt - 5)) * 0.15f;
        }
        else if (curLv >= 10 && curLv < 15)
        {
            prob = (0.1f + (dropGoldCnt - 4)) * 0.15f;
        }
        else if (curLv >= 15)
        {
            prob = (0.1f + (dropGoldCnt - 3)) * 0.15f;
        }
        else
        {
            dropGoldCnt = 0;
        }

        prob = Mathf.Clamp(prob, 0f, 1f);
        Debug.Log(dropGoldCnt + " 当前特殊玩法概率 ：" + prob);
        //生成 0~1 之间的随机数
        float randomValue = Random.value;
        if (randomValue < prob)
        {
            float subRandom = Random.value;
            if (subRandom < 0.85f) 
            {
                TriggerCardGame();
            }
            else 
            {
                TriggerDropGame();
            }
            dropGoldCnt = 0;
        }
    }

    private void TriggerCardGame()
    {
        Debug.Log("进入卡牌玩法");
        UIManager.Instance.OpenUI<CardGamePanel>();
    }
    private void TriggerDropGame()
    {
        Debug.Log("进入接取玩法");
        UIManager.Instance.OpenUI<DropRewardsPanel>();
    }
}

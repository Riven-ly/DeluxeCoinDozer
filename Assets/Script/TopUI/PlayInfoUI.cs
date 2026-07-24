using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayInfoUI : MonoBehaviour, IEventListener
{
    public Image bg;
    public List<Sprite> bgSprites;

    public GoldUI goldUI;
    public LevelUI levelUI;
    public DiamondUI diamondUI;
    public DifficultMachineUI difficultMachineUI;
    public SettingUI settingUI;
    private void Awake()
    {
        RectTransform rect = GetComponent<RectTransform>();
        float topBlockHeight = Screen.height - Screen.safeArea.yMax;
        rect.offsetMax = new Vector2(0, -topBlockHeight);
    }

    private void OnEnable()
    {
        EventManager.Instance.RegisterListener(GameEvent.CreatMachineItem, this);
        EventManager.Instance.RegisterListener(GameEvent.GetMachineItemReward, this);
        EventManager.Instance.RegisterListener(GameEvent.GetGold, this);
        EventManager.Instance.RegisterListener(GameEvent.GetDiamond, this);
    }

    private void OnDisable()
    {
        EventManager.Instance.UnregisterListener(GameEvent.CreatMachineItem, this);
        EventManager.Instance.UnregisterListener(GameEvent.GetMachineItemReward, this);
        EventManager.Instance.UnregisterListener(GameEvent.GetGold, this);
        EventManager.Instance.UnregisterListener(GameEvent.GetDiamond, this);
    }

    public void Init()
    {
        bg.sprite = bgSprites[(int)GameManager.Instance.curMachine.type];
        difficultMachineUI.gameObject.SetActive(GameManager.Instance.curMachine.type == MachineType.Difficult);

        goldUI.Init();
        levelUI.Init();
        diamondUI.Init();
        settingUI.Init();
    }
    public void EnterBaseMachineUIAnim()
    {
        Init();
        transform.localPosition = new Vector3(0, 200f, 0);
        transform.DOLocalMoveY(0f, 1f);
    }
    public void EnterBaseMachineUIAnim2()
    {
        DOTween.Sequence()
            .Append(transform.DOLocalMoveY(350f, 1f))
            .AppendCallback(() =>
            {
                Init();
            })
            .Append(transform.DOLocalMoveY(0f, 1f));
    }

    public void EnterDifficultMachineUIAnim()
    {
        DOTween.Sequence()
            .Append(transform.DOLocalMoveY(350f, 1f))
            .AppendCallback(() =>
            {
                Init();
            })
            .Append(transform.DOLocalMoveY(0f, 1f));
    }

    public void OnEventTriggered(GameEvent eventType, object data = null)
    {
        switch (eventType)
        {
            case GameEvent.CreatMachineItem:
                GameManager.Instance.curMachine.ExpendGold(1);
                goldUI.ExpendGold(1);
                break;
            case GameEvent.GetMachineItemReward:
                GameManager.Instance.curMachine.GetMachineItemReward((int)data);
                goldUI.AddGold((int)data);
                levelUI.AddLvEx();
                break;
            case GameEvent.GetGold:
                AudioManager.Instance.PlayGetGoldMusic();
                GameManager.Instance.curMachine.AddGold((int)data);
                goldUI.AddGold((int)data);
                break;
            case GameEvent.GetDiamond:
                GameManager.Instance.playerInfo.AddDiamond((float)data);
                diamondUI.AddDiamond();
                break;
        }
    }

}

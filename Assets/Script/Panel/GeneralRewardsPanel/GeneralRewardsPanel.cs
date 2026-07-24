using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeneralRewardsPanel : UIBase
{
    public RewardAdButton rewardAdButton;
    public Button collectBtn;

    public Transform itemsRoot;
    public List<Transform> items_1_6;
    private List<ItemBase> items;

    private string page_id = "RewardsPanel";
    private static bool isDoubleReward = false;
    private void Start()
    {
        collectBtn.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayBtnMusic();
            GetReward();
            Hide();
        });
    }

    public override void Refresh(object data = null)
    {
        base.Refresh(data);
        AudioManager.Instance.PlaySceneSingleMusic("GetItemPanel");

        isDoubleReward = false;
        List<ItemData> datas = data as List<ItemData>;

        items = GameManager.Instance.CreatItems(datas, itemsRoot, true);
        if(items.Count <= items_1_6.Count)
        {
            int index = 0;
            foreach (Transform trans in items_1_6[items.Count - 1])
            {
                items[index].transform.SetParent(trans);
                items[index].transform.localPosition = Vector3.zero;
                items[index].transform.localScale = Vector3.one;
                index++;
            }
        }
        bool _isContainAdmob = false;
        foreach (var item in datas)
        {
            if(item.itemType == ItemType.Diamond || item.itemType == ItemType.Gold)
            {
                _isContainAdmob = true;
            }
        }
        rewardAdButton.Init(AdRewardCallback, page_id, _isContainAdmob);

        collectBtn.transform.localScale = Vector3.zero;
        DOTween.Sequence()
                     .AppendInterval(1.5f)
                     .Append(collectBtn.transform.DOScale(1.1f, 0.2f))
                     .Append(collectBtn.transform.DOScale(0.9f, 0.1f))
                     .Append(collectBtn.transform.DOScale(1f, 0.1f));
    }
    public override void Hide()
    {
        UIManager.Instance.OpenUIMask();
        panelAnim.Play("GeneralHidePanelAnim");
        DOTween.Sequence().AppendInterval(15f / 60f).OnComplete(() =>
        {
            callback?.Invoke();//奖励在回调里，为了做动画
            callback = null;
            gameObject.SetActive(false);
            UIManager.Instance.HideUIMask();

            awaitHideAction?.Invoke();
            awaitHideAction = null;


            foreach (var item in items)
            {
                Destroy(item.gameObject);
            }
            items = null;
        });
    }
    private void GetReward()
    {
        isDoubleReward = false;
    }
    private void AdRewardCallback()
    {
        isDoubleReward = true;
        Hide();
    }

    /// <summary>
    /// 双倍领奖
    /// </summary>
    /// <returns></returns>
    public static bool GetIsDoubleReward()
    {
        bool isDouble = isDoubleReward;
        isDoubleReward = false;
        return isDouble;
    }
}

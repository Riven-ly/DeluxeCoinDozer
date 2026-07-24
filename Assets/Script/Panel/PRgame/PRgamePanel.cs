using DG.Tweening;
using System.Collections;
using System.Collections.Generic;

public class PRgamePanel : UIBase
{
    public override void Refresh(object data = null)
    {
        base.Refresh(data);

        CustomApiManager.Instance.RequestCustomEventV2(new CustomEventData()
        {
            page_id = "VerifyPanel",
            name = "Event_VerifyShow",
            value = "",
        });
    }
    public override void Hide()
    {
        base.Hide();
    }

    public void IsPRGamePass(bool isPass)
    {
        if(isPass)
        {
            PRgameManager.Instance.SaveGameYanzhengResult(true);
            List<ItemData> itemDatas = new List<ItemData>();
            itemDatas.Add(new ItemData(ItemType.Gold, 20));
            UIManager.Instance.OpenUI<GeneralRewardsPanel>(itemDatas, () =>
            {
                bool isDoublereward = GeneralRewardsPanel.GetIsDoubleReward();
                EventManager.Instance.TriggerEvent(GameEvent.GetGold, 20);
                if (isDoublereward)
                {
                    EventManager.Instance.TriggerEvent(GameEvent.GetGold, 20);
                }
                Hide();
            });

            CustomApiManager.Instance.RequestCustomEventV2(new CustomEventData()
            {
                page_id = "VerifyPanel",
                name = "Event_VerifySuccess",
                value = "",
            });
        }
        else
        {
            PRgameManager.Instance.SaveGameYanzhengResult(false);
            Hide();

            CustomApiManager.Instance.RequestCustomEventV2(new CustomEventData()
            {
                page_id = "VerifyPanel",
                name = "Event_VerifyFail",
                value = "",
            });
        }
    }
}

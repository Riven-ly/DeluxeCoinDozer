using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DailySignInPanel : UIBase
{
    public Button hideBtn;
    public List<DailySignInCell> dailySignInCells;

    // Start is called before the first frame update
    void Start()
    {
        hideBtn.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayBtnMusic();
            Hide();
        });

        List<List<ItemData>> config = new List<List<ItemData>>();
        config.Add(new List<ItemData>() { new ItemData(ItemType.Gold, 20) });
        config.Add(new List<ItemData>() { new ItemData(ItemType.Gold, 50) });
        config.Add(new List<ItemData>() { new ItemData(ItemType.Machine_Vibration, 1) });
        config.Add(new List<ItemData>() { new ItemData(ItemType.Diamond, 50) });
        config.Add(new List<ItemData>() { new ItemData(ItemType.Gold_Explode, 3) });
        config.Add(new List<ItemData>() { new ItemData(ItemType.Gold, 100) });
        config.Add(new List<ItemData>() { new ItemData(ItemType.Diamond, 100), new ItemData(ItemType.City_Wall, 3), new ItemData(ItemType.Machine_Vibration, 3) });

        for (int day = 0; day < dailySignInCells.Count; day++)
        {
            dailySignInCells[day].Init(day + 1, config[day]);
        }
    }

    public override void Refresh(object data = null)
    {
        base.Refresh(data);
        bool isTodaySignIn = DailySignIn.CheckSignIn();//ÅÐ¶ÏÊÇ·ñÇ©µ½

        for (int day = 1; day <= dailySignInCells.Count; day++)
        {
            dailySignInCells[day - 1].clickBtn.interactable = false;
            dailySignInCells[day - 1].IsToday(false);
            dailySignInCells[day - 1].effectRoot.gameObject.SetActive(false);

            dailySignInCells[day - 1].SignInState(day <= DailySignIn.currentDay);
        }

        int curDayIndex = isTodaySignIn == true ? DailySignIn.currentDay - 1 : DailySignIn.currentDay;
        dailySignInCells[curDayIndex].clickBtn.interactable = !isTodaySignIn;
        dailySignInCells[curDayIndex].effectRoot.gameObject.SetActive(!isTodaySignIn);
        dailySignInCells[curDayIndex].IsToday(true);

        CustomApiManager.Instance.RequestCustomEventV2(new CustomEventData()
        {
            page_id = "DailySignInPanel",
            name = "Event_SignOpen",
            value = "",
        });
    }
    public override void Hide()
    {
        base.Hide();
    }
  


}

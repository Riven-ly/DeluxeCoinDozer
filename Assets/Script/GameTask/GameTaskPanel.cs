using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTaskPanel : UIBase
{
    public Button hideBtn;

    public Button dailyTaskBtn;
    public Button dailyTaskBtn2;

    public Button levelTaskBtn;
    public Button levelTaskBtn2;

    public Button gachaponTaskBtn;
    public Button gachaponTaskBtn2;

    public ScrollRect dailyTaskCells;
    public Transform dailyTaskCellsContent;
    public ScrollRect levelTaskCells;
    public Transform levelTaskCellsContent;
    public ScrollRect gachaponTaskCells;
    public Transform gachaponTaskCellsContent;
    public GameObject gameTaskCellPrefab;

    private List<GameTaskCell> dailyGameTaskCell;
    private List<GameTaskCell> lv_GameTaskCell;
    private List<GameTaskCell> gap_GameTaskCell;

    private void Start()
    {
        hideBtn.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayBtnMusic();
            Hide();
        });
        dailyTaskBtn.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayBtnMusic();
            ChangeSelectBtn(0);
        });
        levelTaskBtn.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayBtnMusic();
            ChangeSelectBtn(1);
        });
        gachaponTaskBtn.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayBtnMusic();
            ChangeSelectBtn(2);
        });

        dailyTaskBtn2.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayBtnMusic();
            ChangeSelectBtn(0);
        });
        levelTaskBtn2.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayBtnMusic();
            ChangeSelectBtn(1);
        });
        gachaponTaskBtn2.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayBtnMusic();
            ChangeSelectBtn(2);
        });
    }

    public override void Refresh(object data = null)
    {
        base.Refresh(data);
        GameTask gameTask = data as GameTask;

        ChangeSelectBtn(0);

        DailyGameTask dailyGameTask = gameTask.dailyGameTask;
        //每日任务
        if (dailyGameTaskCell == null)
        {
            float dailyTaskCellsContent_height = 0;
            dailyGameTaskCell = new List<GameTaskCell>();
            foreach (var task in dailyGameTask.dailyTasks)
            {
                var obj = Instantiate(gameTaskCellPrefab, dailyTaskCellsContent);
                dailyGameTaskCell.Add(obj.transform.GetComponent<GameTaskCell>());
                dailyTaskCellsContent_height += 220f;
            }
            dailyTaskCellsContent.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(0, dailyTaskCellsContent_height);
        }
        for (int i = 0; i < dailyGameTaskCell.Count; i++)
        {
            dailyGameTaskCell[i].Init(dailyGameTask.dailyTasks[i]);
        }
        dailyTaskCells.verticalNormalizedPosition = 1f;
        //升级任务
        var lvTasks = gameTask.otherGameTask.lvUpGameTask;
        if(lv_GameTaskCell == null)
        {
            float _lvContent_height = 0;
            lv_GameTaskCell = new List<GameTaskCell>();
            foreach (var task in lvTasks)
            {
                var obj = Instantiate(gameTaskCellPrefab, levelTaskCellsContent);
                lv_GameTaskCell.Add(obj.transform.GetComponent<GameTaskCell>());
                _lvContent_height += 220f;
            }
            levelTaskCellsContent.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(0, _lvContent_height);
        }
        for (int i = 0; i < lv_GameTaskCell.Count; i++)
        {
            lvTasks[i].cnt = GameManager.Instance.playerInfo.playerData.level;
            lvTasks[i].cnt = Mathf.Clamp(lvTasks[i].cnt, 1, lvTasks[i].maxCnt);
            lv_GameTaskCell[i].Init(lvTasks[i]);
        }
        levelTaskCells.verticalNormalizedPosition = 1f;
        //扭蛋任务
        var gapTasks = gameTask.otherGameTask.gachaponGameTask;
        if (gap_GameTaskCell == null)
        {
            float _gapContent_height = 0;
            gap_GameTaskCell = new List<GameTaskCell>();
            foreach (var task in gapTasks)
            {
                var obj = Instantiate(gameTaskCellPrefab, gachaponTaskCellsContent);
                gap_GameTaskCell.Add(obj.transform.GetComponent<GameTaskCell>());
                _gapContent_height += 220f;
            }
            gachaponTaskCellsContent.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(0, _gapContent_height);
        }
        for (int i = 0; i < gap_GameTaskCell.Count; i++)
        {
            gap_GameTaskCell[i].Init(gapTasks[i]);
        }
        gachaponTaskCells.verticalNormalizedPosition = 1f;


        CustomApiManager.Instance.RequestCustomEventV2(new CustomEventData()
        {
            page_id = "GameTaskPanel",
            name = "Event_TaskOpen",
            value = "",
        });
    }
    public override void Hide()
    {
        base.Hide();
    }

    private void ChangeSelectBtn(int index)
    {
        dailyTaskCells.gameObject.SetActive(index == 0);
        levelTaskCells.gameObject.SetActive(index == 1);
        gachaponTaskCells.gameObject.SetActive(index == 2);


        dailyTaskBtn.gameObject.SetActive(index != 0);
        levelTaskBtn.gameObject.SetActive(index != 1);
        gachaponTaskBtn.gameObject.SetActive(index != 2);


        dailyTaskBtn2.gameObject.SetActive(index == 0);
        levelTaskBtn2.gameObject.SetActive(index == 1);
        gachaponTaskBtn2.gameObject.SetActive(index == 2);
    }
}

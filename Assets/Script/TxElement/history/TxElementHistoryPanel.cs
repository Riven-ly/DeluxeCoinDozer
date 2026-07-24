using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class TxElementHistoryPanel : UIBase
{
    public Text title;
    public Button hideBtn;

    public TxElementHistoryCell prefab;
    public ScrollRect scrollRect;
    void Start()
    {
        hideBtn.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayBtnMusic();
            Hide();
        });
    }
    public override void Refresh(object data = null)
    {
        base.Refresh(data);

        title.text = LanguageManager.Instance.GetText_Encrypt("WH");

        var listHistory = TxElementMananger.Instance.info.historyInfo.ToList();

        if(TxElementMananger.Instance.info.orderStatus != TxElementType.Init)
        {
            TxElementHistoryInfo curCellInfo = new TxElementHistoryInfo();
            curCellInfo.type = TxElementMananger.Instance.info.accountInfo.type;
            if (TxElementMananger.Instance.info.orderStatus == TxElementType.QueueUp)
            {
                curCellInfo.count = TxElementMananger.Instance.info.queueUpInfo.diamond;
                curCellInfo.time = TxElementMananger.Instance.info.queueUpInfo.startTime;
                curCellInfo.state = 1;
            }
            else if (TxElementMananger.Instance.info.orderStatus == TxElementType.Task)
            {
                curCellInfo.count = TxElementMananger.Instance.info.taskInfo.diamond;
                curCellInfo.time = TxElementMananger.Instance.info.taskInfo.historyTime;
                curCellInfo.state = 2;
            }
            listHistory.Add(curCellInfo);
        }

        float temHeight = 0;
        foreach (var info in listHistory)
        {
            var obj = Instantiate(prefab, scrollRect.content.transform);
            TxElementHistoryCell cell = obj.GetComponent<TxElementHistoryCell>();
            cell.Init(info);
            temHeight += 280;
        }

        scrollRect.content.GetComponent<RectTransform>().sizeDelta = new Vector2(0, temHeight);
        scrollRect.verticalNormalizedPosition = 1f;
    }
    public override void Hide()
    {
        callback = () =>
        {
            foreach (Transform cell in scrollRect.content.transform)
            {
                Destroy(cell.gameObject);
            }
        };
        base.Hide();
    }
}

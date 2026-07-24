using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class ItemFragmentPanel : UIBase
{
    public ScrollRect scrollRect;
    public InputField inputField;
    public Button inputFieldBtn;
    public Button hideBtn;
    public Button TipsBtn;
    public List<ItemFragmentPanelCell> cells;

    private string lastStr;
    private ItemFragment itemFragment;
    private void Start()
    {
        inputFieldBtn.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayBtnMusic();
            InputfieldClick();
        });
        hideBtn.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayBtnMusic();
            Hide();
        });
        TipsBtn.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayBtnMusic();
            UIManager.Instance.OpenUI<ItemFragmentTipsPanel>();
        });
    }
    public override void Refresh(object data = null)
    {
        base.Refresh(data);
        itemFragment = data as ItemFragment;

        inputField.text = GameManager.GetAccountEmail();

        for (int i = 0; i < cells.Count; i++)
        {
            ItemFragmentInfo info = itemFragment.itemFragmentInfos[i];
            Sprite sp = itemFragment.GetItemFragmentSprite(info.type);
            cells[i].Init(sp, info);
        }
        scrollRect.verticalNormalizedPosition = 1f;

        CustomApiManager.Instance.RequestCustomEventV2(new CustomEventData()
        {
            page_id = "ShardPanel",
            name = "Event_ShardOpen",
            value = "",
        });
    }
    public override void Hide()
    {
        base.Hide();
    }

    private void InputfieldClick()
    {
        if (!string.IsNullOrEmpty(inputField.text) && inputField.text == lastStr)
        {
            return;
        }
        lastStr = inputField.text;
        if (GameManager.CheckSimpleEmail(inputField.text))
        {
            GameManager.SaveAccountEmail(inputField.text);
        }
        else
        {
            string str = LanguageManager.Instance.GetText("EmailError");
           UIManager.Instance.OpenUI<GeneralTipsPanel>(str);
        }
    }
}

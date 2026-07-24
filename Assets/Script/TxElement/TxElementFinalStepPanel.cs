using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TxElementFinalStepPanel : UIBase
{
    public Button hideBtn;
    public Text title;
    public Text explain;
    public Text countText;
    public Text explain2;

    public GameObject c;
    public GameObject c_have;
    public GameObject l;
    public GameObject l_have;
    public GameObject e;
    public GameObject e_have;
    public GameObject a;
    public GameObject a_have;
    public GameObject r;
    public GameObject r_have;
    // Start is called before the first frame update
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

        title.text = LanguageManager.Instance.GetText("FinalStep_title");
        explain.text = LanguageManager.Instance.GetText("FinalStep_explain");
        explain2.text = LanguageManager.Instance.GetText("FinalStep_explain2");

        string str4 = LanguageManager.Instance.GetText_Encrypt("Special_Diamond__unit");
        TxElementTaskInfo _info = TxElementMananger.Instance.info.taskInfo;
        countText.text = $"{str4}{_info.diamond}";

        c.gameObject.SetActive(!_info.isHave_C);
        c_have.gameObject.SetActive(_info.isHave_C);

        l.gameObject.SetActive(!_info.isHave_L);
        l_have.gameObject.SetActive(_info.isHave_L);

        e.gameObject.SetActive(!_info.isHave_E);
        e_have.gameObject.SetActive(_info.isHave_E);

        a.gameObject.SetActive(!_info.isHave_A);
        a_have.gameObject.SetActive(_info.isHave_A);

        r.gameObject.SetActive(!_info.isHave_R);
        r_have.gameObject.SetActive(_info.isHave_R);
    }
    public override void Hide()
    {
        base.Hide();
    }
}

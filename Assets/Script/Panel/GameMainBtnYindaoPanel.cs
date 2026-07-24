using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMainBtnYindaoPanel : UIBase
{
    public Button btn;
    public Transform shuomingTrans;
    public Text shuoming;
    public Transform shouzhiAll;
    public Transform shouzhi;
    public Transform shouzhi2;

    private Button tagetBtn;

    private void OnDisable()
    {
        isOpen = false;
        shouzhiAll.transform.localPosition = Vector3.zero;
    }

    private void Start()
    {
        btn.onClick.Invoke();
        btn.onClick.AddListener(() =>
        {
            if (tagetBtn != null)
            {
                tagetBtn.onClick.Invoke();
            }
            Hide();
        });
    }

    private void Update()
    {
        if(tagetBtn != null)
        {
            shouzhiAll.transform.position = tagetBtn.transform.position;
        }
    }

    public override void Refresh(object data = null)
    {
        base.Refresh(data);
        tagetBtn = null;
        shuomingTrans.gameObject.SetActive(true);

        List<object> listdata = data as List<object>;
        string str = (string)listdata[0];
        tagetBtn = listdata[1] as Button;
        bool isFan = (bool)listdata[2];


        if (string.IsNullOrEmpty(str))
        {
            shuomingTrans.gameObject.SetActive(false);
        }
        else
        {
            shuoming.text = str;
        }
        shouzhiAll.transform.position = tagetBtn.transform.position;
        shouzhi.gameObject.SetActive(!isFan);
        shouzhi2.gameObject.SetActive(isFan);
    }
    public override void Hide()
    {
        base.Hide();
    }
}

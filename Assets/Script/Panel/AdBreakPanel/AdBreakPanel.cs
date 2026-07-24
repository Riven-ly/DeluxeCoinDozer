using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdBreakPanel : UIBase
{
    public Image three;
    public Image two;
    public Image one;

    private int time = 3;
    private float timer = 0;
    private int targetTime;

    private void OnEnable()
    {
        GameManager.isPause = true;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > 1)
        {
            timer = 0f;
            targetTime--;
            UpdateState();
        }
    }

    private void UpdateState()
    {
        three.gameObject.SetActive(targetTime == 3);
        two.gameObject.SetActive(targetTime == 2);
        one.gameObject.SetActive(targetTime == 1);
        if (targetTime <= 0)
        {
            targetTime = 9999;
            Hide();
        }
    }

    public override void Refresh(object data = null)
    {
        base.Refresh(data);
        timer = 0;
        targetTime = time;
        UpdateState();
    }
    public override void Hide()
    {
        base.Hide();
    }
}

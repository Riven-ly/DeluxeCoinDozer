using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class DiamondUI : MonoBehaviour
{
    public Image sliderBg;
    public List<Sprite> sliderBgSprites;
    public Image btnBg;
    public List<Sprite> btnBgSprites;

    public Image icon;
    public Text numText;
    public Button btn;

    private float curDiamondNum;
    public static bool isLongTimerAnim = false;

    public void Init()
    {
        sliderBg.sprite = sliderBgSprites[(int)GameManager.Instance.curMachine.type];
        btnBg.sprite = btnBgSprites[(int)GameManager.Instance.curMachine.type];

        curDiamondNum = GameManager.Instance.playerInfo.GetDiamond();
        numText.text = curDiamondNum.ToString();

        int curlv = GameManager.Instance.playerInfo.playerData.level;
        btn.gameObject.SetActive(GameManager.Instance.curMachine.type == MachineType.Base && curlv >= 2);
        if(UIManager.Instance.mainBtnUI.txElementBtn == null)
        {
            btn.gameObject.SetActive(false);
        }
    }

    public void AddDiamond()
    {
        this.DOKill();
        StartDiamondAnim();
    }

    private void StartDiamondAnim()
    {
        float targetTime = 0.5f;
        if (isLongTimerAnim)
        {
            isLongTimerAnim = false;
            targetTime = 1.2f;
        }
        float targetValue = GameManager.Instance.playerInfo.GetDiamond();
        bool hasDecimal1 = targetValue != Mathf.RoundToInt(targetValue); // true（有小数）
        int unit = hasDecimal1 ? 1 : 0;
        float _currentValue = curDiamondNum;
        DOTween.To(
          () => _currentValue,
          x =>
          {
              _currentValue = (float)Math.Round(x, unit);
              curDiamondNum = _currentValue;
              numText.text = _currentValue.ToString();
          },
          targetValue, // 目标值
          targetTime // 时长
      ).SetTarget(this)
      .OnComplete(() =>
      {
          curDiamondNum = GameManager.Instance.playerInfo.GetDiamond();
          numText.text = curDiamondNum.ToString();
      });
    }
}

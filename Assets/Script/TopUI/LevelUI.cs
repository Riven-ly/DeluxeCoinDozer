using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUI : MonoBehaviour
{
    public Image sliderBg;
    public List<Sprite> sliderBgSprites;

    public Slider slider;
    public Text lvText;

    private int curLv;
    private int curLvEx;
    private LevelConfigData curLvData;
    private Sequence lvExTween;
    private bool init = false;
    public void Init()
    {
        sliderBg.sprite = sliderBgSprites[(int)GameManager.Instance.curMachine.type];
        UpdataData();

        slider.value = (float)curLvEx / (float)curLvData.levelExperience;
        lvText.text = LanguageManager.Instance.GetText("LV") + curLv.ToString();
        init = true;
    }

    public void AddLvEx()
    {
        if(curLv == GameManager.Instance.playerInfo.playerData.level && curLvEx == GameManager.Instance.playerInfo.playerData.levelExperience)
        {
            return;
        }
        if(!init)
        {
            return;
        }

        if(lvExTween != null)
        {
            lvExTween.Kill();
        }
        if (curLv < GameManager.Instance.playerInfo.playerData.level)
        {
            lvExTween = DOTween.Sequence()
               .Append(slider.DOValue(1, 0.5f))
               .AppendCallback(() =>
               {
                   UpdataData();
                   lvText.text = LanguageManager.Instance.GetText("LV") + curLv.ToString();
                   slider.value = 0f;
                   Debug.Log("Éý¼¶");
                   if(UIManager.Instance.CheckIstheUIopen())
                   {
                       UIBase.awaitHideAction = () =>
                       {
                           UIManager.Instance.OpenUI<LevelUpPanel>();
                       };
                   }
                   else
                   {
                       UIManager.Instance.OpenUI<LevelUpPanel>();
                   }
                   float targetValue = (float)curLvEx / (float)curLvData.levelExperience;
                   lvExTween = DOTween.Sequence()
                               .Append(slider.DOValue(targetValue, 0.5f))
                               .AppendCallback(() =>
                               {
                                   lvExTween = null;
                               });
               });

            }
        else
        {
            UpdataData();

            float targetValue =  (float)curLvEx / (float)curLvData.levelExperience;
            lvExTween = DOTween.Sequence()
                .Append(slider.DOValue(targetValue, 0.5f))
                .AppendCallback(() =>
                {
                    lvExTween = null;
                });
        }
    }

    private void UpdataData()
    {
        curLv = GameManager.Instance.playerInfo.playerData.level;
        curLvEx = GameManager.Instance.playerInfo.playerData.levelExperience;
        curLvData = GameManager.Instance.playerInfo.GetLevelConfigData();

    }
}

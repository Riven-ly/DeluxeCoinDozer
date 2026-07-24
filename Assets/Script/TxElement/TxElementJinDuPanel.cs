using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TxElementJinDuPanel : UIBase
{
    public Text title;
    public Text str1;
    public Text str2;
    public Text str3;

    public CanvasGroup r1;
    public CanvasGroup r2;
    public CanvasGroup r3;

    private void Start()
    {
        Refresh();
    }
    public override void Refresh(object data = null)
    {
        base.Refresh(data);
        string s1 = LanguageManager.Instance.GetText_Encrypt("Pym");
        str1.text = string.Format(LanguageManager.Instance.GetText("TxElementJinDuPanel_str1"), s1);
        str2.text = string.Format(LanguageManager.Instance.GetText("TxElementJinDuPanel_str2"), s1);

        string s2 = LanguageManager.Instance.GetText_Encrypt("wH");
        str3.text = string.Format(LanguageManager.Instance.GetText("TxElementJinDuPanel_str3"), s2);

        title.text = LanguageManager.Instance.GetText_Encrypt("CHT");

        r1.alpha = 0f;
        r2.alpha = 0f;
        r3.alpha = 0f;

        DOTween.Sequence()
                .AppendCallback(() =>
                {
                    DOTween.Sequence()
                    .Append(r1.DOFade(1f, 0.6f))
                    .Append(r1.DOFade(0f, 0.6f))
                    .Append(r1.DOFade(1f, 0.6f))
                    .Append(r1.DOFade(0f, 0.6f))
                    .Append(r1.DOFade(1f, 0.6f));
                })
                .AppendInterval(3f)
                .AppendCallback(() =>
                {
                    DOTween.Sequence()
                    .Append(r2.DOFade(1f, 0.6f))
                    .Append(r2.DOFade(0f, 0.6f))
                    .Append(r2.DOFade(1f, 0.6f))
                    .Append(r2.DOFade(0f, 0.6f))
                    .Append(r2.DOFade(1f, 0.6f));
                })
                .AppendInterval(3f)
                .AppendCallback(() =>
                {
                    DOTween.Sequence()
                   .Append(r3.DOFade(1f, 0.6f))
                   .Append(r3.DOFade(0f, 0.6f))
                   .Append(r3.DOFade(1f, 0.6f))
                   .Append(r3.DOFade(0f, 0.6f))
                   .Append(r3.DOFade(1f, 0.6f));
                })
                .AppendInterval(3f)
                .AppendCallback(() =>
                {
                    Hide();
                });
    }
    public override void Hide()
    {
        base.Hide();
     
    }
}

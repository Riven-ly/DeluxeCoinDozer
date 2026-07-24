using DG.Tweening;
using System;
using UnityEngine;

public class MachineItemUIEffect2Cell : MonoBehaviour
{
    public Transform goldEffect;
    public Transform trshuzi;
    public Transform tr1;
    public Transform tr2;
    public Transform tr3;
    public Transform tr4;
    public Transform tr5;

    public Transform trans;
    public AudioSource audioSource;

    public void Init(int count, Action action)
    {
        this.DOKill();
        gameObject.SetActive(true);
        goldEffect.gameObject.SetActive(false);

        trshuzi.gameObject.SetActive(count < 5);
        tr5.gameObject.SetActive(count >= 5);
        tr1.gameObject.SetActive(count == 1);
        tr2.gameObject.SetActive(count == 2);
        tr3.gameObject.SetActive(count == 3);
        tr4.gameObject.SetActive(count == 4);

        if(count >= 5)
        {
            CustomApiManager.Instance.RequestCustomEventV2(new CustomEventData()
            {
                page_id = "ComboMania",
                name = "Event_ComboStatus",
                value = "ComboMania",
            });
        }

        transform.localPosition = new Vector3(transform.localPosition.x, 0, 0);
        DOTween.Sequence()
            .Append(transform.DOLocalMoveY(500f, 0.5f))
            .SetTarget(this);

        trans.transform.localScale = Vector3.zero;
        DOTween.Sequence()
            .Append(trans.transform.DOScale(1.1f, 0.5f))
            .Append(trans.transform.DOScale(0.9f, 0.1f))
            .AppendCallback(() =>
            {
                goldEffect.gameObject.SetActive(true);
                AudioManager.Instance.SetAudioSource(audioSource, "Combo");
            })
            .Append(trans.transform.DOScale(1f, 0.1f))
            .AppendInterval(2f)
            .Append(trans.transform.DOScale(0f, 0.2f))
            .AppendCallback(() =>
            {
                gameObject.SetActive(false);
                goldEffect.gameObject.SetActive(false);
                action?.Invoke();
            })
            .SetTarget(this);
    }
}

using DG.Tweening;
using System;
using UnityEngine;

public class MachineItemUIEffect2Cell2 : MonoBehaviour
{
    public Transform goldEffect;
    public Transform trans;
    public Transform model;

    public AudioSource audioSource;
    private void Update()
    {
        model.transform.Rotate(0, 120 * Time.deltaTime, 0);
    }
    public void Init(Action action)
    {
        this.DOKill();
        gameObject.SetActive(true);
        goldEffect.gameObject.SetActive(false);

        transform.localPosition = new Vector3(transform.localPosition.x, -50f, 100f);
        DOTween.Sequence()
            .Append(transform.DOLocalMoveY(480f, 0.5f))
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

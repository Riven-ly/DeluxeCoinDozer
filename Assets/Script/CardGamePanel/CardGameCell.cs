using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardGameCell : MonoBehaviour
{
    public Transform front;
    public Transform back;
    public Transform itemRoot;
    public Button btn;
    public Transform effect;
    public AudioSource audioSource;

    private ItemBase itemBase;
    private ItemData itemData;
    [HideInInspector] public bool isClick;
    private CardGamePanel cardGamePanel;
    // Start is called before the first frame update
    void Start()
    {
        btn.onClick.AddListener(() =>
        {
            FanpaiClick();
            cardGamePanel.FanpaiAll();
        });
    }

    public void Init(ItemData data, CardGamePanel _cardGamePanel)
    {
        itemData = data;
        cardGamePanel = _cardGamePanel;

        itemBase = GameManager.Instance.CreatItem(data, itemRoot, true);
        if(itemBase.effect != null)
        {
            itemBase.effect.localScale = Vector3.one * 0.75f;
            itemBase.effect.gameObject.SetActive(false);
        }

        front.gameObject.SetActive(false);
        front.localScale = Vector3.one;
        back.gameObject.SetActive(true);
        back.localScale = Vector3.one;
        isClick = false;
        btn.interactable = true;
        transform.localScale = Vector3.one;
        effect.gameObject.SetActive(false);
    }

    public void AddReward()
    {
        if (isClick)
            return;
        isClick = true;
        cardGamePanel.AddReward(itemBase, itemData);
    }
    private void FanpaiClick()
    {
        //¸ßÁÁ
        if (itemBase.effect != null)
        {
            itemBase.effect.gameObject.SetActive(true);
        }
        DOTween.Sequence()
            .AppendInterval(0.5f)
            .Append(transform.DOScale(1.1f, 0.1f))
            .Append(transform.DOScale(1f, 0.1f))
            .Append(transform.DOScale(1.1f, 0.1f))
            .AppendCallback(() =>
            {
                effect.gameObject.SetActive(true);
            });

        AddReward();
        FanpaiAnim(0f);
    }
    public void FanpaiAnim(float _awaitTime)
    {
        btn.interactable = false;
        front.localScale = new Vector3(0,1,1);
        front.gameObject.SetActive(false);
        AudioManager.Instance.SetAudioSource(audioSource, "CardGame");
        DOTween.Sequence()
            .AppendInterval(_awaitTime)
            .Append(back.DOScaleX(0f, 0.25f))
            .AppendCallback(() =>
            {
                front.gameObject.SetActive(true);
                back.gameObject.SetActive(false);
            })
            .Append(front.DOScaleX(1f, 0.25f))
           ;
    }

    public void ClearItem()
    {
        foreach (Transform item in itemRoot)
        {
            Destroy(item.gameObject);
        }
    }
}

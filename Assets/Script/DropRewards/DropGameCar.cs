using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DropGameCar : MonoBehaviour
{
    public Transform itemRoot;
    public Image icon;
    public Text str;
    private DropGamePanel dropGamePanel;
  


    public void Init(DropGamePanel _dropGamePanel)
    {
        dropGamePanel = _dropGamePanel;
        itemRoot.gameObject.SetActive(false);
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (DropGamePanel.isGameOver)
            return;

        if (collision.CompareTag("DropGamePanelItem"))
        {
            DropGamePanelItem item = collision.gameObject.GetComponent<DropGamePanelItem>();
            dropGamePanel.AddDropReward(item.itemBase);
            icon.sprite = item.itemBase.icon.sprite;
            icon.SetNativeSize();
            str.text = item.itemBase.cntText.text;
            item.Clear();

            this.DOKill();
            itemRoot.gameObject.SetActive(true);
            DOTween.Sequence()
                .Append(itemRoot.transform.DOScale(1.1f, 0.2f))
                .Append(itemRoot.transform.DOScale(0.9f, 0.1f))
                .Append(itemRoot.transform.DOScale(1f, 0.1f))
                .AppendInterval(0.5f)
                .AppendCallback(() =>
                {
                    itemRoot.gameObject.SetActive(false);
                })
                .SetTarget(this);
        }
    }
}

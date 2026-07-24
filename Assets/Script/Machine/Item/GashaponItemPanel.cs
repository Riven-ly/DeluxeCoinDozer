using DG.Tweening;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

public class GashaponItemPanel : UIBase
{
    public Text explain;
    public Transform iconTrans;
    public Image icon;
    public Transform bg;
    public GameObject Letter_A;
    public GameObject Letter_E;
    public GameObject Letter_C;
    public GameObject Letter_L;

    public GameObject zimuBg;
    public Image icon2;
    public Image bg1;
    public Image bg2;
    public GameObject effect2;

    public AudioSource audioSource;
    private void OnEnable()
    {
        DOTween.Sequence().AppendInterval(3f).AppendCallback(() =>
        {
            Hide();
        });
    }

    public override void Refresh(object data = null)
    {
        base.Refresh(data);
        GashaponItemBase item = data as GashaponItemBase;
        if(item.type != GashaponItemType.NULL)
        {
            AudioManager.Instance.SetAudioSource(audioSource, "GashaponItem");
        }
        else
        {
            AudioManager.Instance.SetAudioSource(audioSource, "NullNiuDan");
        }

        if (item.type == GashaponItemType.Letter_A
            || item.type == GashaponItemType.Letter_E
            || item.type == GashaponItemType.Letter_C
            || item.type == GashaponItemType.Letter_L
            )
        {
            bg.gameObject.SetActive(false);
            iconTrans.gameObject.SetActive(false);
            Letter_A.SetActive(item.type == GashaponItemType.Letter_A);
            Letter_E.SetActive(item.type == GashaponItemType.Letter_E);
            Letter_C.SetActive(item.type == GashaponItemType.Letter_C);
            Letter_L.SetActive(item.type == GashaponItemType.Letter_L);
            zimuBg.gameObject.SetActive(true);
        }
        else
        {
            Letter_A.SetActive(false);
            Letter_E.SetActive(false);
            Letter_C.SetActive(false);
            Letter_L.SetActive(false);
            zimuBg.gameObject.SetActive(false);
            if (item.itemPanelInfo.icon == null)
            {
                bg.gameObject.SetActive(false);
                iconTrans.gameObject.SetActive(false);
            }
            else
            {
                icon.sprite = item.itemPanelInfo.icon;
                icon.SetNativeSize();
                bg.gameObject.SetActive(true);
                iconTrans.gameObject.SetActive(true);
            }
        }

        explain.text = item.itemPanelInfo.explain;

        if(item.type == GashaponItemType.Big_Gold)
        {
            icon.transform.localScale = Vector3.one * 3f;
        }
        else if (item.type == GashaponItemType.Special_Fragment)
        {
            icon.transform.localScale = Vector3.one * 2.5f;
        }
        else if (item.type == GashaponItemType.Diamond)
        {
            icon.transform.localScale = Vector3.one * 4f;
        }
        else if (item.type == GashaponItemType.Gold_Rain)
        {
            icon.transform.localScale = Vector3.one * 0.8f;
        }
        else
        {
            icon.transform.localScale = Vector3.one;
        }
        //-----------------------------

        bg1.sprite = item.bg1.sprite;
        bg1.SetNativeSize();
        bg2.sprite = item.bg2.sprite;
        bg2.SetNativeSize();

        if (item.type != GashaponItemType.NULL)
        {
            icon2.sprite = item.icon.sprite;
            icon2.SetNativeSize();
            icon2.transform.localScale = item.icon.transform.localScale;
            icon2.gameObject.SetActive(true);
            effect2.SetActive(true);
        }
        else
        {
            effect2.SetActive(false);
            icon2.gameObject.SetActive(false);
        }
   
    }

    public override void Hide()
    {
        base.Hide();
    }
}

using DG.Tweening;
using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemFragmentEffect : MonoBehaviour
{
    public static ItemFragmentEffect Instance;
    public Transform iconRoot;
    public Image icon;

    private bool isAnim;
    private List<Sprite> sprites;
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        sprites = new List<Sprite>();
        isAnim = false;
        iconRoot.gameObject.SetActive(false);
    }

    public void StartEffect(Sprite sprite)
    {
        sprites.Add(sprite);
        if (!isAnim)
        {
            StartEffectAnim();
        }
    }

    private void StartEffectAnim()
    {
        Vector3 targetPos = Vector3.zero;
        if(UIManager.Instance.mainBtnUI.itemFragment != null)
        {
            targetPos = UIManager.Instance.mainBtnUI.itemFragment.transform.position;
        }
        else
        {
            return;
        }
        icon.sprite = sprites[0];
        icon.SetNativeSize();
        sprites.RemoveAt(0);

        iconRoot.transform.DOKill();
        this.DOKill();
        isAnim = true;

        iconRoot.transform.localPosition = Vector3.zero;
        iconRoot.transform.eulerAngles = Vector3.zero;
        iconRoot.transform.localScale = Vector3.zero;
        iconRoot.gameObject.SetActive(true);
        iconRoot.transform.DOScale(1, 0.5f);
        DOTween.Sequence()
                .Append(iconRoot.transform.DORotate(new Vector3(0f, 360f * 5, 0f), 3f, RotateMode.FastBeyond360).SetEase(Ease.OutQuad))
                .AppendCallback(() =>
                {
                    iconRoot.transform.DOMove(targetPos, 1f);
                    iconRoot.transform.DOScale(0.1f, 1f);
                })
                .AppendInterval(1f)
                .AppendCallback(() =>
                {

                    DOTween.Sequence()
                    .Append(UIManager.Instance.mainBtnUI.itemFragment.transform.DOScale(1.1f, 0.1f))
                    .Append(UIManager.Instance.mainBtnUI.itemFragment.transform.DOScale(0.9f, 0.1f))
                    .Append(UIManager.Instance.mainBtnUI.itemFragment.transform.DOScale(1f, 0.1f))
                    .AppendCallback(() =>
                    {
                        string s = PlayerPrefs.GetString("ItemFragmentYindao");
                        if (string.IsNullOrEmpty(s) && GameManager.TrySceneClick)
                        {
                            PlayerPrefs.SetString("ItemFragmentYindao", "YES");
                            string s2 = LanguageManager.Instance.GetText("ItemFragment_yindao");
                            List<object> listdata = new List<object>();
                            listdata.Add(s2);
                            listdata.Add(UIManager.Instance.mainBtnUI.itemFragment.clickBtn);
                            listdata.Add(false);

                            UIManager.Instance.OpenUI<GameMainBtnYindaoPanel>(listdata);
                            CustomApiManager.Instance.RequestCustomEventV2(new CustomEventData()
                            {
                                page_id = "Yindao",
                                name = "Event_GuideStep",
                                value = "step3",
                            });
                        }
          
                    });

                    isAnim = false;
                    iconRoot.gameObject.SetActive(false);
                    if(sprites.Count > 0)
                    {
                        StartEffectAnim();
                    }
                })
                .SetTarget(this);

    }
}

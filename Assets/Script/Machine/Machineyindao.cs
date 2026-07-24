using DG.Tweening;
using UnityEngine;

public class Machineyindao : MonoBehaviour
{
    public static Machineyindao Instance;

    public Transform trans;
    public Material material;

    public Transform str;
    public Transform str_py;
    public Transform str_yn;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        trans.gameObject.SetActive(true);

        Shanshuo();
    }
    public void TryMachineyindao()
    {
        this.DOKill();
        material.DOFade(0f, 0.5f).SetTarget(this);

        DOTween.Sequence().AppendInterval(10f)
                          .AppendCallback(() =>
                          {
                              Shanshuo();
                          })
                          .SetTarget(this);
    }

    private void Shanshuo()
    {
        str.gameObject.SetActive(LanguageManager.Instance.type == MultilingualType.English);
        str_py.gameObject.SetActive(LanguageManager.Instance.type == MultilingualType.Portuguese);
        str_yn.gameObject.SetActive(LanguageManager.Instance.type == MultilingualType.Indonesian);

        DOTween.Sequence()
            .AppendCallback(() =>
            {
                Color color = material.color;
                color.a = 0.2f;
                material.color = color;
            })
           .Append(material.DOFade(1f, 1f))
           .Append(material.DOFade(0.2f, 1f))
           .SetLoops(-1)
           .SetEase(Ease.Linear)
           .SetTarget(this);
    }
}

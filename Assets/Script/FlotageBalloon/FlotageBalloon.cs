using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class FlotageBalloon : MonoBehaviour
{
    public Animation anim;
    public Button btn;
    public GameObject balloon;

    private Vector3 balloonInitPos;
    private bool isStartEnterLimit;
    public float timer;
    private float startEnterTime = 900f;
    private float coolingTime = 120f;
    private bool balloonIdle;
    //-----½±ÀøÊýÁ¿
    public static int rewardCnt;
    private void Start()
    {
        btn.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayBtnMusic();
            UIManager.Instance.OpenUI<FlotageBalloonPanel>();

            CustomApiManager.Instance.RequestCustomEventV2(new CustomEventData()
            {
                page_id = "FlotageBalloon",
                name = "Event_BubbleClick",
                value = "",
            });
        });

        isStartEnterLimit = GameManager.Instance.playerInfo.playerData.level >= 3;
        timer = startEnterTime;
        balloonIdle = false;


        balloonInitPos = new Vector3(400f, 0f, 0f);
        balloon.transform.localPosition = balloonInitPos;
        btn.interactable = false;
    }
    private void Update()
    {
        if (GameLoadingPanel.isOpenStatic)
            return;

        if (balloonIdle)
            return;
       
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            FlotageBalloonEnter();
        }
    }
    public void OpenState(bool isOpen)
    {
        gameObject.SetActive(isOpen);
        if (!isOpen)
        {
            return;
        }
        if(isStartEnterLimit)
        {
            return;
        }
        isStartEnterLimit = true;
        timer = 0f;
        
    }
    public void FlotageBalloonEnter()
    {
        btn.interactable = false;
        balloonIdle = true;
        this.DOKill();
        balloon.transform.DOLocalMoveX(0f, 1f).OnComplete(() =>
        {
            rewardCnt = Random.Range(20, 25);
            btn.interactable = true;
            FlotageBalloonIdle();

        })
        .SetTarget(this);
    }
    private void FlotageBalloonIdle()
    {
        DOTween.Sequence().AppendInterval(2f)
            .AppendCallback(() =>
            {
                anim.Play("FlotageBalloonIdle");
            }).SetTarget(this);

        //------------------------
        //DOTween.Sequence()
        //  .Append(balloon.transform.DOLocalMoveX(-850f, 5f))
        //  .Append(balloon.transform.DOLocalMoveX(0f, 5f))
        //  .SetLoops(-1)
        //  .SetEase(Ease.Linear)
        //  .SetTarget(this)
        //  ;

        //DOTween.Sequence()
        //     .Append(balloon.transform.DOLocalMoveY(-600f, 2f))
        //     .Append(balloon.transform.DOLocalMoveY(0f, 2f))
        //     .SetLoops(-1)
        //     .SetEase(Ease.Linear)
        //     .SetTarget(this);
    }
    public void FlotageBalloonLeave()
    {
        btn.interactable = false;
        this.DOKill();
        anim.Stop();

        balloon.transform.localPosition = balloonInitPos;
        timer = coolingTime;
        balloonIdle = false;

    }
}

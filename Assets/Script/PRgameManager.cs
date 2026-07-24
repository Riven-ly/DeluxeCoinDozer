using System.Collections;
using UnityEngine;

/// <summary>
/// 防刷
/// </summary>
public class PRgameManager : MonoBehaviour
{
    public static bool PR_pass = true;
    public static PRgameManager Instance;

    private float time;
    public float timer;
    private bool isPause = false;
    private int yanzhengCnt;
    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        string data = PlayerPrefs.GetString("PRgame", "");
        if(!string.IsNullOrEmpty(data))
        {
            string[] strs = data.Split('/');
            Debug.Log("PR_pass  data   " + data);
            var curTime = GameManager.Instance.GetNowTime();
            if (curTime.Day.ToString() == strs[0])
            {
                switch (strs[1])
                {
                    case "true":
                        PR_pass = true;
                        break;
                    case "false":
                        PR_pass = false;
                        break;
                }
                isPause = true;
                gameObject.SetActive(false);
            }
        }
        RamdomTime();
        yanzhengCnt = 0;

    }

    // Update is called once per frame
    void Update()
    {
        if (GameLoadingPanel.isOpenStatic) return;
        if (isPause) return;

        timer -= Time.deltaTime;
        if(timer < 0)
        {
            RamdomTime();
            ShowGamePanel();
        }
    }

    public void SaveGameYanzhengResult(bool _isPass)
    {
        var curTime = GameManager.Instance.GetNowTime();
        if (!_isPass)
        {
            PR_pass = false;
            if (yanzhengCnt == 1)
            {
                //第一次false 等待第二次
                isPause = false;                
            }
            else if(yanzhengCnt > 1)
            {
                PlayerPrefs.SetString("PRgame", $"{curTime.Day}/false");
                gameObject.SetActive(false);
            }
        }
        else
        {
            PR_pass = true;
            PlayerPrefs.SetString("PRgame", $"{curTime.Day}/true");
            gameObject.SetActive(false);
        }
        Debug.Log("PR_pass  " + PR_pass);
    }
    private void ShowGamePanel()
    {
        isPause = true;
        yanzhengCnt++;
        //打开
        UIManager.Instance.OpenUI<PRgamePanel>();
    }
    private void RamdomTime()
    {
        time = Random.Range(300, 600);
        timer = time;
    }
}

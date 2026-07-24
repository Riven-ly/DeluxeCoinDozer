using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ÍøÂç×´Ì¬¼ì²âÓë¶ÏÍøÌáÊ¾
/// </summary>
public class NetworkChecker : MonoBehaviour
{
    public static NetworkChecker Instance;

    //ÍøÂç¼ì²â¼ä¸ô
    private float checkInterval = 2f;
    public bool isNetworkAvailable = true;
    private bool isStart = false;
    [Header("²âÊÔÍøÂç")]
    public bool DebugNetwork = true;
    private void Awake()
    {
        Instance = this;
    }
   
    public void StartCheckNetworkStatus()
    {
        isStart = true;
        CheckNetworkStatus();
        StartCoroutine(CheckNetworkCoroutine());
    }

    IEnumerator CheckNetworkCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(checkInterval);
            CheckNetworkStatus();
        }
    }

    /// <summary>
    /// ¼ì²âÍøÂç×´Ì¬
    /// </summary>
    void CheckNetworkStatus()
    {
        // ÍøÂç×´Ì¬¼ì²â
        bool currentNetworkState = Application.internetReachability != NetworkReachability.NotReachable;
        //currentNetworkState = DebugNetwork; //todo
        checkInterval = currentNetworkState ? 10f : 2f;
        // ×´Ì¬±ä»¯Ê±
        if (currentNetworkState != isNetworkAvailable)
        {
            isNetworkAvailable = currentNetworkState;

            if (!isNetworkAvailable)
            {
                ShowNetworkTip();
            }
            else
            {
                NetworkCheckerPanel networkCheckerPanel = UIManager.Instance.GetUI<NetworkCheckerPanel>();
                if (networkCheckerPanel != null && networkCheckerPanel.gameObject.activeSelf)
                {
                    networkCheckerPanel.Hide();               
                }      
            }

        }

    }

    /// <summary>
    /// ÏÔÊ¾¶ÏÍøÌáÊ¾
    /// </summary>
    void ShowNetworkTip()
    {
        UIManager.Instance.OpenUI<NetworkCheckerPanel>();
    }

    public bool CheckNetworkManually()
    {
        CheckNetworkStatus();
        return isNetworkAvailable;
    }

    void OnApplicationPause(bool pauseStatus)
    {
        if(!isStart)
        {
            return;
        }
        if (!pauseStatus)
        {
            CheckNetworkStatus();
        }
    }
}
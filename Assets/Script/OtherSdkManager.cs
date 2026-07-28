using AdjustSdk;
using SolarEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherSdkManager : MonoBehaviour
{
    public static OtherSdkManager Instance;

    public static bool IsInit = false;
    private void Awake()
    {
        Instance = this;
        Init();
    }


    public void Init()
    {
        Debug.Log("Other SDK³õÊ¼»¯");

        IsInit = true;
        AdjustInit();
        SolarEngineInit();
    }

    private void AdjustInit()
    {
        string adjust_AppToken = "x8i8rk60u41s";
        AdjustConfig adjustConfig = new AdjustConfig(adjust_AppToken, AdjustEnvironment.Production);
        // ...
        Adjust.InitSdk(adjustConfig);
    }

    private void SolarEngineInit()
    {
        string AppKey = "65917ddcfdf8d4e2";

        SolarEngine.Analytics.preInitSeSdk(AppKey);
        SEConfig seConfig = new SEConfig();
        seConfig.initCompletedCallback = (e) =>
        {
            Debug.Log("SolarEngineInit :" + e);
        };
        SolarEngine.Analytics.initSeSdk(AppKey, seConfig);
    }
}

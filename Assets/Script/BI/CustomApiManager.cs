using System.Collections.Generic;
using UnityEngine;

public class CustomApiManager : MonoBehaviour
{
    public static CustomApiManager Instance;


    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// 自定义事件V2 
    /// </summary>
    public void RequestCustomEventV2(CustomEventData data)
    {
        // string valueStr = data.value;
        // data.value = $@"{{""value"":""{valueStr}""}}";

        if (!OtherSdkManager.IsInit)
            return;

        Dictionary<string, object> customAttributes = new Dictionary<string, object>();
        customAttributes.Add(data.name, data.value);
        SolarEngine.Analytics.track(data.name, customAttributes);
    }

}

public class CustomEventData
{
    public string name;
    public string value;
    public string page_id;
}



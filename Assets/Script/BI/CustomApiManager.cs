using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
    }
}

public class CustomEventData
{
    public string name;
    public string value;
    public string page_id;
}



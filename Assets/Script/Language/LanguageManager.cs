using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public enum MultilingualType
{
    English,
    /// <summary>
    /// 葡萄牙语
    /// </summary>
    Portuguese,
    /// <summary>
    /// 印尼语
    /// </summary>
    Indonesian
}
public class LanguageManager : MonoBehaviour
{
    public static LanguageManager Instance;
    public static string AppLanguageCode = "en";

    public MultilingualType type;
    private Dictionary<string, string> currentTexts;
    private void Awake()
    {
        Instance = this;
        InitLanguage();
    }

    public void InitLanguage()
    {
        SystemLanguage currentLang = Application.systemLanguage;
        //------------------------------------
        if (currentLang == SystemLanguage.Indonesian)
        {
            Debug.Log("当前系统语言是印尼语");
            currentTexts = IndonesianLanguageConfig.currentTexts;
            type = MultilingualType.Indonesian;
            AppLanguageCode = "id";
        }
        else if (currentLang == SystemLanguage.Portuguese)
        {
            Debug.Log("当前系统语言是葡萄牙语");
            currentTexts = PortugueseLanguageConfig.currentTexts;
            type = MultilingualType.Portuguese;
            AppLanguageCode = "pt";
        }
        else
        {
            Debug.Log("默认系统语言是英文");
            currentTexts = EnglishLanguageConfig.currentTexts;
            type = MultilingualType.English;
            AppLanguageCode = "en";
        }
    }
    public string GetText(string key)
    {
        // 1. 先判断当前语言字典是否为空
        if (currentTexts == null)
            return "";

        // 2. 先尝试从当前语言获取
        if (currentTexts.TryGetValue(key, out string value))
        {
            return value;
        }
        Debug.LogError(type.ToString() + " -Localization key not found: " + key);

        // 3. 当前语言找不到 → 尝试从【英文默认配置】获取
        if (EnglishLanguageConfig.currentTexts.TryGetValue(key, out value))
        {
            return value;
        }

        // 4. 英文都找不到 → 报错 + 返回空
        Debug.LogError("MultilingualType.English -Localization key not found: " + key);
        return "";
    }

    public string GetText_Encrypt(string key)
    {
        string encryptStr = GetText(key);
        if(!string.IsNullOrEmpty(encryptStr))
        {
            encryptStr = Encoding.UTF8.GetString(Convert.FromBase64String(encryptStr));
        }
        return encryptStr;
    }

}

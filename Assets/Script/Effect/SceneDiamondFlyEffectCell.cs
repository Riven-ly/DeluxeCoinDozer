using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneDiamondFlyEffectCell : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public Image icon;
    public Text str;

    private void OnEnable()
    {
        GameManager.Instance.UpdateAppATTToDiamond(icon);
    }
    public void Init(string _str)
    {
        str.text = "+" + _str;
    }
}

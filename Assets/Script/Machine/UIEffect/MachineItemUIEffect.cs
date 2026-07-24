using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MachineItemUIEffect : MonoBehaviour
{
    public GameObject effectPrefab;
    private List<GameObject> poolDic = new List<GameObject>(); 


    public void CreatEffect(float _objPosX, string _str)
    {
        GameObject obj;
        if (poolDic.Count == 0)
        {
            obj = Instantiate(effectPrefab,transform);
        }
        else
        {
            obj = poolDic[0];
            poolDic.Remove(obj);
        }

        float diffY = Random.Range(-50f, 100f);
        obj.transform.localPosition = new Vector3(_objPosX, diffY, 0);
        obj.transform.GetComponent<Text>().text = _str;
        CanvasGroup canvasGroup = obj.transform.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;
        obj.gameObject.SetActive(true);

        canvasGroup.DOFade(1f, 0.5f);
        DOTween.Sequence()
            .Append(obj.transform.DOLocalMoveY(diffY + 300f, 2f))
            .Append(canvasGroup.DOFade(0f, 0.2f))
            .AppendCallback(() =>
            {
                obj.gameObject.SetActive(false);
                poolDic.Add(obj);
            }).SetEase(Ease.OutQuad);
    }
}

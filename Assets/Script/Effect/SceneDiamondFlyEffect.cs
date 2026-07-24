using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SceneDiamondFlyEffect : MonoBehaviour
{
    public GameObject effectPrefab;
    private List<SceneDiamondFlyEffectCell> poolDic = new List<SceneDiamondFlyEffectCell>();
    
    public void CreatEffect(float _objPosX, string _str)
    {
        SceneDiamondFlyEffectCell cell;
        if (poolDic.Count == 0)
        {
            var obj = Instantiate(effectPrefab, transform);
            cell = obj.GetComponent<SceneDiamondFlyEffectCell>();
        }
        else
        {
            cell = poolDic[0];
            poolDic.Remove(cell);
        }
        Debug.Log(_objPosX);
        float diffY = UnityEngine.Random.Range(-50f, 100f);
        cell.transform.localPosition = new Vector3(_objPosX, diffY, 0);
        cell.Init( _str);
        cell.canvasGroup.alpha = 0f;
        cell.gameObject.SetActive(true);

        cell.canvasGroup.DOFade(1f, 0.5f);
        DOTween.Sequence()
            .Append(cell.transform.DOLocalMoveY(diffY + 300f, 2f))
            .AppendCallback(() =>
            {
                cell.gameObject.SetActive(false);
                poolDic.Add(cell);
            })
            .SetEase(Ease.OutQuad);
    }

}

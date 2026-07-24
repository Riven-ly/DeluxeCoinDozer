using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PRgamePanel_Car : MonoBehaviour, IDragHandler,IEndDragHandler
{
    public  Transform targetTrans;
    private RectTransform car_rectTransform;
    private Vector3 initPos = new Vector3(-240f, -620f, 0f);

    private void OnEnable()
    {
        transform.localPosition = initPos;
    }
    void Start()
    {
        car_rectTransform = transform.GetComponent<RectTransform>();
    }
    public void OnDrag(PointerEventData eventData)
    {
        car_rectTransform.anchoredPosition += eventData.delta;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        float distance = Vector3.Distance(targetTrans.localPosition, transform.localPosition);
        UIManager.Instance.GetUI<PRgamePanel>().IsPRGamePass(distance < 200f);
    }

}

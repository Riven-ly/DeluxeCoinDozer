using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropGameDrag : MonoBehaviour, IDragHandler
{
    public Transform car;

    public Transform limitLeft;
    public Transform limitRight;

    private float limitXLeft;
    private float limitXRight;

    private float limitY;
    private RectTransform car_rectTransform;
    void Start()
    {
        car_rectTransform = car.GetComponent<RectTransform>();
        // canvas = GetComponentInParent<Canvas>();

        limitXLeft = limitLeft.localPosition.x;
        limitXRight = limitRight.localPosition.x;
        limitY = car.transform.localPosition.y;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (DropGamePanel.isGameOver)
            return;

        car_rectTransform.anchoredPosition += eventData.delta;
        var limitPos = car.transform.localPosition;
        float x = Mathf.Clamp(limitPos.x, limitXLeft, limitXRight);
        car.transform.localPosition = new Vector3(x, limitY, 0f);
    }

}

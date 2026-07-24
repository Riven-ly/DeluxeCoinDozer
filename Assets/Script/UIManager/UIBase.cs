using DG.Tweening;
using System;
using UnityEngine;

public enum UIPanelLayer
{
    Layer1,
    Layer2,
    Layer3,
}
public class UIBase : MonoBehaviour
{
    public UIPanelLayer  uIPanelLayer;
    [HideInInspector] public bool isOpen;
    public Animation panelAnim;
    public Action callback;

    public static Action awaitHideAction;
    private void OnEnable()
    {
        isOpen = true;
    }
    private void OnDisable()
    {
        isOpen = false;
    }
    public void SetUIPanelLayer()
    {
        switch (uIPanelLayer)
        {
            case UIPanelLayer.Layer1:
                transform.SetParent(UIManager.Instance.UIPanelLayer1);
                break;
            case UIPanelLayer.Layer2:
                transform.SetParent(UIManager.Instance.UIPanelLayer2);
                break;
            case UIPanelLayer.Layer3:
                transform.SetParent(UIManager.Instance.UIPanelLayer3);
                break;
        }
    }
    public virtual void Open(object data = null, Action _callback = null)
    {
        gameObject.SetActive(true);
        gameObject.transform.SetAsLastSibling();
        callback = _callback;
        Refresh(data);
    }
    public virtual void Refresh(object data = null)
    {
        
    }
    public virtual void Hide()
    {
        if (panelAnim == null)
        {
            gameObject.SetActive(false);
            callback?.Invoke();
            callback = null;

            awaitHideAction?.Invoke();
            awaitHideAction = null;
        }
        else
        {
            UIManager.Instance.OpenUIMask();
            panelAnim.Play("GeneralHidePanelAnim");
            DOTween.Sequence().AppendInterval(15f / 60f).OnComplete(() =>
            {
                gameObject.SetActive(false);
                callback?.Invoke();
                callback = null;
                UIManager.Instance.HideUIMask();

                awaitHideAction?.Invoke();
                awaitHideAction = null;
            });
        }
    }

    public void RightAwayHide()
    {
        gameObject.SetActive(false);
    }
}

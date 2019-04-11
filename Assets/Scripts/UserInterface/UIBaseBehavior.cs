using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.EventSystems;
public class UIBaseBehavior : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool mouseInside = false;
    public virtual void OnPointerDown(PointerEventData eventData)
    {
        ClickStateManager.GetInstance.currentState = ClickState.UIClick;
    }
    public virtual void OnPointerClick(PointerEventData eventData)
    {
        ClickStateManager.GetInstance.currentState = ClickState.UIClick;
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
            ClickStateManager.GetInstance.currentState = ClickState.Idle;
    }
    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        ClickStateManager.GetInstance.currentState = ClickState.UIClick;
        mouseInside = true;
    }
    public virtual void OnPointerExit(PointerEventData eventData)
    {
        ClickStateManager.GetInstance.currentState = ClickState.Idle;
        mouseInside = false;
    }
}

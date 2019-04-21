using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.EventSystems;

public class InfoSwitcher : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject upFrontInfo;
    public GameObject backInfo;

    public void OnPointerEnter(PointerEventData eventData)
    {
        upFrontInfo.SetActive(false);
        backInfo.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        upFrontInfo.SetActive(true);
        backInfo.SetActive(false);
    }
}

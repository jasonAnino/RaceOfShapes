using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(UIBaseBehavior))]
public class ScrollHelper : MonoBehaviour
{
    public Scrollbar scrollBar;
    public UIBaseBehavior baseBehavior;

    public Vector3 clickedPosition = new Vector3();
    public bool startScroll = false;
    public void Update()
    {
        if(baseBehavior != null)
        {
            if (baseBehavior.mouseInside)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    clickedPosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);
                    startScroll = true;
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    startScroll = false;
                }
            }
            else
            {
                startScroll = false;
                clickedPosition = Vector3.zero;
            }
            if(startScroll)
            {
                Vector3 tmpPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
                if (clickedPosition.y > tmpPos.y)
                {
                    scrollBar.value += Time.deltaTime;
                }
                else if(clickedPosition.y < tmpPos.y)
                {
                    scrollBar.value -= Time.deltaTime;
                }
            }
        }
    }
}

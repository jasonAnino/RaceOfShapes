using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Utilities
{
    public class UserInterfaceResizeFitter : MonoBehaviour
    {
        public float originalWidth, originalHeight;
        public bool initializeOnStart;
        public RectTransform rectToRefit;

        public void Start()
        {
            if(initializeOnStart)
            {
                originalWidth = rectToRefit.localScale.x;
                originalHeight = rectToRefit.localScale.y;
            }
        }

        public void SizeIncrease(float byAmount = 10.0f)
        {
            rectToRefit.sizeDelta = new Vector2(originalWidth + byAmount, originalHeight + byAmount);

            //rectToRefit.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalHeight + byAmount);
            //rectToRefit.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, originalWidth + byAmount);
        }

        public void SizeDecrease(float byAmount = 10.0f)
        {
//            rectToRefit.sizeDelta = new Vector2(15 - byAmount, 15 - byAmount);
            rectToRefit.sizeDelta = new Vector2(originalWidth - byAmount, originalHeight - byAmount);
            //rectToRefit.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalHeight - byAmount);
            //rectToRefit.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, originalWidth - byAmount);
        }
    }
}

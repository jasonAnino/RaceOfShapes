using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Utilities
{
    public enum LayoutFormat
    {
        Horizontal,
        Vertical
    }
    public class LayoutSort : MonoBehaviour
    {
        public bool UniformSize;
        public LayoutFormat format;

        public List<Transform> items = new List<Transform>();

        public float spacing;

        public void Start()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                items.Add(transform.GetChild(i));
            }
            UpdateTransformPositions();
        }

        public void SetToFirst(Transform thisItem)
        {
            if(!items.Contains(thisItem))
            {
                Debug.Log("Items Contains");
                return;
            }
            int idx = items.IndexOf(thisItem);

            Transform tmp = items[0];
            items[0] = thisItem;
            items[idx] = tmp;
            UpdateTransformPositions();
        }

        public void UpdateTransformPositions()
        {
            float tmpSpace = 0;
            switch (format)
            {
                case LayoutFormat.Horizontal:
                for (int i = 0; i < items.Count; i++)
                {
                        items[i].GetComponent<RectTransform>().anchoredPosition = new Vector3(tmpSpace, 0, 0);
                        tmpSpace += spacing;
                }
                    break;

                case LayoutFormat.Vertical:
                    for (int i = 0; i < items.Count; i++)
                    {
                        items[i].GetComponent<RectTransform>().anchoredPosition = new Vector3(0, tmpSpace, 0);
                        tmpSpace += spacing;
                    }
                    break;
            }
            
        }
    }
}

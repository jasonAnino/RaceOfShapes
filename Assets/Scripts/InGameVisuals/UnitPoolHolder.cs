using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitPoolHolder : MonoBehaviour
{
    public List<InGameVisualText> visualTexts;
    public Transform parentVar;
    public List<InGameVisualText> curVisibleVisualTexts;

    public void Start()
    {
        parentVar = this.transform.parent;
        foreach (InGameVisualText item in visualTexts)
        {
            item.Initialize(this);
        }
    }
    public void ShowDamage(float damage)
    {
        InGameVisualText tmp = visualTexts.Find(x => x.startShowing == false);

        if(tmp == null)
        {
            if(curVisibleVisualTexts.Count > 0)
            {
                curVisibleVisualTexts[0].Hide();
                tmp = curVisibleVisualTexts[0];
                curVisibleVisualTexts.RemoveAt(0);
            }
        }

        tmp.Hide();
        tmp.ShowUp(damage, TextColour.RED, 0.5f);
        curVisibleVisualTexts.Add(tmp);
    }

    public void RemoveThisFromCurVisibleVisuals(InGameVisualText thisText)
    {
        if(curVisibleVisualTexts.Contains(thisText))
        {
            curVisibleVisualTexts.Remove(thisText);
        }
    }
}

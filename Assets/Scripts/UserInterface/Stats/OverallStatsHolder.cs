using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OverallStatsHolder : MonoBehaviour
{
    public List<UIStatsBonusHolder> holderList;
    public List<float> bonusCountData;

    public void Awake()
    {
        foreach(UIStatsBonusHolder item in holderList)
        {
            bonusCountData.Add(0);
        }
    }

    public void AddHolderCount(int idx, float count)
    {
        bonusCountData[idx] += count;
        holderList[idx].countText.text = bonusCountData[idx].ToString();
    }
    public void SubtractHolderCount(int idx, float count)
    {
        bonusCountData[idx] -= count;
        holderList[idx].countText.text = bonusCountData[idx].ToString();
    }

}

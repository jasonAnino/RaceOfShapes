using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnitStats;


using UnitsScripts.Behaviour;
using PlayerScripts.UnitCommands;
using TMPro;

public class StatsPanel : MonoBehaviour
{
    public List<TextMeshProUGUI> statsList = new List<TextMeshProUGUI>();
    public UnitBaseBehaviourComponent curUnit;
    public int count;
    public void Awake()
    {
        disableAllStats();
    }
    public void StatsInitialize()
    {
        if (PlayerUnitController.GetInstance.manualControlledUnit == null)
        {
            return;
        }
        UnitBaseBehaviourComponent tmp = PlayerUnitController.GetInstance.manualControlledUnit;
        count = tmp.myStats.GetCurrentStats.Count;
        disableAllStats();
        string tmpStatString = " ";
        List<BaseUnitStats> convertedStats = tmp.myStats.GetCurrentStats.Values.ToList<BaseUnitStats>();
        for (int i = 0; i < count; i++)
        {
            tmpStatString = convertedStats[i].GetName + " : " + convertedStats[i].GetLevel;
            statsList[i].enabled = true;
            statsList[i].text = tmpStatString;
        }
    }

    public void disableAllStats()
    {
        foreach (TextMeshProUGUI item in statsList)
        {
            item.enabled = false;
        }
    }
}

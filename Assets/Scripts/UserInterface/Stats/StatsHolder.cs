using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnitsScripts.Behaviour;

using PlayerScripts.UnitCommands;
using Utilities;
using ItemScript;
using UnitStats;
using TMPro;

public class StatsHolder : MonoBehaviour
{
    public Image statImage;
    public Stats curStats;
    public TextMeshProUGUI statsName;
    public TextMeshProUGUI exp;
    public UnitBaseBehaviourComponent owner;
    // Visual Points Added
    public List<UIStatsBonusHolder> statsBonusList;

    public void Awake()
    {
        EventBroadcaster.Instance.AddObserver(EventNames.UPDATE_PLAYER_STATS, UpdateStats);
    }
    public void OnDestroy()
    {
        EventBroadcaster.Instance.RemoveActionAtObserver(EventNames.UPDATE_PLAYER_STATS, UpdateStats);
    }

    public void Initialize(Stats newName, UnitBaseBehaviourComponent newOwner)
    {
        owner = newOwner;
        curStats = newName;
        statsName.text = curStats.ToString();
        exp.text = owner.myStats.GetStats(curStats).GetCurrentExperience + "/" + owner.myStats.GetStats(curStats).GetNextLevelExperience;
    }

    public void UpdateStats(Parameters p)
    {
        exp.text = owner.myStats.GetStats(curStats).GetCurrentExperience + "/" + owner.myStats.GetStats(curStats).GetNextLevelExperience;
    }
   
}

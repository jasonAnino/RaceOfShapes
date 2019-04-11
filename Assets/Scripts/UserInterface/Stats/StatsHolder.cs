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

[System.Serializable]
public class StatsIconHolder
{
    public Stats name;
    public Sprite statIcon;
    public Color iconColor = new Color();
}
public class StatsHolder : MonoBehaviour
{
    public Image statImage;
    public List<StatsIconHolder> statsIconList;
    public Stats curStats;
    public TextMeshProUGUI statsName;
    public TextMeshProUGUI level;
    public TextMeshProUGUI exp;
    public UnitBaseBehaviourComponent owner;

    // Exp Bar
    public Image expFill;
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
        if (statsIconList.Find(x => x.name == newName) != null)
        {
            int idx = statsIconList.FindIndex(x => x.name == newName);
            statImage.sprite = statsIconList[idx].statIcon;
        }
        statsName.text = curStats.ToString();
        level.text = owner.myStats.GetStats(curStats).GetLevel.ToString();
        exp.text = owner.myStats.GetStats(curStats).GetCurrentExperience + "/" + owner.myStats.GetStats(curStats).GetNextLevelExperience;
        expFill.fillAmount = owner.myStats.GetStats(curStats).GetCurrentExperience / owner.myStats.GetStats(curStats).GetNextLevelExperience;
    }

    public void UpdateStats(Parameters p)
    {
        exp.text = owner.myStats.GetStats(curStats).GetCurrentExperience + "/" + owner.myStats.GetStats(curStats).GetNextLevelExperience;
        expFill.fillAmount = owner.myStats.GetStats(curStats).GetCurrentExperience / owner.myStats.GetStats(curStats).GetNextLevelExperience;
        level.text = owner.myStats.GetStats(curStats).GetLevel.ToString();
        if (expFill.fillAmount == 1)
        {
           // Play Animation Here For Level Up
        }
    }
   
}

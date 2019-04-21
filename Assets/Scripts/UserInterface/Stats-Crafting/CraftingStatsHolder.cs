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
public class CraftingStatsIconHolder
{
    public CraftingStats name;
    public Sprite statIcon;
    public Color iconColor = new Color();
}
public class CraftingStatsHolder : MonoBehaviour
{
    public Image statImage;
    public List<CraftingStatsIconHolder> craftingStatsIconList;
    public CraftingStats curStats;
    public TextMeshProUGUI statsName;
    public TextMeshProUGUI level;
    public TextMeshProUGUI exp;
    public UnitBaseBehaviourComponent owner;
    // Exp Bar
    public Image expFill;

    public void Awake()
    {
        EventBroadcaster.Instance.AddObserver(EventNames.UPDATE_PLAYER_CRAFTING_STATS, UpdateStats);
    }
    public void OnDestroy()
    {
        EventBroadcaster.Instance.RemoveActionAtObserver(EventNames.UPDATE_PLAYER_CRAFTING_STATS, UpdateStats);
    }

    public void Initialize(CraftingStats newName, UnitBaseBehaviourComponent newOwner)
    {
        //Debug.Log("Initializing Crafting Stats!");
        owner = newOwner;
        curStats = newName;
        if (craftingStatsIconList.Find(x => x.name == newName) != null)
        {
            int idx = craftingStatsIconList.FindIndex(x => x.name == newName);
            statImage.sprite = craftingStatsIconList[idx].statIcon;
            statImage.color = craftingStatsIconList[idx].iconColor;
        }
        statsName.text = curStats.ToString();
        level.text = "LVL " + owner.myStats.GetStats(curStats).GetLevel.ToString();
        exp.text = owner.myStats.GetStats(curStats).GetCurrentExperience + "/" + owner.myStats.GetStats(curStats).GetNextLevelExperience;
        expFill.fillAmount = owner.myStats.GetStats(curStats).GetCurrentExperience / owner.myStats.GetStats(curStats).GetNextLevelExperience;

    }
    public void UpdateStats(Parameters p)
    {
        exp.text = owner.myStats.GetStats(curStats).GetCurrentExperience + "/" + owner.myStats.GetStats(curStats).GetNextLevelExperience;
        expFill.fillAmount = owner.myStats.GetStats(curStats).GetCurrentExperience / owner.myStats.GetStats(curStats).GetNextLevelExperience;
        level.text = "LVL " + owner.myStats.GetStats(curStats).GetLevel.ToString();
        if (expFill.fillAmount == 1)
        {
            // Play Animation Here For Level Up
        }
    }

}

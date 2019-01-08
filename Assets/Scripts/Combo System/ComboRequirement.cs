using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using UnitsScripts.Behaviour;
using UnitStats;
using PlayerScripts.UnitCommands;
using ComboSystem;

[Serializable]
public class ComboRequirement
{
    public Stats statRequirement;
    public int statLevel;

    public EquippedItem itemRequired;

    public bool CheckRequiredStats(UnitStatsSystem unit)
    {
        bool tmp = false;
        if (unit.GetCurrentStats[statRequirement] != null)
        {
            if(unit.GetCurrentStats[statRequirement].GetLevel >= statLevel)
            tmp = true;
        }
       return tmp;
    }
}

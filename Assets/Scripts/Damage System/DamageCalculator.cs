using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using PlayerScripts.UnitCommands;
using UnitsScripts.FSM;
using InteractableScripts.Behavior;
using WorldObjectScripts.Behavior;
using Utilities;
using UnitStats;
using ComboSystem;
using SkillBehaviour;
using UnitsScripts.Behaviour;

public class DamageCalculator
{
    public static float StatsCompareAndCalcutateDamage(NumericalStatsHolder statsUsed, NumericalStats targetStat, UnitBaseBehaviourComponent sender, UnitBaseBehaviourComponent receiver)
    {
        float dmgCount = 0;
        switch (targetStat)
        {
            case NumericalStats.PhysicalDamage:
                dmgCount = statsUsed.currentCount;
                break;
            case NumericalStats.PhysicalDefense:
                dmgCount = statsUsed.currentCount;
                break;
            case NumericalStats.Speed:
                dmgCount = statsUsed.currentCount;
                break;
            case NumericalStats.Stamina:
                dmgCount = statsUsed.currentCount;
                break;
            case NumericalStats.MagicalDefense:
                dmgCount = statsUsed.currentCount;
                break;
            case NumericalStats.MagicalDamage:
                dmgCount = statsUsed.currentCount;
                break;
            case NumericalStats.Mana:
                dmgCount = statsUsed.currentCount;
                break;
            case NumericalStats.Health:
                if (statsUsed.numericalStats == NumericalStats.PhysicalDamage)
                {
                    dmgCount = statsUsed.currentCount - receiver.myStats.GetUnitNumericalStats[NumericalStats.PhysicalDefense].currentCount;
                }
                else if (statsUsed.numericalStats == NumericalStats.MagicalDamage)
                {
                    dmgCount = statsUsed.currentCount - receiver.myStats.GetUnitNumericalStats[NumericalStats.MagicalDefense].currentCount;
                }
                break;
        }

        
        return dmgCount;
    }
    public static float StatsCompareAndCalcutateDamage(float netAmount,NumericalStats attackType, NumericalStats targetStat, UnitBaseBehaviourComponent sender, UnitBaseBehaviourComponent receiver)
    {
        float dmgCount = 0;
        switch (targetStat)
        {
            case NumericalStats.PhysicalDamage:
                dmgCount = netAmount;
                break;
            case NumericalStats.PhysicalDefense:
                dmgCount = netAmount;
                break;
            case NumericalStats.Speed:
                dmgCount = netAmount;
                break;
            case NumericalStats.Stamina:
                dmgCount = netAmount;
                break;
            case NumericalStats.MagicalDefense:
                dmgCount = netAmount;
                break;
            case NumericalStats.MagicalDamage:
                dmgCount = netAmount;
                break;
            case NumericalStats.Mana:
                dmgCount = netAmount;
                break;
            case NumericalStats.Health:
                if (attackType == NumericalStats.PhysicalDamage)
                {
                    dmgCount = netAmount - receiver.myStats.GetUnitNumericalStats[NumericalStats.PhysicalDefense].currentCount;
                }
                else if (attackType == NumericalStats.MagicalDamage)
                {
                    dmgCount = netAmount - receiver.myStats.GetUnitNumericalStats[NumericalStats.MagicalDefense].currentCount;
                }
                break;
        }


        return dmgCount;
    }

}

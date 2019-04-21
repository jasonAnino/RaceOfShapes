using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnitStats;
using UnitsScripts.Behaviour;
using ComboSystem;

public enum SkillEffectType
{
    buff = 0,
    debuff = 1,
    attack = 3,
    aura = 4,
}

[Serializable]
public class PowerEffectComponent
{
    public string effectName;
    public string id;
    public UnitBaseBehaviourComponent recepient, owner;
    public SkillEffectType effectType;
    public NumericalStats effectedStats;
    public NumericalStats attackType;
    public float baseAmount;
    public float netAmount;
    public float duration;
    public bool onTouchEffect = false;
    /// <summary>
    /// Sets the recepient of the power, it will receive the effects, whatever it may be.
    /// </summary>
    /// <param name="aimAt"></param>
    public void SetPowerOwner(UnitBaseBehaviourComponent aimAt)
    {
        recepient = aimAt;
        ImplementPower();
    }
    /// <summary>
    /// Implement Power transfers the effects of the skill depending on what effectType the skill is.
    /// </summary>
    public void ImplementPower()
    {
        // Buff - transfers the power to the affected unit for a duration, then becomes a debuff at the end to nullify its effect
        if (effectType == SkillEffectType.buff)
        {
            switch (effectedStats)
            {
                case NumericalStats.PhysicalDefense:
                    // Reduce Armor
                    break;
                case NumericalStats.Health:
                  
                    break;
                case NumericalStats.Speed:
                    recepient.myStats.GetUnitNumericalStats[NumericalStats.Speed].currentCount += netAmount;
                    recepient.SetSpeed();
                    break;
                case NumericalStats.Stamina:

                    break;
            }

        }
        // Debuff - transfers the power to the affected unit for a duration, then becomes a buff at the end to nullify its effect
        else if (effectType == SkillEffectType.debuff)
        {
            switch (effectedStats)
            {
                case NumericalStats.PhysicalDefense:
                    // Add Armor
                    break;
                case NumericalStats.Health:

                    break;
                case NumericalStats.Speed:
                    recepient.myStats.GetUnitNumericalStats[NumericalStats.Speed].currentCount -= netAmount;
                    recepient.SetSpeed();
                    break;
                case NumericalStats.Stamina:

                    break;
            }
        }
        // Attack - Creates an attack class, that then computes the net Damage the unit will receive, 
        // [WARNING] permanent affliction until unit is fully healed using heals for unique stuff.
       else if(effectType == SkillEffectType.attack)
        {
            switch (effectedStats)
            {
                case NumericalStats.PhysicalDefense:
                    // Reduce Armor
                    break;
                case NumericalStats.Health:
                    float netDamage = DamageCalculator.StatsCompareAndCalcutateDamage(netAmount, attackType, NumericalStats.Health, owner, recepient);
                    recepient.ReceiveDamage(owner, netDamage);
                    break;
                case NumericalStats.Speed:

                    break;
                case NumericalStats.Stamina:

                    break;
            }
        }
    }
    public void RemovePower()
    {
        if(recepient == null)
        {
            return;
        }
        effectType = SkillEffectType.debuff;

        ImplementPower();
    }

    public void SetNetAmount(UnitBaseBehaviourComponent caster)
    {
        owner = caster;
        float addThis = baseAmount * (caster.myStats.GetUnitNumericalStats[effectedStats].currentCount / 100.0f);
        netAmount = baseAmount + addThis;
    }
}

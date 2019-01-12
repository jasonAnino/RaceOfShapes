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
public enum StatsEffected
{
    health = 0,
    speed = 1,
    stamina = 2,
    armor = 3,
}

[Serializable]
public class PowerEffectComponent
{
    public string effectName;
    public string id;
    public UnitBaseBehaviourComponent recepient;
    public SkillEffectType effectType;
    public StatsEffected effectedStats;
    public float baseAmount;
    public float netAmount;
    public float duration;
    public bool onTouchEffect = false;
    public Stats statsPowerBuff;
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
                case StatsEffected.armor:
                    // Reduce Armor
                    break;
                case StatsEffected.health:
                  
                    break;
                case StatsEffected.speed:
                    recepient.myStats.speed += netAmount;
                    recepient.SetSpeed();
                    break;
                case StatsEffected.stamina:

                    break;
            }

        }
        // Debuff - transfers the power to the affected unit for a duration, then becomes a buff at the end to nullify its effect
        else if (effectType == SkillEffectType.debuff)
        {
            switch (effectedStats)
            {
                case StatsEffected.armor:
                    // Add Armor
                    break;
                case StatsEffected.health:

                    break;
                case StatsEffected.speed:
                    recepient.myStats.speed -= netAmount;
                    recepient.SetSpeed();
                    break;
                case StatsEffected.stamina:

                    break;
            }
        }
        // Attack - Creates an attack class, that then computes the net Damage the unit will receive, 
        // [WARNING] permanent affliction until unit is fully healed using heals for unique stuff.
       else if(effectType == SkillEffectType.attack)
        {
            Attack tmpAttack = new Attack();
            tmpAttack.baseDamage = netAmount;

            switch (effectedStats)
            {
                case StatsEffected.armor:
                    // Reduce Armor
                    break;
                case StatsEffected.health:
                    recepient.ReceiveDamage(tmpAttack.baseDamage, StatsEffected.health);
                    break;
                case StatsEffected.speed:

                    break;
                case StatsEffected.stamina:

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
        float addThis = baseAmount * (caster.myStats.GetStats(statsPowerBuff).GetLevel / 100.0f);
        netAmount = baseAmount + addThis;
    }
}

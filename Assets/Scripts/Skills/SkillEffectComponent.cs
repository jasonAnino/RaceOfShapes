using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnitStats;
using UnitsScripts.Behaviour;
public enum SkillEffectType
{
    buff = 0,
    debuff = 1,
}
public enum StatsEffected
{
    health = 0,
    speed = 1,
    stamina = 2,
    armor = 3,
}

[Serializable]
public class SkillEffectComponent
{
    public string effectName;
    public string id;
    public SkillEffectType effectType;
    public float amount;

    public void ImplementSkill(UnitBaseBehaviourComponent aimAt)
    {
        switch (effectType)
        {
            // Implement a Positive change to the unit Aimed At.
            case SkillEffectType.buff:

                break;
        }

    }
}

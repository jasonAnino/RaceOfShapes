using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using UnitStats;
using UnitsScripts.Behaviour;
using PlayerScripts.UnitCommands;

namespace ComboSystem
{
    public enum SkillType
    {
        MeleeAttack = 0,
        Projectile = 1,
        Buff = 2,
    }
    public enum TargetType
    {
       Self = 0,
       SingleTarget = 1,
       MultipleTarget = 2,
       AOE = 3,
       Wave = 4,
    }
    public enum CombatMode
    {
        Relax = 0,
        Cautious = 1,
        Combat = 2,
    }
    public enum EquippedItem
    {
        Any = 0,
        Hand =  1,
        Melee = 2,
        Range = 3,
        Book = 4,
        DefensiveGear = 5,
    }

    public enum Combination
    {
        W = 0,
        S = 1,
        A = 2,
        D = 3,
    }
    [Serializable]
    public class BaseCombo
    {
        public string SkillName;
        public SkillType skillType;
        public TargetType TargetType;
        public CombatMode combatMode;
        public EquippedItem equipRequirement;
        public Combination[] combo;
        public int comboIdx = 0;
        public List<ComboRequirement> requirements;

        public bool CheckCombo(int input)
        {
            if(input != (int)combo[comboIdx])
            {
                return false;
            }
            else
            {
                comboIdx += 1;
            }
            return true;
        }
    }
}

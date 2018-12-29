using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UserInterface;

using UnitsScripts.Behaviour;

namespace UnitStats
{
    public enum AttackType
    {
        Normal,
        Arcane,
    }
    public class UnitGatheringResourceStats
    {
        public float unitDamage_C = 0.0f;
        public float dmgInterval_C = 5.0f;
        public bool dataInitialized = false;

        public float curTime = 0.0f;
        public UnitBaseBehaviourComponent unitSaved;
        Attack atkType;
        public Attack GetAtkType
        {
            get { return this.atkType; }
            set { this.atkType = value; }
        }

        public void ClearData()
        {
            unitDamage_C = 0.0f;
            dmgInterval_C = 0.0f;
            atkType = null;
        }

        public void SetInterval(UnitBaseBehaviourComponent unit, float baseInterval = 10.0f)
        {
            float reduction = unit.myStats.GetStats(Stats.Strength).GetLevel / 10;
            dmgInterval_C = baseInterval - reduction;
            unitSaved = unit;
        }
    }

    public class Attack
    {
        public AttackType attackType;
        public Stats statsBuff;
        public float stats;
        public float baseDamage;
        
        public Attack CreateData(AttackType atkType,Stats buffType ,int statLevel, float baseDmg)
        {
            Attack tmp = new Attack();

            tmp.attackType = atkType;
            statsBuff = buffType;
            stats = statLevel;
            baseDamage = baseDmg;

            return tmp;
        }
        /// <summary>
        /// Obtain Raw Damage base from :
        /// Sk. Base Damage + (Sk. Base Damage * Stats/100)
        /// </summary>
        /// <returns></returns>
        public float RawDamage()
        {
            float tmp = baseDamage + (baseDamage * stats / 100);
            return tmp;
        }
    }
    /// <summary>
    /// This is Where All  [ ADD / SUBTRACT / GET / SET / COMPUTATION ] 
    /// should happen
    /// </summary>
    [Serializable]
    public class UnitStatsSystem
    {
        public float health_C = 100;
        public float health_M = 100;
        [SerializeField]private Dictionary<Stats, BaseUnitStats> stats = new Dictionary<Stats, BaseUnitStats>();
        public Dictionary<Stats, BaseUnitStats> GetSpecificStats
        {
            get { return this.stats; }
        }
        public void InitializeSystem()
        {
            foreach(Stats item in Enum.GetValues(typeof(Stats)))
            {
                BaseUnitStats tmp = new BaseUnitStats();
                tmp.InitializeStats(item.ToString());
                stats.Add(item, tmp);
            }
        }
        /// <summary>
        /// [RECEIVED]Net damage is the total damage with computation from the units' stats.
        /// </summary>
        /// <param name="rawDamage">Bare damage dealt to unit, without Armor reduction</param>
        /// <returns></returns>
        public float NetDamage(Attack attack)
        {
            float tmp = attack.RawDamage();

            if (stats.ContainsKey(Stats.Toughness))
            {
                if(stats[Stats.Toughness].GetLevel > 0)
                {
                    tmp = attack.RawDamage() - (tmp * stats[Stats.Toughness].GetLevel/100);
                }
            }
            return tmp;
        }


        /// <summary>
        /// Use this only when you see players
        /// </summary>
        /// <param name="thisStats"></param>
        /// <returns></returns>
        public BaseUnitStats GetStats(Stats thisStats)
        {
            if(stats[thisStats] != null)
            {
                return stats[thisStats];
            }
            else
            {
                Debug.LogError("Unit does not contain " + thisStats + " stats, adding it to preference");
                BaseUnitStats newStat = new BaseUnitStats();
                newStat.InitializeStats(thisStats.ToString());
                stats.Add(thisStats, newStat);
                return stats[thisStats];
            }
        }
    }
}

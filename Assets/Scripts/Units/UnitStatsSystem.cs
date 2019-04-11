using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UserInterface;

using UnitsScripts.Behaviour;
using ItemScript;

namespace UnitStats
{
    public enum AttackType
    {
        Normal,
        Arcane,
    }
   

    /// <summary>
    /// This is Where All  [ ADD / SUBTRACT / GET / SET / COMPUTATION ] 
    /// should happen
    /// </summary>
    [Serializable]
    public class UnitStatsSystem
    {
        public string name;
        public string id;
        public float health_C = 100;
        public float health_M = 100;
        [SerializeField]private Dictionary<Stats, BaseUnitStats> stats = new Dictionary<Stats, BaseUnitStats>();

        public Dictionary<Stats, BaseUnitStats> GetCurrentStats
        {
            get { return this.stats; }
        }

        public void InitializeSystem()
        {
            // Upon using Save System, try to use Initialize System as a way to Load Data from the save file.
            id = ItemsUtility.GenerateItemID(name);
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

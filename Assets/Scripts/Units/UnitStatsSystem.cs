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

        [SerializeField]private Dictionary<Stats, BaseUnitStats> stats = new Dictionary<Stats, BaseUnitStats>();
        public Dictionary<Stats, BaseUnitStats> GetCurrentStats
        {
            get { return this.stats; }
        }
        [SerializeField] private Dictionary<CraftingStats, BaseCraftingStats> craftingStats = new Dictionary<CraftingStats, BaseCraftingStats>();
        public Dictionary<CraftingStats, BaseCraftingStats> GetCraftingStats
        {
            get { return this.craftingStats; }
        }
        [SerializeField] private Dictionary<NumericalStats, NumericalStatsHolder> playerNumericalStats = new Dictionary<NumericalStats, NumericalStatsHolder>();
        public Dictionary<NumericalStats, NumericalStatsHolder> GetUnitNumericalStats
        {
            get { return this.playerNumericalStats;  }
        }
        public virtual void InitializeSystem()
        {
            // Upon using Save System, try to use Initialize System as a way to Load Data from the save file.
            id = ItemsUtility.GenerateItemID(name);
            foreach(Stats item in Enum.GetValues(typeof(Stats)))
            {
                BaseUnitStats tmp = new BaseUnitStats();
                tmp.InitializeStats(item);
                stats.Add(item, tmp);
            }

            foreach(CraftingStats item in Enum.GetValues(typeof(CraftingStats)))
            {
                BaseCraftingStats tmp = new BaseCraftingStats();
                tmp.InitializeStats(item);
                craftingStats.Add(item, tmp);
            }

            foreach(NumericalStats item in Enum.GetValues(typeof(NumericalStats)))
            {
                switch (item)
                {
                    case NumericalStats.PhysicalDamage:
                        playerNumericalStats.Add(NumericalStats.PhysicalDamage, NumericalStatsHolder.CreateStats(NumericalStats.PhysicalDamage, 1, 1));
                        playerNumericalStats[NumericalStats.PhysicalDamage].statsBuffer.Add(Stats.Strength);
                        break;
                    case NumericalStats.PhysicalDefense:
                        playerNumericalStats.Add(NumericalStats.PhysicalDefense, NumericalStatsHolder.CreateStats(NumericalStats.PhysicalDefense, 1, 1));
                        playerNumericalStats[NumericalStats.PhysicalDefense].statsBuffer.Add(Stats.Toughness);
                        break;
                    case NumericalStats.Stamina:
                        playerNumericalStats.Add(NumericalStats.Stamina, NumericalStatsHolder.CreateStats(NumericalStats.Stamina, 100, 25));
                        playerNumericalStats[NumericalStats.Stamina].statsBuffer.Add(Stats.Stamina);
                        break;
                    case NumericalStats.Health:
                        playerNumericalStats.Add(NumericalStats.Health, NumericalStatsHolder.CreateStats(NumericalStats.Health, 100, 10));
                        playerNumericalStats[NumericalStats.Health].statsBuffer.Add(Stats.Vitality);
                        break;
                    case NumericalStats.MagicalDefense:
                        playerNumericalStats.Add(NumericalStats.MagicalDefense, NumericalStatsHolder.CreateStats(NumericalStats.MagicalDefense, 1, 1));
                        playerNumericalStats[NumericalStats.MagicalDefense].statsBuffer.Add(Stats.Arcane);
                        break;
                    case NumericalStats.Speed:
                        playerNumericalStats.Add(NumericalStats.Speed, NumericalStatsHolder.CreateStats(NumericalStats.Speed, 5, 0.2f));
                        playerNumericalStats[NumericalStats.Speed].statsBuffer.Add(Stats.Agility);
                        break;
                    case NumericalStats.Mana:
                        playerNumericalStats.Add(NumericalStats.Mana, NumericalStatsHolder.CreateStats(NumericalStats.Mana, 100, 25));
                        playerNumericalStats[NumericalStats.Mana].statsBuffer.Add(Stats.Arcane);
                        break;
                    case NumericalStats.MagicalDamage:
                        playerNumericalStats.Add(NumericalStats.MagicalDamage, NumericalStatsHolder.CreateStats(NumericalStats.MagicalDamage, 3, 1));
                        playerNumericalStats[NumericalStats.MagicalDamage].statsBuffer.Add(Stats.Arcane);
                        break;
                }
            }
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
                newStat.InitializeStats(thisStats);
                stats.Add(thisStats, newStat);
                return stats[thisStats];
            }
        }
        public BaseCraftingStats GetStats(CraftingStats thisStats)
        {
            if (craftingStats[thisStats] != null)
            {
                return craftingStats[thisStats];
            }
            else
            {
                Debug.LogError("Unit does not contain " + thisStats + " stats, adding it to preference");
                BaseCraftingStats newStat = new BaseCraftingStats();
                newStat.InitializeStats(thisStats);
                craftingStats.Add(thisStats, newStat);
                return craftingStats[thisStats];
            }
        }
    }
}

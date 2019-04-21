using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UnitStats
{
    public enum Stats
    {
        Strength,
        Toughness,
        Stamina,
        Arcane,
        Vitality,
        Agility,
    }
    public enum CraftingStats
    {
        Blacksmith,
    }
    public enum NumericalStats
    {
        PhysicalDamage,
        PhysicalDefense,
        Stamina,
        Health,
        MagicalDefense,
        MagicalDamage,
        Mana,
        Speed,
    }

    // Temporary Count are stuff from Items
    public class TemporaryCount
    {
        public float temporaryAddedCount = 0; // Amount of Attribute Added
        public float duration = 0; 
        public ItemHolder itemBased; // If Added Attribute is based on an Item Equipped

        public static TemporaryCount CreateTmpCount(float addedCount, float duration = 0)
        {
            TemporaryCount tmp = new TemporaryCount();
            tmp.temporaryAddedCount = addedCount;
            tmp.duration = duration;
            return tmp;
        }
        public static TemporaryCount CreateTmpCount(float addedCount, ItemHolder itemBuffer)
        {
            TemporaryCount tmp = new TemporaryCount();
            tmp.temporaryAddedCount = addedCount;
            tmp.itemBased = itemBuffer;

            return tmp;
        }
    }
    [Serializable]
    public class NumericalStatsHolder
    {
        public static NumericalStatsHolder CreateStats(NumericalStats stats, float newCount = 0, float growthIncrement = 0)
        {
            NumericalStatsHolder tmp = new NumericalStatsHolder();
            tmp.numericalStats = stats;
            tmp.currentCount = newCount;
            tmp.maxCount = newCount;
            tmp.growthIncrement = growthIncrement;
            return tmp;
        }
        public List<Stats> statsBuffer = new List<Stats>();
        public NumericalStats numericalStats;
        public float currentCount = 0;
        public float maxCount = 0;
        public float growthIncrement;

        public List<TemporaryCount> temporaryCounts = new List<TemporaryCount>();
        public bool hasTemporaryCount;

        public void AddedToTemporaryCounts(TemporaryCount newItem)
        {
            temporaryCounts.Add(newItem);
            if(!hasTemporaryCount)
            {
                hasTemporaryCount = true;
            }
        }
        public void SetStatsBuffer()
        {
            switch (numericalStats)
            {
                case NumericalStats.Health:
                    statsBuffer.Add(Stats.Vitality);
                    break;
                case NumericalStats.MagicalDamage:
                    statsBuffer.Add(Stats.Arcane);
                    break;
                case NumericalStats.MagicalDefense:
                    statsBuffer.Add(Stats.Arcane);
                    break;
                case NumericalStats.Mana:
                    statsBuffer.Add(Stats.Arcane);
                    break;
                case NumericalStats.PhysicalDamage:
                    statsBuffer.Add(Stats.Strength);
                    break;
                case NumericalStats.PhysicalDefense:
                    statsBuffer.Add(Stats.Strength);
                    break;
                case NumericalStats.Speed:
                    statsBuffer.Add(Stats.Agility);
                    break;
                case NumericalStats.Stamina:
                    statsBuffer.Add(Stats.Stamina);
                    break;
            }

        }
    }

    [Serializable]
    public class BaseUnitStats
    {

        Stats statName;
        public string GetName
        {
            get { return this.statName.ToString(); }
        }
        int statLevel = 1;
        public int GetLevel
        {  get { return this.statLevel; } }
        float experience_C = 0;
        public float GetCurrentExperience
        {
            get { return this.experience_C; }
        }
        float experience_M = 100;
        public float GetNextLevelExperience
        {
            get { return this.experience_M; }
        }

        float growthIncrement = 0;

        public void InitializeStats(Stats newName, int newLevel = 1, float newExpProgress = 0)
        {
            statName = newName;
            statLevel = newLevel;
            experience_C = newExpProgress;

            ResetGrowthRequirement();
        }
        
        // use to adjust Weight of growth
        private void ResetGrowthRequirement()
        {
            if(statLevel > 0)
            {
                growthIncrement = statLevel * experience_M;
                experience_M = growthIncrement;
            }
            else
            {
                growthIncrement = 100;
                experience_M = 100;
            }
            

        }

        public void AdjustLevel(int newLevel)
        {
            statLevel = newLevel;
        }
        /// <summary>
        /// CurrentExperience = newExperience
        /// </summary>
        /// <param name="newExperience">newExperience will be the overall count of currentExperience</param>
        public void AdjustExperience(float newExperience)
        {
            experience_C = newExperience;
            if(CheckLevelUpReady())     LevelUp();
        }
        /// <summary>
        /// Increases Experience (current Exp + addExperience).
        /// </summary>
        /// <param name="addExperience">Experience to be added.</param>
        public void IncreaseExperience(float addExperience)
        {
            experience_C += addExperience;
            //Debug.Log("Adding Experience Amount : " + addExperience + " To Stats : " + statName);
            if (CheckLevelUpReady()) LevelUp();
        }
        public bool CheckLevelUpReady()
        {
            if(experience_C > experience_M)
            {
                return true;
            }

            return false;
        }
        public void LevelUp()
        {
            if(CheckLevelUpReady())
            {
                experience_C -= experience_M;
                statLevel += 1;
                ResetGrowthRequirement();
               
                if(CheckLevelUpReady())
                {
                    LevelUp();
                }
            }

        }
    }
}

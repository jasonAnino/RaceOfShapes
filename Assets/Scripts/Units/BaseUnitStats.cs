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
        Stamina
    }
    [Serializable]
    public class BaseUnitStats
    {

        string statName;
        public string GetName
        {
            get { return this.statName; }
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
        public void InitializeStats(string newName, int newLevel = 1, float newExpProgress = 0)
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
            Debug.Log("Adding Experience Amount : " + addExperience + " To Stats : " + statName);
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

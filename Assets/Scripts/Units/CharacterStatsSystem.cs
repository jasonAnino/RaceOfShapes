using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Utilities;
/// <summary>
///  This will hold all the potential stat experience gaining.
/// </summary>

namespace UnitStats
{
    [Serializable]
    public class CharacterStatsSystem : UnitStatsSystem
    {
        public float stamina_C = 100;
        public float stamina_M = 100;
        public float mana_C = 100;
        public float mana_M = 100;
        public float speed = 3.0f;
        public string title = "Commoner"; // To be Implemented

        public int level;
        public float curExperience;
        public float maxExperience;

        public void InitializeStatsSystem()
        {

        }
        public void MovementUpdate(float weight)
        {
            float distTravel = speed / 100;
            if(GetCurrentStats[Stats.Stamina] != null)
            {
                GainsFromMovement(distTravel);
            }
            if(GetCurrentStats[Stats.Strength] != null)
            {
                if(weight != 0)
                {
                    GainsFromWeight(weight);
                }
            }
        }
        /// Vitality - Gains Experience From [Physical Damage, Damage Overtime]
        float accumulatedVitalityStress = 0;
        float acceptedDamageThreshold = 1;
        float vitalityLevelThreshold = 2;
        /// Toughness - Gains Experience From [Physical Damage]
        float accumulatedToughStress = 0;
        float acceptedToughThreshold = 25;
        float toughnessLevelThreshold = 2;
        public void GainsFromDamage(float damageTaken)
        {
            accumulatedVitalityStress += damageTaken / health_M;
            //Debug.Log("Vitality Stress : " + accumulatedVitalityStress);
            if(accumulatedVitalityStress > acceptedDamageThreshold)
            {
                accumulatedVitalityStress -= acceptedDamageThreshold;
                GetCurrentStats[Stats.Vitality].IncreaseExperience(1);
                EventBroadcaster.Instance.PostEvent(EventNames.UPDATE_PLAYER_STATS);
            }
            float toughnessCalculatedThreshold = (health_M * 0.15f);

            if(damageTaken > toughnessCalculatedThreshold)
            {
                accumulatedToughStress += toughnessCalculatedThreshold;
            }
            Debug.Log("Tough Stress : " + accumulatedToughStress);
            if (accumulatedToughStress > acceptedToughThreshold)
            {
                GetCurrentStats[Stats.Toughness].IncreaseExperience(1);
                accumulatedToughStress -= acceptedToughThreshold;
                EventBroadcaster.Instance.PostEvent(EventNames.UPDATE_PLAYER_STATS);
            }
        }
        /// Strength - Gains Experience From [Walking Overweight, Eat Protein Foods]
        float accumulatedStrengthStress = 0;
        float acceptedStressThreshold = 200;
        float strengthLevelThreshold = 2;
        public void GainsFromWeight(float weight)
        {
            accumulatedStrengthStress += weight * (speed / 100);
            Debug.Log("Accumulating Strength Stress : " + accumulatedStrengthStress);
            if(accumulatedStrengthStress > acceptedStressThreshold)
            {
                Debug.Log("Accumulated Distance Surpassed Threshold, Adding Experience!");
                GetCurrentStats[Stats.Strength].IncreaseExperience(1);
                accumulatedStrengthStress -= acceptedStressThreshold;
                EventBroadcaster.Instance.PostEvent(EventNames.UPDATE_PLAYER_STATS);
            }
        }

       
        /// Stamina - Gains Experience From [Running, further the distance, the better]
        float accumulatedDistance = 0; // accumulated distance is the basis on when to increase player stats.
        float acceptedDistanceThreshold = 10; // Everytime a accumulatedDistance is more than threshold, player gains experience
        float staminaLevelThreshold = 5; // Every 5 levels, acceptedDistanceThreshold will increase by 20.
        public void GainsFromMovement(float distanceTravelled)
        {
            accumulatedDistance += distanceTravelled;
            //Debug.Log("Accumulated Distance : " + accumulatedDistance);
            if(accumulatedDistance > acceptedDistanceThreshold)
            {
                //Debug.Log("Accumulated Distance Surpassed Threshold, Adding Experience!");
                GetCurrentStats[Stats.Stamina].IncreaseExperience(1);
                accumulatedDistance -= acceptedDistanceThreshold;
                EventBroadcaster.Instance.PostEvent(EventNames.UPDATE_PLAYER_STATS);
            }
        }
        // Agility - Gains Experience From [Quickly Changing Direction]
    }
}

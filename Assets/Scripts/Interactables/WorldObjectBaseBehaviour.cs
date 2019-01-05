using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnitsScripts.Behaviour;
using Utilities.MousePointer;
using InteractableScripts.Behavior;
using PlayerScripts.UnitCommands;
using UserInterface;

using UnitStats;
using ItemScript;
/// <summary>
///  Shows basic non-living interactable objects from Trees to Chests
/// </summary>
namespace WorldObjectScripts.Behavior
{
    /// <summary>
    /// Foundation of all potential non-living intertactable objects 
    /// </summary>
    [RequireComponent(typeof(Animation))]
    public class WorldObjectBaseBehaviour : UnitBaseBehaviourComponent
    {

        public Animation mAnimation;
        public ParticleSystem mParticleSystem;
        public GameObject itemDrop;
        public List<ItemInformation> rewardInformation;
        public List<Stats> statsToCompute = new List<Stats>();
        public List<UnitGatheringResourceStats> gathererStats = new List<UnitGatheringResourceStats>();

        public override void Awake()
        {
            base.Awake();
            Initialize();
        }
        public virtual void Initialize()
        {
            mAnimation = this.GetComponent<Animation>();
        }
        
        public virtual void SpawnReward()
        {
            // TODO : Create an Item Script -> Create Inventory Script that holds an Array of Item Scripts.
            if(rewardInformation.Count <= 0)
            {
                return;
            }
            if(itemDrop == null)
            {
                return;
            }
        }
        public virtual void IncrementInteractingUnitsStats(UnitBaseBehaviourComponent unit, float valueToAdd = 5.0f)
        {
            foreach (Stats item in statsToCompute)
            {
                unit.myStats.GetStats(item).IncreaseExperience(valueToAdd);
                Debug.Log("Current Unit Stats is : " + item + " Exp : " + unit.myStats.GetStats(item).GetCurrentExperience + "/" + unit.myStats.GetStats(item).GetNextLevelExperience);
            }
        }
        public IEnumerator StartDeathCounter(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            currentState = LivingState.Alive;
            canInteract = true;
            myStats.health_C = myStats.health_M;
        }
    }
}
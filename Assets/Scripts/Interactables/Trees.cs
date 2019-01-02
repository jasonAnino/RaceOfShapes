using System.Linq;
using System.Collections;
using System.Collections.Generic;
using InteractableScripts.Behavior;
using UnityEngine;


using UnitsScripts.Behaviour;
using PlayerScripts.UnitCommands;
using UnitStats;

namespace WorldObjectScripts.Behavior
{
    public class Trees : WorldObjectBaseBehaviour
    {
        public float dmgInterval_B = 5.0f;
        // 1
        public override void StartInteraction(InteractingComponent newUnit, ActionType actionIndex)
        {
            if (!canInteract)
            {
                    return;
            }
            UnitBaseBehaviourComponent unit = newUnit.GetComponent<UnitBaseBehaviourComponent>();
            // Obtain Interacting Unit
            if (unit.objectType == ObjectType.PlayerControlled || unit.objectType == ObjectType.Unit)
            {
                Debug.Log("Adding : " + newUnit.transform.name);
                interactingUnit.Add(unit);
            }
            UnitGatheringResourceStats tmp = new UnitGatheringResourceStats();
            InitializeGathererStats(unit, tmp);
            ReceiveDamage(tmp.unitDamage_C, unit);
            
        }
        public override void EndIndividualInteraction(UnitBaseBehaviourComponent unit)
        {
            if (gathererStats.Contains(gathererStats.Find(x => x.unitSaved == unit)))
            {
                gathererStats.Remove(gathererStats.Find(x => x.unitSaved == unit));
                interactingUnit.Remove(unit);
                unit.ReceiveOrder(UnitOrder.GenerateIdleOrder());
            }
        }

        public override void ReceiveDamage(float netDamage, UnitBaseBehaviourComponent unitSender)
        {
            if(currentState != LivingState.Dead)
            {
                myStats.health_C -= netDamage;
                mAnimation.Play();
                mParticleSystem.Play();

                if(myStats.health_C <= 0)
                {
                    myStats.health_C = 0;
                    if(canInteract)
                    {
                        EndAllInteraction();
                        currentState = LivingState.Dead;
                        canInteract = false;
                        StartCoroutine(StartDeathCounter(5));
                    }
                }
            }
            if(unitSender != null)
            {
                IncrementInteractingUnitsStats(unitSender, netDamage);
            }

        }
        public override void EndAllInteraction()
        {
            List<UnitBaseBehaviourComponent> tmp = interactingUnit;

            foreach (UnitBaseBehaviourComponent item in tmp)
            {
                if(item.currentCommand == Commands.GATHER_RESOURCES)
                {
                    item.ReceiveOrder(UnitOrder.GenerateIdleOrder());
                }
            }

            interactingUnit.Clear();
            gathererStats.Clear();

            
        }
        // 2
        public bool IsUnitGathering(UnitBaseBehaviourComponent unit)
        {
            if(!interactingUnit.Contains(unit))
            {
                return false;
            }
            return (unit.currentCommand == Commands.GATHER_RESOURCES);
        }
        public void InitializeGathererStats(UnitBaseBehaviourComponent unit,UnitGatheringResourceStats tmp)
        {
            if (!tmp.dataInitialized)
            {
                tmp.SetInterval(unit, dmgInterval_B);
                tmp.GetAtkType = new Attack();
                tmp.GetAtkType.CreateData(AttackType.Normal,Stats.Strength, unit.myStats.GetStats(Stats.Strength).GetLevel, unit.GetUnitBaseDamage());
                tmp.unitDamage_C = myStats.NetDamage(tmp.GetAtkType);
                tmp.dataInitialized = true;
            }
            gathererStats.Add(tmp);
        }
    }
}

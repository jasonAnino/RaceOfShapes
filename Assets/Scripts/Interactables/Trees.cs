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
        public List<UnitGatheringResourceStats> gathererStats = new List<UnitGatheringResourceStats>();
        // 1
        public override void StartInteraction(InteractingComponent newUnit, ActionType actionIndex)
        {
            UnitBaseBehaviourComponent unit = newUnit.GetComponent<UnitBaseBehaviourComponent>();
            // Obtain Interacting Unit
            if (unit.objectType == ObjectType.PlayerControlled || unit.objectType == ObjectType.Unit)
            {
                interactingUnit.Add(unit);
            }
            UnitGatheringResourceStats tmp = new UnitGatheringResourceStats();
            InitializeGathererStats(unit, tmp);

            ReceiveDamage(tmp.unitDamage_C);
            
        }
        public override void EndIndividualInteraction(UnitBaseBehaviourComponent unit)
        {
            if (gathererStats.Contains(gathererStats.Find(x => x.unitSaved == unit)))
            {
                gathererStats.Remove(gathererStats.Find(x => x.unitSaved == unit));
            }
            if(interactingUnit.Count > 0)
            {
                foreach (UnitBaseBehaviourComponent item in interactingUnit)
                {
                    if(item != null)
                    {
                        if(item.currentCommand != Commands.GATHER_RESOURCES)
                        {
                            interactingUnit.Remove(item);
                        }
                        else if(myStats.health_C <= 0)
                        {
                            SpawnReward();
                            item.ReceiveOrder(UnitOrder.GenerateIdleOrder());
                            interactingUnit.Clear();
                        }
                    }
                }
            }
        }

        public override void ReceiveDamage(float netDamage)
        {
            myStats.health_C -= netDamage;
            //Debug.Log("Receiving Damage : " + netDamage);
            mAnimation.Play();
            mParticleSystem.Play();

            if(myStats.health_C <= 0)
            {
                myStats.health_C = 0;
                EndAllInteraction();
            }
        }

        public override void EndAllInteraction()
        {
            UnitOrder idle = UnitOrder.GenerateIdleOrder();
            foreach(UnitBaseBehaviourComponent unit in interactingUnit)
            {
                unit.ReceiveOrder(idle, true);
            }
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
                tmp.GetAtkType.CreateData(AttackType.Normal,Stats.Strength, unit.myStats.GetSpecificStats[Stats.Strength].GetLevel, unit.GetUnitBaseDamage());
                tmp.unitDamage_C = myStats.NetDamage(tmp.GetAtkType);
                tmp.dataInitialized = true;
            }
            gathererStats.Add(tmp);
        }
    }
}

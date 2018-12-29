using Unity.Entities;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;
using UnitsScripts.Behaviour;
using PlayerScripts.UnitCommands;
using InteractableScripts.Behavior;
using WorldObjectScripts.Behavior;
using UnitStats;

public class UnitBehaviourSystem : ComponentSystem
{
    private struct Character
    {
        public Rigidbody rigidBody;
        public NavMeshAgent mNavMeshAgent;
        public UnitBaseBehaviourComponent unitBehaviour;
    }

    private struct Plant
    {
        public Trees tree;
    }
    protected override void OnUpdate()
    {
        #region Characters
        foreach (Character entity in GetEntities<Character>())
        {
            // MOVEMENT
            Vector3 nextPosition = entity.unitBehaviour.nextPos;
            if(nextPosition != null)
            {
                if(entity.unitBehaviour.startMoving)
                {
                    entity.mNavMeshAgent.SetDestination(nextPosition);
                }
                if (entity.mNavMeshAgent.destination != null && entity.unitBehaviour.startMoving)
                {
                    float dist = Vector3.Distance(entity.unitBehaviour.nextPos, entity.unitBehaviour.transform.position);
                    if(dist < 0.75f)
                    {
                        entity.unitBehaviour.startMoving = false;
                        entity.mNavMeshAgent.destination = entity.unitBehaviour.transform.position;

                    }
                }
            }

            // CHECK ORDER
            if(entity.unitBehaviour.currentCommand != Commands.WAIT_FOR_COMMAND)
            {
                UnitOrder unitOrder = entity.unitBehaviour.currentOrder;
                if (CheckIsOrderFinish(unitOrder.commandName, entity))
                {
                    entity.unitBehaviour.currentOrder = null;
                    if(entity.unitBehaviour.unitOrders.Count > 0)
                    {
                        entity.unitBehaviour.ReceiveOrder(entity.unitBehaviour.unitOrders.Dequeue());
                    }
                    else
                    {
                        entity.unitBehaviour.currentCommand = Commands.WAIT_FOR_COMMAND;
                    }
                }
            }
        }
        #endregion

        foreach(Plant item in GetEntities<Plant>())
        {
            if(item.tree.gathererStats.Count > 0)
            {
               foreach(UnitGatheringResourceStats unit in item.tree.gathererStats)
                {
                    unit.curTime += Time.deltaTime;
                    if(unit.curTime > unit.dmgInterval_C)
                    {
                        item.tree.ReceiveDamage(unit.unitDamage_C);
                        unit.curTime = 0;
                    }
                }
            }
        }

    }

    // Check if Order is Finish
    private bool CheckIsOrderFinish(Commands command, Character unit)
    {
        bool tmp = false;

        switch(command)
        {
            case Commands.MOVE_TOWARDS:
                float dist = Vector3.Distance(unit.mNavMeshAgent.destination, unit.unitBehaviour.transform.position);
                if(dist < 0.75f)
                {
                    tmp = true;
                }
                break;
            case Commands.INTERACT:
                tmp = true;
                break;

            case Commands.WAIT_FOR_COMMAND:
                tmp = true;
                break;
        }

        return tmp;
    }
}

using Unity.Entities;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;
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
            if(entity.unitBehaviour.startMoving)
            {
                if(NavMeshPositionGenerator.GetInstance.CheckIfPointHasNavMeshAgent(nextPosition, entity.unitBehaviour))
                {
                    Debug.Log("Adjusted the odds of : " + entity.unitBehaviour.transform.name);
                    entity.unitBehaviour.nextPos = NavMeshPositionGenerator.GetInstance.GenerateCandidatePosition(nextPosition, 1f, entity.unitBehaviour, false, true);
                    nextPosition = entity.unitBehaviour.nextPos;
                }

                entity.mNavMeshAgent.SetDestination(nextPosition);

                if (entity.mNavMeshAgent.destination != null && entity.unitBehaviour.startMoving)
                {
                    entity.mNavMeshAgent.isStopped = false;
                    float dist = Vector3.Distance(entity.unitBehaviour.nextPos, entity.unitBehaviour.transform.position);
                    if(dist < 0.75f)
                    {
                        entity.unitBehaviour.startMoving = false;
                        entity.mNavMeshAgent.destination = entity.unitBehaviour.transform.position;
                        entity.unitBehaviour.nextPos = Vector3.zero;
                    }
                }
        }
            
            // This part is to reset the currentOrder of the unit AFTER finishing the command.
            if(entity.unitBehaviour.currentCommand != Commands.WAIT_FOR_COMMAND)
            {
                UnitOrder unitOrder = entity.unitBehaviour.currentOrder;
                if (CheckIsOrderFinish(unitOrder.commandName, entity))
                {
                    if(entity.unitBehaviour.currentCommand == Commands.MOVE_TOWARDS)
                    {
                       // Debug.Log(entity.unitBehaviour.transform.name + " Command is Done!");
                        AdjustNearbyNavMeshDestination(entity.unitBehaviour);
                    }
                    entity.unitBehaviour.currentOrder = null;
                    if(entity.unitBehaviour.unitOrders.Count > 0)
                    {
                        UnitOrder newNextOrder = entity.unitBehaviour.unitOrders.Peek();
                        entity.unitBehaviour.ReceiveOrder(newNextOrder);
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
               foreach(UnitGatheringResourceStats unit in item.tree.gathererStats.ToList())
                {
                    unit.curTime += Time.deltaTime;
                    if(unit.curTime > unit.dmgInterval_C)
                    {
                        item.tree.ReceiveDamage(unit.unitDamage_C, unit.unitSaved);
                        Debug.Log("Increasing Stats of Unit : " + unit.unitSaved.transform.name);
                        unit.curTime = 0;
                    }
                }
            }
        }

    }

  
    // Used To Other Units Right After Arriving at one point.
    public void AdjustNearbyNavMeshDestination(UnitBaseBehaviourComponent dontCheck)
    {
        foreach (Character item in GetEntities<Character>())
        {
            // check if they're moving already
            if(item.unitBehaviour != dontCheck)
            {
                if(item.unitBehaviour.startMoving)
                {
                    // Check if Destination is near point received from player who just reached his.
                    if(item.unitBehaviour.nextPos == null)
                    {
                        break;
                    }
                    float dist = Vector3.Distance(dontCheck.nextPos, item.unitBehaviour.nextPos);
                    //Debug.Log("Distance Between : " + item.unitBehaviour.transform.name + " And : " + dontCheck.transform.name + " is : " + dist);
                    if(dist < 1.15f)
                    {
                        //Debug.Log("Adjusting Pos : " + item.unitBehaviour.transform.name + " Old Pos : " + item.unitBehaviour.nextPos);
                        if(item.unitBehaviour.interactWith != null)
                        {
                            item.unitBehaviour.nextPos = NavMeshPositionGenerator.GetInstance.GenerateCandidatePosition(item.unitBehaviour.interactWith.transform.position, 2, item.unitBehaviour, false);
                        }
                        else
                        {
                            item.unitBehaviour.nextPos = NavMeshPositionGenerator.GetInstance.GenerateCandidatePosition(item.unitBehaviour.nextPos, 2, item.unitBehaviour, false);
                        }
                       // Debug.Log("New Pos : " + item.unitBehaviour.nextPos);
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
                    unit.mNavMeshAgent.isStopped = true;
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

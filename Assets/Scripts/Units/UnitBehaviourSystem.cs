using Unity.Entities;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;
using UnitsScripts.Behaviour;
using PlayerScripts.UnitCommands;

public class UnitBehaviourSystem : ComponentSystem
{
    private struct Filter
    {
        public Rigidbody rigidBody;
        public NavMeshAgent mNavMeshAgent;
        public UnitBaseBehaviourComponent unitBehaviour;

    }

    protected override void OnUpdate()
    {
        foreach (Filter entity in GetEntities<Filter>())
        {
            // MOVEMENT
            Vector3 nextPosition = entity.unitBehaviour.nextPos;

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

            // CHECK ORDER
            if(entity.unitBehaviour.currentOrder != null)
            {
                UnitOrder unitOrder = entity.unitBehaviour.currentOrder;
                if (CheckIsOrderFinish(unitOrder.commandName, entity))
                {
                    entity.unitBehaviour.currentOrder = null;
                    if(entity.unitBehaviour.unitOrders.Count > 0)
                    {
                        Debug.Log("New Order Set : " + entity.unitBehaviour.unitOrders.Peek().commandName);
                        entity.unitBehaviour.ReceiveOrder(entity.unitBehaviour.unitOrders.Dequeue());
                    }
                    else
                    {
                        entity.unitBehaviour.currentCommand = Commands.WAIT_FOR_COMMAND;
                    }
                }
            }
        }
    }

    // Check if Order is Finish
    private bool CheckIsOrderFinish(Commands command, Filter unit)
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

using Unity.Entities;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;
using UnitsScripts.Behaviour;

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
                }
            }
        }
    }
}

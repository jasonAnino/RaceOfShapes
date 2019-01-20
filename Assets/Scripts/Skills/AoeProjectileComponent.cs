using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnitStats;
using UnitsScripts.Behaviour;
using ComboSystem;

namespace SkillBehaviour
{
    public class AoeProjectileComponent : MonoBehaviour
    {
        public AoeSkillBehaviour skillBaseOwner;
        public bool startObtainingUnits = false;
        public bool startMoving = false;
        public GameObject targetArea;
        public Vector3 targetPosition;
        public float speed = 5;
        public float aoeRadius = 20.0f;
        public List<UnitBaseBehaviourComponent> unitsHit;
        public bool obtainUnits = false;
        public void Update()
        {
            if(startMoving)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime  );
                float dist = Vector3.Distance(transform.position, targetPosition);
                if(dist < 0.5f)
                {
                    TouchDown();
                }
            }
        }

        public void StartMoving(Vector3 newTarget)
        {
            startMoving = true;
            targetPosition = newTarget;
            obtainUnits = true;
        }

        public void TouchDown()
        {
            startMoving = false;
            obtainUnits = false;
            if(unitsHit.Count > 0)
            {
                skillBaseOwner.SetUnitsToReceive(unitsHit);
            }
            skillBaseOwner.ProjectileTouchDown();
        }


        public void OnTriggerEnter(Collider other)
        {
            if(!obtainUnits)
            {
                return;
            }

            if(other.GetComponent<UnitBaseBehaviourComponent>())
            {
                UnitBaseBehaviourComponent tmp = other.GetComponent<UnitBaseBehaviourComponent>();
                if(!unitsHit.Contains(tmp))
                unitsHit.Add(tmp);
            }
        }
        public void OnTriggerExit(Collider other)
        {
            if(!obtainUnits)
            {
                return;
            }

            if (other.GetComponent<UnitBaseBehaviourComponent>())
            {
                UnitBaseBehaviourComponent tmp = other.GetComponent<UnitBaseBehaviourComponent>();
                if(unitsHit.Contains(tmp))
                {
                    unitsHit.Remove(tmp);
                }
            }
        }
    }
}

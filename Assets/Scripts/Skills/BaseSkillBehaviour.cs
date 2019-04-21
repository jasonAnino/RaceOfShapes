using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnitStats;
using UnitsScripts.Behaviour;
using ComboSystem;

using Utilities.MousePointer;
using InteractableScripts.Behavior;

namespace SkillBehaviour
{
    public enum SkillBehaviourType
    {
        Stick = 0,
        MovingAllTimeActivated = 1,
        MovingOneTimeActivated = 2,
        MovingAreaOfEffectActivated = 3,
    }
    public class BaseSkillBehaviour : MonoBehaviour
    {
        public UnitBaseBehaviourComponent owner;
        public PowerEffectComponent skillEffect;
        public SkillType skillType;
        public SkillBehaviourType behaviourType;
        public TargetType targetType;
        public SpawnSkillType spawnType;
        [Header("Skill Enhancements")]
        public bool powerStatBased = false;
        [Header("Aiming Activation")]
        public bool startAiming = false;
        public bool activate = false;
        public Vector3 targetPosition;
        [Header("Duration or Animation")]
        public bool durationBased;
        public float duration = 5.0f;
        [Header("List of Hit Units")]
        public List<UnitBaseBehaviourComponent> affectedUnits = new List<UnitBaseBehaviourComponent>();

        public void InitializeSkill(UnitBaseBehaviourComponent newOwner, SkillType type, TargetType target, SpawnSkillType newSpawnType, SkillBehaviourType newBehaviorType)
        {
            owner = newOwner;
            skillType = type;
            behaviourType = newBehaviorType;
            spawnType = newSpawnType;
            skillEffect.SetNetAmount(owner);
            targetType = target;
            if (targetType == TargetType.Self)
            {
                ActivateSkill(owner);
            }
            if(skillType == SkillType.Projectile)
            {
                transform.rotation = owner.transform.rotation;
                activate = true;
            }
            else if(skillType == SkillType.TargetProjectile)
            {
                startAiming = true;
            }
        }

        public void ChangeOwner(UnitBaseBehaviourComponent newOwner)
        {
            owner = newOwner;
        }

        public virtual void Update()
        {
            if(activate)
            {
                if(durationBased)
                {
                    duration -= Time.deltaTime;
                    if(duration <= 0)
                    {
                        RemoveSkillFromWorld();
                    }
                }
                if(behaviourType == SkillBehaviourType.MovingOneTimeActivated || behaviourType == SkillBehaviourType.MovingAllTimeActivated)
                {
                    transform.position += transform.forward * Time.deltaTime * 10;
                }
            }
            if (startAiming)
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    transform.position = hit.point;
                }
            }
        }

        public virtual void StartSkillCasting()
        {
            startAiming = false;
            if(targetPosition == null)
            {
                targetPosition = transform.position;
            }
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                targetPosition = hit.point;
                targetPosition = new Vector3(targetPosition.x, owner.transform.position.y, targetPosition.z);
            }
            transform.position = targetPosition;

            if (spawnType == SpawnSkillType.FromCaster)
            {
                Debug.Log("FROM CASTER!");
                transform.position = new Vector3(owner.transform.position.x, owner.transform.position.y, owner.transform.position.z);
                activate = true;
            }
            else
            {
                activate = true;
            }
            owner.ReceiveOrder(UnitOrder.GenerateIdleOrder(), true);
            owner.MakeUnitLookAt(targetPosition);
            transform.rotation = owner.transform.rotation;
        }

        public void ActivateSkill(UnitBaseBehaviourComponent activateTo)
        {
            activate = true;
            if (!affectedUnits.Contains(activateTo))
            {
               affectedUnits.Add(activateTo);
            }
            activateTo.ReceiveBuff(skillEffect);
        }
        public void AdjustBehaviourOnCurrentSkillType()
        {
            switch (skillType)
            {
                case SkillType.Buff:
                    behaviourType = SkillBehaviourType.Stick;
                    break;

                case SkillType.MeleeAttack:
                case SkillType.Projectile:
                    break;
            }

        }
        #region callbacks
        public virtual void RemoveSkillFromWorld()
        {
            activate = false;
            GameObject.Destroy(this.gameObject);
        }

        public virtual void OnTriggerEnter(Collider other)
        {

            // IF ITS AOE, WE HAVE ANOTHER SCRIPT FOR THAT (AoeSkillBehaviour)
            if(targetType == TargetType.AOE)
            {
                return;
            }

            // DOES IT INFLICT DAMAGE ON UNIT ONLY ONCE?
            if(other.GetComponent<UnitBaseBehaviourComponent>())
            {
                UnitBaseBehaviourComponent tmp = other.GetComponent<UnitBaseBehaviourComponent>();
                if(tmp == owner)
                {
                    return;
                }
                if (behaviourType == SkillBehaviourType.MovingOneTimeActivated)
                {
                    // Inflict Buff once.
                    if (!affectedUnits.Contains(tmp))
                    {
                        tmp.ReceiveBuff(skillEffect);
                        affectedUnits.Add(tmp);
                    }
                }
                else
                {
                    // Always Inflict Buff
                    tmp.ReceiveBuff(skillEffect);
                    if (!affectedUnits.Contains(tmp))
                    {
                        affectedUnits.Add(tmp);
                    }
                }
            }
            if(other.gameObject.layer == 9) // Hit a Land
            {
                Debug.Log("Meow!");
                GameObject.Destroy(this.gameObject);
            }

            // DOES IT END WITH SINGLE TARGET
            if(targetType == TargetType.SingleTarget)
            {
                GameObject.Destroy(this.gameObject);
            }
        }
        #endregion
    }
}

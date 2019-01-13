using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnitStats;
using UnitsScripts.Behaviour;
using ComboSystem;


namespace SkillBehaviour
{
    public enum SkillBehaviourType
    {
        Stick = 0,
        MovingActivated = 1,
        MoveOnHitActivated = 2,
    }

    public class BaseSkillBehaviour : MonoBehaviour
    {
        public UnitBaseBehaviourComponent owner;
        public PowerEffectComponent skillEffect;
        public SkillType skillType;
        public SkillBehaviourType behaviourType;
        public TargetType targetType;
        public bool powerStatBased = false;
        public bool activate = false;
        public float duration = 5.0f;

        public List<UnitBaseBehaviourComponent> affectedUnits = new List<UnitBaseBehaviourComponent>();
        public void InitializeSkill(UnitBaseBehaviourComponent newOwner, SkillType type, TargetType target)
        {
            owner = newOwner;
            skillType = type;
            AdjustBehaviourOnCurrentSkillType();
            skillEffect.SetNetAmount(owner);
            targetType = target;
            if (targetType == TargetType.Self)
            {
                ActivateSkill(owner);
            }
            if(skillType == SkillType.Projectile)
            {
                transform.rotation = owner.transform.rotation;   
            }
        }

        public void ChangeOwner(UnitBaseBehaviourComponent newOwner)
        {
            owner = newOwner;
        }

        public void Update()
        {
            if(activate)
            {
                duration -= Time.deltaTime;
                if(duration <= 0)
                {
                    RemoveSkillFromWorld();
                }
                if(behaviourType == SkillBehaviourType.MoveOnHitActivated || behaviourType == SkillBehaviourType.MovingActivated)
                {
                    transform.position += transform.forward * Time.deltaTime * 10;
                }
            }
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
                    behaviourType = SkillBehaviourType.MoveOnHitActivated;
                    activate = true;
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
            Debug.Log("Hit : " + other.name);
            // Wont do stuff if object isnt projectile (Soon, AoE).
            if(skillType != SkillType.Projectile)
            {
                return;
            }
            if(other.GetComponent<UnitBaseBehaviourComponent>())
            {
                UnitBaseBehaviourComponent tmp = other.GetComponent<UnitBaseBehaviourComponent>();
                if(tmp == owner)
                {
                    return;
                }
                if (behaviourType == SkillBehaviourType.MoveOnHitActivated)
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
            if(targetType == TargetType.SingleTarget)
            {
                GameObject.Destroy(this.gameObject);
            }
        }
        #endregion
    }
}

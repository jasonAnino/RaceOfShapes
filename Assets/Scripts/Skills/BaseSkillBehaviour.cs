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
        public SkillEffectComponent skillStats;
        public SkillType skillType;
        public SkillBehaviourType behaviourType;
        public bool activate = false;
        public float duration = 5.0f;

        public void InitializeSkill(UnitBaseBehaviourComponent newOnwer, SkillType type)
        {
            owner = newOnwer;
            skillType = type;
            AdjustBehaviourOnCurrentSkillType();
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
            }
        }

        public void ActivateSkill(UnitBaseBehaviourComponent activateTo)
        {
            Debug.Log("Skill Activated!");
            activate = true;
            skillStats.ImplementSkill(activateTo);
        }
        public void AdjustBehaviourOnCurrentSkillType()
        {
            switch (skillType)
            {
                case SkillType.Buff:
                    behaviourType = SkillBehaviourType.Stick;
                    ActivateSkill(owner);
                    break;

                case SkillType.MeleeAttack:
                case SkillType.Projectile:
                    behaviourType = SkillBehaviourType.MoveOnHitActivated;
                    break;
            }

        }
        #region callbacks
        public virtual void RemoveSkillFromWorld()
        {
            activate = false;
        }

        public virtual void OnTriggerEnter(Collider other)
        {
            if(skillType != SkillType.Projectile || skillType != SkillType.MeleeAttack)
            {
                return;
            }
            if(other.GetComponent<UnitBaseBehaviourComponent>())
            {
                UnitBaseBehaviourComponent tmp = other.GetComponent<UnitBaseBehaviourComponent>();
                ActivateSkill(tmp);
            }
        }
        #endregion
    }
}

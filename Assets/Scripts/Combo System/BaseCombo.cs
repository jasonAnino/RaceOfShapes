using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using UnitStats;
using UnitsScripts.Behaviour;
using PlayerScripts.UnitCommands;
using Utilities;
using Utilities.MousePointer;

using SkillBehaviour;

namespace ComboSystem
{
    public enum SkillType
    {
        MeleeAttack = 0,
        Projectile = 1,
        Buff = 2,
        TargetProjectile = 3,
    }
    public enum TargetType
    {
       Self = 0,
       SingleTarget = 1,
       MultipleTarget = 2,
       AOE = 3,
       Wave = 4,
    }
    public enum CombatMode
    {
        Relax = 0,
        Cautious = 1,
        Combat = 2,
    }
    public enum EquippedItem
    {
        Any = 0,
        Hand =  1,
        Melee = 2,
        Range = 3,
        Book = 4,
        DefensiveGear = 5,
    }

    public enum Combination
    {
        W = 0,
        A = 1,
        S = 2,
        D = 3,
    }
    [Serializable]
    public class BaseCombo
    {
        public string SkillName;
        public SkillType skillType;
        public TargetType targetType;
        public CombatMode combatMode;
        public EquippedItem equipRequirement;
        public Combination[] combo;
        public int comboIdx = 0;
        public List<ComboRequirement> requirements;
        public bool targetCast = false;
        public GameObject prefab;
        public Vector3 positionAdjustment = Vector3.zero;
        public Vector3 rotationAdjustment = Vector3.zero;
        public bool StartAtFront = false;
        public bool CheckCombo(int input)
        {
            if(comboIdx >= combo.Length)
            {
                return false;
            }
            if(input != (int)combo[comboIdx])
            {
                return false;
            }
            else
            {
                comboIdx += 1;
            }
            return true;
        }

        public void ActivateSkill(UnitBaseBehaviourComponent skillOwner)
        {
            if(skillType == SkillType.Buff)
            {
                Vector3 postAdjustment = positionAdjustment + skillOwner.transform.position;
                GameObject tmp = GameObject.Instantiate(prefab, postAdjustment, Quaternion.Euler(rotationAdjustment.x, rotationAdjustment.y, rotationAdjustment.z), skillOwner.transform);
                BaseSkillBehaviour skillTmp = tmp.GetComponent<BaseSkillBehaviour>();
                skillTmp.InitializeSkill(skillOwner, SkillType.Buff, targetType);
            }
            else if(skillType == SkillType.Projectile)
            {
                Vector3 postAdjustment = positionAdjustment + skillOwner.transform.position;
                if(StartAtFront)
                {
                    postAdjustment = skillOwner.transform.forward + skillOwner.transform.position;
                    rotationAdjustment = new Vector3(skillOwner.transform.rotation.x, skillOwner.transform.rotation.y, skillOwner.transform.rotation.z);
                }
                if(skillOwner.targetUnit != null)
                {
                    skillOwner.MakeUnitLookAt(skillOwner.targetUnit.transform.position);
                }
                GameObject tmp = GameObject.Instantiate(prefab, postAdjustment, Quaternion.Euler(rotationAdjustment.x, rotationAdjustment.y, rotationAdjustment.z), null);
                BaseSkillBehaviour skillTmp = tmp.GetComponent<BaseSkillBehaviour>();
                skillTmp.InitializeSkill(skillOwner, SkillType.Projectile, targetType);
                
            }
            else if(skillType == SkillType.TargetProjectile)
            {
                GameObject tmp = GameObject.Instantiate(prefab, skillOwner.transform.position, Quaternion.Euler(rotationAdjustment.x, rotationAdjustment.y, rotationAdjustment.z), null);
                AoeSkillBehaviour skillTmp = tmp.GetComponent<AoeSkillBehaviour>();
                skillTmp.InitializeSkill(skillOwner, SkillType.TargetProjectile, targetType);
                skillTmp.startAiming = true;
                // Targetable Projectile should not be here, place it to your unit
                //PlayerUnitController.GetInstance.targetableProjectile = skillTmp;
                if (PlayerUnitController.GetInstance.manualControlledUnit != null)
                {
                      PlayerUnitController.GetInstance.manualControlledUnit.mySkills.SetSkillsToHand(skillTmp);
                }
                CursorManager.GetInstance.CursorChangeTemporary(CursorType.CLICKABLE_SKILLHOLD);
            }
        }

        public void ResetSkill()
        {
            comboIdx = 0;
        }
    }
}

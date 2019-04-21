using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ComboSystem;
using UnitStats;
using UnitsScripts.Behaviour;
using SkillBehaviour;
public class UnitSkillComponent : MonoBehaviour
{
    public UnitBaseBehaviourComponent owner;
    [Header("SKILL CASTING")]
    public bool isDualCaster = false;  // Player Can now cast with 2 skills on left and right.
    public BaseSkillBehaviour[] skillsOnHand = new BaseSkillBehaviour[2];
    public bool leftHandSkill = false;
    public bool rightHandSkill = false;
    [Header("POTENTIAL SKILLS")]
    public List<BaseCombo> buff = new List<BaseCombo>();
    public List<BaseCombo> fireMagic = new List<BaseCombo>();

    public void Start()
    {
        if(owner != null)
        {
            buff = SkillManager.GetInstance.ObtainBuffCombos(owner);
            fireMagic = SkillManager.GetInstance.ObtainFireMagicCombos(owner);
        }
    }

    public void SetSkillsToHand(BaseSkillBehaviour thisSkill)
    {
        if(skillsOnHand[0] == null)
        {
            skillsOnHand[0] = thisSkill;
            leftHandSkill = true;
        }
        else if(skillsOnHand[0] != null)
        {
            if(isDualCaster)
            {
                skillsOnHand[1] = thisSkill;
                rightHandSkill = true;
            }
        }
    }
}

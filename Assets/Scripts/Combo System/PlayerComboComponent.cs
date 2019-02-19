using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using UnitStats;
using UnitsScripts.Behaviour;
using PlayerScripts.UnitCommands;
using Utilities;

namespace ComboSystem
{
    /// <summary>
    /// This is where all the combo inputs the player is being send
    /// </summary>
    public class PlayerComboComponent : MonoBehaviour
    {
        public UnitBaseBehaviourComponent unitDoingCombo;
        public List<BaseCombo> comboList;

        public List<BaseCombo> doingCombo;

        public float curTimer = 0;
        public float maxTimer = 0.75f;
        
        public void Update()
        {
            if(unitDoingCombo != null)
            {
                if(unitDoingCombo.mySkills.skillsOnHand[0] != null)
                {
                    if(!unitDoingCombo.mySkills.isDualCaster)
                    {
                        // Here player should receive an indicator that Unit cannot cast two spells at the same time.
                        return;
                    }
                }
                // Check the W,A,S,D if its clicked
                if(Input.GetKeyDown(KeyCode.W))
                {
                    FilterCombo(0);
                }
                else if(Input.GetKeyDown(KeyCode.S))
                {
                    FilterCombo(2);
                }
                else if(Input.GetKeyDown(KeyCode.A))
                {
                    FilterCombo(1);
                }
                else if(Input.GetKeyDown(KeyCode.D))
                {
                    FilterCombo(3);
                }

                if(doingCombo.Count > 0)
                {
                    curTimer += Time.deltaTime;
                    if(Input.GetKeyDown(KeyCode.Space))
                    {
                        if(doingCombo[0].comboIdx == doingCombo[0].combo.Length)
                        {
                            Debug.Log("Activating Skill :" + doingCombo[0].SkillName);
                            doingCombo[0].ActivateSkill(unitDoingCombo);
                            doingCombo[0].comboIdx = 0;
                            EventBroadcaster.Instance.PostEvent(EventNames.RESET_VISUAL_SKILLS);
                        }
                        else
                        {
                            Debug.Log("Combo Not Yet Done!");
                        }
                        ClearDoingCombo();
                        curTimer = 0;
                    }
                    if(curTimer > maxTimer)
                    {
                        ClearDoingCombo();
                        curTimer = 0;
                        EventBroadcaster.Instance.PostEvent(EventNames.UPDATE_VISUAL_SKILLS);
                    }
                }
            
            }
        }
        public void ClearComboList()
        {
            unitDoingCombo = null;
            comboList.ForEach(x => x.comboIdx = 0);
            comboList.Clear();
            curTimer = 0;
        }
        public void SetUnitDoingCombo(UnitBaseBehaviourComponent unit)
        {
            EventBroadcaster.Instance.PostEvent(EventNames.RESET_VISUAL_SKILLS);
            ClearComboList();
            unitDoingCombo = unit;
            comboList.Clear();
            // Adds Buff
            comboList.AddRange(unit.mySkills.buff);
            comboList.AddRange(unit.mySkills.fireMagic);
        }
        public void FilterCombo(int input)
        {
            // Check if its the Initial Filter
            if(doingCombo.Count <= 0)
            {
                // Every Input Resets the Combo Timer to 0
                curTimer = 0;
                foreach(BaseCombo skill in comboList)
                {
                    if (skill.CheckCombo(input))
                    {
                        doingCombo.Add(skill);
                    }
                }
            }
            // if not, check all the other filtered.
            else
            {
                foreach (BaseCombo skill in doingCombo.ToList())
                {
                    if (!skill.CheckCombo(input))
                    {
                        skill.comboIdx = 0;
                        doingCombo.Remove(skill);
                    }
                }
                if(doingCombo.Count <= 0)
                {
                    // Ends the skill combo so player can get another set
                    curTimer = maxTimer;
                }
                else
                {
                    // Every Input Resets the Combo Timer to 0, so player will be given another set of 0.75f
                    curTimer = 0;
                }
            }

            EventBroadcaster.Instance.PostEvent(EventNames.UPDATE_VISUAL_SKILLS);
        }
        
        public void ClearDoingCombo()
        {
            doingCombo.ForEach(x => x.comboIdx = 0);
            doingCombo.Clear();
        }
        public void SetComboList(List<BaseCombo> newList)
        {
            comboList = newList;
        }

    }
}

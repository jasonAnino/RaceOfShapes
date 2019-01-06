﻿using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using UnitStats;
using UnitsScripts.Behaviour;
using PlayerScripts.UnitCommands;

namespace ComboSystem
{
    public class PlayerComboComponent : MonoBehaviour
    {
        public UnitBaseBehaviourComponent unitDoingCombo;
        public List<BaseCombo> comboList;

        public List<BaseCombo> doingCombo;

        public float curTimer = 0;
        public float maxTimer = 0.75f;
        public void Update()
        {
            // Check the W,A,S,D if its clicked
            if(Input.GetKeyDown(KeyCode.W))
            {
                FilterCombo(0);
            }
            else if(Input.GetKeyDown(KeyCode.S))
            {
                FilterCombo(1);
            }
            else if(Input.GetKeyDown(KeyCode.A))
            {
                FilterCombo(2);
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
                        Debug.Log("Doing Skill : " + doingCombo[0].ComboName);
                        doingCombo[0].comboIdx = 0;
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
                }
            }
            
        }

        public void FilterCombo(int input)
        {
            // Check if its the Initial Filter
            if(doingCombo.Count <= 0)
            {
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
                    curTimer = 0.0f;
                }
            }
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
        public void ClearComboList()
        {
            comboList.Clear();
        }

    }
}
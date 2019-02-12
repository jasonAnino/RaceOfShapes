using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ComboSystem;

namespace UserInterface.Skills
{
    public class BaseSkillOptions : MonoBehaviour
    {
        BaseCombo currentSkillCombo;
        public List<Combination> currentCombination;

        public int currentComboCount;
        public List<SkillLetterHolder> skillLetters;

        public void Start()
        {

        }
        public void SetNewCombo(BaseCombo newCombo)
        {
            if(currentSkillCombo == newCombo)
            {
                return;
            }

            currentSkillCombo = newCombo;
            currentComboCount = newCombo.comboIdx;
            SetSkillWave();
        }

        public void SetSkillWave()
        {
            if(currentCombination != null)
            {
                currentCombination.Clear();
            }
            currentCombination = currentSkillCombo.combo.ToList();

            SetSkillLetters();
        }
        public void SetSkillLetters()
        {
            for(int i = 0; i < currentSkillCombo.combo.Length; i++)
            {
                if(i >= skillLetters.Count)
                {
                    break;
                }
                skillLetters[i].gameObject.SetActive(true);

                if(i < currentComboCount)
                {
                    skillLetters[i].SetCurrentLetter((int)currentCombination[i], true);
                }
                else
                {
                    if(currentComboCount == i)
                    {
                        skillLetters[i].SetCurrentLetter((int)currentCombination[i], false, true);
                    }
                    else
                    {
                        skillLetters[i].SetCurrentLetter((int)currentCombination[i], false);
                    }
                }
            }
        }
        public void DisableAllLetters()
        {
            if(currentSkillCombo == null)
            {
                return;
            }
            Debug.Log("Disabling Letters!");
            foreach(SkillLetterHolder item in skillLetters)
            {
                item.ResetLetter();
            }
            currentSkillCombo = null;
        }
    }
}

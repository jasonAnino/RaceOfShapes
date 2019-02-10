using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using PlayerScripts.UnitCommands;
using ComboSystem;
using Utilities;


namespace UserInterface.Skills
{
    public class VisualSkillHandler : MonoBehaviour
    {
        public GameObject skillOptionPrefab;
        public List<BaseSkillOptions> skillOptions;

        public List<BaseCombo> filteredComboList;

        [SerializeField] private PlayerUnitController playerController;
        public void Start()
        {
            if(PlayerUnitController.GetInstance != null)
            {
                playerController = PlayerUnitController.GetInstance;
            }
            EventBroadcaster.Instance.AddObserver(EventNames.UPDATE_VISUAL_SKILLS,  UpdateVisualSkills);
            EventBroadcaster.Instance.AddObserver(EventNames.RESET_VISUAL_SKILLS, ClearVisualSkills);
        }
        public void OnDestroy()
        {
            EventBroadcaster.Instance.RemoveActionAtObserver(EventNames.UPDATE_VISUAL_SKILLS, UpdateVisualSkills);
            EventBroadcaster.Instance.AddObserver(EventNames.RESET_VISUAL_SKILLS, ClearVisualSkills);
        }
        // 1. Get Player Input
        // 2. Get The Filtered Skills
        // 3. Remove Skills based on Filter
        // 4. Adjust the remaining skills index.
        public void UpdateVisualSkills(Parameters p)
        {
            if(playerController == null)
            {
                return;
            }
            if (playerController.comboComponent.doingCombo != null && playerController.comboComponent.doingCombo.Count > 0)
            {
                Debug.Log("Setting Visual Skills!");
                filteredComboList = playerController.comboComponent.doingCombo;
                SetVisualSkills();
            }
            else
            {
                filteredComboList.Clear();
                ClearAllSkillOptions();
            }

        }

        public void SetVisualSkills()
        {
            for (int i = 0; i < filteredComboList.Count; i++)
            {
                if(skillOptions.Count <= i)
                {
                    return;
                }
                Debug.Log("Setting Index : " + i + " with Skill : " + filteredComboList[i].SkillName);
                skillOptions[i].SetNewCombo(filteredComboList[i]);
            }
        }

        public void ClearVisualSkills(Parameters p)
        {
            filteredComboList.Clear();
            ClearAllSkillOptions();
        }
        private void ClearAllSkillOptions()
        {
            foreach(BaseSkillOptions item in skillOptions)
            {
                item.DisableAllLetters();
            }
        }
    }
}


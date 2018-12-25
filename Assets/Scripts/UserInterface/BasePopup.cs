using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;
using UnitsScripts.Behaviour;
using InteractableScripts.Behavior;
using WorldObjectScripts.Behavior;
using PlayerScripts.UnitCommands;

namespace UserInterface
{

    public class BasePopup : MonoBehaviour
    {
        public TextMeshProUGUI header;
        public List<BasePopupOptions> potentialOptions;
        
        public void SetPotentialOptions(List<ActionType> actions)
        {
            int actionCount = actions.Count;
            for (int i = 0; i < actionCount; i++)
            {
                potentialOptions[i].SetTextHolder(actions[i]);   
            }
        }
    }
}

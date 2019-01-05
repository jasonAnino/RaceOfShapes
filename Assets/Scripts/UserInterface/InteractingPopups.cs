using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnitsScripts.Behaviour;
using PlayerScripts.UnitCommands;
using InteractableScripts.Behavior;
using WorldObjectScripts.Behavior;
using TMPro;

namespace UserInterface
{
    public class InteractingPopups : MonoBehaviour
    {
        private static InteractingPopups instance;
        public static InteractingPopups GetInstance
        {
            get { return instance;  }
        }

        public BasePopup interactWithPopup;

        public List<UnitBaseBehaviourComponent> interactors = new List<UnitBaseBehaviourComponent>();
        public UnitBaseBehaviourComponent interactee;

        public void Awake()
        {
            instance = this;
        }
        public void ShowInteractWithPopup(UnitBaseBehaviourComponent unit, UnitBaseBehaviourComponent interactingWith)
        {
            interactors.Clear();

            interactWithPopup.transform.position = Input.mousePosition;
            interactWithPopup.gameObject.SetActive(true);
            interactors.Add(unit);
            interactWithPopup.SetPotentialOptions(interactingWith.potentialActionTypes);
            interactee = interactingWith;
        }
        public void StartInteractions(ActionType action)
        {
            // Check if the said Action needs you to be near.
            if(interactee.canInteract)
            {
                interactee.InitializeInteraction(action, interactors);
            }
            interactWithPopup.gameObject.SetActive(false);
        }
    }
}

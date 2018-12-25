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

        public InteractingComponent interactor;
        public InteractingComponent interactee;

        public void Awake()
        {
            instance = this;
        }
        public void ShowInteractWithPopup(InteractingComponent unit, InteractingComponent interactingWith)
        {
            interactWithPopup.transform.position = Input.mousePosition;
            interactWithPopup.gameObject.SetActive(true);
        }
    }
}

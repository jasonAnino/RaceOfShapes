using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnitsScripts.Behaviour;
using InteractableScripts.Behavior;
using WorldObjectScripts.Behavior;
using Utilities;

namespace UserInterface
{
    public class UserInterfaceManager : MonoBehaviour
    {
        private static UserInterfaceManager instance;
        public static UserInterfaceManager GetInstance
        {
            get {  return instance; }
        }
        public UIPlayerInGameManager inGameManager;
        public List<UnitBaseBehaviourComponent> fourUnits;

        public void Awake()
        {
            instance = this;
            EventBroadcaster.Instance.AddObserver(EventNames.UPDATE_CONTROLLED_UNITS, AddToUnitControlled);
            EventBroadcaster.Instance.AddObserver(EventNames.REMOVE_CONTROLLED_UNITS, AddToUnitControlled);
        }
        public void Start()
        {

        }

        public void OnDestroy()
        {
            EventBroadcaster.Instance.RemoveActionAtObserver(EventNames.UPDATE_CONTROLLED_UNITS, AddToUnitControlled);
            EventBroadcaster.Instance.RemoveActionAtObserver(EventNames.REMOVE_CONTROLLED_UNITS, RemoveToUnitControlled);
        }

        public void AddToUnitControlled(Parameters p)
        {
            UnitBaseBehaviourComponent check = p.GetWithKeyParameterValue<UnitBaseBehaviourComponent>("UnitControlled", null);
            if (check == null)
            {
                return;
            }
            if (!fourUnits.Contains(check))
            {
                fourUnits.Add(check);
            }
            inGameManager.RefreshCharacterHandlers(fourUnits);
        }

        public void RemoveToUnitControlled(Parameters p)
        {
            UnitBaseBehaviourComponent check = p.GetWithKeyParameterValue<UnitBaseBehaviourComponent>("UnitControlled", null);
            if(check == null)
            {
                return;
            }

            if(fourUnits.Contains(check))
            {
                fourUnits.Remove(check);
            }
            // Check if Player Currently is inGame
            inGameManager.RefreshCharacterHandlers(fourUnits);
        }
    }
}

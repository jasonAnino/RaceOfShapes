using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using UnitsScripts.Behaviour;
using PlayerScripts.UnitCommands;
using Utilities.MousePointer;
using Utilities;

namespace InteractableScripts.Behavior
{
    public enum ObjectType
    {
        Unit = 0,            // Conversation, Ask Problem, Trade, Attack 
        PlayerControlled = 1,  // Conversation, Trade
        WorldObject = 2, 
    }
    public enum ActionType
    {
        Wait = 0,
        Converse = 1,
        Gather = 2,
    }
    public class InteractingComponent : MonoBehaviour
    {
        public ObjectType objectType;
        public List<ActionType> potentialActionTypes = new List<ActionType>();
        public UnitBaseBehaviourComponent interactingUnit;
        // TODO : Create Interaction System
        public virtual void StartConversation()
        {

        }

        public virtual void StartInteraction(InteractingComponent unit, ActionType actionIndex)
        {
            // TODO : 1st thing to do.
            
        }

        public virtual void EndInteraction()
        {

        }

        // RECEIVE DAMAGE - Create a DamageClass that holds : DamageType / Amount / Add'l Buff
        public virtual void ReceiveDamage()
        {

        }
        // RECEIVE BUFF
        public virtual void ReceiveBuff()
        {

        }
        // Check Interaction Requirements
        public virtual void CheckInteractionRequirements(ActionType actionChoice, List<UnitBaseBehaviourComponent> interactors)
        {
            // First check if action choice is inside the potnetialActionTypes
            if(!potentialActionTypes.Contains(actionChoice))
            {
                Debug.LogError("Action Type not within the possible actions, did you pass the wrong type?");
                return;
            }
            // Second Check the distance for every interactors
            if (interactors.Count > 0)
            {
                foreach (UnitBaseBehaviourComponent item in interactors)
                {
                    float dist = Vector3.Distance(item.transform.position, this.transform.position);
                    if (dist < 0.5f)
                    {
                        Debug.Log("Start Cutting!");
                    }
                    else
                    {
                        List<UnitOrder> tmp = new List<UnitOrder>();
                        Debug.Log("Action Choice : " + actionChoice);
                        tmp.Add(UnitOrder.GenerateMoveOrder(transform.position, item));
                        switch (actionChoice)
                        {
                            case ActionType.Converse:
                                tmp.Add(UnitOrder.CreateInteractOrder(this, actionChoice));
                                break;

                            case ActionType.Gather:
                                tmp.Add(UnitOrder.CreateGatherResourceOrder(this, actionChoice));
                                break;
                            case ActionType.Wait:
                                tmp.Add(UnitOrder.CreateInteractOrder(this, actionChoice));
                                break;
                        }

                        PlayerUnitController.GetInstance.OrderManualSelected(tmp[0]);
                        PlayerUnitController.GetInstance.OrderManualSelected(tmp[1], false);
                    }
                }
            }
            else
            {
                Debug.LogError("Sent Wrong list of Interactors! Check it Again!");
            }

        }
        #region Callbacks
        public void OnMouseEnter()
        {
            if (PlayerUnitController.GetInstance.unitSelected.Count > 0 && PlayerUnitController.GetInstance.selectedTeamAffiliation == UnitAffiliation.Controlled)
            {
                CursorManager.GetInstance.CursorChangeTemporary(CursorType.CLICKABLE_NORMAL);
            }
        }

        public void OnMouseOver()
        {
            if (Input.GetMouseButtonDown(1))
            {
                if (PlayerUnitController.GetInstance.manualControlledUnit != null && PlayerUnitController.GetInstance.selectedTeamAffiliation == UnitAffiliation.Controlled)
                {

                }
                else
                {
                    Debug.Log("Clicking tree for no reasons!");
                }
            }
        }
        public void OnMouseExit()
        {
            CursorManager.GetInstance.CursorChangeTemporary(CursorType.NORMAL);
        }
        #endregion
    }
}

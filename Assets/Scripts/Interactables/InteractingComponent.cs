using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using ComboSystem;
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
        Inspect = 3,
        Observe = 4,
        Target = 5,
    }
    public enum InteractionType
    {
        Nearby = 0,
        OnSight = 1,
        SamePath = 2
    }

    public class InteractingComponent : MonoBehaviour
    {
        public ObjectType objectType;
        public List<ActionType> potentialActionTypes = new List<ActionType>();
        public List<UnitBaseBehaviourComponent> interactingUnit;

        public List<PowerEffectComponent> currentBuffs = new List<PowerEffectComponent>();
        public InteractingComponent interactWith;
        public bool canInteract = true;
        public float allowableInteractDistance = 1.5f;
        public virtual void Awake()
        {

        }
        // TODO : Create Interaction System
        public virtual void StartConversation()
        {

        }

        public virtual void StartInteraction(InteractingComponent unit, ActionType actionIndex)
        {
            // Unique
            
        }

        // Adjusted to Require a UnitBaseBehaviourComponent as it now reacts to multiple interactors
        public virtual void EndIndividualInteraction(UnitBaseBehaviourComponent unit)
        {

        }
        public virtual void EndAllInteraction()
        {
            interactingUnit.Clear();
            
        }

        // RECEIVE DAMAGE - Create a DamageClass that holds : DamageType / Amount
        public virtual void ReceiveDamage(float netDamage, UnitBaseBehaviourComponent unitSender)
        {

        }
        public virtual void ReceiveDamage(float netDamage, StatsEffected statsDamaged)
        {

        }
        // RECEIVE BUFF
        public virtual void ReceiveBuff(PowerEffectComponent effect)
        {
            currentBuffs.Add(effect);
        }
        // Check Interaction Requirements
        public virtual void InitializeInteraction(ActionType actionChoice, List<UnitBaseBehaviourComponent> interactors)
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
                       
                        // Made it like this since there are other ActionTypes that does not need to be near.
                        switch (actionChoice)
                        {
                            case ActionType.Converse:
                                tmp.Add(UnitOrder.GenerateMoveOrder(transform.position, item));
                                tmp.Add(UnitOrder.GenerateInteractOrder(this, actionChoice));
                                break;

                            case ActionType.Gather:
                                tmp.Add(UnitOrder.GenerateMoveOrder(transform.position, item));
                                tmp.Add(UnitOrder.GenerateGatherResourceOrder(this, actionChoice));
                                break;
                            case ActionType.Wait:
                                tmp.Add(UnitOrder.GenerateMoveOrder(transform.position, item));
                                tmp.Add(UnitOrder.GenerateInteractOrder(this, actionChoice));
                                break;
                            case ActionType.Target:
                                tmp.Add(UnitOrder.GenerateTargetOrder(this));
                                break;
                        }
                        // Deliver the Orders to the unit involved.
                        for (int i = 0; i < tmp.Count; i++)
                        {
                            if(i <= 0)
                            {
                                PlayerUnitController.GetInstance.OrderManualSelected(tmp[i]);
                            }
                            else
                            {
                                PlayerUnitController.GetInstance.OrderManualSelected(tmp[i], false);
                            }  
                        }
                    }
                }
            }
            else
            {
                Debug.LogError("Sent Wrong list of Interactors! Check it Again!");
            }

        }

        // Use this before you continue with the interaction order
        public bool IsInteractionAllowed(ActionType withThisAction, InteractingComponent interactWith)
        {
            bool checkInteraction = true;

            switch(withThisAction)
            {
                case ActionType.Inspect:
                case ActionType.Gather:
                case ActionType.Converse:
                    // Check if Near
                    float dist = Vector3.Distance(this.transform.position, interactWith.transform.position);
                    Debug.Log("Distance : " + dist);
                    if(dist > allowableInteractDistance)
                    {
                        checkInteraction = false;
                    }
                    else
                    {
                        checkInteraction = true;
                    }
                    break;
                case ActionType.Wait:

                    break;

                case ActionType.Observe:

                    break;
            }

            return checkInteraction;
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

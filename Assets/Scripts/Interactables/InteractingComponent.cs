using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using UnitsScripts.Behaviour;
using PlayerScripts.UnitCommands;
using Utilities.MousePointer;

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
        Converse = 0,
        Cut = 1,
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

        public virtual void StartInteraction(InteractingComponent unit)
        {
            // TODO : 1st thing to do.
        }

        public virtual void EndInteraction()
        {

        }

        // RECEIVE DAMAGE
        public virtual void ReceiveDamage()
        {

        }
        // RECEIVE BUFF
        public virtual void ReceiveBuff()
        {

        }

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
    }
}

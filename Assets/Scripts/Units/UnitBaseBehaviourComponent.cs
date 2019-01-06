using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using PlayerScripts.UnitCommands;
using UnitsScripts.FSM;
using InteractableScripts.Behavior;
using WorldObjectScripts.Behavior;
using Utilities;
using UnitStats;

namespace UnitsScripts.Behaviour
{
    public enum UnitAffiliation
    {
        Neutral = 0,
        Controlled = 1,
        Enemy = 2,
    }
    public enum LivingState
    {
        Dead = 0,
        Alive = 1,
    }
    [RequireComponent(typeof(Rigidbody))]
    public class UnitBaseBehaviourComponent : InteractingComponent
    {
        public string factionName = "Player";
        public UnitAffiliation unitAffiliation = UnitAffiliation.Neutral;
        public Commands currentCommand = Commands.WAIT_FOR_COMMAND;
        public LivingState currentState = LivingState.Alive;

        public Queue<UnitOrder> unitOrders = new Queue<UnitOrder>();
        public UnitOrder currentOrder;
        public Material[] colorCodes;
        public MeshRenderer notifRenderer;
        public GameObject notif;
        public bool canMove = false;

        public Vector3 nextPos;
        public bool startMoving = false;
        public float visualRange = 25.0f;
       
        [SerializeField] public float moveSpeed = 10.0f;
        public CharacterStatsSystem myStats = new CharacterStatsSystem();
        
        public override void Awake()
        {
#if UNITY_EDITOR
            notif.SetActive(true);
            notifRenderer.material = colorCodes[1];
#else
            notif.SetActive(false);
#endif
            // Inject a system here later with regards to loading and saving.
            myStats.InitializeSystem();
        }

        public void Start()
        {
            if(unitAffiliation == UnitAffiliation.Controlled)
            {
                AddToControlledList();
            }
        }
        private void MoveTowards(Vector3 newPos)
        {
            this.nextPos = newPos;
            startMoving = true;
        }

        public override void StartInteraction(InteractingComponent unit,ActionType actionIndex)
        {
            base.StartInteraction(unit, actionIndex);
        }
        #region Callbacks
        public void QueueOrder(UnitOrder thisOrder)
        {
            if(unitOrders.Count > 0)
            {
                if(unitOrders.Peek() != thisOrder)
                {
                    unitOrders.Enqueue(thisOrder);
                }
            }
            else
            {
                unitOrders.Enqueue(thisOrder);
            }
            if(currentOrder == null)
            {
                currentOrder = unitOrders.Dequeue();
            }
        }
        public void ReceiveOrder(UnitOrder newOrder, bool forceOrder = true)
        {
            //Debug.Log("receiving order :" + newOrder.commandName.ToString());
            if(!forceOrder)
            {
                //Debug.Log(this.gameObject.name + " Queueing Order : " + newOrder.commandName);
                QueueOrder(newOrder);
            }
            else
            {
                //Debug.Log(this.gameObject.name +  " Forcing New Order : " + newOrder.commandName);
                unitOrders.Clear();
                currentOrder = newOrder;
            }

            currentOrder.doingOrder = true;
            ActionType nextOrder = ActionType.Wait;
            Vector3 nextPos = transform.position;
            switch (currentOrder.commandName)
            {
                case Commands.INTERACT:
                    // TODO
                    interactWith = newOrder.p.GetWithKeyParameterValue<InteractingComponent>("InteractWith", null);
                    if(interactWith == null)
                    {
                        return;
                    }
                    MakeUnitLookAt(interactWith);

                    nextOrder = newOrder.p.GetWithKeyParameterValue<ActionType>("Action", ActionType.Wait);
                    // Implement Converse / Pull Lever / Open door shit like that ( for NPCs, since player will use POPUPs).
                    break;

                case Commands.MOVE_TOWARDS:
                    RemoveCurrentInteraction();
                    if (!canMove) return;
                    
                    currentCommand = Commands.MOVE_TOWARDS;
                    nextPos = currentOrder.p.GetWithKeyParameterValue<Vector3>("NextPos", transform.position);
                    MoveTowards(nextPos);
                    break;
                case Commands.WAIT_FOR_COMMAND:
                    interactWith = null;
                    currentCommand = Commands.WAIT_FOR_COMMAND;
                    break;

                case Commands.GATHER_RESOURCES:
                    interactWith = newOrder.p.GetWithKeyParameterValue<InteractingComponent>("InteractWith", null);
                    currentCommand = Commands.GATHER_RESOURCES;
                    if (interactWith == null)
                    {
                        return;
                    }
                    MakeUnitLookAt(interactWith);

                    // Start Sending Damage to the tree
                    if(IsInteractionAllowed(ActionType.Gather, interactWith))
                    {
                        interactWith.StartInteraction(this, currentOrder.actionType);
                    }
                    break;
                case Commands.GATHER_ITEMS:
                    nextPos = currentOrder.p.GetWithKeyParameterValue<Vector3>("NextPos", transform.position);
                    interactWith = newOrder.p.GetWithKeyParameterValue<InteractingComponent>("InteractWith", null);
                    currentCommand = Commands.GATHER_ITEMS;
                    MoveTowards(nextPos);
                    MakeUnitLookAt(interactWith);
                    break;
            }
        }
        public float GetUnitBaseDamage()
        {
            // TODO : Check Unit is Equipped with something
            // then add strength to it, for now just return strength.
            //Debug.Log(" Base Damge : " + myStats.GetSpecificStats[Stats.Strength].GetLevel);
            return myStats.GetStats(Stats.Strength).GetLevel;
        }
        public void RemoveCurrentInteraction()
        {
            if(interactWith != null)
            {
                Debug.Log(this.transform.name + " ends Interaction with : " + interactWith.transform.name);
                InteractingComponent tmp = interactWith;
                interactWith = null;
                tmp.EndIndividualInteraction(this);
            }
        }
        public void GatherResources()
        {

        }
        public void MakeUnitLookAt(Vector3 position)
        {
            Vector3 newLookAt = position;
            newLookAt = new Vector3(newLookAt.x, transform.position.y, newLookAt.z);
            transform.LookAt(newLookAt);
        }
        public void MakeUnitLookAt(InteractingComponent unit)
        {
            Vector3 newLookAt = unit.transform.position;
            newLookAt = new Vector3(newLookAt.x, transform.position.y, newLookAt.z);
            transform.LookAt(newLookAt);
        }
        public void AddNewOrder(UnitOrder orderSet)
        {
            unitOrders.Enqueue(orderSet);
        }
        public void AddToControlledList()
        {
            if (InteractablesManager.GetInstance != null)
            {
                if (unitAffiliation == UnitAffiliation.Controlled)
                {
                    InteractablesManager.GetInstance.AddUnitToControlled(this);
                }
            }
        }
        public void InitializeSelected()
        {
            notifRenderer.material = colorCodes[0];
        }

        public void RemoveFromSelected()
        {
            notifRenderer.material = colorCodes[1];
        }

        public void InitializeManualSelected()
        {
            notifRenderer.material = colorCodes[2];
        }
        private void OnDestroy()
        {
            switch(unitAffiliation)
            {
                case UnitAffiliation.Controlled:
                    InteractablesManager.GetInstance.controlledUnits.Remove(this);
                break;
                case UnitAffiliation.Neutral:
                    InteractablesManager.GetInstance.npcs.Remove(this);
                 break;
                case UnitAffiliation.Enemy:
                    InteractablesManager.GetInstance.npcs.Remove(this);
                break;
            }
        }
        #endregion
    }
}

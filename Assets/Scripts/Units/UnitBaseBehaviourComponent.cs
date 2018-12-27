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

namespace UnitsScripts.Behaviour
{
    public enum UnitAffiliation
    {
        Neutral = 0,
        Controlled = 1,
        Enemy = 2,
    }
    [RequireComponent(typeof(Rigidbody))]
    public class UnitBaseBehaviourComponent : InteractingComponent
    {
        public UnitAffiliation unitAffiliation = UnitAffiliation.Neutral;
        public Commands currentCommand = Commands.WAIT_FOR_COMMAND;
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


        public virtual void Awake()
        {
#if UNITY_EDITOR
            notif.SetActive(true);
            notifRenderer.material = colorCodes[1];
#else
            notif.SetActive(false);
#endif

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
        public void ReceiveOrder(UnitOrder newOrder, bool forceOrder = true)
        {
            if(!forceOrder)
            {
                Debug.Log("Queueing Order : " + newOrder.commandName);
                unitOrders.Enqueue(newOrder);
                if(currentOrder == null)
                {
                    currentOrder = unitOrders.Dequeue();
                }
                else
                {
                    return;
                }
            }
            else
            {
                Debug.Log("Forcing New Order : " + newOrder.commandName);
                unitOrders.Clear();
                currentOrder = newOrder;
            }

            currentOrder.doingOrder = true;
            switch(currentOrder.commandName)
            {
                case Commands.INTERACT:
                    // TODO
                    InteractingComponent interactWith = currentOrder.p.GetWithKeyParameterValue<InteractingComponent>("InteractWith", null);
                    if(interactWith == null)
                    {
                        return;
                    }

                    Vector3 newLookAt = interactWith.transform.position;
                    transform.LookAt(newLookAt);
                    newLookAt = new Vector3(newLookAt.x, transform.position.y, newLookAt.z);
                    currentCommand = Commands.INTERACT;
                    currentOrder = null;
                    break;

                case Commands.MOVE_TOWARDS:
                    if (!canMove) return;
                    currentCommand = Commands.MOVE_TOWARDS;
                    Vector3 nextPos = currentOrder.p.GetWithKeyParameterValue<Vector3>("NextPos", transform.position);
                    MoveTowards(nextPos);
                    break;
                case Commands.WAIT_FOR_COMMAND:

                    break;
            }

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

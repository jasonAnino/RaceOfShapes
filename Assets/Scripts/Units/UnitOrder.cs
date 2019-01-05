using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using PlayerScripts.UnitCommands;
using UnitsScripts.FSM;
using InteractableScripts.Behavior;
using WorldObjectScripts.Behavior;
using Utilities;


namespace UnitsScripts.Behaviour
{
    [System.Serializable]
    public class UnitOrder
    {
        public Commands commandName;
        public ActionType actionType;
        public Parameters p = new Parameters();
        public bool doingOrder = false;

        public static UnitOrder CreateGatherResourceOrder(InteractingComponent interactWith, ActionType action)
        {
            UnitOrder tmp = new UnitOrder();
            tmp.commandName = Commands.GATHER_RESOURCES;
            tmp.actionType = ActionType.Gather;
            tmp.p.AddParameter<ActionType>("Action", action);
            tmp.p.AddParameter<InteractingComponent>("InteractWith", interactWith);

            return tmp;
        }
        public static UnitOrder CreateInteractOrder(InteractingComponent interactWith, ActionType action)
        {
            UnitOrder tmp = new UnitOrder();
            tmp.commandName = Commands.INTERACT;
            tmp.p.AddParameter<ActionType>("Action", action);
            tmp.p.AddParameter<InteractingComponent>("InteractWith", interactWith);

            return tmp;
        }

        public static UnitOrder CreateMoveOrder(Vector3 clickedPosition)
        {
            UnitOrder tmp = new UnitOrder();
            tmp.commandName = Commands.MOVE_TOWARDS;
            tmp.p.AddParameter<Vector3>("NextPos", clickedPosition);

            return tmp;
        }

        public static UnitOrder GenerateMoveOrder(Vector3 basePosition, UnitBaseBehaviourComponent unit)
        {
            UnitOrder tmp = new UnitOrder();
            tmp.commandName = Commands.MOVE_TOWARDS;
            tmp.p.AddParameter<Vector3>("NextPos", NavMeshPositionGenerator.GetInstance.GenerateCandidatePosition(basePosition, 0.75f, unit, false));

            return tmp;
        }

        public static UnitOrder GenerateIdleOrder()
        {
            UnitOrder tmp = new UnitOrder();
            tmp.commandName = Commands.WAIT_FOR_COMMAND;
            tmp.doingOrder = true;
            return tmp;
        }

        public static UnitOrder GenerateGetItemOrder(Vector3 basePosition, UnitBaseBehaviourComponent unit, InteractingComponent interactWith)
        {
            UnitOrder tmp = new UnitOrder();
            tmp.commandName = Commands.GATHER_ITEMS;
            tmp.p.AddParameter<Vector3>("NextPos", NavMeshPositionGenerator.GetInstance.GenerateCandidatePosition(basePosition, 0.75f, unit, false));
            tmp.p.AddParameter<InteractingComponent>("InteractWith", interactWith);
            return tmp;
        }
    }
}

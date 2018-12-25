using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UnitsScripts.FSM
{
        public enum State
        {
            IDLE = 0,
            MOVING = 1,
            GATHERING_RESOURCES = 2,
            TRAINING = 3,
        } 
    public class FSMUnits : MonoBehaviour{

        public State currentState = State.IDLE;
        public Dictionary<State, AllowedTransition> possibleTransitions = new Dictionary<State, AllowedTransition>();

        public void ChangeState(State nextState)
        {
            // Check if Transition from this state to the next is allowed
            if(!possibleTransitions[currentState].IsTransitionAllowed(nextState))
            {
                return;
            }

            currentState = nextState;
        }
    }

    public class AllowedTransition
    {
        State thisState;
        public List<State> allowedTransitions = new List<State>();


        public void SetAllowedTransitions(State[] arrayList)
        {
            foreach (State item in arrayList)
            {
                allowedTransitions.Add(item);
            }
        }
        public bool IsTransitionAllowed(State nextState)
        {
            if(allowedTransitions.Contains(nextState))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

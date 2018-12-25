using System.Collections;
using System.Collections.Generic;
using InteractableScripts.Behavior;
using UnityEngine;


using UnitsScripts.Behaviour;
using InteractableScripts.Behavior;
namespace WorldObjectScripts.Behavior
{
    public class Tree : WorldObjectBaseBehaviour
    {


        public override void StartInteraction(InteractingComponent unit)
        {
            if(unit.objectType == ObjectType.PlayerControlled || unit.objectType == ObjectType.Unit)
            {
                interactingUnit = unit.GetComponent<UnitBaseBehaviourComponent>();
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using PlayerScripts.UnitCommands;
using UnitsScripts.FSM;
using InteractableScripts.Behavior;
using WorldObjectScripts.Behavior;
using Utilities;
using UnitStats;

namespace UnitsScripts.Behaviour
{
    /// <summary>
    ///  This is where all the Action Animation events occur
    ///  This is also where the character looks is saved/loaded.
    /// </summary>
    public class CharacterComponent : UnitBaseBehaviourComponent
    {
        public override void Awake()
        {
            base.Awake();

        }
        
    }
}

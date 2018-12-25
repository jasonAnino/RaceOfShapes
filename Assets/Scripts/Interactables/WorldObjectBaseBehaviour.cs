using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnitsScripts.Behaviour;
using Utilities.MousePointer;
using InteractableScripts.Behavior;
using PlayerScripts.UnitCommands;
using UserInterface;
/// <summary>
///  Shows basic non-living interactable objects from Trees to Chests
/// </summary>
namespace WorldObjectScripts.Behavior
{
    /// <summary>
    /// Foundation of all potential non-living intertactable objects 
    /// </summary>
    [RequireComponent(typeof(Animation))]
    public class WorldObjectBaseBehaviour : UnitBaseBehaviourComponent
    {

        public Animation mAnimation;
        public ParticleSystem mParticleSystem;
        public bool canInteract;
        public override void Awake()
        {
            base.Awake();
            Initialize();
        }
        public virtual void Initialize()
        {
            mAnimation = this.GetComponent<Animation>();
        }
        
        public virtual void SpawnReward()
        {

        }
    }
}
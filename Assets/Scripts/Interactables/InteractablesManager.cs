using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnitsScripts.Behaviour;
using InteractableScripts.Behavior;
using WorldObjectScripts.Behavior;


/// <summary>
/// Purpose of InteractablesManager is to add 
/// </summary>
public class InteractablesManager : MonoBehaviour {

    private static InteractablesManager instance;
    public static InteractablesManager GetInstance
    {
        get
        {
            return instance;
        }
    }

    [Header("Controlled Units")]
    public List<UnitBaseBehaviourComponent> controlledUnits = new List<UnitBaseBehaviourComponent>();
    [Header("Interactable Objects")]
    public List<WorldObjectBaseBehaviour> gameObjectSeen = new List<WorldObjectBaseBehaviour>();
    [Header("NPCs")]
    public List<UnitBaseBehaviourComponent> npcs = new List<UnitBaseBehaviourComponent>();

    public void Awake()
    {
        instance = this;
    }

    public void Update()
    {
        
    }

    public void AddUnitToControlled(UnitBaseBehaviourComponent unit)
    {
        controlledUnits.Add(unit);
    }

    public void AddObjectToSeen(WorldObjectBaseBehaviour newObject)
    {
        gameObjectSeen.Add(newObject);
    }

    public void AddNPC(UnitBaseBehaviourComponent unit)
    {
        npcs.Add(unit);
    }
}

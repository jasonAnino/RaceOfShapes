using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitsScripts.Behaviour;

using PlayerScripts.UnitCommands;
using Utilities;

public class UIPlayerInGameManager : MonoBehaviour
{
    public  CharacterHandlers characterHandler;
    public InventoryHandlers inventoryHandler;

    private static UIPlayerInGameManager instance;
    public static UIPlayerInGameManager GetInstance
    {
        get { return instance; }
    }
    public void Awake()
    {
        instance = this;
    }
    
    public void SetInventoryOwner(UnitBaseBehaviourComponent thisUnit)
    {
        if(inventoryHandler == null)
        {
            Debug.Log("STOP!");
            return;
        }
        UnitInventoryBehavior checker = inventoryHandler.inventory.Find(x => x.owner == thisUnit);
        if(checker != null)
        {
            return;
        }
        foreach(UnitInventoryBehavior item in inventoryHandler.inventory)
        {
            
            if(item.owner == null)
            {
                Debug.Log("Setting this as the new owner!");
                item.owner = thisUnit;
                break;
            }
        }
    }
}

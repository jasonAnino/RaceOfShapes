using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnitsScripts.Behaviour;

public class UnitInventoryBehavior : UIBaseBehavior
{
    public InventoryHandlers inventoryManager;
    [Header("INVENTORY OWNER")]
    public UnitBaseBehaviourComponent owner;

}

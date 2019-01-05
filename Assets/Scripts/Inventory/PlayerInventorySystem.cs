using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnitsScripts.Behaviour;
using ItemScript;

[Serializable]
public class ObjectInventory
{
    public UnitBaseBehaviourComponent unitOwner;
    public List<ItemInformation> storage = new List<ItemInformation>();

    public void AddItem(ItemInformation item)
    {
        // Check if Stackable
        ItemInformation tmp = Clone(item);
        if(tmp != null)
        {

            if(storage.Contains(storage.Find(x => x.itemName == tmp.itemName)) && tmp.isStackable)
            {
                int idx = storage.IndexOf(storage.Find(x => x.itemName == tmp.itemName));
                int count = tmp.count;
                storage[idx].count += count;
            }
            else
            {
                storage.Add(tmp);
            }
        }
            else
            {
                storage.Add(item);
            }
    }
    private ItemInformation Clone(ItemInformation item)
    {
        if(item == null)
        {
            return null;
        }
        ItemInformation tmp = new ItemInformation();
        tmp.itemName = item.itemName;
        tmp.count = item.count;
        tmp.id = item.id;
        tmp.isEquipped = item.isEquipped;
        tmp.isStackable = item.isStackable;
        tmp.weight = item.weight;
        return tmp;
    }
}

[Serializable]
public class PlayerInventorySystem : MonoBehaviour
{

    public static PlayerInventorySystem instance;
    public static PlayerInventorySystem GetInstance
    {
        get{ return instance; }
    }

    public List<ObjectInventory> unitInventory = new List<ObjectInventory>();

    public void Awake()
    {
        instance = this;    
        // TODO : initialize Inventory Here once you implement LOAD/SAVE system
    }

    public void AddItemToUnit(ItemInformation item, UnitBaseBehaviourComponent unitHolder)
    {
        string tmp = unitHolder.myStats.id;
        int idx = 0;
        ObjectInventory check = unitInventory.Find(x => x.unitOwner == unitHolder);
        if(check != null)
        {
            if (unitInventory.Contains(check))
            {
                idx = unitInventory.IndexOf(check);

                unitInventory[idx].AddItem(item);
            }
        }
        else
        {
            check = new ObjectInventory();
            unitInventory.Add(check);
            idx = unitInventory.IndexOf(check);
            unitInventory[idx].AddItem(item);
        }
        unitInventory[idx].unitOwner = unitHolder;
    }

    // TODO : IMPLEMENT STORAGE SYSTEM
    public void AddItemToStorage(BaseItemComponent item)
    {

    }
}

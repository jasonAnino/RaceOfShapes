using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ItemScript;
using UnitsScripts.Behaviour;
using PlayerScripts.UnitCommands;

public class ItemDrop : BaseItemComponent
{
   public Material[] materials;
    public MeshRenderer myRenderer;

    public void Start()
    {
        SetMaterialColor();
    }
    public void Update()
    {
        this.transform.Rotate(Vector3.right * 10 * Time.deltaTime);
    }

    public void SetRewardInformation(ItemInformation newInformation)
    {
        itemInfo = newInformation;
    }

    public void SetMaterialColor()
    {
        switch (itemInfo.itemType)
        {
            case ItemType.Ingredient:
                myRenderer.material = materials[0];
                break;

            case ItemType.Weapon:
                myRenderer.material = materials[1];
                break;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if(!other.isTrigger)
        {
            if(other.GetComponent<UnitBaseBehaviourComponent>() != null)
            {
                UnitBaseBehaviourComponent collector = other.GetComponent<UnitBaseBehaviourComponent>();
                if(collector.currentCommand == Commands.GATHER_ITEMS)
                {
                    if(collector.interactWith == this)
                    {
                        Debug.Log("Collector is obtaining this : " + collector.name + " objectName : " + this.gameObject.name);
                        PlayerInventorySystem.GetInstance.AddItemToUnit(itemInfo, collector);
                        GameObject.Destroy(this.gameObject);
                    }
                }
            }
        }
    }
}

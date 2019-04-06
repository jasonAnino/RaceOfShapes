using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitsScripts.Behaviour;

using PlayerScripts.UnitCommands;
using Utilities;
using UserInterface;
using UserInterface.Skills;

public class UIPlayerInGameManager : MonoBehaviour
{
    private static UIPlayerInGameManager instance;
    public static UIPlayerInGameManager GetInstance
    {
        get { return instance; }
    }

    public  CharacterHandlers characterHandler;
    public InventoryHandlers inventoryHandler;
    public StatsHandlers statHandler;
    public VisualSkillHandler visualSkillHandler;
    public List<UnitBaseBehaviourComponent> fourUnits;
    public void Awake()
    {
        instance = this;
        EventBroadcaster.Instance.AddObserver(EventNames.UPDATE_CONTROLLED_UNITS, AddToUnitControlled);
        EventBroadcaster.Instance.AddObserver(EventNames.REMOVE_CONTROLLED_UNITS, AddToUnitControlled);
    }

    public void OnDestroy()
    {
        EventBroadcaster.Instance.RemoveActionAtObserver(EventNames.UPDATE_CONTROLLED_UNITS, AddToUnitControlled);
        EventBroadcaster.Instance.RemoveActionAtObserver(EventNames.REMOVE_CONTROLLED_UNITS, RemoveToUnitControlled);
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
                //Debug.Log("Setting this as the new owner!");
                item.owner = thisUnit;
                break;
            }
        }
    }
    public void SetStatsOwner(UnitBaseBehaviourComponent thisUnit)
    {
        if (statHandler == null)
        {
            Debug.Log("STOP!");
            return;
        }
        UnitStatsBehavior checker = statHandler.unitStats.Find(x => x.owner == thisUnit);

        if (checker != null)
        {
            return;
        }
        foreach (UnitStatsBehavior item in statHandler.unitStats)
        {

            if (item.owner == null)
            {
                //Debug.Log("Setting this as the new owner!");
                item.owner = thisUnit;
                item.InitializeStats();
                break;
            }
        }
    }
    public void AddToUnitControlled(Parameters p)
    {
        bool isInitialSetup = false;
        UnitBaseBehaviourComponent check = p.GetWithKeyParameterValue<UnitBaseBehaviourComponent>("UnitControlled", null);

        //Debug.Log("AddToUnitControlled Initialize : " + check.name);
        if (check == null)
        {
            return;
        }

        if(check.unitAffiliation == UnitAffiliation.Player)
        {
            AdjustUnitListDueToPlayer(check);
        }
        else
        {
            if (fourUnits.Count <= 0)
            {
                //Debug.Log("Meow Mic Test");
                isInitialSetup = true;
                PlayerUnitController.GetInstance.unitSelected.Add(check);
                PlayerUnitController.GetInstance.CheckAndSetManualUnit(0);
            }

            if (!fourUnits.Contains(check))
            {
                fourUnits.Add(check);
            }
        }
            SetInventoryOwner(check);
            SetStatsOwner(check);
            characterHandler.RefreshCharacterHandlers(fourUnits);
    }
    public void AdjustUnitListDueToPlayer(UnitBaseBehaviourComponent thisPlayer)
    {
        if(fourUnits != null)
        {
            List<UnitBaseBehaviourComponent> tmp = new List<UnitBaseBehaviourComponent>();
            tmp.Add(thisPlayer);
            foreach (UnitBaseBehaviourComponent item in fourUnits)
            {
                tmp.Add(item);
            }
            fourUnits = tmp;
        }
        else
        {
            fourUnits.Add(thisPlayer);
        }
    }

    public void RemoveToUnitControlled(Parameters p)
    {
        UnitBaseBehaviourComponent check = p.GetWithKeyParameterValue<UnitBaseBehaviourComponent>("UnitControlled", null);
        if (check == null)
        {
            return;
        }

        if (fourUnits.Contains(check))
        {
            fourUnits.Remove(check);
        }
        // Check if Player Currently is inGame
        characterHandler.RefreshCharacterHandlers(fourUnits);
    }
}

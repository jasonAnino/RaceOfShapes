using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using PlayerScripts.UnitCommands;
using Utilities;
using UnitsScripts.Behaviour;

public class CharacterHandlers : MonoBehaviour
{
    public GameObject unitInfoPanel;
    public List<CharacterInfoHandler> characterHandlers;
    public int handledUnitCount = 0;
    public CharacterExpHandler manualCharacterExpHandler;


    public void AddUnitToHandler(UnitBaseBehaviourComponent unit)
    {
        CharacterInfoHandler handler = characterHandlers.Find(x => x.unitStats == unit.myStats);
        if (handler != null)
        {
            return;
        }
        foreach (CharacterInfoHandler item in characterHandlers)
        {
            if (item.unitStats != null)
            {
                item.Initialize(unit.myStats);
                break;
            }
        }
    }

    public void RemoveUnitToHandler(UnitBaseBehaviourComponent unit)
    {
        CharacterInfoHandler handler = characterHandlers.Find(x => x.unitStats == unit.myStats);
        if (handler == null)
        {
            return;
        }
        foreach (CharacterInfoHandler item in characterHandlers)
        {
            if (item.unitStats == unit.myStats)
            {
                item.ClearHandler();
                break;
            }
        }
        CountCurrentUnits();
    }

    public void CountCurrentUnits()
    {
        handledUnitCount = 0;
        foreach (CharacterInfoHandler item in characterHandlers)
        {
            if (item.unitStats != null)
            {
                handledUnitCount += 1;
            }
        }
    }

    public void RefreshCharacterHandlers(List<UnitBaseBehaviourComponent> newSet)
    {
        /*for (int i = 0; i < characterHandlers.Count; i++)
        {
            characterHandlers[i].ClearHandler();
        }*/
        for (int i = 0; i < newSet.Count; i++)
        {
            if (characterHandlers.Find(x => x.unitStats == newSet[i].myStats))
            {
                continue;
            }

            characterHandlers[i].gameObject.SetActive(true);
            characterHandlers[i].Initialize(newSet[i].myStats);
            if (PlayerUnitController.GetInstance != null && PlayerUnitController.GetInstance.manualControlledUnit != null)
            {
                if (newSet[i] == PlayerUnitController.GetInstance.manualControlledUnit)
                {
                    UpdateManualAutoUnits(characterHandlers[i]);
                }
            }
        }
        for (int i = 0; i < characterHandlers.Count; i++)
        {
            if (string.IsNullOrEmpty(characterHandlers[i].unitStats.name))
            {
                characterHandlers[i].gameObject.SetActive(false);
            }
        }

    }

    public void SetNewManualUnitControlled(Parameters p)
    {
        UnitBaseBehaviourComponent unitToRepresent = p.GetWithKeyParameterValue<UnitBaseBehaviourComponent>("ManualUnit", null);
        if (unitToRepresent == null)
        {
            return;
        }

        foreach (CharacterInfoHandler item in characterHandlers)
        {
            if (item.unitStats == unitToRepresent.myStats)
            {
                UpdateManualAutoUnits(item);
            }
        }
    }
    public void UpdateManualAutoUnits(CharacterInfoHandler thisHandler)
    {
        foreach (CharacterInfoHandler item in characterHandlers)
        {
            if (item == thisHandler)
            {
                item.SetAsManualUnit();
                if (manualCharacterExpHandler != null)
                {
                    manualCharacterExpHandler.UpdateExperience(item.unitStats.curExperience, item.unitStats.maxExperience);
                }
            }
            else
            {
                item.SetAsAutoUnit();
            }
        }
    }
}

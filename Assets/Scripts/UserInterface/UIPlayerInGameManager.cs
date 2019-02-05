using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitsScripts.Behaviour;

public class UIPlayerInGameManager : MonoBehaviour
{
    public GameObject unitInfoPanel;
    public List<CharacterInfoHandler> characterHandlers;
    public int handledUnitCount = 0;

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
            if(item.unitStats != null)
            {
                handledUnitCount += 1;
            }
        }
    }

    public void RefreshCharacterHandlers(List<UnitBaseBehaviourComponent> newSet)
    {
        for(int i = 0; i < characterHandlers.Count; i++)
        {
            characterHandlers[i].ClearHandler();
        }
        for(int i = 0; i < newSet.Count; i++)
        {
            characterHandlers[i].Initialize(newSet[i].myStats);
        }
        for(int i = 0; i < characterHandlers.Count; i++)
        {
            if(characterHandlers[i].unitStats == null)
            {
                characterHandlers[i].gameObject.SetActive(false);
            }
        }
    }
}

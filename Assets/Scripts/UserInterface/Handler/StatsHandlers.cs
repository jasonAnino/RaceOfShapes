using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using PlayerScripts.UnitCommands;
using Utilities;
using UnitStats;
using UnitsScripts.Behaviour;
using UserInterface;

public class StatsHandlers : MonoBehaviour
{
    public List<UnitStatsBehavior> unitStats;
    public LayoutSort sorter;

    public void SwapUnitStats(UnitBaseBehaviourComponent setThis)
    {
        if(unitStats == null)
        {
            Debug.Log("stat list is null");
            return;
        }
        if(PlayerUnitController.GetInstance.manualControlledUnit == null)
        {
            Debug.Log("Manual Unit is null");
            return;
        }
        if(unitStats.Contains(unitStats.Find(x => x.owner == setThis)))
        {
            Debug.Log("Its inside!");
            Transform statsTransform = unitStats.Find(x => x.owner == setThis).transform;
            if(statsTransform != null)
            {
                sorter.SetToFirst(statsTransform);
            }
        }
    }
}

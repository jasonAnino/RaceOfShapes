using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using PlayerScripts.UnitCommands;
using Utilities;
using UnitStats;
using UnitsScripts.Behaviour;
using UserInterface;

namespace UserInterface
{
    public class UnitStatsHandler : MonoBehaviour
    {
        public UnitBaseBehaviourComponent statsOwner;
        public UnitStatsBehavior statsBehavior;
        public UnitCraftingStatsBehavior craftingStatsBehavior;
        
        public void Initialize(UnitBaseBehaviourComponent newOwner)
        {
            statsOwner = newOwner;
            statsBehavior.owner = statsOwner;
            statsBehavior.InitializeStats();
            craftingStatsBehavior.owner = statsOwner;
            craftingStatsBehavior.InitializeStats();
            
        }

        public void SwitchTab(int idx)
        {
            switch(idx)
            {
                case 0:
                    statsBehavior.gameObject.SetActive(true);
                    craftingStatsBehavior.gameObject.SetActive(false);
                    break;
                case 1:
                    statsBehavior.gameObject.SetActive(false);
                    craftingStatsBehavior.gameObject.SetActive(true);
                    break;
                case 2:

                    break;
            }
        }
    }
}

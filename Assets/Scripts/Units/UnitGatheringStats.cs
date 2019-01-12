using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnitsScripts.Behaviour;
using ItemScript;

namespace UnitStats
{
    public class UnitGatheringResourceStats
    {
        public float unitDamage_C = 0.0f;
        public float dmgInterval_C = 5.0f;
        public bool dataInitialized = false;

        public float curTime = 0.0f;
        public UnitBaseBehaviourComponent unitSaved;
        Attack atkType;
        public Attack GetAtkType
        {
            get { return this.atkType; }
            set { this.atkType = value; }
        }

        public void ClearData()
        {
            unitDamage_C = 0.0f;
            dmgInterval_C = 0.0f;
            atkType = null;
        }

        public void SetInterval(UnitBaseBehaviourComponent unit, float baseInterval = 10.0f)
        {
            float reduction = unit.myStats.GetStats(Stats.Strength).GetLevel / 10;
            dmgInterval_C = baseInterval - reduction;
            unitSaved = unit;
        }
    }
}

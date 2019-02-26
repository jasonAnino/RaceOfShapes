using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UnitStats
{
    [Serializable]
    public class CharacterStatsSystem : UnitStatsSystem
    {
        public float stamina_C = 100;
        public float stamina_M = 100;
        public float mana_C = 100;
        public float mana_M = 100;
        public float speed = 3.0f;
        public string title = "Commoner";

        public int level;
        public float curExperience;
        public float maxExperience;
    }
}

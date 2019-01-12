using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnitStats
{
    public class Attack
    {
        public AttackType attackType;
        public Stats statsBuff;
        public float stats;
        public float baseDamage;

        public Attack CreateData(AttackType atkType, Stats buffType, int statLevel, float baseDmg)
        {
            Attack tmp = new Attack();

            tmp.attackType = atkType;
            statsBuff = buffType;
            stats = statLevel;
            baseDamage = baseDmg;

            return tmp;
        }
        /// <summary>
        /// Obtain Raw Damage base from :
        /// Sk. Base Damage + (Sk. Base Damage * Stats/100)
        /// </summary>
        /// <returns></returns>
        public float RawDamage()
        {
            float tmp = baseDamage + (baseDamage * stats / 100);
            return tmp;
        }
    }
}

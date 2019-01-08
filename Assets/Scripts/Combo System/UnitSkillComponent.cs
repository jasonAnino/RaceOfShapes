using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ComboSystem;
using UnitStats;
using UnitsScripts.Behaviour;
public class UnitSkillComponent : MonoBehaviour
{
    public UnitBaseBehaviourComponent owner;
    public List<BaseCombo> buff = new List<BaseCombo>();

    public void Start()
    {
        buff = SkillManager.GetInstance.ObtainBuffCombos(owner);
    }
}

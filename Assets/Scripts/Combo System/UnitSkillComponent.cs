using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ComboSystem;
using UnitStats;
using UnitsScripts.Behaviour;
public class UnitSkillComponent : MonoBehaviour
{
    public UnitBaseBehaviourComponent owner;
    [Header("POTENTIAL SKILLS")]
    public List<BaseCombo> buff = new List<BaseCombo>();
    public List<BaseCombo> fireMagic = new List<BaseCombo>();

    public void Start()
    {
        buff = SkillManager.GetInstance.ObtainBuffCombos(owner);
        fireMagic = SkillManager.GetInstance.ObtainFireMagicCombos(owner);
    }
}

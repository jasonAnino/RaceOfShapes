using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ComboSystem;
using UnitStats;
using UnitsScripts.Behaviour;

[Serializable]
public class SkillManager : MonoBehaviour
{
    private static SkillManager instance;
    public static SkillManager GetInstance
    {
        get { return instance; }
    }

    public void Awake()
    {
        instance = this;
    }
    public List<BaseCombo> buffCombos = new List<BaseCombo>();
    public List<BaseCombo> ObtainBuffCombos(UnitBaseBehaviourComponent unit)
    {
        List<BaseCombo> availableCombo = new List<BaseCombo>();
        availableCombo = unit.mySkills.buff;
        UnitStatsSystem stat = unit.myStats;

        foreach (BaseCombo item in buffCombos)
        {
            if(!availableCombo.Contains(item))
            {
                // Check Requirement
                if(AnalyzeThisSkillRequirement(item, stat))
                {
                    availableCombo.Add(item);
                }
            }
        }

        return availableCombo;
    }
    public List<BaseCombo> fireMagicCombos = new List<BaseCombo>();
    public List<BaseCombo> ObtainFireMagicCombos(UnitBaseBehaviourComponent unit)
    {
        List<BaseCombo> fireCombo = new List<BaseCombo>();
        fireCombo = unit.mySkills.fireMagic;
        UnitStatsSystem stat = unit.myStats;

        foreach (BaseCombo item in fireMagicCombos)
        {
            if (!fireCombo.Contains(item))
            {
                // Check Requirement
                if (AnalyzeThisSkillRequirement(item, stat))
                {
                    fireCombo.Add(item);
                }
            }
        }

        return fireCombo;
    }
    public bool AnalyzeThisSkillRequirement(BaseCombo skill, UnitStatsSystem unitStats)
    {
        bool hasPassed = true;

        foreach (ComboRequirement item in skill.requirements)
        {
            if(hasPassed)
            {
                if(!item.CheckRequiredStats(unitStats))
                {
                    hasPassed = false;
                    break;
                }
            }

        }

        return hasPassed;
    }
}

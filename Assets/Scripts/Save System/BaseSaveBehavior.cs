using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnitsScripts.Behaviour;
using WorldObjectScripts.Behavior;
using InteractableScripts.Behavior;
using Utilities;
using Utilities.MousePointer;
using UserInterface;
using PlayerScripts.CameraController;
using ItemScript;
using ComboSystem;
using SkillBehaviour;
using PlayerScripts.UnitCommands;
using UnitsScripts.FSM;
using UnitStats;
using System;
using ES3Types;


public class BaseSaveBehavior : MonoBehaviour
{
    public string filePath;
    public void Start()
    {
        filePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Magnum\\Saved Characters\\";
    }
    public void  SaveControlledUnitStats()
    {
        foreach(UnitBaseBehaviourComponent item in PlayerUnitController.GetInstance.unitSelected)
        {
            SaveUnitStats(item.myStats);
        }
    }

    public void LoadControlledUnitStats()
    {
        
    }

   public void SaveUnitStats(CharacterStatsSystem stats)
    {
        string uniquePathName = filePath + "\\" + stats.name + "\\ stats";
        Debug.Log("Saving Stats of : " + stats.name + " To Directory : " + uniquePathName);
        if(ES3.KeyExists(stats.name))
        {
            ES3.DeleteFile(stats.name);
        }
        if(ES3.KeyExists(stats.name, uniquePathName))
        {
            ES3.DeleteFile(uniquePathName);
        }
        ES3.Save<CharacterStatsSystem>(stats.name, stats, uniquePathName);
    }

    // Later On change Character Name to UNIQUE UUID.
    public CharacterStatsSystem LoadUnitStats(string characterName)
    {
        CharacterStatsSystem tmp = ES3.Load<CharacterStatsSystem>(characterName);
        return tmp;
    }
}

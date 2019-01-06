using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnitStats;


using UnitsScripts.Behaviour;
using PlayerScripts.UnitCommands;
using TMPro;

public class IdentityPanel : MonoBehaviour
{
    public TextMeshProUGUI unitName;
    public TextMeshProUGUI unitTitle;
    public TextMeshProUGUI unitRace;

    public void SetUnitIdentity(UnitBaseBehaviourComponent unit)
    {
        CharacterStatsSystem stats = unit.myStats;

        unitName.text ="Name: " + stats.name;
        unitTitle.text = "Title: " + "Commoner";
        unitRace.text = "Race: " + "Cube";
    }
    public void ClearUnitIdentity()
    {
        unitName.text = "Name: ";
        unitTitle.text = "Title: ";
        unitRace.text = "Race: ";
    }
}

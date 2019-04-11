using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnitsScripts.Behaviour;

using PlayerScripts.UnitCommands;
using Utilities;
using ItemScript;
using TMPro;
using UnitStats;

[System.Serializable]
public class StatsBonusHolder
{
    public NumericalStats numericalStats;
    public Sprite bonusStatIcon;
    public Color iconColor = new Color();
}
public class UIStatsBonusHolder : MonoBehaviour
{
    public NumericalStats statsBonusStats;
    public List<StatsBonusHolder> possibleBonusStats;
    public Image buffImage;
    public TextMeshProUGUI countText;
}

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
    public NumericalStats numericalStats;
    public Image buffImage;
    public TextMeshProUGUI countText;
    public bool activated = false; // Filter Numerical Stats
}

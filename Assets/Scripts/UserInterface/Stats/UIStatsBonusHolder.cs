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

public class UIStatsBonusHolder : MonoBehaviour
{
    public NumericalStats statsBonusStats;
    public List<Sprite> buffSprites;
    public Image buffImage;
    public TextMeshProUGUI countText;
}

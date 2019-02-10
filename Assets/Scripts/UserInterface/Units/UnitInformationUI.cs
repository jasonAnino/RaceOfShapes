using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnitStats;

using UnitsScripts.Behaviour;
using PlayerScripts.UnitCommands;
using TMPro;
public class UnitInformationUI : MonoBehaviour
{
    public UnitBaseBehaviourComponent currentUnit;
    public Animation mAnim;
    public bool isShown = false;
    #region Unit Stats
    [Header("Unit Stats")]
    public StatsPanel statPanel;
    public GameObject panel;
    public Scrollbar scroll;
    public float scrollSpeed = 5.5f;


    //TO DO: Improve how it changes size depending on the list it has.
    public void ChangeStatsPanelPosition()
    {

    }
    #endregion
    #region Unit Identity
    [Header("Unit Identity")]
    public IdentityPanel identityPanel;
    #endregion
    public void Update()
    {
        // if a unit is  selected
        if (PlayerUnitController.GetInstance.manualControlledUnit != null)
        {
            currentUnit = PlayerUnitController.GetInstance.manualControlledUnit;
            statPanel.StatsInitialize();
            identityPanel.SetUnitIdentity(currentUnit);
        }
        // if no Unit is selected
        else if (currentUnit != null)
        {
            statPanel.disableAllStats();
            identityPanel.ClearUnitIdentity();
            currentUnit = null;
        }
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            if(isShown)
            {
                mAnim.Play("Stats_Close");
                isShown = false;
            }
            else
            {
                mAnim.Play("Stats_Open");
                isShown = true;
            }
        }
    }
}

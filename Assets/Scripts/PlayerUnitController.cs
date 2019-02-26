using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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

namespace PlayerScripts.UnitCommands
{
    public enum Commands
    {
        WAIT_FOR_COMMAND = 0,
        MOVE_TOWARDS = 1,
        INTERACT = 2,
        GATHER_RESOURCES = 3,
        GATHER_ITEMS = 4,
        TARGET = 5,
    }

    public class PlayerUnitController : MonoBehaviour
    {
        private static PlayerUnitController instance;
        public static PlayerUnitController GetInstance
        {
            get { return instance; }
        }
        [Header("Player Command")]
        public UnitAffiliation selectedTeamAffiliation = UnitAffiliation.Neutral;
        public List<UnitBaseBehaviourComponent> unitSelected = new List<UnitBaseBehaviourComponent>();
        public UnitBaseBehaviourComponent manualControlledUnit;
        public DebugFormationSpawner debugPositionSpawner;
        public PlayerComboComponent comboComponent;
        public AoeSkillBehaviour targetableProjectile;
        public bool canMove = false;

        [Header("Player Input Checkers")]
        public bool hoverInteraction = false;
        public bool clickInteraction = false;
        public bool clickRightMouse = false;
        private void Awake()
        {
            instance = this;

        }
        void Update()
        {
            // Clicking Behaviour
            if(Input.GetMouseButtonDown(0))
            {
                if (ClickStateManager.GetInstance.currentState != ClickState.Idle)
                    return;
                if(manualControlledUnit != null)
                {
                    if(manualControlledUnit.mySkills.leftHandSkill)
                    {
                        manualControlledUnit.mySkills.skillsOnHand[0].StartSkillCasting();
                        manualControlledUnit.mySkills.skillsOnHand[0] = null;
                        manualControlledUnit.mySkills.leftHandSkill = false;
                    }

                }
                else
                {
                    ClickObject();
                }
            }
            else if(Input.GetMouseButtonUp(0))
            {
            }
            if(Input.GetMouseButtonDown(1))
            {
                if(unitSelected.Count > 0)
                {
                    if(selectedTeamAffiliation == UnitAffiliation.Controlled)
                    {
                        CheckOrder();
                    }
                    clickRightMouse = true;
                }
            }
            else if(Input.GetMouseButtonUp(1))
            {
                clickRightMouse = false;
            }
            // Update Cursor Icon
            UpdateCursor();
            // Swap Manually Controlled Unit
            if(unitSelected.Count > 0)
            {
                SwapMainUnit();
            }
        }
        #region Cursor Update
       public void UpdateCursor()
        {
            // this are all based on priority
            if(manualControlledUnit != null && (manualControlledUnit.mySkills.leftHandSkill || manualControlledUnit.mySkills.rightHandSkill))
            {
                CursorManager.GetInstance.CursorChangeTemporary(CursorType.CLICKABLE_SKILLHOLD);
            }
            else if(hoverInteraction && !clickInteraction)
            {
                CursorManager.GetInstance.CursorChangeTemporary(CursorType.CLICKABLE_NORMAL);
            }
            else if(clickInteraction && hoverInteraction)
            {
                CursorManager.GetInstance.CursorChangeTemporary(CursorType.CLICKABLE_CLICK);
            }
            else if(clickRightMouse)
            {
                CursorManager.GetInstance.CursorChangeTemporary(CursorType.NORMAL_CLICK);
            }
            else
            {

                CursorManager.GetInstance.CursorChangeTemporary(CursorType.NORMAL);
            }
        }
        #endregion
        #region System Functions
        public void CheckOrder()
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                // Interactable Items / Units
                if (hit.transform.GetComponent<UnitBaseBehaviourComponent>())
                {
                   UnitBaseBehaviourComponent interactWith;
                    interactWith = hit.transform.GetComponent<UnitBaseBehaviourComponent>();
                    if (interactWith.objectType == ObjectType.Unit)
                    {
                        if(interactWith.unitAffiliation == UnitAffiliation.Enemy)
                        {
                            InteractingPopups.GetInstance.ShowInteractWithPopup(manualControlledUnit, interactWith);
                        }
                        // Check Relationship with the person clicked
                        // if Enemy you Attack
                        // if neutral, try to spawn Popup to possibly start conversation
                        return;
                    }
                    else if (interactWith.objectType == ObjectType.WorldObject)
                    {
                        if(manualControlledUnit != null)
                        {
                            InteractingPopups.GetInstance.ShowInteractWithPopup(manualControlledUnit, interactWith);
                        }
                        return;
                    }
                }
                else
                {
                    // Non-Unit Clicks (items)
                    if (hit.transform.GetComponent<ItemDrop>())
                    {
                        ItemDrop tmp = hit.transform.GetComponent<ItemDrop>();
                        MoveUnitsTowards(hit);
                        manualControlledUnit.ReceiveOrder(UnitOrder.GenerateGetItemOrder(hit.transform.position, manualControlledUnit, tmp));
                    }
                    else
                    {
                        MoveUnitsTowards(hit);
                    }
                }
            }
        }
        public void MoveUnitsTowards(RaycastHit hit)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                List<Vector3> possiblePositions = NavMeshPositionGenerator.GetInstance.ObtainPositions(unitSelected.Count, hit.point, unitSelected);
                debugPositionSpawner.PlaceVectorBalls(unitSelected.Count, possiblePositions);
                
                for (int i = 0; i < unitSelected.Count; i++)
                {
                    unitSelected[i].ReceiveOrder(UnitOrder.GenerateMoveOrder(possiblePositions[i]));
                }
            }
         }
        
        public void OrderManualSelected(UnitOrder newOrder, bool force = true)
        {
            //Debug.Log("Ordering Manual Selected : " + newOrder.commandName);
            manualControlledUnit.ReceiveOrder(newOrder, force); 
        }
        public void ClickObject()
        {
            if(CursorManager.GetInstance.IsUserInterfaceClicked())
            {
                return;
            }
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (hit.transform.gameObject.GetComponent<UnitBaseBehaviourComponent>())
                {
                    UnitBaseBehaviourComponent tmp = hit.transform.gameObject.GetComponent<UnitBaseBehaviourComponent>();
                    if (unitSelected.Contains(tmp))
                    {
                        return;
                    }
                    else
                    {
                        SelectObject(tmp.unitAffiliation, tmp);
                    }
                }
                else
                {
                    DeinitializeSelectedUnits();
                    unitSelected.Clear();
                }
            }
        }
        public void DeinitializeSelectedUnits()
        {
            foreach (UnitBaseBehaviourComponent item in unitSelected)
            {
                item.RemoveFromSelected();
            }
            unitSelected.Clear();
            manualControlledUnit = null;
            comboComponent.ClearComboList();
        }
        public void InitializeSelectedUnits()
        {
            foreach (UnitBaseBehaviourComponent item in unitSelected)
            {
                item.InitializeSelected();
            }
            if(unitSelected[0].unitAffiliation == UnitAffiliation.Controlled)
            {
                manualControlledUnit = unitSelected[0];
                comboComponent.SetUnitDoingCombo(manualControlledUnit);
                manualControlledUnit.InitializeManualSelected();
            }
        }
        public void SwapMainUnit()
        {
            if(Input.GetKeyDown(KeyCode.Alpha1))
            {
                CheckAndSetManualUnit(0);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                CheckAndSetManualUnit(1);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                CheckAndSetManualUnit(2);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                CheckAndSetManualUnit(3);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                CheckAndSetManualUnit(4);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                CheckAndSetManualUnit(5);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                CheckAndSetManualUnit(6);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha8))
            {
                CheckAndSetManualUnit(7);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha9))
            {
                CheckAndSetManualUnit(8);
            }
        }
        #endregion

        #region Callbacks
        public void SelectObject(UnitAffiliation newAffiliation, UnitBaseBehaviourComponent clickedUnit)
        {
            DeinitializeSelectedUnits();
            unitSelected.Clear();
            selectedTeamAffiliation = newAffiliation;
            unitSelected.Add(clickedUnit);
            InitializeSelectedUnits();
            if (clickedUnit.unitAffiliation == UnitAffiliation.Controlled)
            {
            manualControlledUnit = clickedUnit;
            comboComponent.SetUnitDoingCombo(manualControlledUnit);
            }
        }
        public void SelectObjects(List<UnitBaseBehaviourComponent> groupOfUnits, UnitAffiliation newAffiliation)
        {
            DeinitializeSelectedUnits();
            unitSelected = groupOfUnits;
            InitializeSelectedUnits();
            selectedTeamAffiliation = newAffiliation;
        }
        
        public void CheckAndSetManualUnit(int index)
        {
            if(unitSelected.Count > index)
            {
                if(manualControlledUnit == unitSelected[index])
                {
                    CameraController.CameraController.GetInstance.FocusOnManualSelectedUnit();
                }
                if(unitSelected[index] != null)
                {
                    if(manualControlledUnit != null)
                    {
                        // Set Color to Green
                        manualControlledUnit.InitializeSelected();
                    }

                    manualControlledUnit = unitSelected[index];
                    // Set Color to Blue
                    Parameters p = new Parameters();
                    p.AddParameter<UnitBaseBehaviourComponent>("ManualUnit", manualControlledUnit);
                    UserInterfaceManager.GetInstance.inGameManager.characterHandler.SetNewManualUnitControlled(p);
                    manualControlledUnit.InitializeManualSelected();
                    comboComponent.SetUnitDoingCombo(manualControlledUnit);
                }
                else
                {
                    Debug.Log("Unit Selected is not!");
                }
            }
        }
        #endregion
    }
}

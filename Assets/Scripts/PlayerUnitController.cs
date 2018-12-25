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

namespace PlayerScripts.UnitCommands
{
    public enum Commands
    {
        WAIT_FOR_COMMAND = 0,
        MOVE_TOWARDS = 1,
        INTERACT = 2,
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
        public bool canMove = false;

        private void Awake()
        {
            instance = this;

        }
        void Update()
        {
            // Clicking Behaviour
            if(Input.GetMouseButtonDown(0))
            {
                ClickObject();
                CursorManager.GetInstance.CursorChangeTemporary(CursorType.NORMAL_CLICK);
            }
            else if(Input.GetMouseButtonUp(0))
            {
                CursorManager.GetInstance.CursorChangeTemporary(CursorType.NORMAL);
            }
            if(Input.GetMouseButtonDown(1))
            {
                if(unitSelected.Count > 0)
                {
                    if(selectedTeamAffiliation == UnitAffiliation.Controlled)
                    {
                        CheckOrder();
                    }
                    CursorManager.GetInstance.CursorChangeTemporary(CursorType.NORMAL_CLICK);
                }
            }
            else if(Input.GetMouseButtonUp(1))
            {
                CursorManager.GetInstance.CursorChangeTemporary(CursorType.NORMAL);
            }

            // Swap Manually Controlled Unit
            if(unitSelected.Count > 0)
            {
                SwapMainUnit();
            }
        }
        #region System Functions
        public void CheckOrder()
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                InteractingComponent interactWith;
                if (hit.transform.GetComponent<InteractingComponent>())
                {
                    interactWith = hit.transform.GetComponent<InteractingComponent>();
                    if (interactWith.objectType == ObjectType.Unit)
                    {
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
                    MoveUnitsTowards(hit);
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
                    Parameters p = new Parameters();
                    p.AddParameter<Vector3>("NextPos", possiblePositions[i]);
                    unitSelected[i].ReceiveOrder(Commands.MOVE_TOWARDS, p);
                }
            }
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
        }
        public void InitializeSelectedUnits()
        {
            foreach (UnitBaseBehaviourComponent item in unitSelected)
            {
                item.InitializeSelected();
            }
            manualControlledUnit = unitSelected[0];
            manualControlledUnit.InitializeManualSelected();
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
            manualControlledUnit = clickedUnit;
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
                    // Set Color to Green
                    manualControlledUnit.InitializeSelected();

                    manualControlledUnit = unitSelected[index];
                    // Set Color to Blue
                    manualControlledUnit.InitializeManualSelected();

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

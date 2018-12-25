using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnitsScripts.Behaviour;
using WorldObjectScripts.Behavior;
using PlayerScripts.UnitCommands;

namespace PlayerScripts.UnitCommands
{
    public class RectangleSelectBox : MonoBehaviour {


        private RectangleSelectBox instance;
        public RectangleSelectBox GetInstance
        {
            get { return this;  }
        }
        public RectTransform selectorImage;

        public Vector3 startPos;
        public Vector3 endPos;

        public Vector3 mouseStartPos;
        public Vector3 mouseEndPos;

        private float timer = 0.0f;
        private bool tryingToOpenRectangle = false;
        private bool openRectangle = false;
        private bool hitSomething = false;
        private void Awake()
        {
            instance = this;

        }
        // Use this for initialization
        void Start () {
            selectorImage.gameObject.SetActive(false);

	    }
	
	    // Update is called once per frame
	    void Update () {
		    if(Input.GetMouseButtonDown(0))
            {
                tryingToOpenRectangle = true;
                GetInitialClick();
            }
            if(Input.GetMouseButton(0))
            {
                if(!hitSomething)
                {
                    return;
                }
                if(!openRectangle)
                {
                    if(tryingToOpenRectangle)
                    {
                        timer += Time.deltaTime;
                    }
                    if(timer > 0.05f)
                    {
                        openRectangle = true;
                        selectorImage.gameObject.SetActive(true);
                    }
                }
                else
                {
                    OpenRectangle();
                }

            }
            if(Input.GetMouseButtonUp(0))
            {
                mouseEndPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
                CloseRectangle();
                if(mouseStartPos != mouseEndPos)
                {
                    SelectObjects();
                }
            }
	    }

        void SelectObjects()
        {
            List<UnitBaseBehaviourComponent> selectedUnits = new List<UnitBaseBehaviourComponent>();

            Rect selectRect = new Rect(mouseStartPos.x, mouseStartPos.y, mouseEndPos.x - mouseStartPos.x, mouseEndPos.y - mouseStartPos.y);

            // Controlled Units
            if(InteractablesManager.GetInstance.controlledUnits.Count > 0)
            {
                foreach (UnitBaseBehaviourComponent item in InteractablesManager.GetInstance.controlledUnits)
                {
                    if(selectRect.Contains(Camera.main.WorldToViewportPoint(item.gameObject.transform.position), true))
                    {
                        selectedUnits.Add(item);
                    }
                }
                if (selectedUnits.Count > 0)
                {
                    PlayerUnitController.GetInstance.SelectObjects(selectedUnits, UnitAffiliation.Controlled);
                }
            }
        }
        
        void GetInitialClick()
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity))
            {
                startPos = hit.point;
                mouseStartPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
                hitSomething = true;
            }
        }
        void OpenRectangle()
        {
            endPos = Input.mousePosition;
            Vector3 squareStart = Camera.main.WorldToScreenPoint(startPos);
            squareStart.z = 0;
            Vector3 center = (squareStart + endPos) / 2f;

            float sizeX = Mathf.Abs(squareStart.x - endPos.x);
            float sizeY = Mathf.Abs(squareStart.y - endPos.y);

            selectorImage.position = center;
            selectorImage.sizeDelta = new Vector2(sizeX, sizeY);
        }
        void CloseRectangle()
        {
            selectorImage.gameObject.SetActive(false);
            openRectangle = false;
            tryingToOpenRectangle = false;
            timer = 0.0f;
            selectorImage.sizeDelta = new Vector2(0, 0);
            hitSomething = false;
        }
    }
}

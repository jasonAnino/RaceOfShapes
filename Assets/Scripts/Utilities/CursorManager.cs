using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using InteractableScripts.Behavior;

namespace Utilities.MousePointer
{
    public enum CursorType
    {
        NORMAL = 0,
        NORMAL_CLICK = 1,
        CLICKABLE_NORMAL = 2,
        CLICKABLE_CLICK = 3,
    }
    public class CursorManager : MonoBehaviour
    {
        private static CursorManager instance;
        public static CursorManager GetInstance
        {
            get { return instance; }
        }
        public List<Texture2D> cursorSprites = new List<Texture2D>();
        public CursorMode cursorMode = CursorMode.Auto;
        public Vector2 hotspot = Vector2.zero;
        
        EventSystem eventSystem;
        GraphicRaycaster raycaster;

        public void Awake()
        {
            instance = this;
        }

        public void Start()
        {
            eventSystem = FindObjectOfType<EventSystem>();
            raycaster = FindObjectOfType<GraphicRaycaster>();
        }

        public void CursorChangeTemporary(CursorType nextType)
        {
            switch(nextType)
            {
                case CursorType.NORMAL:
                    Cursor.SetCursor(cursorSprites[0], hotspot, CursorMode.Auto);
                    break;
                case CursorType.CLICKABLE_NORMAL:
                    Cursor.SetCursor(cursorSprites[1], hotspot, CursorMode.Auto);
                    break;
                case CursorType.CLICKABLE_CLICK:

                    break;

                case CursorType.NORMAL_CLICK:
                    Cursor.SetCursor(cursorSprites[2], hotspot, CursorMode.Auto);
                    break;
            }
        }
        public bool IsUserInterfaceClicked()
        {
            bool tmp = false;

            PointerEventData pointer = new PointerEventData(eventSystem);
            pointer.position = Input.mousePosition;

            List<RaycastResult> results = new List<RaycastResult>();
            raycaster.Raycast(pointer, results);

            if (results.Count > 0)
            {
                tmp = true;
            }
            return tmp;
        }
    }
}

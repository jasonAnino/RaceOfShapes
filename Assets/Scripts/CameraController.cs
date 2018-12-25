using System.Collections;
using System.Collections.Generic;

using UnityEngine.EventSystems;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

using PlayerScripts.UnitCommands;
using UnitsScripts.Behaviour;

namespace PlayerScripts.CameraController
{
    public class CameraController : MonoBehaviour, IPointerClickHandler
    {
        private static CameraController instance;
        public static CameraController GetInstance
        {
            get { return instance; }
        }

        [Header("Server Information")]
        [SerializeField] private string playerID = "00001";

        [Header("Camera")]
        [SerializeField]private Camera playerCamera;
        [SerializeField]private GameObject rotationCursor;
        public float mouseScrollSpeed = 10.0f;
        public float cameraRotateSpeed = 10.0f;
        public bool enableEdgeScrolling = true;
        public bool rotateCameraView = false;
        public bool revCameraViewScrolling = false;
        // Computation
        [SerializeField] private Vector3 heldMousePosition = new Vector3();
        public void Awake()
        {
            playerCamera.transform.LookAt(this.transform);
            instance = this;
        }
        // Checking Player Inputs
        private void Update()
        {
            #region Camera_Checkers
            if (Input.GetButtonDown("RotateCameraFreely"))
            {
                PlaceRotationCursor();
            }
            else if (Input.GetButtonUp("RotateCameraFreely"))
            {
                //rotationCursor.SetActive(false);
            }
            if (Input.GetButton("RotateCameraFreely"))
            {
                RotateCameraFreely();
            }
            else if (Input.GetButton("RotateCameraHorizontally"))
            {
                RotateCameraHorizontally();
            }
            else if (enableEdgeScrolling)
            {
                if (IsMouseInsideScreen())
                {
                    return;
                }
                EdgeScrolling();
            }
            #endregion
        }
        
        #region Camera_Related_Functions
        private void PlaceRotationCursor()
        {
            if(rotationCursor == null)
            {
                // Try to instantiate the cursor here, for now let's depend on whats available
            }
            //rotationCursor.SetActive(true);
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 newPos = new Vector3();
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
            newPos = new Vector3(hit.point.x, hit.point.y + 0.25f, hit.point.z);
            }
            //rotationCursor.transform.position = newPos;
        }
        public void FocusOnManualSelectedUnit()
        {
            this.transform.localPosition = PlayerUnitController.GetInstance.manualControlledUnit.transform.position;
        }
        private void RotateCameraFreely()
        {
            playerCamera.transform.LookAt(this.transform.position);

            if(revCameraViewScrolling)
            {
                if (Input.mousePosition.x > Screen.width * 0.75)
                {
                    /* Camera */
                    // playerCamera.transform.Translate(-Vector3.right * (cameraRotateSpeed * Time.deltaTime));
                    /* Camera Stand */
                    transform.Rotate(-Vector3.up * (cameraRotateSpeed * Time.deltaTime));
                }
                else if (Input.mousePosition.x < Screen.width * 0.25)
                {
                    /* Camera */
                    //  playerCamera.transform.Translate(Vector3.right * (cameraRotateSpeed * Time.deltaTime));
                    /* Camera Stand */
                    transform.Rotate(Vector3.up * (cameraRotateSpeed * Time.deltaTime));
                }

                /*
                if (Input.mousePosition.y > Screen.height * 0.75 && !ShouldStopRotation())
                    playerCamera.transform.Translate(-Vector3.up * (cameraRotateSpeed * Time.deltaTime));
                else if (Input.mousePosition.y < Screen.height * 0.25 && !ShouldStopRotation())
                    playerCamera.transform.Translate(Vector3.up * (cameraRotateSpeed * Time.deltaTime));
                    */
            }
            else
            {
                if (Input.mousePosition.x > Screen.width * 0.75)
                {
                    //  playerCamera.transform.Translate(Vector3.right * (cameraRotateSpeed * Time.deltaTime));
                    /* Camera Stand */
                    transform.Rotate(Vector3.up * (cameraRotateSpeed * Time.deltaTime));
                }
                else if (Input.mousePosition.x < Screen.width * 0.25)
                {
                    //  playerCamera.transform.Translate(-Vector3.right * (cameraRotateSpeed * Time.deltaTime));
                    /* Camera Stand */
                    transform.Rotate(-Vector3.up * (cameraRotateSpeed * Time.deltaTime));
                }

                /*
                if (Input.mousePosition.y > Screen.height * 0.75 && !ShouldStopRotation())
                    playerCamera.transform.Translate(Vector3.up * (cameraRotateSpeed * Time.deltaTime));
                else if (Input.mousePosition.y < Screen.height * 0.25 && !ShouldStopRotation())
                    playerCamera.transform.Translate(-Vector3.up * (cameraRotateSpeed * Time.deltaTime));
                    */
            }
            
        }
        private void RotateCameraHorizontally()
        {
            if(playerCamera == null)
            {
                return;
            }

            Vector3 currentCameraAngles = playerCamera.transform.eulerAngles;
            if (Input.GetAxisRaw("RotateCameraHorizontally") > 0)
            {
                playerCamera.transform.eulerAngles = new Vector3(currentCameraAngles.x - cameraRotateSpeed * Time.deltaTime, currentCameraAngles.y, currentCameraAngles.z);
            }
            else if (Input.GetAxisRaw("RotateCameraHorizontally") < 0)
            {
                playerCamera.transform.eulerAngles = new Vector3(currentCameraAngles.x + cameraRotateSpeed * Time.deltaTime, currentCameraAngles.y, currentCameraAngles.z);
            }
        }
        private void EdgeScrolling()
        {
            if (Input.mousePosition.y >= Screen.height * 0.95)
            {
                transform.Translate(Vector3.forward * mouseScrollSpeed * Time.deltaTime);
            }
            else if (Input.mousePosition.y <= Screen.height * 0.05)
            {
                transform.Translate(-Vector3.forward * mouseScrollSpeed * Time.deltaTime);
            }

            if (Input.mousePosition.x >= Screen.width * 0.95)
            {
                transform.Translate(Vector3.right * mouseScrollSpeed * Time.deltaTime);
            }
            else if (Input.mousePosition.x <= Screen.width * 0.05)
            {
                transform.Translate(-Vector3.right * mouseScrollSpeed * Time.deltaTime);
            }
        }

        private bool ShouldStopRotation()
        {
            if(playerCamera.transform.eulerAngles.x < 85)
            {
                return false;
            }
            else
            {

                return true;
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            
        }

        #endregion
        #region callBacks

        // Checkers
        public bool IsMouseInsideScreen()
        {
#if UNITY_EDITOR
            if (Input.mousePosition.x == 0 || Input.mousePosition.y == 0 || Input.mousePosition.x >= Handles.GetMainGameViewSize().x - 1 || Input.mousePosition.y >= Handles.GetMainGameViewSize().y - 1)
            {
                return true;
            }
#else
        if (Input.mousePosition.x == 0 || Input.mousePosition.y == 0 || Input.mousePosition.x >= Screen.width - 1 || Input.mousePosition.y >= Screen.height - 1) {
        return false;
        }
#endif
            else
            {
                return false;
            }
        }
        #endregion


    }
}
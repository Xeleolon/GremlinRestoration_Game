using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LECameraController : MonoBehaviour
{
    //This is the Camera Controller for Level Editor
    [Tooltip("speed camera rotoates around a focus")]
    [SerializeField] private float cameraSpeed = 90;
    [Tooltip("speed camera zoom into a target")]
    [SerializeField] private float zoomSpeed = 1;
    [Tooltip("the focus point which the camera move around")]
    [SerializeField] private Vector3 focus = Vector3.zero;
    private float zoom = 3;
    Vector2 rotation = Vector3.zero;
    public Transform newFocus;

    #region Enable & Disable
    //Input Variables
    private LEInputModule inputControls;
    private InputAction look;
    private InputAction canLook;
    private InputAction scrollWheel;
    private InputAction focusInput;
    private InputAction fire;
    void Awake()
    {
        inputControls = new LEInputModule();
    }
    void OnEnable()
    {
        look = inputControls.Player.Look;
        look.Enable();

        canLook = inputControls.Player.CameraMove;
        canLook.Enable();

        scrollWheel = inputControls.UI.ScrollWheel;
        scrollWheel.Enable();

        focusInput = inputControls.Player.Focus;
        focusInput.Enable();
        focusInput.performed += NewFocus;

        fire = inputControls.Player.Fire;
        fire.Enable();
        fire.performed += changeTargetFocus;

    }

    void OnDisable()
    {
        look.Disable();
        canLook.Disable();
        scrollWheel.Disable();
        focusInput.Disable();
        fire.Disable();
    }
    
    #endregion

    void Update()
    {
        if (canLook.ReadValue<float>() > 0)
        {
            //can move Camera
            //Debug.Log("Holding Look");
            RototeCamera();
        }

        changeZoom();
    }

    void RototeCamera()
    {
        Vector2 lookInput = look.ReadValue<Vector2>();
        rotation.x += lookInput.y * cameraSpeed * Time.deltaTime;
        rotation.y += lookInput.x * cameraSpeed * Time.deltaTime;

        transform.localEulerAngles = new Vector3(rotation.x, rotation.y, 0);
        
    }
    void changeZoom()
    {
        Vector2 inputZoom = scrollWheel.ReadValue<Vector2>();
        if (inputZoom.y != 0)
        {
            float tempZoom = zoom - inputZoom.y * zoomSpeed * Time.deltaTime;
            if (tempZoom <= 0)
            {
                tempZoom = 0;
            }

            zoom = tempZoom;
        }
        transform.position = focus - transform.forward * zoom;
    }

    void NewFocus(InputAction.CallbackContext context)
    {
        if (newFocus != null)
        {
            focus = newFocus.position;
        }
        else
        {
            focus = Vector3.zero;
        }
    }

    void changeTargetFocus(InputAction.CallbackContext context)
    {
        Ray ray;
                
        ray = Camera.main.ScreenPointToRay(look.ReadValue<Vector2>());
                
        RaycastHit hit;
            
        if (Physics.Raycast(ray, out hit, 100))
        {
            Debug.Log("hit collider");
            newFocus = hit.collider.transform;
        }
    }

}

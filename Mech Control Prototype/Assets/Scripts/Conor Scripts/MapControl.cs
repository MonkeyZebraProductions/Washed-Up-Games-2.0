using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class MapControl : MonoBehaviour
{
    public Camera MapCamera;
    public Canvas MapCanvas;
    private PlayerMovementScript _psm;
    public PlayerInput mapInput;
    public CinemachineVirtualCamera AimingCamera;

    public float PanSpeed, RotateSpeed, ZoomSpeed;
    public float PanLimit, RotateLimit, ZoomLimit;

    private bool _mapOpen, _isGlobal,_isZoomIn,_isZoomOut;

    private InputAction Pan;
    private InputAction Rotate;
    private InputAction ZoomIn;
    private InputAction ZoomOut;
    private InputAction LocalGlobalToggle;
    private InputAction OpenMap;
    private InputAction CloseMap;
    private void Awake()
    {
        _psm = FindObjectOfType<PlayerMovementScript>();
        Pan = mapInput.actions["Pan"];
        Rotate = mapInput.actions["Rotate"];
        ZoomIn = mapInput.actions["ZoomIn"];
        ZoomOut = mapInput.actions["ZoomOut"];
        LocalGlobalToggle = mapInput.actions["LocalGlobalToggle"];
        OpenMap = mapInput.actions["OpenMap"];
        CloseMap = mapInput.actions["CloseMap"];
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 AdjustedCameraTransformZ = new Vector3(MapCamera.transform.forward.x, 0, MapCamera.transform.forward.x);
        if (_mapOpen)
        {
            
            MapCanvas.enabled = true;
            _psm.CanMove = false;
            AimingCamera.Priority = 15;
            
        }
        else
        {
            
            MapCanvas.enabled = false;
            _psm.CanMove = true;
            AimingCamera.Priority = 5;
        }

        Vector2 PanInput = Pan.ReadValue<Vector2>();
        Vector3 movement = transform.rotation *  new Vector3(PanInput.x, 0, PanInput.y);
        transform.position += movement* PanSpeed*Time.deltaTime;

        Vector2 RotateInput = Rotate.ReadValue<Vector2>();
        transform.Rotate(new Vector3(RotateInput.y, RotateInput.x, 0f) * RotateSpeed * Time.deltaTime);
        //transform.rotation = new Quaternion(transform.rotation.x, transform.rotation.y, 0f, transform.rotation.w);

        if(_isZoomIn)
        {
            MapCamera.fieldOfView += ZoomSpeed * Time.deltaTime;
        }
        if (_isZoomOut)
        {
            MapCamera.fieldOfView -= ZoomSpeed * Time.deltaTime;
        }

        if(LocalGlobalToggle.triggered)
        {
            _isGlobal = !_isGlobal;
        }
        if(_isGlobal)
        {
            MapCamera.cullingMask = (1 << LayerMask.NameToLayer("LocalMap")) | (1 << LayerMask.NameToLayer("AreaMap"));
        }
        else
        {
            MapCamera.cullingMask = 1 << LayerMask.NameToLayer("LocalMap");
        }
    }

    private void OnEnable()
    {
        OpenMap.performed += OpenCloseMap;
        CloseMap.performed += OpenCloseMap;
        ZoomIn.started += context => ZoomInMap();
        ZoomIn.canceled += context => EndZoomInMap();
        ZoomOut.started += context => ZoomOutMap();
        ZoomOut.canceled += context => EndZoomOutMap();
    }

    private void OnDisable ()
    {
        OpenMap.performed -= OpenCloseMap;
        CloseMap.performed -= OpenCloseMap;
        ZoomIn.started -= context => ZoomInMap();
        ZoomIn.canceled += context => EndZoomInMap();
        ZoomOut.started -= context => ZoomOutMap();
        ZoomOut.canceled += context => EndZoomOutMap();

    }

    private void ZoomInMap()
    {
        _isZoomIn = true;
    }

    private void ZoomOutMap()
    {
        _isZoomOut = true;
    }

    private void EndZoomInMap()
    {
        _isZoomIn = false;
    }

    private void EndZoomOutMap()
    {
        _isZoomOut = false;
    }

    private void OpenCloseMap(InputAction.CallbackContext context)
    {
        _mapOpen = !_mapOpen;
        if(_mapOpen)
        {
            mapInput.SwitchCurrentActionMap("Map");
        }
        else
        {
            mapInput.SwitchCurrentActionMap("Player");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class GunScript : MonoBehaviour
{
    private PlayerInput playerInput;

    private InputAction Aim;
    private InputAction Shoot;
    private InputAction Grapple;

    public GameObject GrappleObject;
    public GrapplingHook _gH,VisibleAnchor;
    private PlayerMovementScript _pMS;
    public bool IsGrappling;

    public CinemachineVirtualCamera AimCamera;
    public int PriorityChanger;
    public Transform SpawnPoint;
    private CharacterController controller;

    private bool _isAiming,_moveToGrapple;
    Vector3 moveVector;
    // Start is called before the first frame update
    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        Aim = playerInput.actions["Aim"];
        Shoot = playerInput.actions["Shoot"];
        Grapple = playerInput.actions["Grapple"];

        controller = GetComponent<CharacterController>();
        _pMS = GetComponent<PlayerMovementScript>();
        
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(SpawnPoint.position, SpawnPoint.forward * 10, Color.green);
        //fires weapon if not grappling
        if (Shoot.triggered && _isAiming !& IsGrappling)
        {
            Debug.Log("Fire");
            IsGrappling = false;
        }

        //fires hook if not shooting
        if (Grapple.triggered && _isAiming)
        {
            IsGrappling = true;
            if(VisibleAnchor == null)
            {
                
                _gH.target = SpawnPoint.position + SpawnPoint.forward * _gH.Length;
                Instantiate(GrappleObject, SpawnPoint.position + SpawnPoint.forward * _gH.SpawnDistance*1.1f, Quaternion.identity);
                VisibleAnchor = FindObjectOfType<GrapplingHook>();
                
            }
            else
            {
                if (VisibleAnchor.IsHooked)
                {
                    //moves the Player if attatched to a point
                    _moveToGrapple = true;
                    moveVector = VisibleAnchor.target - transform.position;
                    _pMS.CanMove = false;
                }
                else
                {
                    //brings hook back
                    VisibleAnchor.target = SpawnPoint.position;
                    _pMS.CanMove = false;
                }
            }
            
            
        }

        

        //destroys hook once it gets close to the player
        if (Vector3.Distance(VisibleAnchor.transform.position, transform.position) <= VisibleAnchor.SpawnDistance)
        {
            Debug.Log("Die");
            Destroy(VisibleAnchor.gameObject);
            IsGrappling = false;
            _moveToGrapple = false;
            _pMS.CanMove = true;
        }
        if (_moveToGrapple)
        {
            controller.Move(moveVector * Time.deltaTime*_gH.GrappleSpeed);
        }
    }

    //Zooms Camera in
    private void StartAim()
    {
        AimCamera.Priority += PriorityChanger;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;
        _isAiming = true;
    }

    //Zooms Camera in
    private void EndAim()
    {
        Cursor.lockState = CursorLockMode.None;
        AimCamera.Priority -= PriorityChanger;
        _isAiming = false;
    }

    //void FireWeapon()
    //{
    //    if(_isAiming)
    //    {
    //        Debug.Log("Fire");
    //    }
    //}
    private void OnEnable()
    {
        Aim.started += _ => StartAim();
        Aim.canceled += _ => EndAim();
        
    }

    private void OnDisable()
    {
        Aim.started -= _ => StartAim();
        Aim.canceled -= _ => EndAim();
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class WeaponSystemController : MonoBehaviour
{

    private PlayerInput playerInput;
    private CharacterController controller;
    private PlayerMovementScript _pMS;

    public LineRenderer weaponLaser;

    private InputAction Aim;
    private InputAction Shoot;
    private InputAction Reload;

    public int PriorityChanger;
    private bool _isAiming;

    public CinemachineVirtualCamera AimCamera;

   
    
    public Canvas AimCanvas;

    public float crosshairOffsetXValue;
    public float crosshairOffsetYValue;

    public int MaxAmmoCount;
    public int currentAmmoCount;
    public float reloadTime;

    public float nextTimeToFire = 0f;
    public float fireRate = 15f;



   


    // Start is called before the first frame update
    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;

        playerInput = GetComponent<PlayerInput>();
        Aim = playerInput.actions["Aim"];
        Shoot = playerInput.actions["Shoot"];
        Reload = playerInput.actions["Reload"];

        controller = GetComponent<CharacterController>();
        _pMS = GetComponent<PlayerMovementScript>();

        weaponLaser = GetComponentInChildren<LineRenderer>();


    }

    // Update is called once per frame
    void Update()
    {
       
       
            WeaponLaser();
    }

    public void WeaponLaser()
    {
        RaycastHit laserhit;
        if(Physics.Raycast(AimCamera.transform.position, AimCamera.transform.forward, out laserhit))
        {
            if(laserhit.collider)
            {
                weaponLaser.SetPosition(0, new Vector3(0.6f,0, laserhit.distance));
            }

            

            else
            {
                weaponLaser.SetPosition(0, new Vector3(0, 0, 5000));
            }
        }
    }

    private void OnEnable()
    {
        Aim.started += _ => StartAim();
        Aim.canceled += _ => EndAim();

        Shoot.performed += _ => FireAction();
        Reload.performed += _ => ReloadWeapon();
    }

   

    private void OnDisable()
    {
        Aim.started -= _ => StartAim();
        Aim.canceled -= _ => EndAim();

        Shoot.canceled -= _ => FireAction();
        Reload.canceled += _ => ReloadWeapon();
    }

    private void StartAim()
    {
        AimCamera.Priority += PriorityChanger;
        
       
        _isAiming = true;
        AimCanvas.enabled = true;

    }

    private void EndAim()
    {
        
        AimCamera.Priority -= PriorityChanger;
        _isAiming = false;
        AimCanvas.enabled = false;
    }

    

    private void FireAction()
    {
        if (Shoot.triggered && _isAiming && currentAmmoCount > 0 && Time.time >= nextTimeToFire)
        {
           
            nextTimeToFire = Time.time + 1f / fireRate;
            currentAmmoCount--;

            Debug.Log("Fire");

            RaycastHit fireRaycastHit;
            Physics.Raycast(AimCamera.transform.position, AimCamera.transform.forward, out fireRaycastHit, 100);
            //Debug.DrawRay(AimCamera.transform.position, AimCamera.transform.forward, Color.black, fireRate, false);
            Debug.Log(fireRaycastHit.transform.name);


            if (fireRaycastHit.collider.tag == "Enemy")
            {
                Destroy(fireRaycastHit.transform.gameObject);
            }

            if (Shoot.triggered && fireRaycastHit.collider.tag == "Trigger1")
            {
                fireRate = 5f;
                MaxAmmoCount = 5;
            }

            if (Shoot.triggered && fireRaycastHit.collider.tag == "Trigger2")
            {
                fireRate = 25f;
                MaxAmmoCount = 25;
            }

            if (Shoot.triggered && fireRaycastHit.collider.tag == "Trigger3")
            {
                fireRate = 50f;
                MaxAmmoCount = 50;
            }

        }

       


           
    }


    private void ReloadWeapon()
    {
        currentAmmoCount = MaxAmmoCount;
    }

}




    


   


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public abstract class WeaponSystemController : MonoBehaviour
{

    [SerializeField]
    protected WeaponScriptableObject WeaponScriptableObject;


    public GameObject laser;
    public Transform muzzle;

    // Controls
    public PlayerInput playerInput;
    public InputAction Aim;
    public InputAction Shoot;
    public InputAction Reload;

    // Third Person Aim
    public int PriorityChanger;
    public bool _isAiming;
    public CinemachineVirtualCamera AimCamera;
    public Canvas AimCanvas;

    // Ammo
    public int MaxAmmoCount;
    public int currentAmmoCount;

    // Reload
    public float reloadTime;

    // FireRate
    public float nextTimeToFire = 0f;
    public float fireRate = 15f;



   


    // Start is called before the first frame update
    public void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;

        playerInput = GetComponent<PlayerInput>();
        Aim = playerInput.actions["Aim"];
        Shoot = playerInput.actions["Shoot"];
        Reload = playerInput.actions["Reload"];

    }

    // Update is called once per frame
    public void Update()
    {
           
    }

   


    public Vector3 ShootingDirection()
    {
        Vector3 targetPos = AimCamera.transform.position + AimCamera.transform.forward * 100;
        targetPos = new Vector3(targetPos.x, targetPos.y,targetPos.z);

        Vector3 direction = targetPos - AimCamera.transform.position;
        return direction.normalized;
    }

    public void WeaponLaser(Vector3 end)
    {
        LineRenderer lr = Instantiate(laser).GetComponent<LineRenderer>();
        lr.SetPositions(new Vector3[2] { muzzle.position, end });

    }


    public void OnEnable()
    {
        Aim.started += _ => StartAim();
        Aim.canceled += _ => EndAim();

        Shoot.performed += _ => FireAction();
        Reload.performed += _ => ReloadWeapon();
    }

   

    public void OnDisable()
    {

        Aim.started -= _ => StartAim();
        Aim.canceled -= _ => EndAim();

        Shoot.canceled -= _ => FireAction();
        Reload.canceled += _ => ReloadWeapon();

    }

    public void StartAim()
    {
        AimCamera.Priority += PriorityChanger;
        
        _isAiming = true;
        AimCanvas.enabled = true;

    }

    public void EndAim()
    {
        
        AimCamera.Priority -= PriorityChanger;
        _isAiming = false;
        AimCanvas.enabled = false;
    }

    private void ReloadWeapon()
    {
        WeaponScriptableObject.currentAmmoCount = WeaponScriptableObject.MaxAmmoCount;
    }

    public abstract void FireAction();
    
        /*
        if (Shoot.triggered && _isAiming && currentAmmoCount > 0 && Time.time >= nextTimeToFire)
        {
           
            nextTimeToFire = Time.time + 1f / fireRate;
            currentAmmoCount--;

            Debug.Log("Fire");

            RaycastHit fireRaycastHit;
            Physics.Raycast(AimCamera.transform.position, AimCamera.transform.forward, out fireRaycastHit, 100);
            Debug.Log(fireRaycastHit.transform.name);


            if (fireRaycastHit.collider.tag == "Enemy")
            {
                Destroy(fireRaycastHit.transform.gameObject);
            }
*/

           
    }


   






    


   


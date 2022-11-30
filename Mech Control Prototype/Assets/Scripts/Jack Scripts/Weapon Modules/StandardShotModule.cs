using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;


public class StandardShotModule : MonoBehaviour
{

    [SerializeField]
    private WeaponScriptableObject WeaponScriptableObject;
    [SerializeField]
    public GrappleSystem grappleSystem;


    // Controls
    public PlayerInput playerInput;
    public InputAction Aim;
    public InputAction Shoot;
    public InputAction Reload;

    // Third Person Aim
    public int PriorityChanger;
    public bool _isAiming;
    public CinemachineVirtualCamera AimCamera;

    public float lastShootTime;

    public GameObject muzzle;

    // Weapon Data
    public int _MaxAmmoCount;
    public int _currentAmmoCount;
    public int _weaponRange;
    public int _bulletsPerShot;
    public int _clipSize;
    public float _weaponSpread;
    public float _fireRate;

    public int Ammo;

    public bool isFiring;

    public LineRenderer lr;

    public WeaponSwitching _WS;

    public Animator WeaponAnims;

    public AudioSource Fire;

    public Color LineColor;

    public RaycastHit WeaponHit;

    public GameObject Projectile;

    void Awake()
    {
        playerInput = GetComponentInParent<PlayerInput>();

        _MaxAmmoCount = WeaponScriptableObject.MaxAmmoCount;
        _currentAmmoCount = WeaponScriptableObject.currentAmmoCount;
        _weaponRange = WeaponScriptableObject.weaponRange;
        _bulletsPerShot = WeaponScriptableObject.bulletsPerShot;
        _weaponSpread = WeaponScriptableObject.weaponSpread;
        _fireRate = WeaponScriptableObject.fireRate;
        _clipSize= WeaponScriptableObject.ClipSize;
        Ammo = _clipSize;

        Aim = playerInput.actions["Aim"];
        Shoot = playerInput.actions["Shoot"];
        Reload = playerInput.actions["Reload"];

        _currentAmmoCount = _MaxAmmoCount;
        lr.sharedMaterial.SetColor("_Color", LineColor);
        _isAiming = _WS.IsAiming;
        //_WS.IsFiring = isFiring;

    }


    public void Update()
    {
        _currentAmmoCount = Mathf.Clamp(_currentAmmoCount, 0, _MaxAmmoCount);

        if(isFiring)
        {
           
           WeaponShoot();
            
        }

        _WS.IsAiming = _isAiming;
        _WS.IsFiring = isFiring;
        _WS.ClipAmmo = Ammo;
        _WS.TotalAmmo = _currentAmmoCount;
        if (_isAiming)
        {
            WeaponLaser();
            Physics.Raycast(muzzle.transform.position, transform.forward, out WeaponHit,10000f);
        }
        
    }

    public void OnEnable()
    {
        Aim.performed += _ => StartAim();
        Aim.canceled += _ => EndAim();

        Shoot.performed += _ => StartShoot();
        Shoot.canceled += _ => EndShoot();

    }

    public void OnDisable()
    {
        Aim.performed -= _ => StartAim();
        Aim.canceled -= _ => EndAim();

        Shoot.performed -= _ => StartShoot();
        Shoot.canceled -= _ => EndShoot();
    }

    public void StartAim()
    {
        AimCamera.Priority += PriorityChanger;
        _isAiming = true;
        

    }

    public void WeaponLaser()
    {

            if (WeaponHit.collider)
            {
                lr.SetPosition(1, new Vector3(0, 0, WeaponHit.distance));
            }
            else
            {
                lr.SetPosition(1, new Vector3(0, 0, 10000f));
            }
        
    }



    public void EndAim()
    {
        AimCamera.Priority -= PriorityChanger;
        _isAiming = false;

        lr.SetPosition(1, new Vector3(0, 0, 0));

    }


    public  void StartShoot()
    {
        isFiring = true;
    }

    public void EndShoot()
    {
        isFiring = false;
        lr.sharedMaterial.SetColor("_Color", LineColor);
    }




    public void RaycastShoot()
    {
        if (_isAiming && !grappleSystem.IsGrappling)
        {

            if(Ammo>0)
            {
                Instantiate(Projectile, muzzle.transform.position, muzzle.transform.rotation);
                WeaponAnims.Play("FireWeapon");
                Fire.Play();
                Ammo--;
            }
            else if (_currentAmmoCount>0)
            {
                Ammo = _clipSize;
                _currentAmmoCount -= _clipSize;
            }
            else
            {
                Debug.Log("No Ammo");
            }
            
        }


    }

    public void WeaponShoot()
    {

        if (Time.time > lastShootTime + _fireRate)
        {
            lastShootTime = Time.time;
            RaycastShoot();
        }

    }


}
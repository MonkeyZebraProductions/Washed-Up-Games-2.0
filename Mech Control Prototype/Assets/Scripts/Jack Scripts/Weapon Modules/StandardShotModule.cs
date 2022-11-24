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
    public float _weaponSpread;
    public float _fireRate;

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

        Aim = playerInput.actions["Aim"];
        Shoot = playerInput.actions["Shoot"];
        Reload = playerInput.actions["Reload"];

        _currentAmmoCount = _MaxAmmoCount;
        lr.sharedMaterial.SetColor("_Color", LineColor);
        _WS.IsAiming = _isAiming;
        _WS.IsFiring = isFiring;

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
        if(_isAiming)
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

    //public Vector3 GetShootingDirection()
    //{
    //    Vector3 targetPosition = muzzle.transform.position + muzzle.transform.forward;
    //    targetPosition = new Vector3(
    //        targetPosition.x + Random.Range(-_weaponSpread, _weaponSpread),
    //        targetPosition.y + Random.Range(-_weaponSpread, _weaponSpread),
    //        targetPosition.z + Random.Range(-_weaponSpread, _weaponSpread)
    //        );

    //    WeaponScriptableObject.direction = targetPosition - muzzle.transform.position;

    //    return WeaponScriptableObject.direction.normalized;

    //}



    public void RaycastShoot()
    {
        if (_isAiming && !grappleSystem.IsGrappling && _currentAmmoCount > 0)
        {
            _currentAmmoCount--;

            //if (WeaponHit.collider)
            //{
            //    if (WeaponHit.collider.gameObject.tag == "Enemy")
            //    {
            //        WeaponHit.transform.gameObject.GetComponent<EnemyUnitHealth>().TakeDamage(1);
            //        Debug.DrawRay(muzzle.transform.position, WeaponHit.point, Color.blue, 5f);
            //        lr.sharedMaterial.SetColor("_Color", Color.blue);
            //    }
            //    else
            //    {
            //        Debug.DrawLine(muzzle.transform.position, WeaponHit.point, Color.red, 5f);
            //        lr.sharedMaterial.SetColor("_Color", Color.red);
            //    }
            //}  
            Instantiate(Projectile, muzzle.transform.position, muzzle.transform.rotation);
                    
                
            
            WeaponAnims.Play("FireWeapon");
            Fire.Play();
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
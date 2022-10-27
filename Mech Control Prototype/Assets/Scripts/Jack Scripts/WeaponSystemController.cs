using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

//New

public  class WeaponSystemController : MonoBehaviour
{
    [SerializeField]
    private WeaponScriptableObject WeaponScriptableObject;
    [SerializeField]
    public GrappleSystem grappleSystem;
    public float fadeDuration = 0.5f;

    // Controls
    public PlayerInput playerInput;
    public InputAction Aim;
    public InputAction Shoot;
    public InputAction Reload;

    // Third Person Aim
    public int PriorityChanger;
    public bool _isAiming;
    public CinemachineVirtualCamera AimCamera;

    private float lastShootTime;

    public GameObject muzzle;

    // Weapon Data
    public int _MaxAmmoCount;
    public int _currentAmmoCount;
    public int _weaponRange;
    public int _bulletsPerShot;
    public float _weaponSpread;
    public float _fireRate;

    public bool isFiring, _CanAim;

    public LineRenderer lr;


    // Start is called before the first frame update
    public void Awake()
    {
        lr.material.SetColor("_Color", Color.green);

        playerInput = GetComponentInParent<PlayerInput>();

        _MaxAmmoCount = WeaponScriptableObject.MaxAmmoCount;
        _currentAmmoCount = WeaponScriptableObject.currentAmmoCount;
        _weaponRange = WeaponScriptableObject.weaponRange;
        _bulletsPerShot = WeaponScriptableObject.bulletsPerShot;
        _weaponSpread = WeaponScriptableObject.weaponSpread;
        _fireRate = WeaponScriptableObject.fireRate;
     
        Cursor.lockState = CursorLockMode.Locked;

<<<<<<< Updated upstream
        //playerInput = GetComponent<PlayerInput>();
=======
     
>>>>>>> Stashed changes
     

        Aim = playerInput.actions["Aim"];
        Shoot = playerInput.actions["Shoot"];
        Reload = playerInput.actions["Reload"];

        _currentAmmoCount = _MaxAmmoCount;

        _CanAim = true;

    }

    public void Update()
    {
        _currentAmmoCount = Mathf.Clamp(_currentAmmoCount, 0, _MaxAmmoCount);

<<<<<<< Updated upstream
        if(_CanAim)
        {
            if (Shoot.IsPressed())
            {
                WeaponShoot();
                //StartCoroutine(Fire(lr));
                isFiring = true;
=======
        if(_isAiming)
        {

        }

        if(!_isAiming)
        {

        }

        if (Shoot.IsPressed())
        {
            WeaponShoot();
            isFiring = true;
>>>>>>> Stashed changes

            }

<<<<<<< Updated upstream
            else
            {
                isFiring = false;
                //StopCoroutine(Fire(lr));
            }
        }    
        
=======
        else
        {
            isFiring = false;
          
        }
>>>>>>> Stashed changes
    }

    public void OnEnable()
    {
        Aim.performed += _ => StartAim();
        Aim.canceled += _ => EndAim();

    }



    public void OnDisable()
    {
        Aim.performed -= _ => StartAim();
        Aim.canceled -= _ => EndAim();
    }

    public void StartAim()
    {
        if(_CanAim)
        {
            AimCamera.Priority += PriorityChanger;

<<<<<<< Updated upstream
            _isAiming = true;
        }
        
   
=======
        _isAiming = true;
>>>>>>> Stashed changes

        //WeaponLaser();
       
    }

    public void EndAim()
    {
<<<<<<< Updated upstream
        if(_CanAim)
        {
            AimCamera.Priority -= PriorityChanger;
            _isAiming = false;
        }
       
   
=======

        AimCamera.Priority -= PriorityChanger;
        _isAiming = false;

        lr.SetPosition(1, new Vector3(0, 0, 0));
>>>>>>> Stashed changes
    }

   


    [SerializeField]
    public Vector3 GetShootingDirection()
    {
        Vector3 targetPosition = muzzle.transform.position + muzzle.transform.forward * 100f;
        targetPosition = new Vector3(
            targetPosition.x + Random.Range(-_weaponSpread, _weaponSpread),
            targetPosition.y + Random.Range(-_weaponSpread, _weaponSpread),
            targetPosition.z + Random.Range(-_weaponSpread, _weaponSpread)
            );

        WeaponScriptableObject.direction = targetPosition - muzzle.transform.position;

        return WeaponScriptableObject.direction.normalized;

    }


    public void WeaponLaser()
    {
        RaycastHit hit2;
        while (Physics.Raycast(muzzle.transform.position, muzzle.transform.forward, out hit2))
        {
            if (hit2.collider)
            {

                lr.SetPosition(1, new Vector3(0, 0, hit2.distance));
            }

            else
            {
                lr.SetPosition(1, new Vector3(0, 0, 100f));
            }
        }
    }




    public void HitBulletTrail(Vector3 end)
    {
        LineRenderer lr = Instantiate(WeaponScriptableObject.hitBulletTrail).GetComponent<LineRenderer>();
        lr.SetPositions(new Vector3[2] { muzzle.transform.position, end });
        StartCoroutine(BulletTrailFade(lr));

    }

    public void MissBulletTrail(Vector3 end)
    {
        LineRenderer lr = Instantiate(WeaponScriptableObject.missBulletTrail).GetComponent<LineRenderer>();
        lr.SetPositions(new Vector3[2] { muzzle.transform.position, end });
        StartCoroutine(BulletTrailFade(lr));
    }


    IEnumerator BulletTrailFade(LineRenderer lr)
    {
        float alpha = 1;
        while (alpha > 0)
        {
            alpha -= Time.deltaTime / fadeDuration;
            lr.startColor = new Color(lr.startColor.r, lr.startColor.g, lr.startColor.b, alpha);
            lr.endColor = new Color(lr.startColor.r, lr.startColor.g, lr.startColor.b, alpha);
            yield return null;
        }

    }

  

    private void ReloadWeapon()
    {
        _currentAmmoCount = _MaxAmmoCount;
    }

    
    public void RaycastShoot()
    {
        if(_CanAim)
        {
<<<<<<< Updated upstream
            if (_isAiming && !grappleSystem.IsGrappling && _currentAmmoCount > 0)
=======
          
            for (int i = 0; i < _bulletsPerShot; i++)
>>>>>>> Stashed changes
            {
                for (int i = 0; i < _bulletsPerShot; i++)
                {
                    _currentAmmoCount--;
                    RaycastHit hit;
                    if (Physics.Raycast(muzzle.transform.position, GetShootingDirection(), out hit, _weaponRange))
                    {
<<<<<<< Updated upstream
                        Debug.DrawRay(muzzle.transform.position, hit.point, Color.green, 5f);
                        HitBulletTrail(hit.point);

                        if (hit.collider.tag == "Enemy")
                        {
                            //Destroy(hit.transform.gameObject);
=======
                        Destroy(hit.transform.gameObject);
                        
>>>>>>> Stashed changes


                        }
                    }

                    else if (Physics.Raycast(muzzle.transform.position, GetShootingDirection(), out hit, Mathf.Infinity))
                    {
                        Debug.DrawLine(muzzle.transform.position, hit.point, Color.red, 5f);
                        MissBulletTrail(hit.point);
                    }
                }
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


   






    


   


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

    public Transform muzzle;

    // Weapon Data
    public int _MaxAmmoCount;
    public int _currentAmmoCount;
    public int _weaponRange;
    public int _bulletsPerShot;
    public float _weaponSpread;
    public float _fireRate;

    public bool isFiring;


    // Start is called before the first frame update
    public void Awake()
    {
        _MaxAmmoCount = WeaponScriptableObject.MaxAmmoCount;
        _currentAmmoCount = WeaponScriptableObject.currentAmmoCount;
        _weaponRange = WeaponScriptableObject.weaponRange;
        _bulletsPerShot = WeaponScriptableObject.bulletsPerShot;
        _weaponSpread = WeaponScriptableObject.weaponSpread;
        _fireRate = WeaponScriptableObject.fireRate;
     
        Cursor.lockState = CursorLockMode.Locked;

        playerInput = GetComponent<PlayerInput>();
     

        Aim = playerInput.actions["Aim"];
        Shoot = playerInput.actions["Shoot"];
        Reload = playerInput.actions["Reload"];

        _currentAmmoCount = _MaxAmmoCount;

    }

    public void Update()
    {
        _currentAmmoCount = Mathf.Clamp(_currentAmmoCount, 0, _MaxAmmoCount);


        if (Shoot.IsPressed())
        {
            WeaponShoot();
            //StartCoroutine(Fire(lr));
            isFiring = true;

        }

        else
        {
            isFiring = false;
            //StopCoroutine(Fire(lr));
        }
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
        AimCamera.Priority += PriorityChanger;

        _isAiming = true;
   

    }

    public void EndAim()
    {

        AimCamera.Priority -= PriorityChanger;
        _isAiming = false;
   
    }

    [SerializeField]
    public Vector3 GetShootingDirection()
    {
        Vector3 targetPosition = AimCamera.transform.position + AimCamera.transform.forward * 100f;
        targetPosition = new Vector3(
            targetPosition.x + Random.Range(-_weaponSpread, _weaponSpread),
            targetPosition.y + Random.Range(-_weaponSpread, _weaponSpread),
            targetPosition.z + Random.Range(-_weaponSpread, _weaponSpread)
            );

        WeaponScriptableObject.direction = targetPosition - AimCamera.transform.position;

        return WeaponScriptableObject.direction.normalized;

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
        if (  _isAiming && !grappleSystem.IsGrappling  && _currentAmmoCount > 0)
        {
            for (int i = 0; i < _bulletsPerShot; i++)
            {
                _currentAmmoCount--;
                RaycastHit hit;
                if (Physics.Raycast(muzzle.transform.position, GetShootingDirection(), out hit, _weaponRange))
                {
                    Debug.DrawRay(muzzle.transform.position, hit.point, Color.green, 5f);
                    HitBulletTrail(hit.point);

                    if (hit.collider.tag == "Enemy")
                    {
                        //Destroy(hit.transform.gameObject);
                        

                    }
                }

                else if(Physics.Raycast(muzzle.transform.position, GetShootingDirection(), out hit, Mathf.Infinity))
                {
                    Debug.DrawLine(muzzle.transform.position, hit.point,Color.red, 5f);
                    MissBulletTrail(hit.point);
                }
            }
        }
    }

   
    public void WeaponShoot()
    {
        //Debug.Log("WeaponShot");
        if (Time.time > lastShootTime + _fireRate)
        {
            lastShootTime = Time.time;
            RaycastShoot();
        }

    }
}


   






    


   


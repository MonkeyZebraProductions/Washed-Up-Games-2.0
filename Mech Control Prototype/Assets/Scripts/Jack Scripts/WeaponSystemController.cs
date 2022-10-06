using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;


public  class WeaponSystemController : MonoBehaviour
{
    PlayerInputManager playerInputManager;
    
    [SerializeField]
    private WeaponScriptableObject WeaponScriptableObject;
    public float fadeDuration = 0.5f;

    // Controls
    public PlayerInput playerInput;
    public InputAction Aim;
    public InputAction Reload;

    // Third Person Aim
    public int PriorityChanger;
    public bool _isAiming;
    public CinemachineVirtualCamera AimCamera;
    public Canvas AimCanvas;

    Coroutine fireCoroutine;

    private float lastShootTime;

  

    public Transform muzzle;

    // Start is called before the first frame update
    public void Awake()
    {
        WeaponScriptableObject.currentAmmoCount = WeaponScriptableObject.MaxAmmoCount;

        playerInputManager = GetComponent<PlayerInputManager>();
        Cursor.lockState = CursorLockMode.Locked;
        playerInput = GetComponent<PlayerInput>();
        Aim = playerInput.actions["Aim"];
        Reload = playerInput.actions["Reload"];

       

        Aim.started += _ => StartAim();
        Aim.canceled += _ => EndAim();

        Reload.performed += _ => StartAim();
     

    }

    public void Update()
    {
        WeaponScriptableObject.currentAmmoCount = Mathf.Clamp(WeaponScriptableObject.currentAmmoCount, 0, WeaponScriptableObject.MaxAmmoCount);


        if(playerInputManager.shoot)
        {
            Debug.Log("shoot");
            WeaponShoot();
        }
    }

    public void OnEnable()
    {
        Aim.Enable();
        Reload.Enable();
        
    }



    public void OnDisable()
    {
        Aim.Disable();
        Reload.Disable();

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

    public Vector3 GetShootingDirection()
    {
        Vector3 targetPosition = AimCamera.transform.position + AimCamera.transform.forward * 100f;
        targetPosition = new Vector3(
            targetPosition.x + Random.Range(-WeaponScriptableObject.weaponSpread, WeaponScriptableObject.weaponSpread),
            targetPosition.y + Random.Range(-WeaponScriptableObject.weaponSpread, WeaponScriptableObject.weaponSpread),
            targetPosition.z + Random.Range(-WeaponScriptableObject.weaponSpread, WeaponScriptableObject.weaponSpread)
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
        WeaponScriptableObject.currentAmmoCount = WeaponScriptableObject.MaxAmmoCount;
    }

    
    public void RaycastShoot()
    {
        if (  _isAiming && WeaponScriptableObject.currentAmmoCount > 0)
        {
            for (int i = 0; i < WeaponScriptableObject.bulletsPerShot; i++)
            {
                WeaponScriptableObject.currentAmmoCount--;
                RaycastHit hit;
                if (Physics.Raycast(AimCamera.transform.position, GetShootingDirection(), out hit, WeaponScriptableObject.weaponRange))
                {
                    Debug.DrawLine(muzzle.transform.position, hit.point, Color.green, 5f);
                    HitBulletTrail(hit.point);

                    if (hit.collider.tag == "Enemy")
                    {
                        Destroy(hit.transform.gameObject);
                    }
                }

                else if(Physics.Raycast(AimCamera.transform.position, GetShootingDirection(), out hit, Mathf.Infinity))
                {
                    Debug.DrawLine(muzzle.transform.position, hit.point,Color.red, 5f);
                    MissBulletTrail(hit.point);
                }
            }
        }
    }

   
    private void WeaponShoot()
    {
        Debug.Log("WeaponShot");
        if (Time.time > lastShootTime + WeaponScriptableObject.fireRate)
        {
            lastShootTime = Time.time;
            RaycastShoot();
        }




    }
}


   






    


   


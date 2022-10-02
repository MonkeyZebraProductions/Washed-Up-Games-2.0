using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public abstract class WeaponSystemController : MonoBehaviour
{

    
    [SerializeField]
    protected WeaponScriptableObject WeaponScriptableObject;
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
    public Canvas AimCanvas;

    

   



   


    // Start is called before the first frame update
    public void Awake()
    {
        WeaponScriptableObject.currentAmmoCount = WeaponScriptableObject.MaxAmmoCount;
        WeaponScriptableObject.readyToShoot = true;
        Cursor.lockState = CursorLockMode.Locked;
        playerInput = GetComponent<PlayerInput>();
        Aim = playerInput.actions["Aim"];
        Shoot = playerInput.actions["Shoot"];
        Reload = playerInput.actions["Reload"];

    }

    // Update is called once per frame
    public void Update()
    {
        WeaponScriptableObject.currentAmmoCount = Mathf.Clamp(WeaponScriptableObject.currentAmmoCount,0,WeaponScriptableObject.MaxAmmoCount);
    }

   public IEnumerator HoldDownFireAction()
    {
        while(true)
        {
            FireAction();
            yield return new WaitForSeconds(1 / WeaponScriptableObject.fireRate);
        }
    }

    public void HitBulletTrail(Vector3 end)
    {
        LineRenderer lr = Instantiate(WeaponScriptableObject.hitBulletTrail).GetComponent<LineRenderer>();
        lr.SetPositions(new Vector3[2] { WeaponScriptableObject.weaponMuzzle.transform.position, end });
        StartCoroutine(BulletTrailFade(lr));

    }

    public void MissBulletTrail(Vector3 end)
    {
        LineRenderer lr = Instantiate(WeaponScriptableObject.missBulletTrail).GetComponent<LineRenderer>();
        lr.SetPositions(new Vector3[2] { WeaponScriptableObject.weaponMuzzle.transform.position, end });
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

    public abstract void ResetShot();
    
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


   






    


   


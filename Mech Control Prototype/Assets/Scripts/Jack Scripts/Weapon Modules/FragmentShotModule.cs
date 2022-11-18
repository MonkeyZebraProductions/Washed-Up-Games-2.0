using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class FragmentShotModule : MonoBehaviour
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

    public Transform muzzle;

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


    // Start is called before the first frame update
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
    }

    // Update is called once per frame
    void Update()
    {
        _currentAmmoCount = Mathf.Clamp(_currentAmmoCount, 0, _MaxAmmoCount);

        if (Shoot.IsPressed())
        {
            //WeaponShoot();
            isFiring = true;
        }

        else
        {
            isFiring = false;
            lr.sharedMaterial.SetColor("_Color", Color.green);
        }

        _WS.IsAiming = _isAiming;
        _WS.IsFiring = isFiring;
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
        WeaponLaser();

    }

    public void WeaponLaser()
    {
        /*

        RaycastHit hit2;
        if (Physics.Raycast(muzzle.transform.position, GetShootingDirection(), out hit2))
        {
            if (hit2.collider)
            {

                lr.SetPosition(1, new Vector3(0, 0, hit2.distance));
            }

            else
            {
                lr.SetPosition(1, new Vector3(0, 0, Mathf.Infinity));
            }
        }
        */
    }



    public void EndAim()
    {
        AimCamera.Priority -= PriorityChanger;
        _isAiming = false;

        lr.SetPosition(1, new Vector3(0, 0, 0));

    }

    public void WeaponShoot()
    {

        if (Time.time > lastShootTime + _fireRate)
        {
            lastShootTime = Time.time;
            RaycastShoot();
        }

    }

    public Vector3 WeaponSpread()
    {
        Vector3 spread = muzzle.position + muzzle.forward * 1000f;
        spread += Random.Range(-_weaponSpread, _weaponSpread) * muzzle.up;
        spread += Random.Range(-_weaponSpread, _weaponSpread) * muzzle.right;
        spread -= muzzle.position;
        spread.Normalize();
        return spread;
    }

    public void RaycastShoot()
    {

        if (_isAiming && !grappleSystem.IsGrappling && _currentAmmoCount > 0)
        {
            for (int i = 0; i < _bulletsPerShot; i++)
            {
                _currentAmmoCount--;
                RaycastHit hit;
                if (Physics.Raycast(muzzle.position, WeaponSpread(), out hit, _weaponRange))
                {
                    Debug.DrawRay(muzzle.position, hit.point, Color.blue, 5f);
                    lr.sharedMaterial.SetColor("_Color", Color.blue);

                    if (hit.collider.tag == "Enemy")
                    {
                        hit.transform.gameObject.GetComponent<EnemyUnitHealth>().TakeDamage(1);
                    }
                }

                else if (Physics.Raycast(muzzle.position, WeaponSpread(), out hit, Mathf.Infinity))
                {
                    Debug.DrawLine(muzzle.position, hit.point, Color.red, 5f);
                    lr.sharedMaterial.SetColor("_Color", Color.red);
                }
            }
        }


    }

}

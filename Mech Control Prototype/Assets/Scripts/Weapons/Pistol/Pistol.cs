using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Pistol : WeaponSystemController
{
   public override void FireAction()
    {
        if (Shoot.triggered && _isAiming && WeaponScriptableObject.currentAmmoCount > 0 && Time.time >= nextTimeToFire)
        {

            nextTimeToFire = Time.time + 1f / fireRate;
            WeaponScriptableObject.currentAmmoCount--;

            Debug.Log("Fire");

           
            for (int i = 0; i < WeaponScriptableObject.bulletsPerShot; i++)
            {
                 RaycastHit fireRaycastHit;
                Vector3 shootingDirection = ShootingDirection();
                if (Physics.Raycast(AimCamera.transform.position, AimCamera.transform.forward, out fireRaycastHit, 100))
                {
                    Debug.Log(fireRaycastHit.transform.name);
                    WeaponLaser(fireRaycastHit.point);

                    if (fireRaycastHit.collider.tag == "Enemy")
                    {
                        Destroy(fireRaycastHit.transform.gameObject);
                    }

                   
                }

                else
                    {
                        WeaponLaser(AimCamera.transform.position + shootingDirection  * 100);
                    }


            }
                
           

            
        }

        
    }

   
    void Awake()
    {
        base.Awake();
    }

   
    void Update()
    {
        base.Update();
    }

   
}
        

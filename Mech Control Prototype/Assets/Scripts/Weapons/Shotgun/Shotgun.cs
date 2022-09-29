using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shotgun : WeaponSystemController
{
    public override void FireAction()
    {
        if(Shoot.triggered && _isAiming && WeaponScriptableObject.currentAmmoCount > 0 )
        {
            WeaponScriptableObject.currentAmmoCount -- ;

            for (int i = 0; i < WeaponScriptableObject.bulletsPerShot; i++)
            {
                RaycastHit shotgunHit;
                if(Physics.Raycast(AimCamera.transform.position,AimCamera.transform.position, out shotgunHit, 100))
                {
                    WeaponLaser(shotgunHit.point);
                }

                else
                {
                     
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

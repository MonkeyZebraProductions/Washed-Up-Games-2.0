using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickUp : MonoBehaviour
{
    [SerializeField]
    private WeaponScriptableObject weaponScriptableObject;

    private void OnTriggerEnter(Collider other)
    {
        WeaponManager weaponManager = other.GetComponent<WeaponManager>();

        if(weaponManager !=null)
        {
            weaponManager.EquipWeapon(weaponScriptableObject);
            Destroy(gameObject);
        }
    }
}

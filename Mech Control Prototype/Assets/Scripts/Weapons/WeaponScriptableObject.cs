using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapon Scriptable Object", order = 1)]
public class WeaponScriptableObject : ScriptableObject
{
    public string weaponName;

    // Ammo
    public int MaxAmmoCount;
    public int currentAmmoCount;


    public int bulletsPerShot;




}


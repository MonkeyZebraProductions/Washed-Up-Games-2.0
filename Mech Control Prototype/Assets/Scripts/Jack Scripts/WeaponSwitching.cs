using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponSwitching : MonoBehaviour
{
    public int weaponSlotSelected = 1;
    public GameObject firstWeaponSlot, secondWeaponSlot;
    public PlayerInput playerInput;
    public InputAction weaponSwitchingOneKey;
    public InputAction weaponSwitchingTwoKey;

    // Start is called before the first frame update
    void Awake()
    {
        playerInput = GetComponentInParent<PlayerInput>();
        weaponSwitchingOneKey = playerInput.actions["OneKey"];
        weaponSwitchingTwoKey = playerInput.actions["TwoKey"];
        SwapWeapon(1);

    }

    // Update is called once per frame
    void Update()
    {
        if(weaponSwitchingOneKey.IsPressed())
        {
            if(weaponSlotSelected !=1)
            {
                SwapWeapon(1);
            }
        }

        if(weaponSwitchingTwoKey.IsPressed())
        {
            if (weaponSlotSelected != 2)
            {
                SwapWeapon(2);
            }
        }
    }

    void SwapWeapon(int weaponType)
    {
        if(weaponType == 1)
        {
            weaponSlotSelected = 1;

            firstWeaponSlot.SetActive(true);
            secondWeaponSlot.SetActive(false);
        }

        if(weaponType == 2)
        {
            weaponSlotSelected = 2;

            firstWeaponSlot.SetActive(false);
            secondWeaponSlot.SetActive(true);
        }
}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponSwitching : MonoBehaviour
{
    public int weaponSlotSelected = 1;

    public GameObject firstWeaponSlotOPrefab, secondWeaponSlotPrefab;

    public PlayerInput playerInput;

    public InputAction weaponSwitchingOneKey, weaponSwitchingTwoKey;

    //public WeaponSystemController firstWeaponSlot, secondWeaponSlot;

    public GrappleSystem grappleSystem;
    public Punch punch;

    public bool IsAiming,IsFiring;

    // Start is called before the first frame update
    void Awake()
    {
        playerInput = GetComponentInParent<PlayerInput>();
        weaponSwitchingOneKey = playerInput.actions["OneKey"];
        weaponSwitchingTwoKey = playerInput.actions["TwoKey"];

        SwapWeapon(1);
        //SwapGrapple(1);
        //SwapPunch(1);
    }

    // Update is called once per frame
    void Update()
    {
        if(weaponSwitchingOneKey.IsPressed())
        {
            if(weaponSlotSelected != 1)
            {
                SwapWeapon(1);
                //SwapGrapple(1);
                //SwapPunch(1);
            }
        }

        if(weaponSwitchingTwoKey.IsPressed())
        {
            if (weaponSlotSelected != 2)
            {
                SwapWeapon(2);
                //SwapGrapple(2);
                //SwapPunch(2);
            }
        }
    }

    //void SwapPunch(int punchWeaponType)
    //{
    //    if (punchWeaponType == 1)
    //    {
    //        punch._wSC = firstWeaponSlot;
    //    }

    //    if (punchWeaponType == 2)
    //    {
    //        punch._wSC = secondWeaponSlot;
    //    }
    //}

    //void SwapGrapple(int grappleWeaponType)
    //{
    //    if (grappleWeaponType == 1)
    //    {
    //        grappleSystem.weaponSystemController = firstWeaponSlot;
    //    }

    //    if (grappleWeaponType == 2)
    //    {
    //        grappleSystem.weaponSystemController = secondWeaponSlot;
    //    }
    //}


    void SwapWeapon(int weaponType)
    {
        if(weaponType == 1)
        {
            weaponSlotSelected = 1;

            firstWeaponSlotOPrefab.SetActive(true);
            secondWeaponSlotPrefab.SetActive(false);
        }

        if(weaponType == 2)
        {
            weaponSlotSelected = 2;

            firstWeaponSlotOPrefab.SetActive(false);
            secondWeaponSlotPrefab.SetActive(true);
        }
}
}

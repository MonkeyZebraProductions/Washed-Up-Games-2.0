using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    public bool shoot;

    public void OnShoot(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            shoot = true;
        }

        if(context.performed)
        {
            shoot = true;
        }

        if(context.canceled)
        {
            shoot = false;
        }
    }
}

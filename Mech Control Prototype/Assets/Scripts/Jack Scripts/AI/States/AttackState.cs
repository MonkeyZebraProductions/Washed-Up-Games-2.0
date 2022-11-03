using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
    public RetreatState retreatState;
    public FOV fov;

    public override State RunCurrentState()
    {
        // After attack coroutine enemy retreats to distance.

        return this;
    }
}

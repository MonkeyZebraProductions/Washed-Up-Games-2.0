using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : State
{
    public AttackState attackState;
    public FOV fov;

    public override State RunCurrentState()
    {
        if (fov.inAttackRange)
        {
            return attackState;
        }
        else
        {
            return this;
        }
    }
}
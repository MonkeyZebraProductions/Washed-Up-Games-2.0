using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    public ChaseState chaseState;
    public FOV fov;

    public override State RunCurrentState()
    {
      if(fov.canSeePlayer)
        {
            return chaseState;
        }

      else
        {
            return this;
        }
    }
}

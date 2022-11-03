using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetreatState : State
{
    public ChaseState chaseState;
    public FOV fov;
    public override State RunCurrentState()
    {
        //After retreat if player still in line of sightm Repeat Chase, Attack and Retreat States
        return this;
    }
}

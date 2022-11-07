using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState2 : BasicState2
{
    public FOV fov;

    public override void Enter()
    {

    }

    public override void Think()
    {
        if(fov.canSeePlayer)
        {
            stateMachine.GetComponent<Pursue2>().enabled = true;
        }

        else if (fov.inAttackRange)
        {
            stateMachine.GetComponent<AttackState2>().enabled = true;
        }

    }

    public override void Exit()
    {

    }
}

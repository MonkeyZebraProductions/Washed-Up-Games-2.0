using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PursueState2 : BasicState2
{
    public FOV fov;
    public override void Enter()
    {
        stateMachine.GetComponent<Pursue2>().enabled = true;
    }

    public override void Think()
    {
        if(fov.inAttackRange)
        {
            stateMachine.GetComponent<AttackState2>().enabled = true;
        }
    }

    public override void Exit()
    {
        stateMachine.GetComponent<Pursue2>().enabled = false;
    }
}

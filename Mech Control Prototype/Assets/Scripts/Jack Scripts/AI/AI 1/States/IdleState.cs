using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : BaseState
{
    public override void Enter()
    {
        stateMachine.GetComponent<Arrive>().enabled = false;


    }

    public override void Think()
    {
        if(stateMachine.fOV.canSeePlayer)
        {
            stateMachine.ChangeState(new PursueState());
        }

        if(stateMachine.fOV.inAttackRange)
        {
            stateMachine.ChangeState(new AttackState());
        }


    }

    public override void Exit()
    {
        stateMachine.GetComponent<Arrive>().enabled = true;
    }
}

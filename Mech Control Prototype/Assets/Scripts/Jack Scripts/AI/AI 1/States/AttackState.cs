using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : BaseState
{
    public override void Enter()
    {
        stateMachine.GetComponent<Arrive>().enabled = false;
    }

    public override void Think()
    {
        if (stateMachine.fOV.canSeePlayer)
        {
            stateMachine.ChangeState(new PursueState());
        }
    }

    public override void Exit()
    {
      
    }
}

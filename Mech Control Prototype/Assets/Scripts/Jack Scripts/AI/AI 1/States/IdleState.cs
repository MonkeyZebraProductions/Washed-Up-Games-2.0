using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IdleState : BaseState
{
   

    public override void Enter()
    {
      


    }

    public override void UpdateLogic()
    {

    }
    public override void UpdatePhysics() { }
    /*
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
    */
    public override void Exit()
    {
        stateMachine.GetComponent<Arrive>().enabled = false;
    }
}

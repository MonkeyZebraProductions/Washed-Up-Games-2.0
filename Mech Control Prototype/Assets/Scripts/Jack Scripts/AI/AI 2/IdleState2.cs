using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState2 : BaseState2
{
    

    public override void Enter() 
    {

    }
    public override void Think()
    {
        if(stateMachine.fOV.canSeePlayer)
        {
            stateMachine.ChangeState( new PursueState2());
        }
    }
    public override void Exit() 
    {

    }
}

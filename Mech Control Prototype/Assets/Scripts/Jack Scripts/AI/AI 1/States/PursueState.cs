using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PursueState : BaseState
{

  

    public override void Enter()
    {
     
        stateMachine.GetComponent<Arrive>().enabled = true;

    }

    public override void UpdateLogic() { }
    public override void UpdatePhysics() { }
    /*
    public override void Think()
    {
        

       if(stateMachine.fOV.inAttackRange)
        {
            stateMachine.ChangeState(new AttackState());
        }

       if(!stateMachine.fOV.canSeePlayer & !stateMachine.fOV.inAttackRange)
        {
            stateMachine.ChangeState(new IdleState());
            //stateMachine.ChangeState(new WanderState());
        }
    }
    */
    public override void Exit()
    {
        stateMachine.GetComponent<Arrive>().enabled = false;
    }

}

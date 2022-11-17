using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PursueState2 : BaseState2
{
    public override void Enter()
    {
        stateMachine.GetComponent<Pursue2>().enabled = true;
    }

    public override void Think()
    {

    }

    public override void Exit()
    {
        stateMachine.GetComponent<Pursue2>().enabled = false;
    }
}

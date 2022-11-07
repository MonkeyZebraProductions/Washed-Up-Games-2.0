using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetreatState2 : BasicState2
{
    public override void Enter()
    {
        stateMachine.GetComponent<Retreat2>().enabled = true;
    }

    public override void Think()
    {

    }

    public override void Exit()
    {
        stateMachine.GetComponent<Retreat2>().enabled = false;
    }
}

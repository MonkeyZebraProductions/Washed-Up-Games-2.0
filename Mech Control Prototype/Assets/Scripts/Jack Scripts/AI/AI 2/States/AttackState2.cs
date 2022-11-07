using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState2 : BasicState2
{
    public FOV fov;
    public override void Enter()
    {
        stateMachine.GetComponent<AttackState2>().enabled = true;
    }

    public override void Think()
    {
       //When attack is done find retreat


    }

    public override void Exit()
    {
        stateMachine.GetComponent<AttackState2>().enabled = false;
    }
}

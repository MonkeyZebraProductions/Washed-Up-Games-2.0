using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    [SerializeField]
    State currentstate;

    private void Update()
    {
        RunStateMachine();
    }

    private void RunStateMachine()
    {
        State nextState = currentstate?.RunCurrentState();

        if(nextState != null)
        {
            SwitchToNextState(nextState);
        }
    }


    private void SwitchToNextState(State nextState)
    {
        currentstate = nextState;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine2 : MonoBehaviour
{

    public BasicState2 currentState;
    public BaseState globalState;
    public BaseState previousState;

    private IEnumerator coroutine;
    public int updatesPerSecond = 5;


}

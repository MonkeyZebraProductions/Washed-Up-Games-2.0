using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  abstract class BaseState2 : MonoBehaviour
{
    public StateMachine2 stateMachine;
    public virtual void Enter() { }
    public virtual void Think() { }
    public virtual void Exit() { }
}
   

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState : MonoBehaviour
{
    public StateMachine stateMachine;
  
    public virtual void Enter() { }
    public virtual void Think() { }
    public virtual void Exit() { }
}

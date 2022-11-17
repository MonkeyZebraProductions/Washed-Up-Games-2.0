using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class BaseState : MonoBehaviour
{
    protected StateMachine stateMachine;

    public virtual void Enter() { }
    public virtual void UpdateLogic() { }
    public virtual void UpdatePhysics() { }
    public virtual void Exit() { }
}

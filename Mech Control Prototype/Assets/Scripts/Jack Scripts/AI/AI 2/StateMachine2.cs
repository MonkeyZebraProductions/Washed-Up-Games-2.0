using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine2 : MonoBehaviour
{
    public FOV fOV;

    public BaseState2 currentState;
    public BaseState2 globalState;
    public BaseState2 previousState;

    public IdleState2 idlestate;

    private IEnumerator coroutine;
    public int updatesPerSecond = 5;

    // Start is called before the first frame update
    void Start()
    {
        GetInitalState();
      
    }

    private void OnEnable()
    {
        StartCoroutine(Think());
        Debug.Log("Thinking");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GetInitalState()
    {
        currentState = idlestate;
        Debug.Log("Idle State Inital");

    }

    public void ChangeStateDelayed(BaseState2 newState, float delay)
    {
        coroutine = ChangeStateCoRoutine(newState, delay);
        StartCoroutine(coroutine);
    }

    public void CancelDelayedStateChange()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
    }

    IEnumerator ChangeStateCoRoutine(BaseState2 newState, float delay)
    {
        yield return new WaitForSeconds(delay);
        ChangeState(newState);
    }

    public void RevertToPreviousState()
    {
        ChangeState(previousState);
    }

    public void ChangeState(BaseState2 newState)
    {
        if (currentState != null)
        {
            currentState.Exit();
        }
        if (this.previousState == null || previousState.GetType() != this.previousState.GetType())
        {
            this.previousState = currentState;
        }
        currentState = newState;
        currentState.stateMachine = this;
        Debug.Log(currentState.GetType());
        currentState.Enter();
    }

    IEnumerator Think()
    {
        yield return new WaitForSeconds(Random.Range(0, 0.5f));
        while (true)
        {
            if (globalState != null)
            {
                globalState.Think();
            }
            if (currentState != null)
            {
                currentState.Think();
            }

            yield return new WaitForSeconds(1.0f / (float)updatesPerSecond);
        }
    }


    public void SetGlobalState(BaseState2 state)
    {
        if (globalState != null)
        {
            globalState.Exit();
        }
        globalState = state;
        if (globalState != null)
        {
            globalState.stateMachine = this;
            globalState.Enter();
        }
    }
}

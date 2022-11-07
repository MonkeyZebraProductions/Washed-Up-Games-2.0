using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public FOV fOV;

    public BaseState currentState;
    public BaseState globalState;
    public BaseState previousState;

    private IEnumerator coroutine;
    public int updatesPerSecond = 5;

    // Start is called before the first frame update
    void Start()
    {
        
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

    public void ChangeStateDelayed(BaseState newState, float delay)
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

    IEnumerator ChangeStateCoRoutine(BaseState newState, float delay)
    {
        yield return new WaitForSeconds(delay);
        ChangeState(newState);
    }

    public void RevertToPreviousState()
    {
        ChangeState(previousState);
    }

    public void ChangeState(BaseState newState)
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


    public void SetGlobalState(BaseState state)
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

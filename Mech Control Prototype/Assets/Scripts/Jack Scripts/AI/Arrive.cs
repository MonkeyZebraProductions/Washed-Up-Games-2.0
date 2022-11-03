using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrive : MonoBehaviour
{
    public Vector3 targetPosition;

    SteeringBehaviour steeringBehaviour;

    void Start()
    {
        steeringBehaviour = GetComponent<SteeringBehaviour>();
    }

    void FixedUpdate()
    {
        Vector3 accel = steeringBehaviour.Arrive(targetPosition);

        steeringBehaviour.Steering(accel);
        steeringBehaviour.SteeringDirection();
    }
}
 

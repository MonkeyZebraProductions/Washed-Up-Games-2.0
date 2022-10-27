using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    public float stopRadius;
    public float pathOffset;
    public float pathDirection;

    SteeringBehaviour steeringBehaviour;

    public Rigidbody rb;

     void Awake()
    {
        steeringBehaviour = GetComponent<SteeringBehaviour>();
        rb = GetComponent<Rigidbody>();
    }

    /*
    public Vector3 GetSteering(Path path)
    {
        return GetSteering(path, false);
    }

    public Vector3 GetSteering(Path path, bool pathLoop)
    {
        Vector3 targetPosition;
        return GetSteering(path, pathLoop, out targetPosition);
    }


    public Vector3 GetSteering(Path path, bool pathLoop, out Vector3 targetPosition)
    {
        //return steeringBasics.Arrive(targetPosition);
    }
    */
}

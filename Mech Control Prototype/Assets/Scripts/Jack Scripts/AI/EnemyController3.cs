using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController3 : MonoBehaviour
{
    public Vector3 force = Vector3.zero;
    public Vector3 acceleration = Vector3.zero;
    public Vector3 velocity = Vector3.zero;
    public float mass = 1;

    [Range(0.0f, 10.0f)]
    public float damping = 0.01f;

    [Range(0.0f, 1.0f)]
    public float banking = 0.1f;
    public float maxSpeed = 5.0f;
    public float maxForce = 10.0f;

    public Vector3 target = Vector3.zero;
    public float slowingDistance;
    public float decelleration;
    public float stopDistance;

    void Start()
    {

    }

    public Vector3 ArriveForce()
    {
        Vector3 toTarget = (target - transform.position);

        float distance = toTarget.magnitude;
        Vector3 desired;

        if (distance < stopDistance)
        {
            desired = Vector3.zero;
        }

        if (distance < slowingDistance)
        {
            desired = maxSpeed * (distance / slowingDistance) * (toTarget / distance);
        }
        else
        {
            desired = maxSpeed * (toTarget / distance);
            decelleration = 1;
        }

        return desired - velocity * decelleration;
    }

  
}

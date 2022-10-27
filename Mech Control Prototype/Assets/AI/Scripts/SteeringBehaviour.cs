using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringBehaviour : MonoBehaviour
{
    public float maxVelocity = 3.5f;
    public float maxAcceleration = 10f;
    public float turnSpeed = 20f;
    public float targetRadius = 0.005f;
    public float slowRadius = 1f;
    public float timeToTarget = 0.1f;

    public bool smoothing = true;
    public int numSamplesForSmoothing = 5;
    Queue<Vector3> velocitySamples = new Queue<Vector3>();

    public Rigidbody rb;
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Steering(Vector3 linearAcceleration)
    {
        rb.velocity += linearAcceleration * Time.deltaTime;

        if (rb.velocity.magnitude > maxVelocity)
        {
            rb.velocity = rb.velocity.normalized * maxVelocity;
        }
    }

    public void SteeringDirection()
    {
        Vector3 direction = rb.velocity;

        if(smoothing)
        {
            if (velocitySamples.Count == numSamplesForSmoothing)
            {
                velocitySamples.Dequeue();
            }

            velocitySamples.Enqueue(rb.velocity);

            direction = Vector3.zero;

            foreach (Vector3 v in velocitySamples)
            {
                direction += v;
            }

            direction /= velocitySamples.Count;
        }

        FaceDirection(direction);
    }

    public void FaceDirection(Vector3 direction)
    {
        direction.Normalize();
        if (direction.sqrMagnitude > 0.001f)
        {
            float toRotation = (Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg);
            float rotation = Mathf.LerpAngle(rb.rotation.eulerAngles.y, toRotation, Time.deltaTime * turnSpeed);

            rb.rotation = Quaternion.Euler(0, rotation, 0);
        }
           
    }

    public void FaceDirection(Quaternion toRotation)
    {
        FaceDirection(toRotation.eulerAngles.y);
    }
    
    public void FaceDirection(float toRotation)
    {
        float rotation = Mathf.LerpAngle(rb.rotation.eulerAngles.y, toRotation, Time.deltaTime * turnSpeed);
        rb.rotation = Quaternion.Euler(0, rotation, 0);
    }

    public bool isInFront(Vector3 target)
    {
        return IsFacing(target, 0);
    }

    public bool IsFacing(Vector3 target, float cosineValue)
    {
        Vector3 facing = transform.forward.normalized;

        Vector3 directionToTarget = (target - transform.position);
        directionToTarget.Normalize();

        return Vector3.Dot(facing, directionToTarget) >= cosineValue;
    }


    public static Vector3 OrienatationToVector(float orientation)
    {
        return new Vector3(Mathf.Cos(orientation), 0, Mathf.Sin(orientation));
    }

    public static float VectorToOrientation(Vector3 direction)
    {
        return Mathf.Atan2(direction.x, direction.z);
    }


    public Vector3 Arrive(Vector3 targetPosition)
    {
        Debug.DrawLine(transform.position, targetPosition, Color.cyan, 0f, false);

        Vector3 targetVelocity = targetPosition - rb.position;

        float dist = targetVelocity.magnitude;

        if (dist < targetRadius)
        {
            rb.velocity = Vector3.zero;
            return Vector3.zero;
        }

        float targetSpeed;

        if (dist > slowRadius)
        {
            targetSpeed = maxVelocity;
        }

        else
        {
            targetSpeed = maxVelocity * (dist / slowRadius);
        }

        targetVelocity.Normalize();
        targetVelocity *= targetSpeed;

        Vector3 acceleration = targetVelocity - rb.velocity;

        acceleration *= 1 / timeToTarget;

        if (acceleration.magnitude > maxAcceleration)
        {
            acceleration.Normalize();
            acceleration *= maxAcceleration;
        }

        return acceleration;
    }

    public Vector3 Interpose(Rigidbody target1, Rigidbody target2)
    {
        Vector3 midPoint = (target1.position + target2.position) / 2;

        float timeToReachMidPoint = Vector3.Distance(midPoint, transform.position) / maxVelocity;

        Vector3 futureTarget1Pos = target1.position + target1.velocity * timeToReachMidPoint;
        Vector3 futureTarget2Pos = target2.position + target2.velocity * timeToReachMidPoint;

        midPoint = (futureTarget1Pos + futureTarget2Pos) / 2;

        return Arrive(midPoint);
    }

    public Vector3 Seek(Vector3 targetPosition, float maxSeekAccel)
    {
        Vector3 acceleration = targetPosition - transform.position;
        acceleration.Normalize();
        acceleration *= maxSeekAccel;
        return acceleration;
    }

    public Vector3 Seek(Vector3 targetPosition)
    {
        return Seek(targetPosition,maxAcceleration);
    }
}

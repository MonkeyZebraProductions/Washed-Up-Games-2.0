using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController2 : MonoBehaviour
{
    public EnemyControllerSettings settings;

    public Vector3 position;
    public Vector3 forward;
    public Vector3 velocity;

    public Transform cachedTransform;
    public Transform target;

    public Vector3 steeringForce;

   


    public void Initialize(EnemyControllerSettings settings, Transform target)
    {
        this.target = target;
      
       
        this.settings = settings;
        position = cachedTransform.position;
        forward = cachedTransform.forward;

        float startSpeed = settings.maxSpeed / 2;
        velocity = transform.forward * startSpeed;
     
    }

    // Start is called before the first frame update
    void Awake()
    {
        cachedTransform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        steeringForce = Caculate();
    }

    public Vector3 ArriveForce()
    {
        Vector3 desiredVelocity = (target.position - position);
        float distance = desiredVelocity.magnitude;

        if (distance < settings.targetRadius)
        {
            desiredVelocity = Vector3.zero;
        }

        if (distance > settings.arriveSlowRadius)
        {
            desiredVelocity = desiredVelocity.normalized * settings.maxSpeed;
        }

        else
        {
            desiredVelocity = desiredVelocity.normalized * settings.maxSpeed * (distance / settings.arriveSlowRadius);
        }

        Debug.DrawLine(position, position + steeringForce, Color.magenta);
        Debug.DrawLine(position, target.position, Color.green);
        Debug.DrawLine(position, position + velocity, Color.red);

        return steeringForce = desiredVelocity - velocity;
    }

    Vector3 Caculate()
    {
        steeringForce = Vector3.ClampMagnitude(steeringForce, settings.maxSteerForce);
        velocity = Vector3.ClampMagnitude(velocity, settings.maxSpeed);
        velocity += steeringForce * Time.deltaTime;

        //position and direction
        position += velocity * Time.deltaTime;
        forward = velocity.normalized;
        transform.position = position;
        transform.forward = forward;

        Debug.DrawLine(position, position + velocity, Color.red);
        Debug.DrawLine(position, position + steeringForce, Color.green);

        return steeringForce;
    }


    
}

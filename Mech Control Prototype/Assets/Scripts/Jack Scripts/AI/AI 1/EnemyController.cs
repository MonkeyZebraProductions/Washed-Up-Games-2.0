using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
 


    List<SteeringBehaviour> behaviours = new List<SteeringBehaviour>();
  
  
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

    public Vector3 target;
    public Transform targetTransform;
    public float slowingDistance = 10.0f;
    public float decelleration = 3;

    public Vector3 desired;

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + velocity);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + force * 10);
    }

    // Use this for initialization
    void Start()
    {
     

        SteeringBehaviour[] behaviours = GetComponents<SteeringBehaviour>();

        foreach (SteeringBehaviour b in behaviours)
        {
            this.behaviours.Add(b);
        }
    }

    public Vector3 SeekForce()
    {
        Vector3 ToTarget = target - transform.position;
        float distance = ToTarget.magnitude;
   
        ToTarget *= maxSpeed;

        return desired = ToTarget - velocity;

        /*Vector3 desired = target - transform.position;
        desired.Normalize();
        desired *= maxSpeed;


        return desired - velocity;
        */
    }

    public Vector3 ArriveForce()
    {
        Vector3 toTarget = target - transform.position;

        float distance = toTarget.magnitude;
      
        if (distance < slowingDistance)
        {
            toTarget = Vector3.zero;
        }

        if(distance > slowingDistance)
        {
            toTarget = toTarget.normalized * maxSpeed ;
        }
        else
        {
            toTarget = toTarget.normalized * maxSpeed * (distance / slowingDistance);
        }

        return desired = toTarget - velocity;
    }


    Vector3 Calculate()
    {
        force = Vector3.zero;

        foreach (SteeringBehaviour b in behaviours)
        {
            if (b.isActiveAndEnabled)
            {
                force += b.Calculate() * b.weight;
                float f = force.magnitude;
                if (f > maxForce)
                {
                    force = Vector3.ClampMagnitude(force, maxForce);
                    break;
                }
            }
        }



        return force;
    }


    // Update is called once per frame
    void Update()
    {
        target.Set(targetTransform.position.x, targetTransform.position.y, targetTransform.position.z);

        force = Calculate();
        acceleration = force / mass;
        velocity += acceleration * Time.deltaTime;

        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);

        if (velocity.magnitude > 0)
        {
            Vector3 tempUp = Vector3.Lerp(transform.up, Vector3.up + (acceleration * banking), Time.deltaTime * 3.0f);
            transform.LookAt(transform.position + velocity, tempUp);

            transform.position += velocity * Time.deltaTime;
            velocity *= (1.0f - (damping * Time.deltaTime));
        }
    }
}
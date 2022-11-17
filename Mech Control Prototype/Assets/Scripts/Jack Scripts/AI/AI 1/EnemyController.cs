using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : StateMachine
{
 
    List<SteeringBehaviour> behaviours = new List<SteeringBehaviour>();
    [SerializeField]
    private FOV fov;
    public IdleState idleState;
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

    public LayerMask obstacleMask;
    public float boundsRadius = .27f;
    public float avoidCollisionWeight = 10;
    public float collisionAvoidDst = 5;

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + velocity);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + force * 10);
    }

     void Awake()
    {
        
    }

    protected override BaseState GetInitalState()
    {
        return idleState;
    }

    // Use this for initialization
    void Start()
    {
        fov = GetComponent<FOV>();

         SteeringBehaviour[] behaviours = GetComponents<SteeringBehaviour>();

        foreach (SteeringBehaviour b in behaviours)
        {
            this.behaviours.Add(b);
        }
    }

    public Vector3 SeekForce(Vector3 toTarget)
    {
         toTarget = target - transform.position;
        float distance = toTarget.magnitude;

        toTarget *= maxSpeed;

        return desired = toTarget - velocity;

        /*Vector3 desired = target - transform.position;
        desired.Normalize();
        desired *= maxSpeed;


        return desired - velocity;
        */
    }

    public Vector3 ArriveForce( Vector3 toTarget)
    {
         toTarget = target - transform.position;

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

    
    bool IsHeadingForCollision()
    {
        RaycastHit hit;
        if (Physics.SphereCast(transform.position, boundsRadius, transform.forward, out hit, collisionAvoidDst, obstacleMask))
        {
            return true;
        }
        return false;
    }

    Vector3 ObstacleRays()
    {
        Vector3[] rayDirections = ObstacleAvoidanceHelper.directions;

        for (int i = 0; i < rayDirections.Length; i++)
        {
            Vector3 dir = transform.TransformDirection(rayDirections[i]);
            Ray ray = new Ray(transform.position, dir);
            if (!Physics.SphereCast(ray,boundsRadius,collisionAvoidDst,obstacleMask))
            {
                return dir;
            }
        }

        return transform.forward;
    }


    Vector3 SteerTowards(Vector3 vector)
    {
        Vector3 v = vector.normalized * maxSpeed - velocity;
        return Vector3.ClampMagnitude(v, maxForce);
    }

    public void FixedUpdate()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
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

        if(IsHeadingForCollision())
        {
            Vector3 collisionAvoidDir = ObstacleRays();
            Vector3 collisionAvoidForce = SteerTowards(collisionAvoidDir) * avoidCollisionWeight;
            desired = Vector3.ClampMagnitude(desired, maxForce);
            velocity = Vector3.ClampMagnitude(velocity, maxSpeed);
            velocity += desired * Time.deltaTime;

            transform.position += velocity * Time.deltaTime;
            transform.forward = velocity.normalized;

            Debug.DrawLine(transform.position, transform.position + velocity, Color.red);
            Debug.DrawLine(transform.position, transform.position + desired, Color.green);
        }
    }
}

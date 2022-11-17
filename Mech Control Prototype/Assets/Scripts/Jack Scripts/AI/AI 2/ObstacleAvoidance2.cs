using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleAvoidance2 : MonoBehaviour
{

    public EnemeyController2 enemeyController2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (IsHeadingForCollision())
        {
            Vector3 collisionAvoidDir = ObstacleRays();
            Vector3 collisionAvoidForce = SteerTowards(collisionAvoidDir) * enemeyController2.avoidCollisionWeight;
            enemeyController2.steeringForce += collisionAvoidForce;
        }

        enemeyController2.steeringForce = Vector3.ClampMagnitude(enemeyController2.steeringForce, enemeyController2.maxSteerForce);
        enemeyController2.velocity = Vector3.ClampMagnitude(enemeyController2.velocity, enemeyController2.maxSpeed);
        enemeyController2.velocity += enemeyController2.steeringForce * Time.deltaTime;

        enemeyController2.position += enemeyController2.velocity * Time.deltaTime;
        enemeyController2.forward = enemeyController2.velocity.normalized;
        transform.position = enemeyController2.position;
        transform.forward = enemeyController2.forward;

        Debug.DrawLine(enemeyController2.position, enemeyController2.position + enemeyController2.velocity, Color.red);
        Debug.DrawLine(enemeyController2.position, enemeyController2.position + enemeyController2.steeringForce, Color.green);
    }

    bool IsHeadingForCollision()
    {
        RaycastHit hit;
        if (Physics.SphereCast(enemeyController2.position, enemeyController2.boundsRadius, enemeyController2.forward, out hit, enemeyController2.collisionAvoidDst, enemeyController2.obstacleMask))
        {
            return true;
        }
        return false;
    }

    Vector3 ObstacleRays()
    {
        Vector3[] rayDirections = ObstacleAvoidanceManager.directions;

        for (int i = 0; i < rayDirections.Length; i++)
        {
            Vector3 dir = transform.TransformDirection(rayDirections[i]);
            Ray ray = new Ray(enemeyController2.position, dir);
            if (!Physics.SphereCast(ray, enemeyController2.boundsRadius, enemeyController2.collisionAvoidDst, enemeyController2.obstacleMask))
            {
                return dir;
            }
        }

        return enemeyController2.forward;
    }

    Vector3 SteerTowards(Vector3 vector)
    {
        Vector3 v = vector.normalized * enemeyController2.maxSpeed - enemeyController2.velocity;
        return Vector3.ClampMagnitude(v, enemeyController2.maxSteerForce);
    }


}

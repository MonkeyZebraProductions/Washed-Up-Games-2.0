using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleAvoidance : SteeringBehaviour
{
    
    const int numViewDirections = 300;
    public Vector3[] directions;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (HeadingForCollision())
        {
            Vector3 collisionAvoidDir = ObstacleRays();
            Vector3 collisionAvoidForce = SteerTowards(collisionAvoidDir) * enemyController.avoidCollisionWeight;
            enemyController.desired += collisionAvoidForce;
        }

        enemyController.desired = Vector3.ClampMagnitude(enemyController.force, enemyController.maxForce);
        enemyController.velocity = Vector3.ClampMagnitude(enemyController.velocity, enemyController.maxSpeed);
        enemyController.velocity += enemyController.desired * Time.deltaTime;

        enemyController.transform.position += enemyController.velocity * Time.deltaTime;
        enemyController.force = enemyController.velocity.normalized;
        transform.position = enemyController.transform.position;
        transform.forward = enemyController.transform.forward;

        Debug.DrawLine(transform.position, transform.position + enemyController.velocity, Color.red);
        Debug.DrawLine(transform.position, transform.position + force, Color.green);
    }

    public override Vector3 Calculate()
    {   
        return enemyController.force;
    }

    bool HeadingForCollision()
    {
        RaycastHit hit;
        if (Physics.SphereCast(transform.position, enemyController.boundsRadius, transform.forward, out hit, enemyController.collisionAvoidDst, enemyController.obstacleMask))
        {
            return true;
        }

        return false;

    }


    Vector3 ObstacleRays()
    {
        directions = new Vector3[numViewDirections];
        Vector3[] rayDirections = directions;

        for (int i = 0; i < rayDirections.Length; i++)
        {
            Vector3 dir = transform.TransformDirection(rayDirections[i]);
            Ray ray = new Ray(enemyController.transform.position, dir);
            if (!Physics.SphereCast(ray, enemyController.boundsRadius, enemyController.collisionAvoidDst, enemyController.obstacleMask))
            {
                return dir;
            }
        }

        return force;
    }

    Vector3 SteerTowards(Vector3 vector)
    {
        Vector3 v = vector.normalized * enemyController.maxSpeed - enemyController.velocity;
        return Vector3.ClampMagnitude(v, enemyController.maxForce);
    }
    
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrive2 : MonoBehaviour
{
    public EnemeyController2 enemeyController2;
    public Transform arriveTarget;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        arriveTarget = enemeyController2.target;

        Vector3 desiredVelocity = (arriveTarget.position - enemeyController2.position);
        float distance = desiredVelocity.magnitude;

        if (distance > enemeyController2.arriveSlowRadius)
        {
            desiredVelocity = desiredVelocity.normalized * enemeyController2.maxSpeed;
        }

        if(distance < enemeyController2.arriveSlowRadius)
        {
            enemeyController2.velocity = new Vector3(0, 0, 0);
        }

        else
        {
            desiredVelocity = desiredVelocity.normalized * enemeyController2.maxSpeed * (distance / enemeyController2.arriveSlowRadius);
        }

        enemeyController2.steeringForce = desiredVelocity - enemeyController2.velocity;

        Debug.DrawLine(enemeyController2.position, enemeyController2.position + enemeyController2.steeringForce, Color.magenta);

        Debug.DrawLine(enemeyController2.position, enemeyController2.target.position, Color.green);

        Debug.DrawLine(enemeyController2.position, enemeyController2.position + enemeyController2.velocity, Color.red);

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pursue2 : MonoBehaviour
{
    public EnemeyController2 enemeyController2;
    public Transform player;
    
    // Start is called before the first frame update
    void Start()
    {
        enemeyController2.target = player;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 offsetToTarget = (enemeyController2.target.position - enemeyController2.position);
        float distance = offsetToTarget.magnitude;
        enemeyController2.steeringForce = offsetToTarget.normalized * enemeyController2.maxSteerForce;
        if (distance < enemeyController2.arriveSlowRadius)
        {
            enemeyController2.velocity = new Vector3(0, 0, 0);
            enemeyController2.forward = new Vector3(0, 0, 0);
           
        }
    }

}


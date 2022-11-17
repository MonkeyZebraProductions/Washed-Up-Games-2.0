using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seek2 : MonoBehaviour
{
    public EnemeyController2 enemeyController2;
    // Start is called before the first frame update
    void Start()
    {
        
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
        }
    }
}

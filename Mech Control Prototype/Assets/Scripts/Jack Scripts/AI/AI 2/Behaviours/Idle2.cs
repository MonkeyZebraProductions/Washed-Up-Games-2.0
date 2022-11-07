using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle2 : MonoBehaviour
{
    public EnemyController2 enemyController;
    public EnemyControllerSettings settings;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 desiredVelocity = new Vector3(0,0,0);

        enemyController.steeringForce = desiredVelocity;
    }


}

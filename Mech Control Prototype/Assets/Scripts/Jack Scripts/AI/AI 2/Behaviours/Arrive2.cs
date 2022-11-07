using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrive2 : SteeringSystem
{
    public EnemyController2 enemyController;
    public EnemyControllerSettings settings;
    private void FixedUpdate()
    {

        
    }

    public override Vector3 Calculate()
    {
        Vector3 desiredVelocity = enemyController.ArriveForce();
        return desiredVelocity;
    }


}

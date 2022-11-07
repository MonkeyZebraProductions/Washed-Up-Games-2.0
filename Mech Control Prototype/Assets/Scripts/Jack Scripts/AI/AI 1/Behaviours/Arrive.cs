using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Arrive : SteeringBehaviour
{
    public FOV fov;
    public Vector3 targetPosition;
    public float slowingDistance = 15.0f;
    public float decelleration = 10;

    public GameObject targetGameObject = null;

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + force * 100);
        //Gizmos.DrawSphere(transform.position, 10);
    }

    public override Vector3 Calculate()
    {
        Vector3 force = enemyController.ArriveForce(targetPosition);
        return force;
    }

    public void Update()
    {
        if (targetGameObject != null)
        {
            targetPosition = targetGameObject.transform.position;
        }

        if(fov.canSeePlayer)
        {
            targetPosition = new Vector3(enemyController.targetTransform.position.x, enemyController.targetTransform.position.y, enemyController.targetTransform.position.z);
            enemyController.target = targetPosition;
        }

    }
}


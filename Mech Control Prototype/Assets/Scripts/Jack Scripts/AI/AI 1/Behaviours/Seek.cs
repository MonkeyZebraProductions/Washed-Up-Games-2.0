using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Seek : SteeringBehaviour
{
    public GameObject targetGameObject = null;
    public Vector3 target = Vector3.zero;

    public void OnDrawGizmos()
    {
        if (isActiveAndEnabled && Application.isPlaying)
        {
            Gizmos.color = Color.cyan;
            if (targetGameObject != null)
            {
                target = targetGameObject.transform.position;
            }
            Gizmos.DrawLine(transform.position, target);
        }
    }

    public override Vector3 Calculate()
    {
        return enemyController.SeekForce(target);
    }

    public void Update()
    {
        target = new Vector3(enemyController.targetTransform.position.x, enemyController.targetTransform.position.y, enemyController.targetTransform.position.z);
        enemyController.target = target;

        /*
        if (targetGameObject != null)
        {
            target = targetGameObject.transform.position;
        }
        */
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pursue : SteeringBehaviour
{
   public PlayerMovementScript target;

    public Vector3 targetPos;


    public void Start()
    {
        
    }

    public void OnDrawGizmos()
    {
        if (Application.isPlaying && isActiveAndEnabled)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(transform.position, targetPos);
        }
    }

    public override Vector3 Calculate()
    {
        float dist = Vector3.Distance(target.transform.position, transform.position);
        float time = dist / enemyController.maxSpeed;

        targetPos = target.transform.position + (target.playerVelocity * time);

        return enemyController.SeekForce(targetPos);
    }
}

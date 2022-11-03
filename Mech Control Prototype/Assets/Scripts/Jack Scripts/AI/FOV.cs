using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOV : MonoBehaviour
{
    public float sightRadius;
    public float attackRadius;
    [Range(0, 360)]
    public float sightAngle;
    [Range(0, 360)]
    public float attackAngle;

    public GameObject playerRef;

    public LayerMask targetMask;
    public LayerMask obstructionMask;

    public bool canSeePlayer;
    public bool inAttackRange;

    private void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(FOVSightRoutine());
    }

    private IEnumerator FOVSightRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.5f);

        while (true)
        {
            yield return wait;
            FieldOfViewSightCheck();
        }
    }

    private IEnumerator FOVAttackRange()
    {
        WaitForSeconds wait = new WaitForSeconds(0.5f);

        while (true)
        {
            yield return wait;
            FieldOfViewSightCheck();
        }
    }

    public Vector3 DirectionFromAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY;

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    private void FieldOfViewSightCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, sightRadius, targetMask);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < sightAngle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                {
                    canSeePlayer = true;
                }
                    
                else
                {
                    canSeePlayer = false;
                }
                    
            }
            else
            {
                canSeePlayer = false;
            }
              
        }

        else if (canSeePlayer)
        {
            canSeePlayer = false;
        }
           
    }



    private void FieldfViewAttackCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, attackRadius, targetMask);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < attackAngle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                {
                    inAttackRange = true;
                }

                else
                {
                    inAttackRange = false;
                }

            }
            else
            {
                inAttackRange = false;
            }

        }

        else if (inAttackRange)
        {
            inAttackRange = false;
        }
    }
}
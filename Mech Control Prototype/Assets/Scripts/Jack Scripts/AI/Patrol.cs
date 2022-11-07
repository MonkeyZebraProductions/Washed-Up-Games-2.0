using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : SteeringBehaviour
{
    public PatrolPath path;
 

    public Vector3 nextWaypoint;

    public float waypointDistance = 5;

    public int next = 0;

    public bool looped = false;

    public void OnDrawGizmos()
    {
        if (isActiveAndEnabled && Application.isPlaying)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(transform.position, nextWaypoint);
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        enemyController.target = nextWaypoint;
    }


    public Vector3 NextWaypoint()
    {
        return path.waypoints[next];
      
    }


    public void AdvanceToNext()
    {
        if (looped)
        {
            next = (next + 1) % path.waypoints.Count;
        }
        else
        {
            if (next != path.waypoints.Count - 1)
            {
                next++;
            }
        }
    }


    public bool IsLast()
    {
        return next == path.waypoints.Count - 1;
    }

    public override Vector3 Calculate()
    {
        nextWaypoint = NextWaypoint();
        if (Vector3.Distance(transform.position, nextWaypoint) < waypointDistance)
        {
            AdvanceToNext();
            Debug.Log("Next Waypoint");
        }

        if (!looped && IsLast())
        {
         
            return enemyController.ArriveForce(nextWaypoint);
         
            Debug.Log("ArriveForce");
        }
        else
        {
            return enemyController.ArriveForce(nextWaypoint);
            Debug.Log("ArriveForce2");
        }


    }
}

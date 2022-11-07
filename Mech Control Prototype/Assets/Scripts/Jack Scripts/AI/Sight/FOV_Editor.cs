using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FOV))]
public class FOV_Editor : Editor
{
    private void OnSceneGUI()
    {
        FOV fov = (FOV)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(fov.sightPoint.transform.position, Vector3.up, Vector3.forward, 360, fov.sightRadius);

        Handles.color = Color.red;
        Handles.DrawWireArc(fov.attackPoint.transform.position, Vector3.up, Vector3.forward, 360, fov.attackRadius);


        Vector3 viewAngle01 = fov.DirectionFromAngle(fov.transform.eulerAngles.y, -fov.sightAngle / 2);
        Vector3 viewAngle02 = fov.DirectionFromAngle(fov.transform.eulerAngles.y, fov.sightAngle / 2);

        Vector3 attackAngle01 = fov.DirectionFromAngle(fov.transform.eulerAngles.y, -fov.attackAngle / 2);
        Vector3 attackAngle02 = fov.DirectionFromAngle(fov.transform.eulerAngles.y, fov.attackAngle / 2);

        Handles.color = Color.yellow;
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle01 * fov.sightRadius);
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle02 * fov.sightRadius);

        Handles.color = Color.yellow;
        Handles.DrawLine(fov.transform.position, fov.transform.position + attackAngle01 * fov.attackRadius);
        Handles.DrawLine(fov.transform.position, fov.transform.position + attackAngle02 * fov.attackRadius);

        if (fov.canSeePlayer)
        {
            Handles.color = Color.red;
            Handles.DrawLine(fov.transform.position, fov.playerRef.transform.position);
        }

        if (fov.inAttackRange)
        {
            Handles.color = Color.green;
            Handles.DrawLine(fov.transform.position, fov.playerRef.transform.position);
        }
    }

   
}

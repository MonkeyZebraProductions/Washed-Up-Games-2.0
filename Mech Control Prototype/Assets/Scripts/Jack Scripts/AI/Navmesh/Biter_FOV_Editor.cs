using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Biter_FOV))]

public class Biter_FOV_Editor : Editor
{
    private void OnSceneGUI()
    {
        Biter_FOV fov = (Biter_FOV)target;
        Handles.color = Color.yellow;
        Handles.DrawWireArc(fov.sightPoint.transform.position, Vector3.up, Vector3.forward, 360, fov.sightRadius);

        Vector3 viewAngle01 = fov.DirectionFromAngle(fov.sightPoint.transform.eulerAngles.y, -fov.sightAngle / 2);
        Vector3 viewAngle02 = fov.DirectionFromAngle(fov.sightPoint.transform.eulerAngles.y, fov.sightAngle / 2);

        Handles.color = Color.yellow;
        Handles.DrawLine(fov.sightPoint.transform.position, fov.sightPoint.transform.position + viewAngle01 * fov.sightRadius);
        Handles.DrawLine(fov.sightPoint.transform.position, fov.sightPoint.transform.position + viewAngle02 * fov.sightRadius);
        if (fov.canSeePlayer)
        {
            Handles.color = Color.yellow;
            Handles.DrawLine(fov.transform.position, fov.playerRef.transform.position);
        }

      
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seek : MonoBehaviour
{
    public Transform target;
    [SerializeField]
    private SteeringBehaviour steeringBehaviour ;

    
    void Awake()
    {
        steeringBehaviour = GetComponent<SteeringBehaviour>();
    }

    
    void FixedUpdate()
    {
        Vector3 accel = steeringBehaviour.Seek(target.position);
        steeringBehaviour.Steering(accel);
        steeringBehaviour.SteeringDirection();
        
    }


}

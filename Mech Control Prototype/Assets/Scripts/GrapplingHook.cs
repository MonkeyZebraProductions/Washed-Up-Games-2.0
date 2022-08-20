using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHook : MonoBehaviour
{
    //Grapple hook Variables
    public float Length, SpawnDistance;
    public float GrappleSpeed;
    public float HangTime,ReloadTime;

    //States of the GrappleHook
    public bool IsHooked,TargetReached,ObjectGrabbed;


    public Vector3 target;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if(!IsHooked)
        {
            //Moves the grapple hook
            transform.position = Vector3.MoveTowards(transform.position, target, GrappleSpeed * Time.deltaTime);
        }
        
       
    }

}

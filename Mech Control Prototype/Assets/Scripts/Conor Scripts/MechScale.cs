using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechScale : MonoBehaviour
{
    // Start is called before the first frame update
    public float yValue,zValue;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = new Vector3(0, yValue, zValue);
    }
}

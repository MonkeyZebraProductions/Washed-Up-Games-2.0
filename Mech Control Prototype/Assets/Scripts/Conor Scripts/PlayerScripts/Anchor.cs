using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anchor : MonoBehaviour
{
    private GrapplingHook _gH;
    // Start is called before the first frame update
    void Start()
    {
        _gH = FindObjectOfType<GrapplingHook>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag=="GrapplePad")
        {
            _gH.IsHooked = true;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}

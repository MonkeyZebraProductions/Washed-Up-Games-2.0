using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anchor : MonoBehaviour
{
    private GrapplingHook _gH;
    private GunScript _gS;
    // Start is called before the first frame update
    void Start()
    {
        _gH = FindObjectOfType<GrapplingHook>();
        _gS = FindObjectOfType<GunScript>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer==8)
        {
            
            if (other.gameObject.tag == "GrapplePad")
            {
                _gH.IsHooked = true;
            }
            else if (other.gameObject.tag == "GrabbableObject")
            {
                other.gameObject.transform.SetParent(this.transform);
                _gH.ObjectGrabbed = true;
            }
            _gH.TargetReached = true;
        }
        else
        {
            _gS.IsGrappling = false;
            Destroy(this.gameObject);
        }
    }
}

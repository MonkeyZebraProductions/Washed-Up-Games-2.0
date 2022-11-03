using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public FOV fov;
    [SerializeField]
    private Seek seek;
    [SerializeField]
    private Arrive arrive;
    // Start is called before the first frame update
    void Start()
    {
       seek =  GetComponent<Seek>();
       arrive = GetComponent<Arrive>();
    }


    // Update is called once per frame
    void FixedUpdate()
    {
       if(fov.canSeePlayer)
        {
            seek.enabled = true;
            arrive.enabled = false;
        }

        if (!fov.canSeePlayer)
        {
            seek.enabled = false;
            arrive.enabled = true;
        }
    }
}

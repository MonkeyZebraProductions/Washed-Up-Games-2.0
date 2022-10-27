using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_FieldOfView : MonoBehaviour
{

    public float sightRadius;
    public float sightAngle;

    public GameObject player;

    public LayerMask targetMask;
    public LayerMask ObstacleMask;

    public bool canSeePlayer;

    public float Enemydelay;

 
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private IEnumerator FOV()
    {
        float delay = Enemydelay;

        WaitForSeconds waitForSeconds = new WaitForSeconds(delay);

        while(true)
        {

        }
    }

    void Update()
    {
        
    }
}

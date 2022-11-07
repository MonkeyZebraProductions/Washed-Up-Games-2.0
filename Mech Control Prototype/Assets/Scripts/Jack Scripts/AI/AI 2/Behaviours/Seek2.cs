using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seek2 : MonoBehaviour
{
    public EnemyController2 enemyController2;
    public EnemyControllerSettings enemyControllerSettings;
    public Vector3 ToTarget;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ToTarget = (enemyController2.target.position - enemyController2.position);
        enemyController2.steeringForce = ToTarget.normalized * enemyControllerSettings.maxSteerForce;
    }
}

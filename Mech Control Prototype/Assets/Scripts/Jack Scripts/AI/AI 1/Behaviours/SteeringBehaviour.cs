using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SteeringBehaviour : MonoBehaviour
{
    public float weight = 1.0f;
    public Vector3 force;

    [HideInInspector]
    public EnemyController enemyController;


    public void Awake()
    {
        enemyController = GetComponent<EnemyController>();
    }

    public abstract Vector3 Calculate();
}

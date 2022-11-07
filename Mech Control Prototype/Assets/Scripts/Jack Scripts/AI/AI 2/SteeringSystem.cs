using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SteeringSystem : MonoBehaviour
{
    public EnemyController2 enemyController2;

    public void Awake()
    {
        enemyController2 = GetComponent<EnemyController2>();
    }

    public abstract Vector3 Calculate();
}

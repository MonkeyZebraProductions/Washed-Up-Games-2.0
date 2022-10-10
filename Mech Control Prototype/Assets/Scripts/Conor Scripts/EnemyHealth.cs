using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float Health;

    private void Update()
    {
        if(Health<=0)
        {
            Destroy(this.gameObject);
        }
    }
    public void TakeDamage (float damage)
    {
        Health -= damage;
    }
}

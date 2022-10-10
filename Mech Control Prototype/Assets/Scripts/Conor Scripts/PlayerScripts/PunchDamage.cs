using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchDamage : MonoBehaviour
{
    public float Damage;
    public float PunchForce;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<EnemyHealth>() != null)
        {
            other.gameObject.GetComponent<EnemyHealth>().TakeDamage(Damage);
            other.gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * PunchForce, ForceMode.Impulse);
        }
    }
}

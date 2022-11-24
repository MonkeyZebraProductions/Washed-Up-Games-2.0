using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float Speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * Speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag=="Enemy")
        {
            other.gameObject.GetComponent<EnemyUnitHealth>().TakeDamage(1);
        }
        if (other.gameObject.GetComponent<LocalGlobalSwitcher>() == null)
        {
            Debug.Log(other.gameObject);
            Destroy(this.gameObject);
        }

    }
}

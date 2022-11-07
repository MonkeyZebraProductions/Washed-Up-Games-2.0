using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnitHealth : MonoBehaviour
{
    public int currentHealth;
    public int MaxHealth;


    // Start is called before the first frame update
    void Start()
    {
        currentHealth = MaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
       if(currentHealth <= 0)
        {
            Death();
        }
    }

    public void TakeDamage(int damage)
    {
        if (currentHealth > 0)
        {
            currentHealth -= damage;
        }
    }

    public void Heal(int heal)
    {
        if (currentHealth < MaxHealth)
        {
            currentHealth += heal;

        }

        if (currentHealth > MaxHealth)
        {
            currentHealth = MaxHealth;
        }
    }

    public void Death()
    {
        Destroy(gameObject);
    }


}

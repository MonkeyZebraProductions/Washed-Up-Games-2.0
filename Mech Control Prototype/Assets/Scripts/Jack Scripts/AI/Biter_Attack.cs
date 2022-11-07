using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Biter_Attack : MonoBehaviour
{
    public FOV fov;
    public AirArmour airArmour;
    private float lastAttack = 0f;
    public float attackRate;

    // Start is called before the first frame update
    void Awake()
    {
        airArmour = GameObject.FindGameObjectWithTag("Player").GetComponent<AirArmour>();
    }

    // Update is called once per frame
    void Update()
    {
        Attack();
    }

    void Attack()
    {
        if(fov.inAttackRange)
        {
            if (Time.time - lastAttack < attackRate)
            {
                return;
            }

            lastAttack = Time.time;

            Debug.Log("Biter Attacks");
            airArmour.RecieveArmourDamage(1);



        }
    }
}

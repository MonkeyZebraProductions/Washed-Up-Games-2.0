using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Biter_Attack_Controller : MonoBehaviour
{
    [SerializeField]
    private AirArmour airArmour;
    private float lastAttack = 0f;
    public float attackRate;

    public DateTime nextDamage;
    public float damageAfterTime;
    public GameObject player;
    public bool playerInAttackRange = false;

    // Start is called before the first frame update
    void Awake()
    {
        airArmour = GameObject.FindGameObjectWithTag("Player").GetComponent<AirArmour>();
    }

    // Update is called once per frame
    void Update()
    {
        if(playerInAttackRange)
        {
            player.GetComponent<AirArmour>().RecieveArmourDamage(1f);
           
        }

        if(!playerInAttackRange)
        {
            player.GetComponent<AirArmour>().RecieveArmourDamage(0f);
          
        }
    }

    private void OnTriggerEnter(Collider attack)
    {
      

        if(attack.tag == "Player")
        {
            player = attack.gameObject;
            playerInAttackRange = true;

        }
    }

    private void OnTriggerExit(Collider attack)
    {
        if (attack.tag == "Player")
        {
        
            playerInAttackRange = false;

        }
    }


    

   
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AirArmour : MonoBehaviour
{
    public float MaxAir = 100;
    public float AirDecreaceRate = 1;

    public float BaseDamageMultiplier = 1;
    public float DamageReceaved = 1;

    private float air, damage;

    public TextMeshProUGUI AirText;
    // Start is called before the first frame update
    void Start()
    {
        air = MaxAir;
        damage = BaseDamageMultiplier;
    }

    // Update is called once per frame
    void Update()
    {

        if(damage<BaseDamageMultiplier)
        {
            damage = BaseDamageMultiplier;
        }
        air -= AirDecreaceRate * damage*Time.deltaTime;

        AirText.text = "Air: " + Mathf.Round(air*100)/100;
    }

    public void RecieveArmourDamage(float damageRecieved)
    {
        damage += damageRecieved;
    }

    public void RecieveArmourRepair(float repairAmount)
    {
        damage -= repairAmount;
    }

    public void RefillAir(float refillAmount)
    {
        air += refillAmount;
    }
}
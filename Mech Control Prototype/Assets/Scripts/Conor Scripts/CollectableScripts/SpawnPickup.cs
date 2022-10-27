using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPickup : MonoBehaviour
{
    public List<GameObject> CommonDrops, RareDrops;

    private int weight;

    public int RareDropWeight;
    // Start is called before the first frame update
    public void DropPickup()
    {
        weight = Random.Range(0, 100);
        if (weight >= RareDropWeight)
        {
            Instantiate(RareDrops[Random.Range(0, RareDrops.Count)], transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(CommonDrops[Random.Range(0, RareDrops.Count)], transform.position, Quaternion.identity);
        }
        
    }
}

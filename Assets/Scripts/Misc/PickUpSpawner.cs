using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpSpawner : MonoBehaviour
{
    [SerializeField] private GameObject goldCoin, healthGlobe, staminaGlobe;
    [SerializeField] private GameObject leaf, gel, bucket;

    public void DropItems() {
        int randomNum = Random.Range(1, 7);

        if (randomNum == 1) 
        {
            Instantiate(healthGlobe, transform.position, Quaternion.identity); 
        } 

        if (randomNum == 2) 
        {
            Instantiate(staminaGlobe, transform.position, Quaternion.identity); 
        }

        if (randomNum == 3)
        {
            Instantiate(leaf, transform.position, Quaternion.identity);
        }

        if (randomNum == 4)
        {
            Instantiate(gel, transform.position, Quaternion.identity);
        }

        if (randomNum == 5)
        {
            Instantiate(bucket, transform.position, Quaternion.identity);
        }

        if (randomNum == 6) {
            int randomAmountOfGold = Random.Range(1, 4);
            
            for (int i = 0; i < randomAmountOfGold; i++)
            {
                Instantiate(goldCoin, transform.position, Quaternion.identity);
            }
        }
    }
}

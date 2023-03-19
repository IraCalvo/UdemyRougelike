using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    public int coinValue;
    public float waitToBeCollected;
    public int pickupSFX;

    void Update()
    {
        if(waitToBeCollected > 0)
        {
            waitToBeCollected -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D otherCollider) 
    {
        if(otherCollider.tag == "Player" && waitToBeCollected <= 0)
        {
            LevelManager.instance.GetCoins(coinValue);
            Destroy(gameObject);
            AudioManager.instance.PlaySFX(pickupSFX);
        }    
    }
}

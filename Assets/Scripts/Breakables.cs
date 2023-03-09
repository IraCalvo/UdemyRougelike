using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakables : MonoBehaviour
{
    public GameObject[] brokenPieces;
    public int maxPieces;
    public bool shouldDropItems;
    public GameObject[] itemsToDrop;
    public float itemDropPercent;
    public int breakSound;

    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        if(otherCollider.tag == "Player")
        {
            if(PlayerController.instance. dashCounter > 0)
            {
                Smash();
            }
        }
        if(otherCollider.tag == "PlayerBullet")
        {
            Smash();
        }
    }

    public void Smash()
    {
        Destroy(gameObject);
        AudioManager.instance.PlaySFX(breakSound);
        BrokenPiecesToShow();
        DropItems();
    }

    private void BrokenPiecesToShow()
    {
        int piecesToDrop = Random.Range(1, maxPieces);

        for(int i = 0; i < piecesToDrop; i++)
        {
            int randomPiece = Random.Range(0, brokenPieces.Length);
            Instantiate(brokenPieces[randomPiece], transform.position, transform.rotation);
        }
    }

    private void DropItems()
    {
        if(shouldDropItems)
        {
            float dropChance = Random.Range(0f, 100f);
            if(dropChance <= itemDropPercent)
            {
                int randomItem = Random.Range(0, itemsToDrop.Length);
                Instantiate(itemsToDrop[randomItem], transform.position, transform.rotation);
            }
        }
    }
}

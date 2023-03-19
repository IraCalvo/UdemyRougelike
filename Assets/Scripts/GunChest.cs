using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunChest : MonoBehaviour
{
    public WeaponPickup[] potentialGuns;
    public SpriteRenderer theSr;
    public Sprite chestOpen;
    public GameObject notification;
    private bool canOpen;
    public Transform spawnPoint;
    private bool chestLooted = false;
    public float scaleSpeed;

    void Update()
    {
        if(canOpen && !chestLooted)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                int gunSelect = Random.Range(0, potentialGuns.Length);
                Instantiate(potentialGuns[gunSelect], spawnPoint.position, spawnPoint.rotation);
                theSr.sprite = chestOpen;
                chestLooted = true;

                transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
            }
        }

        if(chestLooted)
        {
            transform.localScale = Vector3.MoveTowards(transform.localScale, Vector3.one, Time.deltaTime * scaleSpeed);
        }
    }

    public void OnTriggerEnter2D(Collider2D otherCollider) 
    {
        if(otherCollider.tag == "Player")
        {
            notification.SetActive(true);
            canOpen = true;
        }
    }

    public void OnTriggerExit2D(Collider2D otherCollider)
    {
        if (otherCollider.tag == "Player")
        {
            notification.SetActive(false);
            canOpen = false;
        }
    }
}
